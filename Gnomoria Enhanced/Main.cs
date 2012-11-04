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

        // Variables
        private bool optLogging;
        private bool optBackup;
        private bool optAutoLoadSave;
        private string optAutoLoadSaveGame;

        private int _skillColStart = 4;
        private int _statColStart = 2;

        private Gnomoria gnomoria;
        private string saveFolderPath;
        private string _gnomoriaBaseFolder;
        private string _gnomoriaSaveBackupFolder;
        private Font dataGridViewCharSkillsHeaderFont;
        private bool _loadGameFailed = false;
        private string _errorMessage;
        private string _loadedSaveGame;
        
        // Log Tab
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.ListBox lbLog;
        private ListBoxLog listBoxLog;

        // Background workers
        private BackgroundWorker _workerInitialize;
        private BackgroundWorker _workerLoadGame;
        private FileSystemWatcher _FSWatcher = new FileSystemWatcher();

        #endregion

        #region Initialization
        // Constructor
        public Main()
        {
            optLogging = Properties.Settings.Default.Logging;
            Console.WriteLine("Option Logging: {0}", optLogging);
            optBackup = Properties.Settings.Default.Backup;
            Console.WriteLine("Option Backup: {0}", optBackup);
            optAutoLoadSave = Properties.Settings.Default.AutoLoadSave;
            Console.WriteLine("Option AutoLoadSave: {0}", optAutoLoadSave);
            optAutoLoadSaveGame = Properties.Settings.Default.AutoLoadSaveGame;

            InitializeComponent();     

            // Not Ready so don't show
            lblTabOverviewDate.Visible = false;
            lblTabOverviewKingdomName.Visible = false;
            lblTabOverviewTotalWorth.Visible = false;
        }

        public void Main_Load(object sender, System.EventArgs e)
        {
            // Loads Log Tab
            Load_Debug();

            // Setup up Data View Headers
            dataGridViewCharSkillsHeaderFont = new Font(new FontFamily("Arial"), 11);

            typeof(DataGridView).InvokeMember(
                "DoubleBuffered", 
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, 
                dataGridViewCharSkills, 
                new object[] { true });

            // Setup UI for Initialization
            openToolStripMenuItem.Enabled = false;
            log(LogLevel.Info, "Starting Initialization");
            this.toolStripStatusLabel.Text = "Initializing...";

            // Initializes Gnomoria in a new thread
            _workerInitialize = new BackgroundWorker();
            _workerInitialize.DoWork += new DoWorkEventHandler(this.Initialize_DoWork);
            _workerInitialize.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.Initialize_RunWorkerCompleted);
            _workerInitialize.RunWorkerAsync();

            // Waits for Gnomoria to Initialize before continuing
            while (this._workerInitialize.IsBusy)
            {
                this.toolStripProgressBar.Increment(1);
                Application.DoEvents();
                Thread.Sleep(250);
            }
            log(LogLevel.Info, "Initialization Complete");
        }

        public void Main_Shown(object sender, System.EventArgs e)
        {

            if (optAutoLoadSave && optAutoLoadSaveGame != null)
            {
                log(LogLevel.Info, "Auto loading world: " + optAutoLoadSaveGame);
                openToolStripMenuItem.Enabled = false;
                LoadGame(optAutoLoadSaveGame);
            }

            // Setup _FSWatcher
            _FSWatcher.SynchronizingObject = this;
            _FSWatcher.Path = gnomoria.getSaveGameFolder();
            _FSWatcher.Filter = "*.sav";
            _FSWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.CreationTime;
            _FSWatcher.Changed += new FileSystemEventHandler(saveGame_Modified);
            _FSWatcher.Created += new FileSystemEventHandler(saveGame_Modified);
            _FSWatcher.Deleted += new FileSystemEventHandler(saveGame_Modified);
            _FSWatcher.Renamed += new RenamedEventHandler(saveGame_Renamed);
            _FSWatcher.EnableRaisingEvents = true;

            
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
                _loadedSaveGame = openDlg.SafeFileName;
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
                this.tabControl.Controls.Add(this.tabLog);
            }
            else
            {
                a = "Disabled";
                this.tabControl.Controls.Remove(this.tabLog);
            }

            log(LogLevel.Debug,"Log mode has been changed to: " + a);
        }

        private void autoBackupSavedGamesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            string a;
            if (autoBackupSavedGamesToolStripMenuItem.Checked)
            {
                a = "Enabled";
                optBackup = true;
            }
            else
            {
                a = "Disabled";
                optBackup = false;
            }

            log(LogLevel.Debug, "Auto backup mode has been " + a);
        }
        #endregion

        #region Button Clicks
        // Button Click
        private void btnChatStatsHelp_Click(object sender, EventArgs e)
        {

        }
        #endregion        

        #region Background Workers
        // Background Workers

        // Initialize Backgroundworker

        private void Initialize_DoWork(object sender, DoWorkEventArgs e)
        {
            gnomoria = new Gnomoria();            
            Result res = gnomoria.Initialize();
            if (res.Success == false)
            {
                log(LogLevel.Error,"[Main] gnomoria.Initialize Failed with error: " + res.ErrorMessage);
            }
            saveFolderPath = gnomoria.getSaveGameFolder();
            _gnomoriaBaseFolder = gnomoria.getSettingsFolder();
            Directory.CreateDirectory(_gnomoriaBaseFolder + @"BackupWorlds");
            _gnomoriaSaveBackupFolder = _gnomoriaBaseFolder + @"BackupWorlds";
            log(LogLevel.Debug, "Ensured that backup folder (" + _gnomoriaSaveBackupFolder + ") exists");
        }

        private void Initialize_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBar.Value = 100;
            if (e.Error == null)
            {
                log(LogLevel.Info,"gnomoria.Initialize Completed without incident");
            }
            this.toolStripProgressBar.Value = 0;
            this.toolStripStatusLabel.Text = "Please load a game to start";
            this.openToolStripMenuItem.Enabled = true;
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
                _loadGameFailed = true;
                _errorMessage = res.ErrorMessage;
            }
            else
            {
                bw.ReportProgress(0,"Parsing Character Skills...");
                Result res2 = gnomoria.Load_Character_Skills(_skillColStart);
                if (res2.Success == false)
                {
                    log(LogLevel.Error,"[Main] LoadGame_DoWork failed at gnomoria.Load_Character_Skills with message: " + res2.ErrorMessage);
                    e.Result = "Failed: " + res2.ErrorMessage;
                    _loadGameFailed = true;
                    _errorMessage = res2.ErrorMessage;
                }
                else
                {
                    bw.ReportProgress(0, "Parsing Character Stats...");
                    Result res3 = gnomoria.Load_Character_Stats();
                    if (res3.Success == false)
                    {
                        log(LogLevel.Error, "[Main] LoadGame_DoWork failed at gnomoria.Load_Character_Stats with message: " + res3.ErrorMessage);
                        e.Result = "Failed: " + res3.ErrorMessage;
                        _loadGameFailed = true;
                        _errorMessage = res3.ErrorMessage;
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

        #region Tabs
        // Tab Loading

        public void tabCharSkills_Load()
        {
            this.tabCharSkills.Show();

            dataGridViewCharSkills.SuspendLayout();

            // Load has completed successfully display information
            dataGridViewCharSkills.DataSource = gnomoria.getCharSkills();
            dataGridViewCharSkills.AllowUserToAddRows = false;
            dataGridViewCharSkills.AllowUserToDeleteRows = false;

            dataGridViewCharSkills.Columns[0].Visible = false;
            dataGridViewCharSkills.Columns[_skillColStart].Visible = false;
            dataGridViewCharSkills.Columns[1].Frozen = true;
            dataGridViewCharSkills.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCharSkills.Columns[2].Frozen = true;
            dataGridViewCharSkills.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //dataGridViewCharSkills.Columns[3].Frozen = true;
            //dataGridViewCharSkills.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridViewCharSkills.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dataGridViewCharSkills.Sort(dataGridViewCharSkills.Columns[1], ListSortDirection.Ascending); // Sort by Name Asc 

            this.dataGridViewCharSkills_CellClick(null, new DataGridViewCellEventArgs(-1, -1));

            dataGridViewCharSkills.ResumeLayout();

            tabCharSkills.Show();
        }

        public void tabCharStats_Load()
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

            dataGridViewCharStats.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dataGridViewCharStats.Sort(dataGridViewCharStats.Columns[1], ListSortDirection.Ascending); // Sort by Name Asc 

            this.dataGridViewCharStats_CellClick(null, new DataGridViewCellEventArgs(-1, -1));

            dataGridViewCharStats.ResumeLayout();

            //tabCharStats.Show();

        }

        public void tabPageOverview_Load()
        {
            //Dictionary<string, string> overview = gnomoria.getKingdomOverview();

            //lblTabOverviewKingdomNameValue.Text = overview["KingdomName"];
            //lblTabOverviewTotalWorth.Text = overview["TotalWorth"];
            //lblTabOverviewDateValue.Text = overview["GameDate"];
            lblTabOverviewKingdomNameValue.Text = "Loaded Game: " + _loadedSaveGame;
        }
        #endregion

        #region DataGrid
        // DataGrid Methods
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

        private void dataGridViewCharStats_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= _statColStart)
            {
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

        #region Random Methods
        // Random Methods

        private void LoadGame(string saveGame)
        {
            log(LogLevel.Info, "Loading Game: " + saveGame);
            this.toolStripStatusLabel.Text = "Loading Game: " + saveGame;
            this.toolStripProgressBar.Value = 0;

            // Clear up old save game
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
            _workerLoadGame.RunWorkerAsync(saveGame);

            while (this._workerLoadGame.IsBusy)
            {
                this.toolStripProgressBar.Increment(1);
                Application.DoEvents();
                Thread.Sleep(250);
            }

            if (!_loadGameFailed)
            {
                tabPageOverview_Load();
                tabCharStats_Load();
                tabCharSkills_Load();
                
                log(LogLevel.Info, "Game Loaded");
                this.tabControl.SelectedTab = tabCharSkills;
            }
            else
            {
                // Loading the game failed - Display error message and exit program
                // TODO: Figure out how to fix OutOfMemoryException
                Console.WriteLine("ERROR: {0}", _errorMessage);

                string errorMsg;

                if (_errorMessage == "Exception of type 'System.OutOfMemoryException' was thrown.")
                {
                    errorMsg = "Loading saved game failed with the following error:\n" + _errorMessage
                        + "\n\n"
                        + "This is usually caused by editing the game and enlarging the ore";
                }
                else
                {
                    errorMsg = "Loading saved game failed with the following error:\n" + _errorMessage;
                }

                MessageBox.Show(errorMsg,
                        "Error loading saved game",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1);

                Application.Exit();
            }
        }

        private void Load_Debug()
        {
            Console.WriteLine("DEBUG MODE INITIALIZING");
            this.tabLog = new System.Windows.Forms.TabPage();
            this.lbLog = new System.Windows.Forms.ListBox();

            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.lbLog);
            this.tabLog.Name = "tabLog";
            this.tabLog.Text = "Log";
            this.tabLog.UseVisualStyleBackColor = true;

            // 
            // lbLog
            // 
            this.lbLog.BackColor = System.Drawing.SystemColors.Window;
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Name = "lbLog";
            this.lbLog.TabStop = false;
            this.lbLog.Dock = System.Windows.Forms.DockStyle.Fill;

            listBoxLog = new ListBoxLog(lbLog, "{4} [{5}] : {8}", optLogging);


            if (optLogging)
            {
                // this.tabControl.Controls.Add(this.tabLog);
                this.logToolStripMenuItem.Checked = true;
                log(LogLevel.Info, "DEBUG MODE ENABLED");
            }
        }

        private void log(GnomoriaEnhanced.UI.LogLevel level, string message)
        {
            listBoxLog.Log(level, message);
        }
        #endregion

        #region FSWatcher
        private void saveGame_Modified(object sender, FileSystemEventArgs e)
        {
            log(LogLevel.Debug, "[FSWatcher] File: " + e.FullPath + " " + e.ChangeType);
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                log(LogLevel.Info, "[FSWatcher] File: " + e.Name + " has been Changed (most likely a new save)");
                if (optBackup)
                {
                    log(LogLevel.Debug, "[FSWatcher] Save Game: " + e.Name);
                    string dir = e.Name.Split('.')[0];
                    string worldDir = _gnomoriaSaveBackupFolder + @"\" + dir;
                    Directory.CreateDirectory(worldDir);

                    DateTime dt = DateTime.Now;
                    string filename = String.Format("{0:yyyyMMddHHmmss}", dt);
                    filename = filename + ".sav";
                    log(LogLevel.Debug, "[FSWatcher] File Name: " + filename);

                    log(LogLevel.Debug, "[FSWatcher] Copying Save from: " + e.FullPath + " to: " + worldDir + "\\" + filename);
                    System.IO.File.Copy(e.FullPath, worldDir + "\\" + filename, true);
                    log(LogLevel.Info, "[FSWatcher] Backup complete");
                }
                else
                {
                    log(LogLevel.Info, "[FSWatcher] Not backing up saved game because auto backup has not been enabled");
                }
            }
        }

        private void saveGame_Renamed(object sender, RenamedEventArgs e)
        {
            log(LogLevel.Debug, "[FSWatcher] File: " + e.OldFullPath + " renamed to " + e.FullPath);

        }

        #endregion

        #region Exit
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            log(LogLevel.Info,"Saving defaults...");
            Properties.Settings.Default.AutoLoadSave = optAutoLoadSave;
            Properties.Settings.Default.AutoLoadSaveGame = optAutoLoadSaveGame;
            Properties.Settings.Default.Backup = optBackup;
            Properties.Settings.Default.Logging = optLogging;
            Properties.Settings.Default.Save();
            log(LogLevel.Info, "Done saving defaults");
        }
        #endregion

        
    }
}
