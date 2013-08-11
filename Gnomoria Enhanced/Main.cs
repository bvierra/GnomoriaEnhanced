using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using GELibrary;
using GnomoriaEnhanced.UI;

namespace GnomoriaEnhanced
{
    public partial class Main : Form
    {
        #region Variables

        // Options loaded from the Gnomoria Enhanced.exe.config file
        private bool optLogging;
        private bool optBackup;
        private bool optAutoLoadSavedGame;
        private string optAutoLoadSavedGamePath;

        private int _skillColStart = 4;
        private int _statColStart = 2;

        private GameModel gnomoria;
        private string saveFolderPath;
        private string _gnomoriaBaseFolder;
        private string _gnomoriaSaveBackupFolder;
        private Font dataGridViewCharSkillsHeaderFont;
        private string _loadErrorMessage;
        private string _loadedSavedGameName;
        
        // Status variables
        private bool _settingsVerified = false;
        private bool _gameModelInitialized = false;
        private bool _savedGameLoaded = false;

        // Log Tab
        private ListBoxLog listBoxLog;

        // Background workers
        private BackgroundWorker _workerInitialize;
        private BackgroundWorker _workerLoadGame;
        private FileSystemWatcher _savedGameWatcher = null;

        #endregion

        #region Initialization
        // Constructor
        public Main()
        {
            optLogging = Properties.Settings.Default.Logging;
            Console.WriteLine("Option Logging: {0}", optLogging);
            optBackup = Properties.Settings.Default.Backup;
            Console.WriteLine("Option Backup: {0}", optBackup);
            optAutoLoadSavedGame = Properties.Settings.Default.AutoLoadSave;
            Console.WriteLine("Option AutoLoadSave: {0}", optAutoLoadSavedGame);
            optAutoLoadSavedGamePath = Properties.Settings.Default.AutoLoadSaveGame;

            InitializeComponent();     
        }

        public void Main_Load(object sender, System.EventArgs e)
        {
            // Loads Log Tab
            Load_Debug();

            // Setup up Data View Headers
            dataGridViewCharSkillsHeaderFont = new Font(new FontFamily("Arial"), 11);

            // Note: using a double buffer doesn't prevent issue with column names not painted correctly
            // when scrolling back to the left, but it improves the overall painting experience.
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                dataGridViewCharSkills,
                new object[] { true });

            // Verify the game settings and display a warning message if something is wrong.
            VerifyGnomoriaSettings();
            
            // Initialize the Gnomoria game model
            // Note: Using parameter force = false to do nothing if the game settings were KO.
            InitializeGameModel(false);

            // Hide the tabs showing game data until a saved game is loaded.
            tabControl.Controls.Remove(this.tabCharSkills);
            tabControl.Controls.Remove(this.tabCharStats);
            tabControl.Controls.Remove(this.tabItemFinder);

            // Reset menu items according to status
            ResetMenuItems();
        }

        public void Main_Shown(object sender, System.EventArgs e)
        {
            if (optAutoLoadSavedGame && optAutoLoadSavedGamePath != null)
            {
                log(LogLevel.Info, "Auto loading world: " + optAutoLoadSavedGamePath);
                openToolStripMenuItem.Enabled = false;
                LoadGame(optAutoLoadSavedGamePath);
            }
        }
        #endregion

        #region Menu Items
        // Menu Items
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.InitialDirectory = saveFolderPath;
            openDlg.Filter = "Gnomoria Save Files (*.sav)|world*.sav";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                log(LogLevel.Info, "Opening Game: " + openDlg.SafeFileName.ToString());
                _loadedSavedGameName = openDlg.SafeFileName;
                LoadGame(openDlg.SafeFileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void logToolStripMenuItem_Checked(object sender, EventArgs e)
        {
            string a;
            if (logToolStripMenuItem.Checked)
            {
                a = "Enabled";
                listBox1.Visible = true;
            }
            else
            {
                a = "Disabled";
                listBox1.Visible = false;
            }

            log(LogLevel.Debug,"Log mode has been changed to: " + a);
        }

        private void autoBackupSavedGamesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (autoBackupSavedGamesToolStripMenuItem.Checked)
            {
                log(LogLevel.Debug, "Auto backup mode has been enabled.");
                optBackup = true;

                // Activate the saved game watcher if it was not active already.
                // Note: if the game model is not correctly initialized, this will reset optBackup to false.
                InitializeSavedGameWatcher();
            }
            else
            {
                log(LogLevel.Debug, "Auto backup mode has been disabled.");
                optBackup = false;
            }
        }

        private void initializeGameModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Force game model initialization
            InitializeGameModel(true);

            // Try to start the saved game watcher
            InitializeSavedGameWatcher();

            // Update menu items accordingly.
            ResetMenuItems();
        }

        /// <summary>
        /// Reset the active/inactive state of menu items based on the application status.
        /// </summary>
        private void ResetMenuItems()
        {
            initializeGameModelToolStripMenuItem.Enabled = !_gameModelInitialized;
            openToolStripMenuItem.Enabled = _gameModelInitialized;
        }

        #endregion

        #region Button Clicks

        private void findItemButton_Click(object sender, EventArgs e)
        {
            FindItem();
        }

        private void ItemFamilyCombo_SelectedItemChanged(Object sender, EventArgs e)
        {
            UpdateItemFinderCombos(true); // True = update the list of subfamilies and the list of item types.
        }

        private void ItemSubFamilyCombo_SelectedItemChanged(Object sender, EventArgs e)
        {
            UpdateItemFinderCombos(false); // False = do not update the list of subfamilies, only the item types.
        }

        #endregion

        #region Background Workers
        // Background Workers

        // Initialize Backgroundworker

        private void Initialize_DoWork(object sender, DoWorkEventArgs e)
        {
            gnomoria = new GameModel();            
            Result res = gnomoria.Initialize();
            if (res.Success == true)
            {
                saveFolderPath = gnomoria.getSavedGameFolder();
                _gnomoriaBaseFolder = gnomoria.getSettingsFolder();
                Directory.CreateDirectory(_gnomoriaBaseFolder + @"BackupWorlds");
                _gnomoriaSaveBackupFolder = _gnomoriaBaseFolder + @"BackupWorlds";
                log(LogLevel.Debug, "Ensured that backup folder (" + _gnomoriaSaveBackupFolder + ") exists");
            }

            // Pass Result object as e.Result to the sender object.
            e.Result = new Result(res.Success, res.ErrorMessage);
        }

        private void Initialize_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBar.Value = 100;
            if (e.Error == null)
            {
                Result res = e.Result as Result;
                if (res.Success == true)
                {
                    log(LogLevel.Info, "Game model initialization completed.");
                    _gameModelInitialized = true;
                }
                else
                {
                    log(LogLevel.Error, "Game model initialization failed: " + res.ErrorMessage);
                    log(LogLevel.Error, "Advice: verify that you are running GnomoriaEnhanced from the Gnomoria directory!");

                    _gameModelInitialized = false;
                }
            }
        }
        

        // Load Game Background Worker

        private void LoadGame_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;

            Result res = gnomoria.LoadGame(e.Argument.ToString());

            if (res.Success == false)
            {
                log(LogLevel.Error,"[Main] LoadGame_DoWork failed at gnomoria.LoadGame with message: " + res.ErrorMessage);
                e.Result = "Failed: " + res.ErrorMessage;
                _loadErrorMessage = res.ErrorMessage;
            }
            else
            {
                bw.ReportProgress(0,"Parsing Character Skills...");
                Result res2 = gnomoria.Load_Character_Skills();
                if (res2.Success == false)
                {
                    log(LogLevel.Error,"[Main] LoadGame_DoWork failed at gnomoria.Load_Character_Skills with message: " + res2.ErrorMessage);
                    e.Result = "Failed: " + res2.ErrorMessage;
                    _loadErrorMessage = res2.ErrorMessage;
                }
                else
                {
                    bw.ReportProgress(0, "Parsing Character Stats...");
                    Result res3 = gnomoria.Load_Character_Stats();
                    if (res3.Success == false)
                    {
                        log(LogLevel.Error, "[Main] LoadGame_DoWork failed at gnomoria.Load_Character_Stats with message: " + res3.ErrorMessage);
                        e.Result = "Failed: " + res3.ErrorMessage;
                        _loadErrorMessage = res3.ErrorMessage;
                    }
                    else
                    {
                        _savedGameLoaded = true;
                    }
                }
            }
        }

        private void LoadGame_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.toolStripProgressBar.Value = 100;
            if (e.Error != null)
            {
                log(LogLevel.Error,"[Main] LoadGame_RunWorkerCompleted failed with message: " + e.Result.ToString());
            }
            else
            {
                this.toolStripStatusLabel.Text = "Loaded Game!";
                this.toolStripProgressBar.Value = 0;
            }
        }

        private void LoadGame_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.toolStripStatusLabel.Text = e.UserState.ToString();
        }
        #endregion

        #region Tabs and game loading methods
        // Tab Loading

        /// <summary>
        /// Load a saved game from the disk and update the displayed information. Uses a background worker.
        /// </summary>
        /// <param name="savedGame">Saved game file name, relative to the Saved game directory</param>
        private void LoadGame(string savedGame)
        {
            // Stop here if the game model is not initialized yet.
            if (_gameModelInitialized == false)
            {
                log(LogLevel.Warning, "Can't load saved game " + savedGame + " before the game model has been initialized.");
                return;
            }

            log(LogLevel.Info, "Loading Game: " + savedGame);
            this.toolStripStatusLabel.Text = "Loading Game: " + savedGame;
            this.toolStripProgressBar.Value = 0;

            // Clear up old saved game
            if (dataGridViewCharSkills.DataSource != null)
            {
                dataGridViewCharSkills.DataSource = null;
            }

            if (dataGridViewCharStats.DataSource != null)
            {
                dataGridViewCharStats.DataSource = null;
            }

            // Setup Background Worker
            _workerLoadGame = new BackgroundWorker();
            _workerLoadGame.DoWork += new DoWorkEventHandler(this.LoadGame_DoWork);
            _workerLoadGame.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.LoadGame_RunWorkerCompleted);
            _workerLoadGame.ProgressChanged += new ProgressChangedEventHandler(LoadGame_ProgressChanged);
            _workerLoadGame.WorkerReportsProgress = true;
            _workerLoadGame.RunWorkerAsync(savedGame);

            while (this._workerLoadGame.IsBusy)
            {
                this.toolStripProgressBar.Increment(1);
                Application.DoEvents();
                Thread.Sleep(250);
            }

            // The worker will update the _savedGameLoaded status variable on success
            if (_savedGameLoaded == true)
            {
                // Add back the tabs showing game data, unless it has been already
                if (tabControl.Controls.Contains(this.tabCharSkills) == false)
                {
                    tabControl.Controls.Add(this.tabCharSkills);
                    tabControl.Controls.Add(this.tabCharStats);
                    tabControl.Controls.Add(this.tabItemFinder);
                }

                LoadTabPageOverview();
                LoadTabCharStats();
                LoadTabCharSkills();
                LoadTabItemFinder();

                log(LogLevel.Info, "Game Loaded");
                this.tabControl.SelectedTab = tabCharSkills;
            }
            else
            {
                // Loading the game failed - Display error message and exit program
                // TODO: Figure out how to fix OutOfMemoryException
                Console.WriteLine("ERROR: {0}", _loadErrorMessage);

                string errorMsg;

                if (_loadErrorMessage == "Exception of type 'System.OutOfMemoryException' was thrown.")
                {
                    errorMsg = "Loading saved game failed with the following error:\n" + _loadErrorMessage
                        + "\n\n"
                        + "This is usually caused by editing the game and enlarging the ore";
                }
                else
                {
                    errorMsg = "Loading saved game failed with the following error:\n" + _loadErrorMessage;
                }

                MessageBox.Show(errorMsg,
                        "Error loading saved game",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1);

                Application.Exit();
            }
        }

        /// <summary>
        /// Initialize the Character Skills tab. Called by LoadGame().
        /// </summary>
        public void LoadTabCharSkills()
        {
            this.tabCharSkills.Show();

            dataGridViewCharSkills.SuspendLayout();

            // Load has completed successfully display information
            dataGridViewCharSkills.DataSource = gnomoria.getCharSkills();
            dataGridViewCharSkills.AllowUserToAddRows = false;
            dataGridViewCharSkills.AllowUserToDeleteRows = false;

            dataGridViewCharSkills.Columns[0].Visible = false;
            dataGridViewCharSkills.Columns[1].Frozen = true;
            dataGridViewCharSkills.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCharSkills.Columns[2].Frozen = true;
            dataGridViewCharSkills.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Change default alignment to middle right for numeric columns
            for (int col = _skillColStart; col < dataGridViewCharSkills.Columns.Count; col++)
            {
                dataGridViewCharSkills.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dataGridViewCharSkills.Sort(dataGridViewCharSkills.Columns[1], ListSortDirection.Ascending); // Sort by Name Asc 

            // Force header resizing to make sure all skill names are displayed correctly.
            dataGridViewCharSkills.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewCharSkills_CellClick(null, new DataGridViewCellEventArgs(-1, -1));

            dataGridViewCharSkills.ResumeLayout();
        }

        /// <summary>
        /// Initialize the Character Stats tab. Called by LoadGame().
        /// </summary>
        public void LoadTabCharStats()
        {
            this.tabCharStats.Show();

            dataGridViewCharStats.SuspendLayout();

            // Load has completed successfully display information
            dataGridViewCharStats.DataSource = gnomoria.getCharStats();
            dataGridViewCharStats.AllowUserToAddRows = false;
            dataGridViewCharStats.AllowUserToDeleteRows = false;

            dataGridViewCharStats.Columns[0].Visible = false;
            dataGridViewCharStats.Columns[1].Frozen = true;
            dataGridViewCharStats.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Change default alignment to middle right for numeric columns
            for (int col = _statColStart; col < dataGridViewCharStats.Columns.Count; col++)
            {
                dataGridViewCharStats.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            dataGridViewCharStats.Sort(dataGridViewCharStats.Columns[1], ListSortDirection.Ascending); // Sort by Name Asc 

            // Force header resizing to make sure all skill names are displayed correctly.
            dataGridViewCharStats.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewCharStats_CellClick(null, new DataGridViewCellEventArgs(-1, -1));

            dataGridViewCharStats.ResumeLayout();
        }

        /// <summary>
        /// Initialize the Kingdom overview tab. Called by LoadGame().
        /// </summary>
        public void LoadTabPageOverview()
        {
            Dictionary<string, string> overview = gnomoria.getKingdomOverview();

            lblTabOverviewKingdomNameValue.Text = overview["KingdomName"] + " (File: " + _loadedSavedGameName + ")";
            lblTabOverviewTotalWorthValue.Text  = overview["TotalWealth"];
            lblTabOverviewDateValue.Text        = overview["GameDate"];
        }

        #endregion

        #region Item Finder
        public void LoadTabItemFinder()
        {
            // Initialize the item finder combo boxes
            itemFamilyCombo.DataSource    = ItemFamily.GetFamilies();
            itemQualityCombo.DataSource   = Enum.GetValues(typeof(GameLibrary.ItemQuality));
            itemQualityCombo.SelectedItem = GameLibrary.ItemQuality.Any;

            UpdateItemFinderCombos(true); // True = update the list of subfamilies and the list of item types.
        }
        
        /// <summary>
        /// Update the dataGridViewItems table with the list of items described by the Item Finder combos and present in the game map.
        /// Called when the "Find Items" button is clicked.
        /// </summary>
        private void FindItem()
        {
            // Stop here if the no saved game has been loaded yet.
            if (_savedGameLoaded == false)
            {
                log(LogLevel.Warning, "Can't find an item before a saved game has been loaded.");
                return;
            }

            dataGridViewItems.SuspendLayout();

            if (dataGridViewItems.DataSource != null)
            {
                dataGridViewItems.DataSource = null;
            }

            // Get the list of matching items from the game model and set it as the model for dataGridViewItems
            IList<GameLibrary.ItemID> itemIDs = ItemFamily.GetItemIDs(
                itemFamilyCombo.SelectedItem.ToString(),
                itemSubFamilyCombo.SelectedItem.ToString(),
                itemTypeCombo.SelectedItem.ToString());
            GameLibrary.ItemQuality quality = (GameLibrary.ItemQuality)Enum.Parse(typeof(GameLibrary.ItemQuality), itemQualityCombo.SelectedItem.ToString(), true);
            dataGridViewItems.DataSource = gnomoria.FindItems(itemIDs, quality);

            // Set various properties of the grid view
            dataGridViewItems.AllowUserToAddRows = false;
            dataGridViewItems.AllowUserToDeleteRows = false;

            dataGridViewItems.Columns[0].Visible = false;
            dataGridViewItems.Columns[1].Frozen = true;
            dataGridViewItems.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridViewItems.Sort(dataGridViewItems.Columns[1], ListSortDirection.Ascending); // Sort by Name Asc 

            dataGridViewItems.ResumeLayout();

            // Update result
            findItemResultLabel.Text = "Found " + dataGridViewItems.Rows.Count + " objects.";
        }

        /// <summary>
        /// Update the subfamily and item type combo lists when the family or subfamily selection changes.
        /// </summary>
        /// <param name="reloadSubFamilies">If true, update the list of subfamilies and the list of item types. If false, only update the item types.</param>
        private void UpdateItemFinderCombos(bool reloadSubFamilies)
        {
            if (reloadSubFamilies)
                itemSubFamilyCombo.DataSource = ItemFamily.GetSubFamilies(itemFamilyCombo.SelectedItem.ToString());

            itemTypeCombo.DataSource      = ItemFamily.GetItemNames(itemFamilyCombo.SelectedItem.ToString(), itemSubFamilyCombo.SelectedItem.ToString());
        }

        #endregion

        #region DataGrid
        // DataGrid Methods

        /// <summary>
        /// Called when formatting the dataGridViewCharStats table. 
        /// Does a special handling for attributes and skills, displaying the value over average 
        /// + 1x standard deviation (or 2x) if a different way.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewCharSkills_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Display value over the average + standard deviation in red or bold red.
            if (e.RowIndex >= 0 && e.ColumnIndex >= _skillColStart)
            {
                DataGridViewCell cell = dataGridViewCharSkills.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string tooltip = "";
                string attrName = dataGridViewCharSkills.Columns[e.ColumnIndex].HeaderText;
                double attrValue = Convert.ToDouble(e.Value);
                double avg = gnomoria.getAverageAttribute(attrName);
                double stddev = gnomoria.getStdDevAttribute(attrName);
                Font currentFont = dataGridViewCharSkills.DefaultCellStyle.Font;

                if (attrValue >= (avg + 2 * stddev))
                {
                    e.CellStyle.BackColor = Color.MediumSeaGreen;
                    tooltip = "This value is highly above average (green). ";
                }
                else if (attrValue > (avg + stddev))
                {
                    e.CellStyle.BackColor = Color.MediumAquamarine; // LightGreen;
                    tooltip = "This value is signicantly above average (lightgreen). ";
                }

                if (e.ColumnIndex >= _skillColStart + 7) // Only for skills, not attributes
                {
                    string profName = dataGridViewCharSkills.Rows[e.RowIndex].Cells[2].Value.ToString();
                    if (gnomoria.isSkillUsedByProfession(profName, attrName))
                    {
                        e.CellStyle.Font = new Font(currentFont.FontFamily, currentFont.Size, FontStyle.Bold);
                        tooltip += "This skill is used by the gnome profession (bold font). ";
                    }
                }

                if (tooltip.Length > 0)
                    cell.ToolTipText = tooltip;
            }
        }

        /// <summary>
        /// Called when painting the dataGridViewCharSkills table. 
        /// Does a special handling only for column headers containing skill names, and paint the skill name vertically, adjusting the header height if needed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewCharSkills_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= _skillColStart)
            {
                e.PaintBackground(e.ClipBounds, true);
                Rectangle rect = this.dataGridViewCharSkills.GetColumnDisplayRectangle(e.ColumnIndex, true);
                Size titleSize = TextRenderer.MeasureText(e.Value.ToString(), dataGridViewCharSkillsHeaderFont);
                if (this.dataGridViewCharSkills.ColumnHeadersHeight < titleSize.Width)
                {
                    this.dataGridViewCharSkills.ColumnHeadersHeight = titleSize.Width;
                }

                e.Graphics.TranslateTransform(0, titleSize.Width);
                e.Graphics.RotateTransform(-90.0F);

                // This is the key line for bottom alignment - we adjust the PointF based on the 
                // ColumnHeadersHeight minus the current text width. ColumnHeadersHeight is the
                // maximum of all the columns since we paint cells twice - though this fact
                // may not be true in all usages!   
                e.Graphics.DrawString(e.Value.ToString(), dataGridViewCharSkillsHeaderFont, Brushes.Black, new PointF(rect.Y - (dataGridViewCharSkills.ColumnHeadersHeight - titleSize.Width), rect.X));

                // The old line for comparison
                //e.Graphics.DrawString(e.Value.ToString(), this.Font, Brushes.Black, new PointF(rect.Y, rect.X));


                e.Graphics.RotateTransform(90.0F);
                e.Graphics.TranslateTransform(0, -titleSize.Width);
                e.Handled = true;
            }
        }

        /// <summary>
        /// When the top left cell of the table is clicked, recompute the column header height to make sure all skill names are displayed (vertically) correctly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewCharSkills_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 && e.RowIndex == -1)
            {
                dataGridViewCharSkills.ColumnHeadersHeight = 5;

                for (int cl = _skillColStart; cl < dataGridViewCharSkills.Columns.Count; cl++)
                {
                    int width = TextRenderer.MeasureText(dataGridViewCharSkills.Columns[cl].HeaderCell.Value.ToString(), dataGridViewCharSkillsHeaderFont).Width;

                    dataGridViewCharSkills.Columns[cl].Width = dataGridViewCharSkills.Columns[cl].MinimumWidth = (int)dataGridViewCharSkillsHeaderFont.Size * 3;
                    if (width > dataGridViewCharSkills.ColumnHeadersHeight)
                        dataGridViewCharSkills.ColumnHeadersHeight = width + (int)(dataGridViewCharSkillsHeaderFont.Size * 1.85);

                }
            }
        }

        /// <summary>
        /// Called when painting the dataGridViewCharStats table. 
        /// Does a special handling only for column headers containing skill names, and paint the skill name vertically, adjusting the header height if needed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewCharStats_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= _statColStart)
            {
                // Special handling of the header row (displaying column headers): print header names vertically
                e.PaintBackground(e.ClipBounds, true);
                Rectangle rect = this.dataGridViewCharStats.GetColumnDisplayRectangle(e.ColumnIndex, true);
                Size titleSize = TextRenderer.MeasureText(e.Value.ToString(), dataGridViewCharSkillsHeaderFont);
                if (this.dataGridViewCharStats.ColumnHeadersHeight < titleSize.Width)
                {
                    this.dataGridViewCharStats.ColumnHeadersHeight = titleSize.Width;
                }

                e.Graphics.TranslateTransform(0, titleSize.Width);
                e.Graphics.RotateTransform(-90.0F);

                // This is the key line for bottom alignment - we adjust the PointF based on the 
                // ColumnHeadersHeight minus the current text width. ColumnHeadersHeight is the
                // maximum of all the columns since we paint cells twice - though this fact
                // may not be true in all usages!   
                e.Graphics.DrawString(e.Value.ToString(), dataGridViewCharSkillsHeaderFont, Brushes.Black, new PointF(rect.Y - (dataGridViewCharStats.ColumnHeadersHeight - titleSize.Width), rect.X));

                // The old line for comparison
                //e.Graphics.DrawString(e.Value.ToString(), this.Font, Brushes.Black, new PointF(rect.Y, rect.X));


                e.Graphics.RotateTransform(90.0F);
                e.Graphics.TranslateTransform(0, -titleSize.Width);
                e.Handled = true;
            }
        }

        /// <summary>
        /// When the top left cell of the table is clicked, recompute the column header height to make sure all skill names are displayed (vertically) correctly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewCharStats_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 && e.RowIndex == -1)
            {
                dataGridViewCharStats.ColumnHeadersHeight = 5;

                for (int cl = _statColStart; cl < dataGridViewCharStats.Columns.Count; cl++)
                {
                    int width = TextRenderer.MeasureText(dataGridViewCharStats.Columns[cl].HeaderCell.Value.ToString(), dataGridViewCharSkillsHeaderFont).Width;

                    dataGridViewCharStats.Columns[cl].Width = dataGridViewCharStats.Columns[cl].MinimumWidth = (int)dataGridViewCharSkillsHeaderFont.Size * 3;
                    if (width > dataGridViewCharStats.ColumnHeadersHeight)
                        dataGridViewCharStats.ColumnHeadersHeight = width + (int)(dataGridViewCharSkillsHeaderFont.Size * 1.85);

                }
            }
        }
        #endregion

        #region Initialization Methods

        /// <summary>
        /// Use a background Worker thread to initialize the GnomanEmpire object used to manipulate saved games. 
        /// This method will do nothing is the _configVerified flag is false, unless the forceInit parameter is true.
        /// </summary>        
        private void InitializeGameModel(bool forceInit)
        {
            if (_settingsVerified == false) 
            {
                if (forceInit == false)
                {
                    log(LogLevel.Warning, "Not attempting to initialize the game model because Gnomoria settings verification failed.");
                    return;
                }
                else
                {
                    log(LogLevel.Warning, "Forcing initilization of the game model.");
                }
            }

            if (_gameModelInitialized)
            {
                log(LogLevel.Warning, "Not attempting to initialize the game model because that's already done.");
                return;
            }

            // Setup UI for Initialization
            openToolStripMenuItem.Enabled = false;
            log(LogLevel.Info, "Starting initialization of the game model.");
            this.toolStripStatusLabel.Text = "Initializing...";

            // Initializes Gnomoria in a new thread
            _workerInitialize = new BackgroundWorker();
            _workerInitialize.DoWork += new DoWorkEventHandler(this.Initialize_DoWork);
            _workerInitialize.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.Initialize_RunWorkerCompleted);
            _workerInitialize.RunWorkerAsync();

            // Waits for Gnomoria game model to Initialize before continuing
            while (this._workerInitialize.IsBusy)
            {
                this.toolStripProgressBar.Increment(1);
                Application.DoEvents();
                Thread.Sleep(250);
            }

            // The worker will update the _gameModelInitialized flag
            if (this._gameModelInitialized)
            {
                this.toolStripProgressBar.Value = 0;
                this.toolStripStatusLabel.Text = "Please load a game to start";
            }

            ResetMenuItems();
        }

        /// <summary>
        /// Verify if the Gnomoria settings are compatible with the execution of the GE program.
        /// Will set (or reset) the _configVerified flag and update the UI accordingly.
        /// </summary>
        private void VerifyGnomoriaSettings()
        {
            // Reset status flag
            this._settingsVerified = false;
            this.toolStripStatusLabel.Text = "Checking Gnomoria settings...";

            // Open Gnomoria configuration file in My Documents\My Games\Gnomoria\settings.ini
            String settingsFilePath = Microsoft.VisualBasic.FileIO.SpecialDirectories.MyDocuments + "\\My Games\\Gnomoria\\settings.ini";

            // If the config file can't be found, return an failed status.
            if (File.Exists(settingsFilePath) == false)
            {
                lblTabOverviewVerifConfigValue.Text = "Cannot find Gnomoria settings file.";
                log(LogLevel.Error, "Cannot find Gnomoria settings file " + settingsFilePath);
                return;
            }

            // Verify the "Full Screen Mode" setting. If enabled, Gnomoria Enhanced displays a blank screen when initializing the GnomanEmpire class.
            IniFile settingsFile = new IniFile(settingsFilePath);
            String mode = settingsFile.IniReadValue("Display", "FullScreenMode");
            if (mode.Equals("0") == false)
            {
                log(LogLevel.Warning, "It is advised to configure Gnomoria in Windowed mode.");
                log(LogLevel.Warning, "When in Full Screen Mode, the screen will turn blank when initializing the game model.");
                // This is a warning only, continue to test other settings.
            }

            // At this point, everything is fine, set status variable and label text.
            log(LogLevel.Info, "Checked Gnomoria settings in " + settingsFilePath + ", OK.");

            this._settingsVerified = true;
            this.lblTabOverviewVerifConfigValue.Text = "Settings OK!";
            this.toolStripStatusLabel.Text = "Gnomoria settings OK!";
            ResetMenuItems();
        }

        #endregion

        #region AutoBackup

        /// <summary>
        /// 
        /// </summary>
        private void InitializeSavedGameWatcher()
        {
            // Setup the saved game watcher.
            if (_gameModelInitialized)
            {
                if (_savedGameWatcher == null)
                {
                    _savedGameWatcher = new FileSystemWatcher();
                    _savedGameWatcher.SynchronizingObject = this;
                    _savedGameWatcher.Path = gnomoria.getSavedGameFolder();
                    _savedGameWatcher.Filter = "*.sav";
                    _savedGameWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.CreationTime;
                    _savedGameWatcher.Changed += new FileSystemEventHandler(savedGame_Modified);
                    _savedGameWatcher.Created += new FileSystemEventHandler(savedGame_Modified);
                    _savedGameWatcher.Deleted += new FileSystemEventHandler(savedGame_Modified);
                    _savedGameWatcher.Renamed += new RenamedEventHandler(savedGame_Renamed);
                    _savedGameWatcher.EnableRaisingEvents = true;
                    log(LogLevel.Info, "[AutoBackup] Saved game watcher running.");
                }
                else
                {
                    log(LogLevel.Debug, "[AutoBackup] Saved game watcher already running, skipped.");
                }
            }
            else
            {
                log(LogLevel.Warning, "[AutoBackup] Cannot initialize the saved game watcher because the game model is not initialized yet.");
            }

            // Reset the auto backup icon if it was not possible to initialize the saved game watcher.
            autoBackupSavedGamesToolStripMenuItem.Checked = (_savedGameWatcher != null);
        }

        private void savedGame_Modified(object sender, FileSystemEventArgs e)
        {
            log(LogLevel.Debug, "[AutoBackup] File: " + e.FullPath + " " + e.ChangeType);
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                if (optBackup)
                {
                    log(LogLevel.Info, "[AutoBackup] File: " + e.Name + " has been Changed (most likely a new save)");
                    string dir = e.Name.Split('.')[0];
                    string worldDir = _gnomoriaSaveBackupFolder + @"\" + dir;
                    Directory.CreateDirectory(worldDir);

                    DateTime dt = DateTime.Now;
                    string filename = String.Format("{0:yyyyMMddHHmmss}", dt);
                    filename = filename + ".sav";
                    
                    log(LogLevel.Debug, "[AutoBackup] Copying Save from: " + e.FullPath + " to: " + worldDir + "\\" + filename);
                    System.IO.File.Copy(e.FullPath, worldDir + "\\" + filename, true);
                    log(LogLevel.Info, "[AutoBackup] Backup complete");
                }
                else
                {
                    log(LogLevel.Debug, "[AutoBackup] Ignoring saved game update because auto backup is disabled.");
                }
            }
        }

        private void savedGame_Renamed(object sender, RenamedEventArgs e)
        {
            log(LogLevel.Debug, "[AutoBackup] File: " + e.OldFullPath + " renamed to " + e.FullPath);
        }

        #endregion

        #region Log methods
        /// <summary>
        /// Create and initialize the ListBoxLog component and add it to listBox1.
        /// </summary>
        private void Load_Debug()
        {
            Console.WriteLine("DEBUG MODE INITIALIZING");

            listBoxLog = new ListBoxLog(listBox1, "{4} [{5}] : {8}", optLogging);

            if (optLogging)
            {
                // this.tabControl.Controls.Add(this.tabLog);
                this.logToolStripMenuItem.Checked = true;
                log(LogLevel.Info, "DEBUG MODE ENABLED");
            }
        }

        /// <summary>
        /// Log a message into the log list box (which might be visible or not).
        /// </summary>
        /// <param name="level">One of the LogLevel values.</param>
        /// <param name="message">The message to display.</param>
        private void log(GnomoriaEnhanced.UI.LogLevel level, string message)
        {
            listBoxLog.Log(level, message);
        }

        #endregion // Log methods

        #region Exit
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            log(LogLevel.Info,"Saving defaults...");
            Properties.Settings.Default.AutoLoadSave = optAutoLoadSavedGame;
            Properties.Settings.Default.AutoLoadSaveGame = optAutoLoadSavedGamePath;
            Properties.Settings.Default.Backup = optBackup;
            Properties.Settings.Default.Logging = optLogging;
            Properties.Settings.Default.Save();
            log(LogLevel.Info, "Done saving defaults");
        }
        #endregion

    }
}
