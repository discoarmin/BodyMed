namespace BodyMed
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainerHaupt = new System.Windows.Forms.SplitContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.version = new System.Windows.Forms.ToolStripStatusLabel();
            this.ribbonUpDown1 = new System.Windows.Forms.RibbonUpDown();
            this.ribbon1 = new System.Windows.Forms.Ribbon();
            this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHaupt)).BeginInit();
            this.splitContainerHaupt.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerHaupt
            // 
            this.splitContainerHaupt.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.splitContainerHaupt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerHaupt.Location = new System.Drawing.Point(0, 0);
            this.splitContainerHaupt.Name = "splitContainerHaupt";
            this.splitContainerHaupt.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainerHaupt.Size = new System.Drawing.Size(993, 519);
            this.splitContainerHaupt.SplitterDistance = 241;
            this.splitContainerHaupt.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.SystemColors.ControlDark;
            this.statusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusText,
            this.version});
            this.statusStrip.Location = new System.Drawing.Point(0, 497);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(993, 22);
            this.statusStrip.TabIndex = 1;
            // 
            // statusText
            // 
            this.statusText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusText.ForeColor = System.Drawing.Color.Black;
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(933, 17);
            this.statusText.Spring = true;
            this.statusText.Text = "Bereit ...";
            this.statusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // version
            // 
            this.version.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.version.ForeColor = System.Drawing.Color.Black;
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(45, 17);
            this.version.Text = "version";
            this.version.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ribbonUpDown1
            // 
            this.ribbonUpDown1.TextBoxText = "";
            this.ribbonUpDown1.TextBoxWidth = 50;
            // 
            // ribbon1
            // 
            this.ribbon1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ribbon1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.ribbon1.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.Minimized = false;
            this.ribbon1.Name = "ribbon1";
            // 
            // 
            // 
            this.ribbon1.OrbDropDown.BorderRoundness = 8;
            this.ribbon1.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.OrbDropDown.Name = "";
            this.ribbon1.OrbDropDown.Size = new System.Drawing.Size(527, 72);
            this.ribbon1.OrbDropDown.TabIndex = 0;
            this.ribbon1.OrbImage = null;
            this.ribbon1.OrbStyle = System.Windows.Forms.RibbonOrbStyle.Office_2013;
            this.ribbon1.OrbText = "Datei";
            this.ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
            this.ribbon1.Size = new System.Drawing.Size(993, 56);
            this.ribbon1.TabIndex = 0;
            this.ribbon1.Tabs.Add(this.ribbonTab1);
            this.ribbon1.TabsMargin = new System.Windows.Forms.Padding(12, 26, 20, 0);
            this.ribbon1.Text = "ribbon1";
            this.ribbon1.ThemeColor = System.Windows.Forms.RibbonTheme.Black;
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.Text = "ribbonTab1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 519);
            this.Controls.Add(this.ribbon1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.splitContainerHaupt);
            this.Name = "Form1";
            this.Text = "Abnehmen";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHaupt)).EndInit();
            this.splitContainerHaupt.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerHaupt;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.ToolStripStatusLabel version;
        private System.Windows.Forms.RibbonUpDown ribbonUpDown1;
        private System.Windows.Forms.Ribbon ribbon1;
        private System.Windows.Forms.RibbonTab ribbonTab1;
        private System.Windows.Forms.RibbonPanel ribbonPanel1;
    }
}

