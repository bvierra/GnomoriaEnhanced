namespace GnomoriaEnhanced.UI
{
    partial class WorldForm
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.lblKingdomName = new System.Windows.Forms.Label();
            this.cbReload = new System.Windows.Forms.CheckBox();
            this.comboBoxMenuGroup = new System.Windows.Forms.ComboBox();
            this.lbMenuItem = new System.Windows.Forms.ListBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressBar});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 450);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(392, 25);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 20);
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.progressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(136, 19);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // lblKingdomName
            // 
            this.lblKingdomName.AutoSize = true;
            this.lblKingdomName.Location = new System.Drawing.Point(13, 13);
            this.lblKingdomName.Name = "lblKingdomName";
            this.lblKingdomName.Size = new System.Drawing.Size(71, 17);
            this.lblKingdomName.TabIndex = 1;
            this.lblKingdomName.Text = "Loading...";
            // 
            // cbReload
            // 
            this.cbReload.AutoSize = true;
            this.cbReload.Enabled = false;
            this.cbReload.Location = new System.Drawing.Point(213, 12);
            this.cbReload.Name = "cbReload";
            this.cbReload.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbReload.Size = new System.Drawing.Size(164, 21);
            this.cbReload.TabIndex = 2;
            this.cbReload.Text = "Auto Reload on Save";
            this.cbReload.UseVisualStyleBackColor = true;
            // 
            // comboBoxMenuGroup
            // 
            this.comboBoxMenuGroup.Enabled = false;
            this.comboBoxMenuGroup.FormattingEnabled = true;
            this.comboBoxMenuGroup.Location = new System.Drawing.Point(13, 34);
            this.comboBoxMenuGroup.Name = "comboBoxMenuGroup";
            this.comboBoxMenuGroup.Size = new System.Drawing.Size(364, 24);
            this.comboBoxMenuGroup.TabIndex = 3;
            this.comboBoxMenuGroup.SelectedIndexChanged += new System.EventHandler(this.ComboBoxMenuGroup_SelectedChangeCommitted);
            this.comboBoxMenuGroup.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxMenuGroup_SelectedChangeCommitted);
            // 
            // lbMenuItem
            // 
            this.lbMenuItem.Enabled = false;
            this.lbMenuItem.FormattingEnabled = true;
            this.lbMenuItem.ItemHeight = 16;
            this.lbMenuItem.Location = new System.Drawing.Point(13, 65);
            this.lbMenuItem.Name = "lbMenuItem";
            this.lbMenuItem.ScrollAlwaysVisible = true;
            this.lbMenuItem.Size = new System.Drawing.Size(364, 372);
            this.lbMenuItem.TabIndex = 4;
            this.lbMenuItem.SelectedIndexChanged += new System.EventHandler(this.lbMenuItem_SelectedIndexChanged);
            // 
            // WorldForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 475);
            this.Controls.Add(this.lbMenuItem);
            this.Controls.Add(this.comboBoxMenuGroup);
            this.Controls.Add(this.cbReload);
            this.Controls.Add(this.lblKingdomName);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "WorldForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "World";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.Shown += new System.EventHandler(this.WorldForm_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.Label lblKingdomName;
        private System.Windows.Forms.CheckBox cbReload;
        private System.Windows.Forms.ComboBox comboBoxMenuGroup;
        private System.Windows.Forms.ListBox lbMenuItem;
    }
}