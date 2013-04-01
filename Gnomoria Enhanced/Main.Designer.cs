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
            this.initializeGameModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTabOverviewVerifConfig = new System.Windows.Forms.Label();
            this.lblTabOverviewVerifConfigValue = new System.Windows.Forms.Label();
            this.lblTabOverviewKingdomName = new System.Windows.Forms.Label();
            this.lblTabOverviewKingdomNameValue = new System.Windows.Forms.Label();
            this.lblTabOverviewTotalWorth = new System.Windows.Forms.Label();
            this.lblTabOverviewTotalWorthValue = new System.Windows.Forms.Label();
            this.lblTabOverviewDate = new System.Windows.Forms.Label();
            this.lblTabOverviewDateValue = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
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
            this.tableLayoutPanel1.SuspendLayout();
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
            this.menuStrip.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(983, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.initializeGameModelToolStripMenuItem,
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // initializeGameModelToolStripMenuItem
            // 
            this.initializeGameModelToolStripMenuItem.Name = "initializeGameModelToolStripMenuItem";
            this.initializeGameModelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.initializeGameModelToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.initializeGameModelToolStripMenuItem.Text = "&Initialize game model";
            this.initializeGameModelToolStripMenuItem.Click += new System.EventHandler(this.initializeGameModelToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
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
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "&Settings";
            // 
            // autoBackupSavedGamesToolStripMenuItem
            // 
            this.autoBackupSavedGamesToolStripMenuItem.CheckOnClick = true;
            this.autoBackupSavedGamesToolStripMenuItem.Name = "autoBackupSavedGamesToolStripMenuItem";
            this.autoBackupSavedGamesToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.autoBackupSavedGamesToolStripMenuItem.Text = "Auto Backup Saved Games";
            this.autoBackupSavedGamesToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.autoBackupSavedGamesToolStripMenuItem_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(212, 6);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.CheckOnClick = true;
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.logToolStripMenuItem.Text = "Log";
            this.logToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.logToolStripMenuItem_Checked);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 439);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip.Size = new System.Drawing.Size(983, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(59, 17);
            this.toolStripStatusLabel.Text = "Status Bar";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(225, 16);
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageOverview);
            this.tabControl.Controls.Add(this.tabCharSkills);
            this.tabControl.Controls.Add(this.tabCharStats);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(983, 415);
            this.tabControl.TabIndex = 2;
            // 
            // tabPageOverview
            // 
            this.tabPageOverview.Controls.Add(this.tableLayoutPanel1);
            this.tabPageOverview.Location = new System.Drawing.Point(4, 22);
            this.tabPageOverview.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageOverview.Name = "tabPageOverview";
            this.tabPageOverview.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageOverview.Size = new System.Drawing.Size(975, 389);
            this.tabPageOverview.TabIndex = 0;
            this.tabPageOverview.Text = "Overview";
            this.tabPageOverview.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.72727F));
            this.tableLayoutPanel1.Controls.Add(this.lblTabOverviewVerifConfig, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTabOverviewVerifConfigValue, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTabOverviewKingdomName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTabOverviewKingdomNameValue, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTabOverviewTotalWorth, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTabOverviewTotalWorthValue, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTabOverviewDate, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTabOverviewDateValue, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.listBox1, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(971, 385);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // lblTabOverviewVerifConfig
            // 
            this.lblTabOverviewVerifConfig.AutoSize = true;
            this.lblTabOverviewVerifConfig.Location = new System.Drawing.Point(2, 0);
            this.lblTabOverviewVerifConfig.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTabOverviewVerifConfig.Name = "lblTabOverviewVerifConfig";
            this.lblTabOverviewVerifConfig.Size = new System.Drawing.Size(165, 13);
            this.lblTabOverviewVerifConfig.TabIndex = 7;
            this.lblTabOverviewVerifConfig.Text = "Verifying Gnomoria configuration: ";
            // 
            // lblTabOverviewVerifConfigValue
            // 
            this.lblTabOverviewVerifConfigValue.AutoSize = true;
            this.lblTabOverviewVerifConfigValue.Location = new System.Drawing.Point(267, 0);
            this.lblTabOverviewVerifConfigValue.Name = "lblTabOverviewVerifConfigValue";
            this.lblTabOverviewVerifConfigValue.Size = new System.Drawing.Size(67, 13);
            this.lblTabOverviewVerifConfigValue.TabIndex = 9;
            this.lblTabOverviewVerifConfigValue.Text = "(Not verified)";
            // 
            // lblTabOverviewKingdomName
            // 
            this.lblTabOverviewKingdomName.AutoSize = true;
            this.lblTabOverviewKingdomName.Location = new System.Drawing.Point(2, 20);
            this.lblTabOverviewKingdomName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTabOverviewKingdomName.Name = "lblTabOverviewKingdomName";
            this.lblTabOverviewKingdomName.Size = new System.Drawing.Size(82, 13);
            this.lblTabOverviewKingdomName.TabIndex = 0;
            this.lblTabOverviewKingdomName.Text = "Kingdom Name:";
            // 
            // lblTabOverviewKingdomNameValue
            // 
            this.lblTabOverviewKingdomNameValue.AutoSize = true;
            this.lblTabOverviewKingdomNameValue.Location = new System.Drawing.Point(266, 20);
            this.lblTabOverviewKingdomNameValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTabOverviewKingdomNameValue.Name = "lblTabOverviewKingdomNameValue";
            this.lblTabOverviewKingdomNameValue.Size = new System.Drawing.Size(91, 13);
            this.lblTabOverviewKingdomNameValue.TabIndex = 8;
            this.lblTabOverviewKingdomNameValue.Text = "(No game loaded)";
            // 
            // lblTabOverviewTotalWorth
            // 
            this.lblTabOverviewTotalWorth.AutoSize = true;
            this.lblTabOverviewTotalWorth.Location = new System.Drawing.Point(2, 40);
            this.lblTabOverviewTotalWorth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTabOverviewTotalWorth.Name = "lblTabOverviewTotalWorth";
            this.lblTabOverviewTotalWorth.Size = new System.Drawing.Size(66, 13);
            this.lblTabOverviewTotalWorth.TabIndex = 2;
            this.lblTabOverviewTotalWorth.Text = "Total Worth:";
            // 
            // lblTabOverviewTotalWorthValue
            // 
            this.lblTabOverviewTotalWorthValue.AutoSize = true;
            this.lblTabOverviewTotalWorthValue.Location = new System.Drawing.Point(266, 40);
            this.lblTabOverviewTotalWorthValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTabOverviewTotalWorthValue.Name = "lblTabOverviewTotalWorthValue";
            this.lblTabOverviewTotalWorthValue.Size = new System.Drawing.Size(91, 13);
            this.lblTabOverviewTotalWorthValue.TabIndex = 3;
            this.lblTabOverviewTotalWorthValue.Text = "(No game loaded)";
            // 
            // lblTabOverviewDate
            // 
            this.lblTabOverviewDate.AutoSize = true;
            this.lblTabOverviewDate.Location = new System.Drawing.Point(2, 60);
            this.lblTabOverviewDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTabOverviewDate.Name = "lblTabOverviewDate";
            this.lblTabOverviewDate.Size = new System.Drawing.Size(33, 13);
            this.lblTabOverviewDate.TabIndex = 4;
            this.lblTabOverviewDate.Text = "Date:";
            // 
            // lblTabOverviewDateValue
            // 
            this.lblTabOverviewDateValue.AutoSize = true;
            this.lblTabOverviewDateValue.Location = new System.Drawing.Point(266, 60);
            this.lblTabOverviewDateValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTabOverviewDateValue.Name = "lblTabOverviewDateValue";
            this.lblTabOverviewDateValue.Size = new System.Drawing.Size(91, 13);
            this.lblTabOverviewDateValue.TabIndex = 5;
            this.lblTabOverviewDateValue.Text = "(No game loaded)";
            // 
            // listBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.listBox1, 2);
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 83);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(965, 299);
            this.listBox1.TabIndex = 10;
            this.listBox1.TabStop = false;
            // 
            // tabCharSkills
            // 
            this.tabCharSkills.Controls.Add(this.dataGridViewCharSkills);
            this.tabCharSkills.Location = new System.Drawing.Point(4, 22);
            this.tabCharSkills.Margin = new System.Windows.Forms.Padding(2);
            this.tabCharSkills.Name = "tabCharSkills";
            this.tabCharSkills.Padding = new System.Windows.Forms.Padding(2);
            this.tabCharSkills.Size = new System.Drawing.Size(975, 389);
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
            this.dataGridViewCharSkills.Location = new System.Drawing.Point(2, 2);
            this.dataGridViewCharSkills.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewCharSkills.MultiSelect = false;
            this.dataGridViewCharSkills.Name = "dataGridViewCharSkills";
            this.dataGridViewCharSkills.ReadOnly = true;
            this.dataGridViewCharSkills.RowTemplate.Height = 24;
            this.dataGridViewCharSkills.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewCharSkills.Size = new System.Drawing.Size(971, 385);
            this.dataGridViewCharSkills.TabIndex = 0;
            this.dataGridViewCharSkills.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCharSkills_CellClick);
            this.dataGridViewCharSkills.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewCharSkills_CellPainting);
            // 
            // tabCharStats
            // 
            this.tabCharStats.Controls.Add(this.dataGridViewCharStats);
            this.tabCharStats.Controls.Add(this.tabCharStatsBottomPanel);
            this.tabCharStats.Location = new System.Drawing.Point(4, 22);
            this.tabCharStats.Margin = new System.Windows.Forms.Padding(2);
            this.tabCharStats.Name = "tabCharStats";
            this.tabCharStats.Padding = new System.Windows.Forms.Padding(2);
            this.tabCharStats.Size = new System.Drawing.Size(975, 389);
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
            this.dataGridViewCharStats.Location = new System.Drawing.Point(2, 2);
            this.dataGridViewCharStats.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewCharStats.MultiSelect = false;
            this.dataGridViewCharStats.Name = "dataGridViewCharStats";
            this.dataGridViewCharStats.ReadOnly = true;
            this.dataGridViewCharStats.RowTemplate.Height = 24;
            this.dataGridViewCharStats.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewCharStats.Size = new System.Drawing.Size(971, 360);
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
            this.tabCharStatsBottomPanel.Location = new System.Drawing.Point(2, 362);
            this.tabCharStatsBottomPanel.Margin = new System.Windows.Forms.Padding(2);
            this.tabCharStatsBottomPanel.Name = "tabCharStatsBottomPanel";
            this.tabCharStatsBottomPanel.Padding = new System.Windows.Forms.Padding(2);
            this.tabCharStatsBottomPanel.Size = new System.Drawing.Size(971, 25);
            this.tabCharStatsBottomPanel.TabIndex = 0;
            this.tabCharStatsBottomPanel.Visible = false;
            // 
            // btnCharStatsHelp
            // 
            this.btnCharStatsHelp.Enabled = false;
            this.btnCharStatsHelp.Location = new System.Drawing.Point(4, 2);
            this.btnCharStatsHelp.Margin = new System.Windows.Forms.Padding(2);
            this.btnCharStatsHelp.Name = "btnCharStatsHelp";
            this.btnCharStatsHelp.Size = new System.Drawing.Size(56, 19);
            this.btnCharStatsHelp.TabIndex = 0;
            this.btnCharStatsHelp.Text = "Help";
            this.btnCharStatsHelp.UseVisualStyleBackColor = true;
            this.btnCharStatsHelp.Click += new System.EventHandler(this.btnChatStatsHelp_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 461);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(999, 499);
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
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.Label lblTabOverviewKingdomName;
        private System.Windows.Forms.ToolStripMenuItem autoBackupSavedGamesToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblTabOverviewVerifConfig;
        private System.Windows.Forms.Label lblTabOverviewVerifConfigValue;
        private System.Windows.Forms.Label lblTabOverviewKingdomNameValue;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ToolStripMenuItem initializeGameModelToolStripMenuItem;
    }
}