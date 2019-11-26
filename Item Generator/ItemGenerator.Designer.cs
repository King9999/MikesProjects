namespace Item_Generator
{
    partial class ItemGenerator
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
            this.ButtonGenerate = new System.Windows.Forms.Button();
            this.ComboBox_ItemType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ComboBox_ItemSubType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBox_ItemLevel = new System.Windows.Forms.MaskedTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonGenerate
            // 
            this.ButtonGenerate.Location = new System.Drawing.Point(179, 254);
            this.ButtonGenerate.Name = "ButtonGenerate";
            this.ButtonGenerate.Size = new System.Drawing.Size(114, 35);
            this.ButtonGenerate.TabIndex = 0;
            this.ButtonGenerate.Text = "Generate Item";
            this.ButtonGenerate.UseVisualStyleBackColor = true;
            this.ButtonGenerate.Click += new System.EventHandler(this.ButtonGenerate_Click);
            // 
            // ComboBox_ItemType
            // 
            this.ComboBox_ItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_ItemType.FormattingEnabled = true;
            this.ComboBox_ItemType.Items.AddRange(new object[] {
            "Weapon",
            "Armor",
            "Accessory"});
            this.ComboBox_ItemType.Location = new System.Drawing.Point(168, 58);
            this.ComboBox_ItemType.Name = "ComboBox_ItemType";
            this.ComboBox_ItemType.Size = new System.Drawing.Size(163, 21);
            this.ComboBox_ItemType.TabIndex = 1;
            this.ComboBox_ItemType.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ItemType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "1. Select item type:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // ComboBox_ItemSubType
            // 
            this.ComboBox_ItemSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_ItemSubType.FormattingEnabled = true;
            this.ComboBox_ItemSubType.Location = new System.Drawing.Point(168, 118);
            this.ComboBox_ItemSubType.Name = "ComboBox_ItemSubType";
            this.ComboBox_ItemSubType.Size = new System.Drawing.Size(163, 21);
            this.ComboBox_ItemSubType.TabIndex = 3;
            this.ComboBox_ItemSubType.SelectedIndexChanged += new System.EventHandler(this.ComboBox_ItemSubType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "2. Select item subtype:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Select Item Level (1-200):";
            // 
            // TextBox_ItemLevel
            // 
            this.TextBox_ItemLevel.Location = new System.Drawing.Point(168, 174);
            this.TextBox_ItemLevel.Mask = "099";
            this.TextBox_ItemLevel.Name = "TextBox_ItemLevel";
            this.TextBox_ItemLevel.Size = new System.Drawing.Size(76, 20);
            this.TextBox_ItemLevel.TabIndex = 7;
            this.TextBox_ItemLevel.Text = "1";
            this.TextBox_ItemLevel.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.TextBox_ItemLevel.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.TextBox_ItemLevel_MaskInputRejected);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(457, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // ItemGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 301);
            this.Controls.Add(this.TextBox_ItemLevel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ComboBox_ItemSubType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ComboBox_ItemType);
            this.Controls.Add(this.ButtonGenerate);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ItemGenerator";
            this.Text = "Item Generator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonGenerate;
        private System.Windows.Forms.ComboBox ComboBox_ItemType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ComboBox_ItemSubType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox TextBox_ItemLevel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

