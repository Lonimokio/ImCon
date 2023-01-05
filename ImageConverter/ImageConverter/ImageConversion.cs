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
            this.Back = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.QualityB)).BeginInit();
            this.SuspendLayout();
            // 
            // convert
            // 
            this.convert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.convert.Location = new System.Drawing.Point(287, 291);
            this.convert.Name = "convert";
            this.convert.Size = new System.Drawing.Size(213, 80);
            this.convert.TabIndex = 0;
            this.convert.Text = "Convert";
            this.convert.UseVisualStyleBackColor = true;
            this.convert.Visible = false;
            this.convert.Click += new System.EventHandler(this.convert_Click);
            // 
            // TypeB
            // 
            this.TypeB.FormattingEnabled = true;
            this.TypeB.Items.AddRange(new object[] {
            "webp"});
            this.TypeB.Location = new System.Drawing.Point(367, 240);
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
            this.SLabel.Location = new System.Drawing.Point(243, 207);
            this.SLabel.Name = "SLabel";
            this.SLabel.Size = new System.Drawing.Size(317, 30);
            this.SLabel.TabIndex = 2;
            this.SLabel.Text = "What format to be converted to?";
            this.SLabel.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(281, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(236, 65);
            this.label3.TabIndex = 5;
            this.label3.Text = "Converter";
            // 
            // Connect
            // 
            this.Connect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Connect.Location = new System.Drawing.Point(315, 215);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(156, 70);
            this.Connect.TabIndex = 6;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // FLabel
            // 
            this.FLabel.AutoSize = true;
            this.FLabel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FLabel.Location = new System.Drawing.Point(281, 148);
            this.FLabel.Name = "FLabel";
            this.FLabel.Size = new System.Drawing.Size(212, 30);
            this.FLabel.TabIndex = 8;
            this.FLabel.Text = "Enter Database name";
            // 
            // Back
            // 
            this.Back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Back.Location = new System.Drawing.Point(673, 26);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(99, 60);
            this.Back.TabIndex = 9;
            this.Back.Text = "Back";
            this.Back.UseVisualStyleBackColor = true;
            this.Back.Visible = false;
            this.Back.Click += new System.EventHandler(this.button1_Click);
            // 
            // InputB
            // 
            this.InputB.FormattingEnabled = true;
            this.InputB.HorizontalScrollbar = true;
            this.InputB.ItemHeight = 15;
            this.InputB.Location = new System.Drawing.Point(72, 122);
            this.InputB.Name = "InputB";
            this.InputB.Size = new System.Drawing.Size(120, 94);
            this.InputB.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Images";
            // 
            // InputB2
            // 
            this.InputB2.FormattingEnabled = true;
            this.InputB2.ItemHeight = 15;
            this.InputB2.Location = new System.Drawing.Point(72, 222);
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
            this.Credits.Text = "Alpha 1.0.0\r\nIntelectual property of Arttu Mutka\r\nOriginal idea by Petri Kaija";
            // 
            // ProgressB
            // 
            this.ProgressB.ForeColor = System.Drawing.Color.Red;
            this.ProgressB.Location = new System.Drawing.Point(72, 322);
            this.ProgressB.Name = "ProgressB";
            this.ProgressB.Size = new System.Drawing.Size(120, 23);
            this.ProgressB.TabIndex = 14;
            this.ProgressB.Visible = false;
            // 
            // Left
            // 
            this.Left.AutoSize = true;
            this.Left.Location = new System.Drawing.Point(334, 111);
            this.Left.Name = "Left";
            this.Left.Size = new System.Drawing.Size(13, 15);
            this.Left.TabIndex = 15;
            this.Left.Text = "0";
            // 
            // Done
            // 
            this.Done.AutoSize = true;
            this.Done.Location = new System.Drawing.Point(446, 111);
            this.Done.Name = "Done";
            this.Done.Size = new System.Drawing.Size(13, 15);
            this.Done.TabIndex = 16;
            this.Done.Text = "0";
            // 
            // DoneL
            // 
            this.DoneL.AutoSize = true;
            this.DoneL.Location = new System.Drawing.Point(436, 94);
            this.DoneL.Name = "DoneL";
            this.DoneL.Size = new System.Drawing.Size(35, 15);
            this.DoneL.TabIndex = 18;
            this.DoneL.Text = "Done";
            this.DoneL.Visible = false;
            // 
            // LeftL
            // 
            this.LeftL.AutoSize = true;
            this.LeftL.Location = new System.Drawing.Point(329, 94);
            this.LeftL.Name = "LeftL";
            this.LeftL.Size = new System.Drawing.Size(27, 15);
            this.LeftL.TabIndex = 17;
            this.LeftL.Text = "Left";
            this.LeftL.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Location = new System.Drawing.Point(-6, 388);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(817, 81);
            this.panel1.TabIndex = 19;
            // 
            // QualityB
            // 
            this.QualityB.Location = new System.Drawing.Point(367, 262);
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
            this.DBName.Location = new System.Drawing.Point(366, 181);
            this.DBName.Name = "DBName";
            this.DBName.Size = new System.Drawing.Size(55, 23);
            this.DBName.TabIndex = 21;
            // 
            // ImageConversion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.CancelButton = this.Back;
            this.ClientSize = new System.Drawing.Size(800, 448);
            this.Controls.Add(this.DBName);
            this.Controls.Add(this.Credits);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.DoneL);
            this.Controls.Add(this.LeftL);
            this.Controls.Add(this.Done);
            this.Controls.Add(this.Left);
            this.Controls.Add(this.ProgressB);
            this.Controls.Add(this.InputB2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InputB);
            this.Controls.Add(this.Back);
            this.Controls.Add(this.FLabel);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SLabel);
            this.Controls.Add(this.TypeB);
            this.Controls.Add(this.convert);
            this.Controls.Add(this.QualityB);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Green;
            this.Name = "ImageConversion";
            this.Text = "ImageConverter";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QualityB)).EndInit();
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
        private Button Back;
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