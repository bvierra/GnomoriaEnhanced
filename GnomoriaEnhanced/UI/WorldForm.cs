using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using Game;
using GameLibrary;
using SevenZip;

using GnomoriaEnhanced.Exporter;

namespace GnomoriaEnhanced.UI
{
    public partial class WorldForm : Form
    {
        #region Variables
        private GnomoriaEnhanced.Main _parent;
        private GnomoriaEnhanced.Gnomoria _gnomoria;
        private GnomanEmpire _gnomanEmpire;
        private Dictionary<string, List<string>> menuGroups;
        private bool _isDirty;
        private string _saveFile;           

        // Background workers
        private BackgroundWorker _worker;

        // Professions
        private List<CharacterSkillType> _professionSkillList;

        // Other Forms
        private World.ProfessionsModifyForm _professionsModifyForm;
        private World.GnomesSkills _gnomesSkills;
        #endregion

        #region WorldForm
        public WorldForm(GnomoriaEnhanced.Main parent)
        {
            _parent = parent;
            _gnomoria = _parent._gnomoria;
            this.MemoryUsage();
            this.Visible = false;
            InitializeComponent();
            this.ComboBoxMenuGroup_Initialize();
            _worker = new BackgroundWorker();
            _worker.ProgressChanged += new ProgressChangedEventHandler(this._worker_ProgressChanged);
            _worker.WorkerReportsProgress = true;
            
        }

        private void WorldForm_Shown(object sender, EventArgs e)
        {
            DateTime Start = DateTime.Now;
            this.LoadGame();
            TimeSpan Elapsed = DateTime.Now - Start;
            _parent.Debug(String.Format("Loading game took: {0}ms", Elapsed));
            this.comboBoxMenuGroup.Enabled = true;
            this.lbMenuItem.Enabled = true;
        }

        private void ComboBoxMenuGroup_Initialize()
        {
            menuGroups = new Dictionary<string, List<string>>();
            // Gnomes (index 0)
            List<string> gnomesList = new List<string>();
            gnomesList.Add("v-- Select an option --v");
            gnomesList.Add("Skills");
            gnomesList.Add("Stats");
            menuGroups.Add("Gnomes", gnomesList);

            // Professions (index 1)
            List<string> professionsList = new List<string>();
            professionsList.Add("v-- Select an option --v");
            professionsList.Add("Modify");
            professionsList.Add("Export");
            professionsList.Add("Import");
            menuGroups.Add("Professions", professionsList);

            foreach (var group in menuGroups)
            {
                _parent.Debug(String.Format("Menu Group Initialization: Added Key {0}", group.Key));
                comboBoxMenuGroup.Items.Add(group.Key);
            }
            

        }

        private void ComboBoxMenuGroup_SelectedChangeCommitted(object sender, EventArgs e)
        {
            _parent.Debug(String.Format("Menu Group Index Selected: {0}", comboBoxMenuGroup.SelectedIndex));
            switch (comboBoxMenuGroup.SelectedIndex)
            {
                case 0:
                    // Gnomes
                    lbMenuItem.DataSource = menuGroups["Gnomes"];
                    lbMenuItem.Enabled = true;
                    break;
                case 1:
                    // Professions
                    lbMenuItem.DataSource = menuGroups["Professions"];
                    lbMenuItem.Enabled = true;
                    break;
                default:
                    _parent.Warn("MenuGroup invalid index selected... Really? How did you do this, and why?");
                    break;
            }
        }

        private void lbMenuItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            _parent.Debug(String.Format("Menu Item Index Selected: {0}", lbMenuItem.SelectedIndex));
            if (comboBoxMenuGroup.SelectedIndex == 0)
            {
                // Gnomes
                switch (lbMenuItem.SelectedIndex)
                {
                    case 0:

                        break;
                    case 1:
                        _parent.Info("Professions Modify selected");
                        this.GnomesSkills();
                        break;
                    default:

                        break;

                }
            }
            else if (comboBoxMenuGroup.SelectedIndex == 1)
            {
                // Professions
                switch (lbMenuItem.SelectedIndex)
                {
                    case 0:

                        break;
                    case 1:
                        _parent.Info("Professions Modify selected");
                        this.ProfessionModify();
                        break;
                    case 2:
                        _parent.Info("Professions Export selected");
                        this.ProfessionExport();
                        break;
                    case 3:
                        _parent.Info("Professions Import selected");
                        this.ProfessionImport();
                        break;
                    default:

                        break;
                }
            }
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (_saveFile == null)
            {
            }
            else if (_isDirty)
            {
                DialogResult close = MessageBox.Show("Are you sure you want to save this game and close this Kingdom?",
                    "Save and Close Kingdom?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                if (close == DialogResult.Yes)
                {
                    // Save Game
                    SaveGame();
                }
                else if (close == DialogResult.No)
                {
                    _parent.Info("Closing kingdom without saving");
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                DialogResult close = MessageBox.Show("Are you sure you want to close this Kingdom?",
                    "Close Kingdom?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (close == DialogResult.No)
                    e.Cancel = true;
            }
        }
        #endregion

        #region Skills Group
        private void GnomesSkills()
        {
            if (_gnomesSkills == null)
            {
                _gnomesSkills = new World.GnomesSkills(this);
            }

            _gnomesSkills.Show();
        }
        #endregion

        #region Profession Group
        private void ProfessionModify()
        {
            if (_professionsModifyForm != null && _professionsModifyForm.NeedReload)
            {
                _parent.Debug("Professions Modify Form needs reload, reloading now");
                _professionsModifyForm.Close();
                _professionsModifyForm = new World.ProfessionsModifyForm(this);
            }
            else if (_professionsModifyForm == null)
            {
                _professionsModifyForm = new World.ProfessionsModifyForm(this);
            }

            _professionsModifyForm.Show();
        }

        private void ProfessionExport()
        {
            _parent.Debug(String.Format("Profession Export Path: {0}", GnomanEmpire.SaveFolderPath(@"Addons\Gnomoria Enhanced\data\Prefessions\")));
            Directory.CreateDirectory(GnomanEmpire.SaveFolderPath(@"Addons\Gnomoria Enhanced\data\Professions\"));
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.InitialDirectory = GnomanEmpire.SaveFolderPath(@"Addons\Gnomoria Enhanced\data\Professions\");
            saveDlg.Filter = "Gnomoria Profession Exports (*.gpe)|*.gpe";

            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                _parent.Debug(String.Format("Profession Export to: {0}", saveDlg.FileName));
                FileStream fileStream = null;
                try
                {
                    SevenZipCompressor sevenZipCompressor = new SevenZipCompressor();
                    sevenZipCompressor.CompressionMethod = CompressionMethod.Lzma2;
                    sevenZipCompressor.CompressionLevel = CompressionLevel.Fast;
                    fileStream = new FileStream(saveDlg.FileName + ".temp", FileMode.Create, FileAccess.Write);
                    using (FileStream fileStream2 = new FileStream(saveDlg.FileName + ".data", FileMode.Create, FileAccess.Write))
                    {
                        BinaryWriter writer = new BinaryWriter(fileStream2);
                        writer.Write(_gnomanEmpire.Fortress.Professions.Count);
                        foreach (Profession current in _gnomanEmpire.Fortress.Professions)
                        {
                            current.Serialize(writer);
                        }
                        fileStream2.Flush();
                        sevenZipCompressor.CompressFiles(fileStream, new string[]
			            {
				            saveDlg.FileName + ".data"
			            });
                        fileStream.Flush();
                    }
                    fileStream.Close();
                    File.Delete(saveDlg.FileName);
                    File.Move(saveDlg.FileName + ".temp", saveDlg.FileName);

                }
                catch (Exception ex)
                {
                    _parent.Warn(String.Format("Error could not save professions: {0}", ex.Message));
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                    }
                    if (File.Exists(saveDlg.FileName + ".temp"))
                    {
                        File.Delete(saveDlg.FileName + ".temp");
                    }
                    File.Delete(saveDlg.FileName + ".data");
                    _parent.Info("Exported Professions");
                    
                }
                
            }
        }

        private void ProfessionImport()
        {
            DialogResult response = MessageBox.Show("Importing a profession list will overwrite all professions in the saved game. Are you sure you want to do this?",
                    "Continue Import?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
            if (response == DialogResult.No)
                return;

            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.InitialDirectory = GnomanEmpire.SaveFolderPath(@"Addons\Gnomoria Enhanced\data\Professions\");
            openDlg.Filter = "Gnomoria Profession File (*.gpe)|*.gpe";

            if (openDlg.ShowDialog() != DialogResult.OK)
                return;

            _parent.Debug("Removing all professions from kingdom");
            int num2 = _gnomanEmpire.Fortress.Professions.Count;
            _parent.Debug(String.Format("Professions Left: {0}", _gnomanEmpire.Fortress.Professions.Count));
            while (num2 >= 0)
            {
                _parent.Debug(String.Format(" * Removing profession: {0}",num2));
                _gnomanEmpire.Fortress.RemoveProfession(num2);
                num2--;
            }
            _parent.Debug(String.Format("Professions Left: {0}", _gnomanEmpire.Fortress.Professions.Count));

            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(openDlg.FileName, FileMode.Open, FileAccess.Read);
                using (FileStream fileStream2 = new FileStream(openDlg.FileName + ".temp", FileMode.Create, FileAccess.ReadWrite))
                {
                    SevenZipExtractor sevenZipExtractor = new SevenZipExtractor(fileStream);
                    sevenZipExtractor.ExtractFile(0, fileStream2);
                    fileStream2.Position = 0L;
                    BinaryReader reader = new BinaryReader(fileStream2);
                    int num = reader.ReadInt32();
                    _parent.Debug(String.Format(" * Importing {0} professions", num));
                    for (int i = 0; i < num; i++)
                    {
                        _gnomanEmpire.Fortress.AddProfession(new Profession(reader));
                    }
                }
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
            File.Delete(openDlg.FileName + "temp");
            statusLabel.Text = "Professions Imported!";
            if (_professionsModifyForm != null)
                _professionsModifyForm.NeedReload = true;
          

            
        }

        private void ProfessionSetDirty()
        {
            if (_professionsModifyForm != null)
            {
                _professionsModifyForm.NeedReload = true;
            }
        }

        #endregion

        #region SaveGame (with Background Worker methods)
        private void SaveGame()
        {
            _worker.DoWork += new DoWorkEventHandler(this.SaveGame_DoWork);
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.SaveGame_RunWorkerCompleted);
            statusLabel.Text = String.Format("Saving {0}...", _saveFile);
            _worker.RunWorkerAsync();

            while (this._worker.IsBusy)
            {
                this.progressBar.Increment(1);
                Application.DoEvents();
                Thread.Sleep(250);
            }
        }

        private void SaveGame_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            try
            {
                string worldName = _saveFile.Split('.')[0];
                _gnomoria.BackupWorld(_saveFile, _gnomoria.GetGnomoriaSaveBackupPath() + worldName);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception while backing up old save: " + ex.Message);
                e.Result = "Caught exception while backing up old save: " + ex.Message;
                DialogResult error = MessageBox.Show("Error saving game!\nException:\n" + e.Result,
                    "Could not save game!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                e.Cancel = true;
            }

            try
            {
                _gnomanEmpire.SaveGame();
                while (System.IO.File.Exists(_gnomoria.GetGnomoriaSavePath() + "data"))
                {
                    Thread.Sleep(250);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception while saving game: " + ex.Message);
                e.Result = "Caught exception while backing up old save: " + ex.Message;
                DialogResult error = MessageBox.Show("Error saving game!\nException:\n" + e.Result,
                    "Could not save game!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                e.Cancel = true;
            }

        }

        private void SaveGame_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            progressBar.Value = 0;
            if (e.Cancelled)
            {
            }
            else
            {
                Console.WriteLine("Save game completed");
                statusLabel.Text = "Game Saved!";
            }

            this._worker.DoWork -= new DoWorkEventHandler(this.SaveGame_DoWork);
            this._worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(this.SaveGame_RunWorkerCompleted);
            this.MemoryUsage();
        }
        #endregion

        #region LoadGame (with Background Worker methods)
        private void LoadGame()
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.InitialDirectory = _parent._gnomoria.GetGnomoriaSavePath();
            openDlg.Filter = "Gnomoria Save Files (*.sav)|world*.sav";
           
            _worker.DoWork += new DoWorkEventHandler(this.LoadGame_DoWork);
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.LoadGame_RunWorkerCompleted);
            

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                this.Visible = true;
                _parent.Info("Opening Game: " + openDlg.SafeFileName.ToString());
                this._saveFile = openDlg.SafeFileName.ToString();
                statusLabel.Text = String.Format("Loading {0}...", openDlg.SafeFileName);
                _worker.RunWorkerAsync(openDlg.SafeFileName);

                while (this._worker.IsBusy)
                {
                    this.progressBar.Increment(1);
                    Application.DoEvents();
                    Thread.Sleep(250);
                }

                _parent.Debug("Loading skill definitions...");
                _professionSkillList = SkillDef.AllLaborSkills();

                this.lblKingdomName.Text = _gnomanEmpire.World.AIDirector.PlayerFaction.Name;
                this.Text = String.Format("Gnomoria Enhanced (Kingdom: {0})", _gnomanEmpire.World.AIDirector.PlayerFaction.Name);
                comboBoxMenuGroup.SelectedIndex = 0;
                this.MemoryUsage();
            }
            else
            {
                this.Close();
            }
        }

        private void LoadGame_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            try
            {
                _gnomanEmpire = _gnomoria.LoadGame(e.Argument.ToString());
            }
            catch (Exception ex)
            {
                bw.ReportProgress(100, String.Format("LoadGame failed with exception: {0}", ex.Message));
                e.Result = String.Format("LoadGame failed with exception: {0}", ex.Message);
            }
        }

        private void LoadGame_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar.Value = 0;
            this._worker.DoWork -= new DoWorkEventHandler(this.LoadGame_DoWork);
            this._worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(this.LoadGame_RunWorkerCompleted);
            if (e.Result != null)
            {
                MessageBox.Show(e.Result.ToString());
                _parent.Error(String.Format("LoadGame failed with exception: {0}", e.Result));
                this.Close();
            }
            statusLabel.Text = "Game Loaded!";
            this._worker.DoWork -= new DoWorkEventHandler(this.LoadGame_DoWork);
            this._worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(this.LoadGame_RunWorkerCompleted);
        }
        #endregion

        #region General Methods
        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
            this.statusLabel.Text = e.UserState.ToString();
        }
        #endregion

        #region Debug / Testing Methods
        private void MemoryUsage()
        {
            long usage = GC.GetTotalMemory(true);
            usage = (usage / 1024L) / 1024L;
            Console.WriteLine("Total Memory: {0:0.00}mb", usage);
        }
        #endregion

        #region Accessors
        public GnomanEmpire gnomanEmpire
        {
            get { return _gnomanEmpire; }
        }

        public bool isDirty
        {
            get { return _isDirty; }
            set { _isDirty = true; }

        }

        public GnomoriaEnhanced.Main parent
        {
            get { return _parent; }
        }
        #endregion

    }

}