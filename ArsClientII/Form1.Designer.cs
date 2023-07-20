namespace ArsClientII
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            richTextBox1 = new RichTextBox();
            errorProvider1 = new ErrorProvider(components);
            label17 = new Label();
            label19 = new Label();
            label20 = new Label();
            label21 = new Label();
            label22 = new Label();
            tabPage3 = new TabPage();
            label16 = new Label();
            label15 = new Label();
            tabPage1 = new TabPage();
            button9 = new Button();
            button7 = new Button();
            button6 = new Button();
            button5 = new Button();
            label2 = new Label();
            label1 = new Label();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            tabControl1 = new TabControl();
            label3 = new Label();
            label8 = new Label();
            label9 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            tabPage3.SuspendLayout();
            tabPage1.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(851, 12);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Horizontal;
            richTextBox1.Size = new Size(397, 379);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(334, 60);
            label17.Name = "label17";
            label17.Size = new Size(88, 15);
            label17.TabIndex = 5;
            label17.Text = "MV(100D15TM)";
            label17.Click += label17_Click;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(334, 36);
            label19.Name = "label19";
            label19.Size = new Size(83, 15);
            label19.TabIndex = 7;
            label19.Text = "MA(100D5TM)";
            label19.Click += label19_Click;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(335, 90);
            label20.Name = "label20";
            label20.Size = new Size(82, 15);
            label20.TabIndex = 8;
            label20.Text = "MV(100D1TM)";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(473, 36);
            label21.Name = "label21";
            label21.Size = new Size(44, 15);
            label21.TabIndex = 9;
            label21.Text = "label21";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(473, 65);
            label22.Name = "label22";
            label22.Size = new Size(44, 15);
            label22.TabIndex = 10;
            label22.Text = "label22";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(label16);
            tabPage3.Controls.Add(label15);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(308, 210);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Vocie";
            tabPage3.UseVisualStyleBackColor = true;
            tabPage3.Click += tabPage3_Click;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(21, 119);
            label16.Name = "label16";
            label16.Size = new Size(13, 15);
            label16.TabIndex = 1;
            label16.Text = "  ";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(27, 18);
            label15.Name = "label15";
            label15.Size = new Size(0, 15);
            label15.TabIndex = 0;
            label15.Click += label15_Click;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(button9);
            tabPage1.Controls.Add(button7);
            tabPage1.Controls.Add(button6);
            tabPage1.Controls.Add(button5);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(textBox2);
            tabPage1.Controls.Add(textBox1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(308, 210);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Downloader";
            tabPage1.UseVisualStyleBackColor = true;
            tabPage1.Click += tabPage1_Click;
            // 
            // button9
            // 
            button9.Location = new Point(10, 177);
            button9.Name = "button9";
            button9.Size = new Size(292, 23);
            button9.TabIndex = 7;
            button9.Text = "Disk";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button7
            // 
            button7.Location = new Point(10, 148);
            button7.Name = "button7";
            button7.Size = new Size(292, 23);
            button7.TabIndex = 6;
            button7.Text = "Text Checker";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button6
            // 
            button6.Location = new Point(10, 119);
            button6.Name = "button6";
            button6.Size = new Size(292, 23);
            button6.TabIndex = 5;
            button6.Text = "ICO Maker";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.Location = new Point(10, 91);
            button5.Name = "button5";
            button5.Size = new Size(292, 23);
            button5.TabIndex = 4;
            button5.Text = "MP# Download";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 58);
            label2.Name = "label2";
            label2.Size = new Size(49, 15);
            label2.TabIndex = 3;
            label2.Text = "Save (P)";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 23);
            label1.Name = "label1";
            label1.Size = new Size(50, 15);
            label1.TabIndex = 2;
            label1.Text = "Path (D)";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(102, 50);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(200, 23);
            textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(102, 21);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(200, 23);
            textBox1.TabIndex = 0;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(316, 238);
            tabControl1.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(473, 90);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 11;
            label3.Text = "label3";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(583, 34);
            label8.Name = "label8";
            label8.Size = new Size(74, 15);
            label8.TabIndex = 16;
            label8.Text = "MV(200TM5)";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(730, 34);
            label9.Name = "label9";
            label9.Size = new Size(38, 15);
            label9.TabIndex = 17;
            label9.Text = "label9";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(583, 65);
            label4.Name = "label4";
            label4.Size = new Size(87, 15);
            label4.TabIndex = 18;
            label4.Text = "MV(200DTM15)";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(730, 65);
            label5.Name = "label5";
            label5.Size = new Size(38, 15);
            label5.TabIndex = 19;
            label5.Text = "label5";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(334, 15);
            label6.Name = "label6";
            label6.Size = new Size(212, 15);
            label6.TabIndex = 20;
            label6.Text = "Bitcoin Moving Averages 100 & 200 Days";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1260, 432);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label3);
            Controls.Add(label22);
            Controls.Add(label21);
            Controls.Add(label20);
            Controls.Add(label19);
            Controls.Add(label17);
            Controls.Add(tabControl1);
            Controls.Add(richTextBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form1";
            Text = resources.GetString("$this.Text");
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox richTextBox1;
        private ErrorProvider errorProvider1;
        private Label label17;
        private Label label20;
        private Label label19;
        private Label label22;
        private Label label21;
        private Label label3;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button button9;
        private Button button7;
        private Button button6;
        private Button button5;
        private Label label2;
        private Label label1;
        private TextBox textBox2;
        private TextBox textBox1;
        private TabPage tabPage3;
        private Label label16;
        private Label label15;
        private Label label9;
        private Label label8;
        private Label label5;
        private Label label4;
        private Label label6;
    }
}