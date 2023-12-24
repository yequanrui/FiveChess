namespace FiveChess
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.gobangBoardGroupBox = new System.Windows.Forms.GroupBox();
            this.startButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.computerRadioButton = new System.Windows.Forms.RadioButton();
            this.personRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.whiteLabel = new System.Windows.Forms.Label();
            this.blackLabel = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lbl = new System.Windows.Forms.Label();
            this.timerT = new System.Windows.Forms.Timer(this.components);
            this.escButton = new System.Windows.Forms.Button();
            this.aboutButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gobangBoardGroupBox
            // 
            this.gobangBoardGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gobangBoardGroupBox.BackColor = System.Drawing.Color.BurlyWood;
            this.gobangBoardGroupBox.Location = new System.Drawing.Point(4, 10);
            this.gobangBoardGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gobangBoardGroupBox.Name = "gobangBoardGroupBox";
            this.gobangBoardGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gobangBoardGroupBox.Size = new System.Drawing.Size(604, 610);
            this.gobangBoardGroupBox.TabIndex = 0;
            this.gobangBoardGroupBox.TabStop = false;
            this.gobangBoardGroupBox.Text = "棋盘";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(633, 309);
            this.startButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(92, 33);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "开始";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // backButton
            // 
            this.backButton.Location = new System.Drawing.Point(633, 365);
            this.backButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(92, 33);
            this.backButton.TabIndex = 4;
            this.backButton.Text = "悔棋";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.computerRadioButton);
            this.groupBox1.Controls.Add(this.personRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(619, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(121, 100);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "下棋顺序";
            // 
            // computerRadioButton
            // 
            this.computerRadioButton.AutoSize = true;
            this.computerRadioButton.Location = new System.Drawing.Point(21, 61);
            this.computerRadioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.computerRadioButton.Name = "computerRadioButton";
            this.computerRadioButton.Size = new System.Drawing.Size(83, 24);
            this.computerRadioButton.TabIndex = 1;
            this.computerRadioButton.Text = "电脑先下";
            this.computerRadioButton.UseVisualStyleBackColor = true;
            // 
            // personRadioButton
            // 
            this.personRadioButton.AutoSize = true;
            this.personRadioButton.Checked = true;
            this.personRadioButton.Location = new System.Drawing.Point(21, 27);
            this.personRadioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.personRadioButton.Name = "personRadioButton";
            this.personRadioButton.Size = new System.Drawing.Size(83, 24);
            this.personRadioButton.TabIndex = 0;
            this.personRadioButton.TabStop = true;
            this.personRadioButton.Text = "玩家先下";
            this.personRadioButton.UseVisualStyleBackColor = true;
            this.personRadioButton.CheckedChanged += new System.EventHandler(this.personRadioButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.whiteLabel);
            this.groupBox2.Controls.Add(this.blackLabel);
            this.groupBox2.Controls.Add(this.pictureBox2);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(619, 124);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(121, 151);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "棋色";
            // 
            // whiteLabel
            // 
            this.whiteLabel.AutoSize = true;
            this.whiteLabel.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.whiteLabel.ForeColor = System.Drawing.Color.Blue;
            this.whiteLabel.Location = new System.Drawing.Point(57, 97);
            this.whiteLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.whiteLabel.Name = "whiteLabel";
            this.whiteLabel.Size = new System.Drawing.Size(52, 27);
            this.whiteLabel.TabIndex = 1;
            this.whiteLabel.Text = "电脑";
            // 
            // blackLabel
            // 
            this.blackLabel.AutoSize = true;
            this.blackLabel.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.blackLabel.ForeColor = System.Drawing.Color.Red;
            this.blackLabel.Location = new System.Drawing.Point(57, 36);
            this.blackLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blackLabel.Name = "blackLabel";
            this.blackLabel.Size = new System.Drawing.Size(52, 27);
            this.blackLabel.TabIndex = 0;
            this.blackLabel.Text = "玩家";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::FiveChess.Properties.Resources.whitestone;
            this.pictureBox2.Location = new System.Drawing.Point(9, 89);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 42);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::FiveChess.Properties.Resources.blackstone;
            this.pictureBox1.Location = new System.Drawing.Point(9, 29);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // timer
            // 
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lbl
            // 
            this.lbl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl.ForeColor = System.Drawing.Color.Red;
            this.lbl.Location = new System.Drawing.Point(624, 546);
            this.lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(110, 50);
            this.lbl.TabIndex = 7;
            this.lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerT
            // 
            this.timerT.Enabled = true;
            this.timerT.Interval = 500;
            this.timerT.Tick += new System.EventHandler(this.timerT_Tick);
            // 
            // escButton
            // 
            this.escButton.AutoSize = true;
            this.escButton.Location = new System.Drawing.Point(633, 477);
            this.escButton.Name = "escButton";
            this.escButton.Size = new System.Drawing.Size(92, 33);
            this.escButton.TabIndex = 6;
            this.escButton.Text = "退出";
            this.escButton.UseVisualStyleBackColor = true;
            this.escButton.Click += new System.EventHandler(this.escButton_Click);
            // 
            // aboutButton
            // 
            this.aboutButton.Location = new System.Drawing.Point(633, 421);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(92, 33);
            this.aboutButton.TabIndex = 5;
            this.aboutButton.Text = "关于";
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(751, 623);
            this.Controls.Add(this.aboutButton);
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gobangBoardGroupBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.escButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.backButton);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "五子棋";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.MainForm_HelpButtonClicked);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gobangBoardGroupBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton computerRadioButton;
        private System.Windows.Forms.RadioButton personRadioButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label blackLabel;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label whiteLabel;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.Timer timerT;
        private System.Windows.Forms.Button escButton;
        private System.Windows.Forms.Button aboutButton;
    }
}