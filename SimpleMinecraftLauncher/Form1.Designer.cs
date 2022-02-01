namespace SimpleMinecraftLauncher
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.windowTopPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.bMin = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.bgPanel = new System.Windows.Forms.Panel();
            this.ctxButton = new System.Windows.Forms.Button();
            this.loadingPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.loggerLabel = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.versionList = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.configVersion = new System.Windows.Forms.Label();
            this.devLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new SimpleMinecraftLauncher.RippleButton();
            this.windowTopPanel.SuspendLayout();
            this.bgPanel.SuspendLayout();
            this.loadingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // windowTopPanel
            // 
            this.windowTopPanel.BackColor = System.Drawing.Color.Gainsboro;
            this.windowTopPanel.Controls.Add(this.label1);
            this.windowTopPanel.Controls.Add(this.bMin);
            this.windowTopPanel.Controls.Add(this.bClose);
            this.windowTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.windowTopPanel.Location = new System.Drawing.Point(0, 0);
            this.windowTopPanel.Name = "windowTopPanel";
            this.windowTopPanel.Size = new System.Drawing.Size(710, 32);
            this.windowTopPanel.TabIndex = 0;
            this.windowTopPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.windowTopPanel_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Simple Minecraft Launcher";
            // 
            // bMin
            // 
            this.bMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bMin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.bMin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bMin.FlatAppearance.BorderSize = 0;
            this.bMin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(255)))));
            this.bMin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(255)))));
            this.bMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bMin.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bMin.Location = new System.Drawing.Point(657, 6);
            this.bMin.Name = "bMin";
            this.bMin.Size = new System.Drawing.Size(20, 20);
            this.bMin.TabIndex = 2;
            this.bMin.UseVisualStyleBackColor = false;
            this.bMin.Click += new System.EventHandler(this.bMin_Click);
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.bClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bClose.FlatAppearance.BorderSize = 0;
            this.bClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(60)))), ((int)(((byte)(50)))));
            this.bClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bClose.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bClose.Location = new System.Drawing.Point(683, 6);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(20, 20);
            this.bClose.TabIndex = 0;
            this.bClose.UseVisualStyleBackColor = false;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bgPanel
            // 
            this.bgPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bgPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.bgPanel.Controls.Add(this.ctxButton);
            this.bgPanel.Controls.Add(this.loadingPanel);
            this.bgPanel.Controls.Add(this.loggerLabel);
            this.bgPanel.Controls.Add(this.logBox);
            this.bgPanel.Controls.Add(this.label4);
            this.bgPanel.Controls.Add(this.label3);
            this.bgPanel.Controls.Add(this.versionList);
            this.bgPanel.Controls.Add(this.textBox1);
            this.bgPanel.Controls.Add(this.button1);
            this.bgPanel.Location = new System.Drawing.Point(13, 44);
            this.bgPanel.Name = "bgPanel";
            this.bgPanel.Size = new System.Drawing.Size(685, 355);
            this.bgPanel.TabIndex = 1;
            // 
            // ctxButton
            // 
            this.ctxButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctxButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ctxButton.BackgroundImage = global::SimpleMinecraftLauncher.Properties.Resources.more_options_button;
            this.ctxButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ctxButton.FlatAppearance.BorderSize = 0;
            this.ctxButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(255)))));
            this.ctxButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(255)))));
            this.ctxButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ctxButton.Font = new System.Drawing.Font("Bahnschrift Condensed", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctxButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ctxButton.Location = new System.Drawing.Point(439, 166);
            this.ctxButton.Name = "ctxButton";
            this.ctxButton.Size = new System.Drawing.Size(20, 21);
            this.ctxButton.TabIndex = 4;
            this.ctxButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ctxButton.UseVisualStyleBackColor = false;
            this.ctxButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // loadingPanel
            // 
            this.loadingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingPanel.Controls.Add(this.label5);
            this.loadingPanel.Controls.Add(this.progressBar);
            this.loadingPanel.Location = new System.Drawing.Point(3, 3);
            this.loadingPanel.Name = "loadingPanel";
            this.loadingPanel.Size = new System.Drawing.Size(679, 41);
            this.loadingPanel.TabIndex = 12;
            this.loadingPanel.Visible = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label5.Location = new System.Drawing.Point(180, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(288, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Пожалуйста, подождите, происходит фоновая работа...";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(3, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(673, 23);
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 11;
            // 
            // loggerLabel
            // 
            this.loggerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loggerLabel.AutoSize = true;
            this.loggerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.loggerLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loggerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.loggerLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.loggerLabel.Location = new System.Drawing.Point(476, 90);
            this.loggerLabel.Name = "loggerLabel";
            this.loggerLabel.Size = new System.Drawing.Size(56, 16);
            this.loggerLabel.TabIndex = 5;
            this.loggerLabel.Text = "Logger";
            this.loggerLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // logBox
            // 
            this.logBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(50)))));
            this.logBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logBox.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.logBox.Location = new System.Drawing.Point(479, 109);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.Size = new System.Drawing.Size(203, 243);
            this.logBox.TabIndex = 10;
            this.logBox.Text = "Запуск\n";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(200, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Выберите клиент";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(200, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Ваш никнейм";
            // 
            // versionList
            // 
            this.versionList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.versionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.versionList.FormattingEnabled = true;
            this.versionList.Location = new System.Drawing.Point(203, 166);
            this.versionList.Name = "versionList";
            this.versionList.Size = new System.Drawing.Size(230, 21);
            this.versionList.TabIndex = 6;
            this.versionList.SelectedIndexChanged += new System.EventHandler(this.versionList_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textBox1.Location = new System.Drawing.Point(203, 111);
            this.textBox1.MaxLength = 16;
            this.textBox1.Name = "textBox1";
            this.textBox1.ShortcutsEnabled = false;
            this.textBox1.Size = new System.Drawing.Size(256, 22);
            this.textBox1.TabIndex = 5;
            this.textBox1.WordWrap = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // configVersion
            // 
            this.configVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.configVersion.AutoSize = true;
            this.configVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.configVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.configVersion.Location = new System.Drawing.Point(8, 400);
            this.configVersion.Name = "configVersion";
            this.configVersion.Size = new System.Drawing.Size(108, 16);
            this.configVersion.TabIndex = 4;
            this.configVersion.Text = "config version ";
            // 
            // devLabel
            // 
            this.devLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.devLabel.AutoSize = true;
            this.devLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.devLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.devLabel.Location = new System.Drawing.Point(538, 401);
            this.devLabel.Name = "devLabel";
            this.devLabel.Size = new System.Drawing.Size(160, 16);
            this.devLabel.TabIndex = 5;
            this.devLabel.Text = "dev by artybyte (2022)";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.label6.Location = new System.Drawing.Point(191, 399);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(303, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "special for Acasual Emergency Foundation";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(141)))), ((int)(((byte)(148)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.button1.HighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(200)))), ((int)(((byte)(225)))));
            this.button1.Location = new System.Drawing.Point(203, 212);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button1.Name = "button1";
            this.button1.RippleColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.button1.Size = new System.Drawing.Size(256, 36);
            this.button1.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(710, 420);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.devLabel);
            this.Controls.Add(this.configVersion);
            this.Controls.Add(this.bgPanel);
            this.Controls.Add(this.windowTopPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(710, 420);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Launcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.windowTopPanel.ResumeLayout(false);
            this.windowTopPanel.PerformLayout();
            this.bgPanel.ResumeLayout(false);
            this.bgPanel.PerformLayout();
            this.loadingPanel.ResumeLayout(false);
            this.loadingPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel windowTopPanel;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel bgPanel;
        private System.Windows.Forms.Label configVersion;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox versionList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label loggerLabel;
        public System.Windows.Forms.RichTextBox logBox;
        internal System.Windows.Forms.Panel loadingPanel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label devLabel;
        public RippleButton button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ctxButton;
    }
}

