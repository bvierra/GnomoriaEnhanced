using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using log4net;
using log4net.Appender;

namespace GnomoriaEnhanced.UI
{
    public partial class Logging : Form, IAppender
    {
        private static ILog _logger = LogManager.GetLogger(typeof(Logging));

        /// <summary>
        /// This logger property is used to log events
        /// </summary>
        public static ILog Logger
        {
            get { return _logger; }
        }

        public Logging()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Show progress in tbLog
        /// </summary>
        /// <param name="loggingEvent"></param>
        public void DoAppend(log4net.Core.LoggingEvent loggingEvent)
        {

            tbLog.AppendText(String.Format("[{0}] {1} {2}",loggingEvent.Level.Name, loggingEvent.MessageObject.ToString(), Environment.NewLine));

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
            base.OnClosing(e);
        }

        #region Public Logging Shortcuts
        public void Debug(string msg)
        {
            Logger.Debug(msg);
        }

        public void Info(string msg)
        {
            Logger.Info(msg);
        }

        public void Warn(string msg)
        {
            Logger.Warn(msg);
        }

        public void Error(string msg)
        {
            Logger.Error(msg);
        }

        public void Fatal(string msg)
        {
            Logger.Fatal(msg);
        }
        #endregion

        private void btnLog_Click(object sender, EventArgs e)
        {
            Logger.Debug("Button Clicked");
        }

        private void Logging_Loaded(object sender, EventArgs e)
        {
            this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            this.Top = 0;
            this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
        }

    }
}
