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

using Game;
using GameLibrary;

namespace GnomoriaEnhanced
{
    public partial class Main : Form
    {
        #region Variables
        // Options
        private bool optBackupSaves = false;
        private bool optShowLogWindow = true;

        // Forms
        private UI.Logging _loggingForm;
        private UI.WorldForm _worldForm;

        //
        public Gnomoria _gnomoria;
        
        #endregion

        #region Initializer
        public Main(UI.Logging loggingForm)
        {
            _loggingForm = loggingForm;
            if (optShowLogWindow)
            {
                Console.WriteLine("Showing Logging Window...");
                _loggingForm.Show();
            }
            InitializeComponent();
            _gnomoria = new Gnomoria(this);            
            GetVersions();
            _gnomoria.Invoke();
        }
        #endregion

        #region Controls
        private void cbLog_CheckStateChanged(object sender, EventArgs e)
        {
            if (_loggingForm.Visible == false)
            {
                _loggingForm.Show();
            }
            else
            {
                _loggingForm.Hide();
            }
        }

        private void cbBackupSaves_Click(object sender, EventArgs e)
        {
            if (optBackupSaves)
            {
                optBackupSaves = false;
                Info("Backup Saves has been disabled");
                cbBackupSaves.Checked = false;
            }
            else
            {
                optBackupSaves = true;
                Info("Backup Saves has been enabled");
                cbBackupSaves.Checked = true;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Info("Launching Gnomoria");
            
        }

        private void btnLoadGame_Click(object sender, EventArgs e)
        {
            if (_worldForm == null || _worldForm.IsDisposed)
            {
                _worldForm = new UI.WorldForm(this);
            }
            _worldForm.Show();
            
        }

        #endregion

        #region Logging
        public void Debug(string msg)
        {
            _loggingForm.Debug(msg);
        }

        public void Info(string msg)
        {
            _loggingForm.Info(msg);
        }

        public void Warn(string msg)
        {
            _loggingForm.Warn(msg);
        }

        public void Error(string msg)
        {
            _loggingForm.Error(msg);
        }

        public void Fatal(string msg)
        {
            _loggingForm.Fatal(msg);
        }
        #endregion

        #region Version Information
        private void GetVersions()
        {
            Debug("Getting Gnomoria Version");
            try
            {
                int[] gnomoriaVersion = _gnomoria.GetInstalledVersion();
                // TODO: Check Gnomoria Latest Version
                string version = "v";
                version += Convert.ToString(gnomoriaVersion[0]);
                version += "." + Convert.ToString(gnomoriaVersion[1]);
                version += "." + Convert.ToString(gnomoriaVersion[2]);
                version += "." + Convert.ToString(gnomoriaVersion[3]);
                lblGnomoriaVersion.Text = version;
                lblGnomoriaUpdateType.Text = "(BMT)";
            }
            catch (GEException.NotInstalledException)
            {
                Warn("Gnomoria Installation could not be found!");
                btnPlay.Enabled = false;
                lblGnomoriaVersion.Text = "Unknown";
                lblGnomoriaUpdateType.Text = "";
            }
            catch (SystemException e)
            {
                Error("Getting Version for Gnomoria failed with Exception: " + e.Message);
                btnPlay.Enabled = false;
                lblGnomoriaVersion.Text = "Unknown";
                lblGnomoriaUpdateType.Text = "";
            }

            // Get GE Version
            string gnomoriaEnhancedVersion = this.GetType().Assembly.GetName().Version.ToString();
            lblGnomoriaEnhancedVersion.Text = "v" + gnomoriaEnhancedVersion;
        }
        #endregion

        #region Debug
        public void MemoryUsage()
        {
            long usage = GC.GetTotalMemory(true);
            usage = (usage / 1024L) / 1024L;
            this.Debug(String.Format("Total Memory: {0:0.00}mb", usage));
        }
        #endregion


    }
}
