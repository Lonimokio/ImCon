namespace ImageConverter
{
    partial class ImageConversion
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.convert = new System.Windows.Forms.Button();
            this.TypeB = new System.Windows.Forms.ComboBox();
            this.SLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Connect = new System.Windows.Forms.Button();
            this.FLabel = new System.Windows.Forms.Label();
            this.InputB = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.InputB2 = new System.Windows.Forms.ListBox();
            this.Credits = new System.Windows.Forms.Label();
            this.ProgressB = new System.Windows.Forms.ProgressBar();
            this.Left = new System.Windows.Forms.Label();
            this.Done = new System.Windows.Forms.Label();
            this.DoneL = new System.Windows.Forms.Label();
            this.LeftL = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.QualityB = new System.Windows.Forms.NumericUpDown();
            this.DBName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ConnectionBox = new System.Windows.Forms.TextBox();
            this.PathB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Deletion = new System.Windows.Forms.CheckBox();
            this.SmallImages = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.Convertion = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.QualityB)).BeginInit();
            this.Convertion.SuspendLayout();
            this.SuspendLayout();
            // 
            // convert
            // 
            this.convert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.convert.Location = new System.Drawing.Point(227, 140);
            this.convert.Name = "convert";
            this.convert.Size = new System.Drawing.Size(213, 80);
            this.convert.TabIndex = 0;
            this.convert.Text = "Convert";
            this.convert.UseVisualStyleBackColor = true;
            this.convert.Click += new System.EventHandler(this.convert_Click);
            // 
            // TypeB
            // 
            this.TypeB.FormattingEnabled = true;
            this.TypeB.Items.AddRange(new object[] {
            "webp"});
            this.TypeB.Location = new System.Drawing.Point(280, 112);
            this.TypeB.Name = "TypeB";
            this.TypeB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TypeB.Size = new System.Drawing.Size(54, 23);
            this.TypeB.TabIndex = 1;
            this.TypeB.Text = "webp";
            this.TypeB.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // SLabel
            // 
            this.SLabel.AutoSize = true;
            this.SLabel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SLabel.Location = new System.Drawing.Point(181, 56);
            this.SLabel.Name = "SLabel";
            this.SLabel.Size = new System.Drawing.Size(317, 30);
            this.SLabel.TabIndex = 2;
            this.SLabel.Text = "What format to be converted to?";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(310, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 65);
            this.label3.TabIndex = 5;
            this.label3.Text = "ImCon";
            // 
            // Connect
            // 
            this.Connect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Connect.Location = new System.Drawing.Point(687, 322);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(101, 43);
            this.Connect.TabIndex = 6;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // FLabel
            // 
            this.FLabel.AutoSize = true;
            this.FLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FLabel.Location = new System.Drawing.Point(682, 280);
            this.FLabel.Name = "FLabel";
            this.FLabel.Size = new System.Drawing.Size(110, 17);
            this.FLabel.TabIndex = 8;
            this.FLabel.Text = "Choose database";
            // 
            // InputB
            // 
            this.InputB.FormattingEnabled = true;
            this.InputB.HorizontalScrollbar = true;
            this.InputB.ItemHeight = 15;
            this.InputB.Location = new System.Drawing.Point(36, 40);
            this.InputB.Name = "InputB";
            this.InputB.Size = new System.Drawing.Size(120, 94);
            this.InputB.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Images";
            // 
            // InputB2
            // 
            this.InputB2.FormattingEnabled = true;
            this.InputB2.ItemHeight = 15;
            this.InputB2.Location = new System.Drawing.Point(36, 133);
            this.InputB2.Name = "InputB2";
            this.InputB2.Size = new System.Drawing.Size(120, 94);
            this.InputB2.TabIndex = 12;
            // 
            // Credits
            // 
            this.Credits.AutoSize = true;
            this.Credits.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Credits.ForeColor = System.Drawing.Color.DarkGreen;
            this.Credits.Location = new System.Drawing.Point(597, 396);
            this.Credits.Name = "Credits";
            this.Credits.Size = new System.Drawing.Size(191, 45);
            this.Credits.TabIndex = 13;
            this.Credits.Text = "Alpha 1.1.0\r\nIntelectual property of Arttu Mutka\r\nOriginal idea by Petri Kaija";
            // 
            // ProgressB
            // 
            this.ProgressB.ForeColor = System.Drawing.Color.Red;
            this.ProgressB.Location = new System.Drawing.Point(36, 223);
            this.ProgressB.Name = "ProgressB";
            this.ProgressB.Size = new System.Drawing.Size(120, 23);
            this.ProgressB.TabIndex = 14;
            // 
            // Left
            // 
            this.Left.AutoSize = true;
            this.Left.Location = new System.Drawing.Point(269, 29);
            this.Left.Name = "Left";
            this.Left.Size = new System.Drawing.Size(13, 15);
            this.Left.TabIndex = 15;
            this.Left.Text = "0";
            // 
            // Done
            // 
            this.Done.AutoSize = true;
            this.Done.Location = new System.Drawing.Point(381, 29);
            this.Done.Name = "Done";
            this.Done.Size = new System.Drawing.Size(13, 15);
            this.Done.TabIndex = 16;
            this.Done.Text = "0";
            // 
            // DoneL
            // 
            this.DoneL.AutoSize = true;
            this.DoneL.Location = new System.Drawing.Point(371, 12);
            this.DoneL.Name = "DoneL";
            this.DoneL.Size = new System.Drawing.Size(35, 15);
            this.DoneL.TabIndex = 18;
            this.DoneL.Text = "Done";
            // 
            // LeftL
            // 
            this.LeftL.AutoSize = true;
            this.LeftL.Location = new System.Drawing.Point(264, 12);
            this.LeftL.Name = "LeftL";
            this.LeftL.Size = new System.Drawing.Size(27, 15);
            this.LeftL.TabIndex = 17;
            this.LeftL.Text = "Left";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Location = new System.Drawing.Point(-6, 388);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(817, 80);
            this.panel1.TabIndex = 19;
            // 
            // QualityB
            // 
            this.QualityB.Location = new System.Drawing.Point(340, 112);
            this.QualityB.Name = "QualityB";
            this.QualityB.Size = new System.Drawing.Size(54, 23);
            this.QualityB.TabIndex = 20;
            this.QualityB.ValueChanged += new System.EventHandler(this.QualityB_ValueChanged);
            // 
            // DBName
            // 
            this.DBName.FormattingEnabled = true;
            this.DBName.Items.AddRange(new object[] {
            "SKJ"});
            this.DBName.Location = new System.Drawing.Point(687, 300);
            this.DBName.Name = "DBName";
            this.DBName.Size = new System.Drawing.Size(101, 23);
            this.DBName.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(280, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "Format";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(341, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 15);
            this.label4.TabIndex = 23;
            this.label4.Text = "Quality";
            // 
            // ConnectionBox
            // 
            this.ConnectionBox.Enabled = false;
            this.ConnectionBox.Location = new System.Drawing.Point(688, 359);
            this.ConnectionBox.Name = "ConnectionBox";
            this.ConnectionBox.Size = new System.Drawing.Size(100, 23);
            this.ConnectionBox.TabIndex = 24;
            this.ConnectionBox.Text = "Connected to: ";
            // 
            // PathB
            // 
            this.PathB.Location = new System.Drawing.Point(286, 240);
            this.PathB.Name = "PathB";
            this.PathB.Size = new System.Drawing.Size(100, 23);
            this.PathB.TabIndex = 25;
            this.PathB.Text = "C:\\winskj\\";
            this.PathB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(308, 222);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 15);
            this.label5.TabIndex = 26;
            this.label5.Text = "Save path";
            // 
            // Deletion
            // 
            this.Deletion.AutoSize = true;
            this.Deletion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Deletion.Location = new System.Drawing.Point(446, 140);
            this.Deletion.Name = "Deletion";
            this.Deletion.Size = new System.Drawing.Size(108, 17);
            this.Deletion.TabIndex = 27;
            this.Deletion.Text = "Enable Deletion";
            this.Deletion.UseVisualStyleBackColor = true;
            // 
            // SmallImages
            // 
            this.SmallImages.AutoSize = true;
            this.SmallImages.Location = new System.Drawing.Point(446, 165);
            this.SmallImages.Name = "SmallImages";
            this.SmallImages.Size = new System.Drawing.Size(133, 19);
            this.SmallImages.TabIndex = 28;
            this.SmallImages.Text = "Enable small images";
            this.SmallImages.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(446, 190);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(88, 19);
            this.checkBox3.TabIndex = 29;
            this.checkBox3.Text = "Placeholder";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // Convertion
            // 
            this.Convertion.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Convertion.Controls.Add(this.convert);
            this.Convertion.Controls.Add(this.checkBox3);
            this.Convertion.Controls.Add(this.QualityB);
            this.Convertion.Controls.Add(this.SmallImages);
            this.Convertion.Controls.Add(this.TypeB);
            this.Convertion.Controls.Add(this.ProgressB);
            this.Convertion.Controls.Add(this.Deletion);
            this.Convertion.Controls.Add(this.InputB2);
            this.Convertion.Controls.Add(this.SLabel);
            this.Convertion.Controls.Add(this.label1);
            this.Convertion.Controls.Add(this.label5);
            this.Convertion.Controls.Add(this.InputB);
            this.Convertion.Controls.Add(this.Left);
            this.Convertion.Controls.Add(this.PathB);
            this.Convertion.Controls.Add(this.Done);
            this.Convertion.Controls.Add(this.LeftL);
            this.Convertion.Controls.Add(this.label4);
            this.Convertion.Controls.Add(this.DoneL);
            this.Convertion.Controls.Add(this.label2);
            this.Convertion.Location = new System.Drawing.Point(59, 77);
            this.Convertion.Name = "Convertion";
            this.Convertion.Size = new System.Drawing.Size(584, 268);
            this.Convertion.TabIndex = 30;
            this.Convertion.TabStop = false;
            this.Convertion.Text = "Convertion";
            // 
            // ImageConversion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(809, 461);
            this.Controls.Add(this.Convertion);
            this.Controls.Add(this.ConnectionBox);
            this.Controls.Add(this.DBName);
            this.Controls.Add(this.Credits);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.FLabel);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.label3);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.DarkGreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(825, 500);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(825, 500);
            this.Name = "ImageConversion";
            this.Text = "ImCon";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QualityB)).EndInit();
            this.Convertion.ResumeLayout(false);
            this.Convertion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button convert;
        private ComboBox TypeB;
        private Label SLabel;
        private Label label3;
        private Button Connect;
        private Label FLabel;
        private ListBox InputB;
        private Label label1;
        private ListBox InputB2;
        private Label Credits;
        private ProgressBar ProgressB;
        private Label Left;
        private Label Done;
        private Label DoneL;
        private Label LeftL;
        private Panel panel1;
        private NumericUpDown QualityB;
    }
}