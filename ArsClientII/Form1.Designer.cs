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
            label7 = new Label();
            label16 = new Label();
            label15 = new Label();
            tabControl1 = new TabControl();
            label3 = new Label();
            label8 = new Label();
            label9 = new Label();
            label4 = new Label();
            label5 = new Label();
            groupBox1 = new GroupBox();
            label6 = new Label();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            groupBox2 = new GroupBox();
            sqlCommand1 = new Microsoft.Data.SqlClient.SqlCommand();
            groupBox3 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            tabPage3.SuspendLayout();
            tabControl1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(1, 347);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBox1.Size = new Size(561, 82);
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
            label17.Location = new Point(9, 77);
            label17.Name = "label17";
            label17.Size = new Size(88, 15);
            label17.TabIndex = 5;
            label17.Text = "MV(100D15TM)";
            label17.Click += label17_Click;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(9, 39);
            label19.Name = "label19";
            label19.Size = new Size(83, 15);
            label19.TabIndex = 7;
            label19.Text = "MA(100D5TM)";
            label19.Click += label19_Click;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(10, 111);
            label20.Name = "label20";
            label20.Size = new Size(82, 15);
            label20.TabIndex = 8;
            label20.Text = "MV(100D1TM)";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(123, 39);
            label21.Name = "label21";
            label21.Size = new Size(22, 15);
            label21.TabIndex = 9;
            label21.Text = "     ";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(123, 77);
            label22.Name = "label22";
            label22.Size = new Size(22, 15);
            label22.TabIndex = 10;
            label22.Text = "     ";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(label7);
            tabPage3.Controls.Add(label16);
            tabPage3.Controls.Add(label15);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(143, 85);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Vocie";
            tabPage3.UseVisualStyleBackColor = true;
            tabPage3.Click += tabPage3_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Historic", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(1, 33);
            label7.Name = "label7";
            label7.Size = new Size(0, 13);
            label7.TabIndex = 2;
            label7.Click += label7_Click;
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
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(151, 113);
            tabControl1.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(123, 111);
            label3.Name = "label3";
            label3.Size = new Size(22, 15);
            label3.TabIndex = 11;
            label3.Text = "     ";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(10, 147);
            label8.Name = "label8";
            label8.Size = new Size(74, 15);
            label8.TabIndex = 16;
            label8.Text = "MV(200TM5)";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(123, 147);
            label9.Name = "label9";
            label9.Size = new Size(22, 15);
            label9.TabIndex = 17;
            label9.Text = "     ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(10, 175);
            label4.Name = "label4";
            label4.Size = new Size(87, 15);
            label4.TabIndex = 18;
            label4.Text = "MV(200DTM15)";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(123, 175);
            label5.Name = "label5";
            label5.Size = new Size(22, 15);
            label5.TabIndex = 19;
            label5.Text = "     ";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label19);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label21);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label17);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(label22);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label20);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(169, 30);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(228, 308);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            groupBox1.Text = "Bitcoin Moving Averages 100 & 200 Days";
            groupBox1.Enter += groupBox1_Enter_1;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(178, 9);
            label6.Name = "label6";
            label6.Size = new Size(48, 15);
            label6.TabIndex = 22;
            label6.Text = "Balance";
            label6.Click += label6_Click;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(104, 22);
            webView21.Name = "webView21";
            webView21.Size = new Size(546, 301);
            webView21.TabIndex = 23;
            webView21.ZoomFactor = 1D;
            webView21.Click += webView21_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(webView21);
            groupBox2.Location = new Point(689, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(559, 329);
            groupBox2.TabIndex = 39;
            groupBox2.TabStop = false;
            groupBox2.Text = "Chart";
            // 
            // sqlCommand1
            // 
            sqlCommand1.CommandTimeout = 30;
            sqlCommand1.EnableOptimizedParameterBinding = false;
            // 
            // groupBox3
            // 
            groupBox3.Location = new Point(400, 24);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(283, 317);
            groupBox3.TabIndex = 40;
            groupBox3.TabStop = false;
            groupBox3.Text = "groupBox3";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1260, 432);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(label6);
            Controls.Add(groupBox1);
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
            tabControl1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            groupBox2.ResumeLayout(false);
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
        private TabPage tabPage3;
        private Label label16;
        private Label label15;
        private Label label9;
        private Label label8;
        private Label label5;
        private Label label4;
        private GroupBox groupBox1;
        private Label label6;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private Label label7;
        private GroupBox groupBox2;
        private Microsoft.Data.SqlClient.SqlCommand sqlCommand1;
        private GroupBox groupBox3;
    }
}