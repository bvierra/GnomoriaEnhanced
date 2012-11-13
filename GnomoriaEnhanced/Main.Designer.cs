namespace GnomoriaEnhanced
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoadGame = new System.Windows.Forms.Button();
            this.cbLog = new System.Windows.Forms.CheckBox();
            this.cbBackupSaves = new System.Windows.Forms.CheckBox();
            this.groupboxChangeLog = new System.Windows.Forms.GroupBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabCLGnomoria = new System.Windows.Forms.TabPage();
            this.tabCLGnomoriaEnhanced = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.lblGnomoria = new System.Windows.Forms.Label();
            this.lblGnomoriaVersion = new System.Windows.Forms.Label();
            this.lblGnomoriaEnhancedVersion = new System.Windows.Forms.Label();
            this.lblGnomoriaEnhanced = new System.Windows.Forms.Label();
            this.lblGnomoriaUpdateType = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.groupboxChangeLog.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadGame
            // 
            this.btnLoadGame.Location = new System.Drawing.Point(12, 12);
            this.btnLoadGame.Name = "btnLoadGame";
            this.btnLoadGame.Size = new System.Drawing.Size(141, 27);
            this.btnLoadGame.TabIndex = 0;
            this.btnLoadGame.Text = "Load Saved Game";
            this.btnLoadGame.UseVisualStyleBackColor = true;
            this.btnLoadGame.Click += new System.EventHandler(this.btnLoadGame_Click);
            // 
            // cbLog
            // 
            this.cbLog.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbLog.AutoSize = true;
            this.cbLog.Location = new System.Drawing.Point(562, 12);
            this.cbLog.Name = "cbLog";
            this.cbLog.Size = new System.Drawing.Size(75, 27);
            this.cbLog.TabIndex = 2;
            this.cbLog.Text = "View Log";
            this.cbLog.UseVisualStyleBackColor = true;
            this.cbLog.CheckStateChanged += new System.EventHandler(this.cbLog_CheckStateChanged);
            // 
            // cbBackupSaves
            // 
            this.cbBackupSaves.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbBackupSaves.AutoSize = true;
            this.cbBackupSaves.Location = new System.Drawing.Point(448, 12);
            this.cbBackupSaves.Name = "cbBackupSaves";
            this.cbBackupSaves.Size = new System.Drawing.Size(108, 27);
            this.cbBackupSaves.TabIndex = 3;
            this.cbBackupSaves.Text = "Backup Saves";
            this.cbBackupSaves.UseVisualStyleBackColor = true;
            this.cbBackupSaves.Click += new System.EventHandler(this.cbBackupSaves_Click);
            // 
            // groupboxChangeLog
            // 
            this.groupboxChangeLog.Controls.Add(this.tabControl);
            this.groupboxChangeLog.Location = new System.Drawing.Point(13, 66);
            this.groupboxChangeLog.Name = "groupboxChangeLog";
            this.groupboxChangeLog.Size = new System.Drawing.Size(624, 249);
            this.groupboxChangeLog.TabIndex = 4;
            this.groupboxChangeLog.TabStop = false;
            this.groupboxChangeLog.Text = "Change Log";
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl.Controls.Add(this.tabCLGnomoria);
            this.tabControl.Controls.Add(this.tabCLGnomoriaEnhanced);
            this.tabControl.Location = new System.Drawing.Point(7, 22);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(611, 218);
            this.tabControl.TabIndex = 0;
            // 
            // tabCLGnomoria
            // 
            this.tabCLGnomoria.Location = new System.Drawing.Point(4, 4);
            this.tabCLGnomoria.Name = "tabCLGnomoria";
            this.tabCLGnomoria.Padding = new System.Windows.Forms.Padding(3);
            this.tabCLGnomoria.Size = new System.Drawing.Size(603, 189);
            this.tabCLGnomoria.TabIndex = 0;
            this.tabCLGnomoria.Text = "Gnomoria";
            this.tabCLGnomoria.UseVisualStyleBackColor = true;
            // 
            // tabCLGnomoriaEnhanced
            // 
            this.tabCLGnomoriaEnhanced.Location = new System.Drawing.Point(4, 4);
            this.tabCLGnomoriaEnhanced.Name = "tabCLGnomoriaEnhanced";
            this.tabCLGnomoriaEnhanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabCLGnomoriaEnhanced.Size = new System.Drawing.Size(603, 189);
            this.tabCLGnomoriaEnhanced.TabIndex = 1;
            this.tabCLGnomoriaEnhanced.Text = "Gnomoria Enhanced";
            this.tabCLGnomoriaEnhanced.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus,
            this.toolStripProgressBar1});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 358);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(649, 25);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(0, 20);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.RightToLeftLayout = true;
            this.toolStripProgressBar1.Size = new System.Drawing.Size(200, 19);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // lblGnomoria
            // 
            this.lblGnomoria.AutoSize = true;
            this.lblGnomoria.Location = new System.Drawing.Point(13, 46);
            this.lblGnomoria.Name = "lblGnomoria";
            this.lblGnomoria.Size = new System.Drawing.Size(126, 17);
            this.lblGnomoria.TabIndex = 6;
            this.lblGnomoria.Text = "Gnomoria Version:";
            // 
            // lblGnomoriaVersion
            // 
            this.lblGnomoriaVersion.AutoSize = true;
            this.lblGnomoriaVersion.Location = new System.Drawing.Point(146, 46);
            this.lblGnomoriaVersion.Name = "lblGnomoriaVersion";
            this.lblGnomoriaVersion.Size = new System.Drawing.Size(91, 17);
            this.lblGnomoriaVersion.TabIndex = 7;
            this.lblGnomoriaVersion.Text = "v99.99.99.99";
            // 
            // lblGnomoriaEnhancedVersion
            // 
            this.lblGnomoriaEnhancedVersion.AutoSize = true;
            this.lblGnomoriaEnhancedVersion.Location = new System.Drawing.Point(546, 46);
            this.lblGnomoriaEnhancedVersion.Name = "lblGnomoriaEnhancedVersion";
            this.lblGnomoriaEnhancedVersion.Size = new System.Drawing.Size(91, 17);
            this.lblGnomoriaEnhancedVersion.TabIndex = 8;
            this.lblGnomoriaEnhancedVersion.Text = "v99.99.99.99";
            // 
            // lblGnomoriaEnhanced
            // 
            this.lblGnomoriaEnhanced.AutoSize = true;
            this.lblGnomoriaEnhanced.Location = new System.Drawing.Point(350, 46);
            this.lblGnomoriaEnhanced.Name = "lblGnomoriaEnhanced";
            this.lblGnomoriaEnhanced.Size = new System.Drawing.Size(194, 17);
            this.lblGnomoriaEnhanced.TabIndex = 9;
            this.lblGnomoriaEnhanced.Text = "Gnomoria Enhanced Version:";
            // 
            // lblGnomoriaUpdateType
            // 
            this.lblGnomoriaUpdateType.AutoSize = true;
            this.lblGnomoriaUpdateType.Location = new System.Drawing.Point(244, 46);
            this.lblGnomoriaUpdateType.Name = "lblGnomoriaUpdateType";
            this.lblGnomoriaUpdateType.Size = new System.Drawing.Size(33, 17);
            this.lblGnomoriaUpdateType.TabIndex = 10;
            this.lblGnomoriaUpdateType.Text = "(---)";
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(262, 325);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(120, 30);
            this.btnPlay.TabIndex = 11;
            this.btnPlay.Text = "Play Gnomoria";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 383);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.lblGnomoriaUpdateType);
            this.Controls.Add(this.lblGnomoriaEnhanced);
            this.Controls.Add(this.lblGnomoriaEnhancedVersion);
            this.Controls.Add(this.lblGnomoriaVersion);
            this.Controls.Add(this.lblGnomoria);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupboxChangeLog);
            this.Controls.Add(this.cbBackupSaves);
            this.Controls.Add(this.cbLog);
            this.Controls.Add(this.btnLoadGame);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Main";
            this.Text = "Gnomoria Enhanced";
            this.groupboxChangeLog.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadGame;
        private System.Windows.Forms.CheckBox cbLog;
        private System.Windows.Forms.CheckBox cbBackupSaves;
        private System.Windows.Forms.GroupBox groupboxChangeLog;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabCLGnomoria;
        private System.Windows.Forms.TabPage tabCLGnomoriaEnhanced;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Label lblGnomoria;
        private System.Windows.Forms.Label lblGnomoriaVersion;
        private System.Windows.Forms.Label lblGnomoriaEnhancedVersion;
        private System.Windows.Forms.Label lblGnomoriaEnhanced;
        private System.Windows.Forms.Label lblGnomoriaUpdateType;
        private System.Windows.Forms.Button btnPlay;
    }
}

