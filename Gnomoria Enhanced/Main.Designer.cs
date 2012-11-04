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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoBackupSavedGamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControl = new GnomoriaEnhanced.UI.TabControl();
            this.tabPageOverview = new System.Windows.Forms.TabPage();
            this.lblTabOverviewDateValue = new System.Windows.Forms.Label();
            this.lblTabOverviewDate = new System.Windows.Forms.Label();
            this.lblTabOverviewTotalWorthValue = new System.Windows.Forms.Label();
            this.lblTabOverviewTotalWorth = new System.Windows.Forms.Label();
            this.lblTabOverviewKingdomNameValue = new System.Windows.Forms.Label();
            this.lblTabOverviewKingdomName = new System.Windows.Forms.Label();
            this.tabCharSkills = new System.Windows.Forms.TabPage();
            this.dataGridViewCharSkills = new System.Windows.Forms.DataGridView();
            this.tabCharStats = new System.Windows.Forms.TabPage();
            this.dataGridViewCharStats = new System.Windows.Forms.DataGridView();
            this.tabCharStatsBottomPanel = new System.Windows.Forms.Panel();
            this.btnCharStatsHelp = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageOverview.SuspendLayout();
            this.tabCharSkills.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCharSkills)).BeginInit();
            this.tabCharStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCharStats)).BeginInit();
            this.tabCharStatsBottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1309, 28);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(167, 24);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoBackupSavedGamesToolStripMenuItem,
            this.toolStripSeparator1,
            this.logToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.settingsToolStripMenuItem.Text = "&Settings";
            // 
            // autoBackupSavedGamesToolStripMenuItem
            // 
            this.autoBackupSavedGamesToolStripMenuItem.CheckOnClick = true;
            this.autoBackupSavedGamesToolStripMenuItem.Name = "autoBackupSavedGamesToolStripMenuItem";
            this.autoBackupSavedGamesToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.autoBackupSavedGamesToolStripMenuItem.Text = "Auto Backup Saved Games";
            this.autoBackupSavedGamesToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.autoBackupSavedGamesToolStripMenuItem_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(252, 6);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.CheckOnClick = true;
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.logToolStripMenuItem.Text = "Log";
            this.logToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.logToolStripMenuItem_Checked);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 535);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1309, 25);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(75, 20);
            this.toolStripStatusLabel.Text = "Status Bar";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(300, 19);
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageOverview);
            this.tabControl.Controls.Add(this.tabCharSkills);
            this.tabControl.Controls.Add(this.tabCharStats);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 28);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1309, 507);
            this.tabControl.TabIndex = 2;
            // 
            // tabPageOverview
            // 
            this.tabPageOverview.Controls.Add(this.lblTabOverviewDateValue);
            this.tabPageOverview.Controls.Add(this.lblTabOverviewDate);
            this.tabPageOverview.Controls.Add(this.lblTabOverviewTotalWorthValue);
            this.tabPageOverview.Controls.Add(this.lblTabOverviewTotalWorth);
            this.tabPageOverview.Controls.Add(this.lblTabOverviewKingdomNameValue);
            this.tabPageOverview.Controls.Add(this.lblTabOverviewKingdomName);
            this.tabPageOverview.Location = new System.Drawing.Point(4, 25);
            this.tabPageOverview.Name = "tabPageOverview";
            this.tabPageOverview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOverview.Size = new System.Drawing.Size(1301, 478);
            this.tabPageOverview.TabIndex = 0;
            this.tabPageOverview.Text = "Overview";
            this.tabPageOverview.UseVisualStyleBackColor = true;
            // 
            // lblTabOverviewDateValue
            // 
            this.lblTabOverviewDateValue.AutoSize = true;
            this.lblTabOverviewDateValue.Location = new System.Drawing.Point(127, 45);
            this.lblTabOverviewDateValue.Name = "lblTabOverviewDateValue";
            this.lblTabOverviewDateValue.Size = new System.Drawing.Size(0, 17);
            this.lblTabOverviewDateValue.TabIndex = 5;
            // 
            // lblTabOverviewDate
            // 
            this.lblTabOverviewDate.AutoSize = true;
            this.lblTabOverviewDate.Location = new System.Drawing.Point(12, 45);
            this.lblTabOverviewDate.Name = "lblTabOverviewDate";
            this.lblTabOverviewDate.Size = new System.Drawing.Size(42, 17);
            this.lblTabOverviewDate.TabIndex = 4;
            this.lblTabOverviewDate.Text = "Date:";
            this.lblTabOverviewDate.Visible = false;
            // 
            // lblTabOverviewTotalWorthValue
            // 
            this.lblTabOverviewTotalWorthValue.AutoSize = true;
            this.lblTabOverviewTotalWorthValue.Location = new System.Drawing.Point(127, 24);
            this.lblTabOverviewTotalWorthValue.Name = "lblTabOverviewTotalWorthValue";
            this.lblTabOverviewTotalWorthValue.Size = new System.Drawing.Size(0, 17);
            this.lblTabOverviewTotalWorthValue.TabIndex = 3;
            // 
            // lblTabOverviewTotalWorth
            // 
            this.lblTabOverviewTotalWorth.AutoSize = true;
            this.lblTabOverviewTotalWorth.Location = new System.Drawing.Point(9, 24);
            this.lblTabOverviewTotalWorth.Name = "lblTabOverviewTotalWorth";
            this.lblTabOverviewTotalWorth.Size = new System.Drawing.Size(86, 17);
            this.lblTabOverviewTotalWorth.TabIndex = 2;
            this.lblTabOverviewTotalWorth.Text = "Total Worth:";
            this.lblTabOverviewTotalWorth.Visible = false;
            // 
            // lblTabOverviewKingdomNameValue
            // 
            this.lblTabOverviewKingdomNameValue.AutoSize = true;
            this.lblTabOverviewKingdomNameValue.Location = new System.Drawing.Point(124, 7);
            this.lblTabOverviewKingdomNameValue.Name = "lblTabOverviewKingdomNameValue";
            this.lblTabOverviewKingdomNameValue.Size = new System.Drawing.Size(0, 17);
            this.lblTabOverviewKingdomNameValue.TabIndex = 1;
            // 
            // lblTabOverviewKingdomName
            // 
            this.lblTabOverviewKingdomName.AutoSize = true;
            this.lblTabOverviewKingdomName.Location = new System.Drawing.Point(9, 7);
            this.lblTabOverviewKingdomName.Name = "lblTabOverviewKingdomName";
            this.lblTabOverviewKingdomName.Size = new System.Drawing.Size(108, 17);
            this.lblTabOverviewKingdomName.TabIndex = 0;
            this.lblTabOverviewKingdomName.Text = "Kingdom Name:";
            this.lblTabOverviewKingdomName.Visible = false;
            // 
            // tabCharSkills
            // 
            this.tabCharSkills.Controls.Add(this.dataGridViewCharSkills);
            this.tabCharSkills.Location = new System.Drawing.Point(4, 25);
            this.tabCharSkills.Name = "tabCharSkills";
            this.tabCharSkills.Padding = new System.Windows.Forms.Padding(3);
            this.tabCharSkills.Size = new System.Drawing.Size(1301, 478);
            this.tabCharSkills.TabIndex = 1;
            this.tabCharSkills.Text = "Character Skills";
            this.tabCharSkills.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCharSkills
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dataGridViewCharSkills.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewCharSkills.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial Narrow", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCharSkills.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewCharSkills.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCharSkills.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCharSkills.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridViewCharSkills.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewCharSkills.MultiSelect = false;
            this.dataGridViewCharSkills.Name = "dataGridViewCharSkills";
            this.dataGridViewCharSkills.ReadOnly = true;
            this.dataGridViewCharSkills.RowTemplate.Height = 24;
            this.dataGridViewCharSkills.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewCharSkills.Size = new System.Drawing.Size(1295, 472);
            this.dataGridViewCharSkills.TabIndex = 0;
            this.dataGridViewCharSkills.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCharSkills_CellClick);
            this.dataGridViewCharSkills.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewCharSkills_CellPainting);
            // 
            // tabCharStats
            // 
            this.tabCharStats.Controls.Add(this.dataGridViewCharStats);
            this.tabCharStats.Controls.Add(this.tabCharStatsBottomPanel);
            this.tabCharStats.Location = new System.Drawing.Point(4, 25);
            this.tabCharStats.Name = "tabCharStats";
            this.tabCharStats.Padding = new System.Windows.Forms.Padding(3);
            this.tabCharStats.Size = new System.Drawing.Size(1301, 478);
            this.tabCharStats.TabIndex = 2;
            this.tabCharStats.Text = "Character Stats";
            this.tabCharStats.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCharStats
            // 
            this.dataGridViewCharStats.AllowUserToAddRows = false;
            this.dataGridViewCharStats.AllowUserToDeleteRows = false;
            this.dataGridViewCharStats.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewCharStats.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewCharStats.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewCharStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCharStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCharStats.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewCharStats.MultiSelect = false;
            this.dataGridViewCharStats.Name = "dataGridViewCharStats";
            this.dataGridViewCharStats.ReadOnly = true;
            this.dataGridViewCharStats.RowTemplate.Height = 24;
            this.dataGridViewCharStats.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewCharStats.Size = new System.Drawing.Size(1295, 442);
            this.dataGridViewCharStats.TabIndex = 0;
            this.dataGridViewCharStats.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCharStats_CellClick);
            this.dataGridViewCharStats.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewCharStats_CellPainting);
            // 
            // tabCharStatsBottomPanel
            // 
            this.tabCharStatsBottomPanel.BackColor = System.Drawing.Color.LightGray;
            this.tabCharStatsBottomPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabCharStatsBottomPanel.Controls.Add(this.btnCharStatsHelp);
            this.tabCharStatsBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabCharStatsBottomPanel.Location = new System.Drawing.Point(3, 445);
            this.tabCharStatsBottomPanel.Name = "tabCharStatsBottomPanel";
            this.tabCharStatsBottomPanel.Padding = new System.Windows.Forms.Padding(3);
            this.tabCharStatsBottomPanel.Size = new System.Drawing.Size(1295, 30);
            this.tabCharStatsBottomPanel.TabIndex = 0;
            this.tabCharStatsBottomPanel.Visible = false;
            // 
            // btnCharStatsHelp
            // 
            this.btnCharStatsHelp.Enabled = false;
            this.btnCharStatsHelp.Location = new System.Drawing.Point(6, 2);
            this.btnCharStatsHelp.Name = "btnCharStatsHelp";
            this.btnCharStatsHelp.Size = new System.Drawing.Size(75, 23);
            this.btnCharStatsHelp.TabIndex = 0;
            this.btnCharStatsHelp.Text = "Help";
            this.btnCharStatsHelp.UseVisualStyleBackColor = true;
            this.btnCharStatsHelp.Click += new System.EventHandler(this.btnChatStatsHelp_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1309, 560);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(1327, 605);
            this.Name = "Main";
            this.Text = "Gnomoria Enhanced";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageOverview.ResumeLayout(false);
            this.tabPageOverview.PerformLayout();
            this.tabCharSkills.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCharSkills)).EndInit();
            this.tabCharStats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCharStats)).EndInit();
            this.tabCharStatsBottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.TabPage tabPageOverview;
        private System.Windows.Forms.TabPage tabCharSkills;
        private System.Windows.Forms.DataGridView dataGridViewCharSkills;
        private GnomoriaEnhanced.UI.TabControl tabControl;
        private System.Windows.Forms.TabPage tabCharStats;
        private System.Windows.Forms.DataGridView dataGridViewCharStats;
        private System.Windows.Forms.Panel tabCharStatsBottomPanel;
        private System.Windows.Forms.Button btnCharStatsHelp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.Label lblTabOverviewDateValue;
        private System.Windows.Forms.Label lblTabOverviewDate;
        private System.Windows.Forms.Label lblTabOverviewTotalWorthValue;
        private System.Windows.Forms.Label lblTabOverviewTotalWorth;
        private System.Windows.Forms.Label lblTabOverviewKingdomNameValue;
        private System.Windows.Forms.Label lblTabOverviewKingdomName;
        private System.Windows.Forms.ToolStripMenuItem autoBackupSavedGamesToolStripMenuItem;
    }
}