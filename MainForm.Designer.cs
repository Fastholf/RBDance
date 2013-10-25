namespace DanceDance
{
    partial class main_f
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main_f));
            this.connect_rAll_b = new System.Windows.Forms.Button();
            this.DCOn_rAll_b = new System.Windows.Forms.Button();
            this.DCOff_rAll_b = new System.Windows.Forms.Button();
            this.disconnect_rAll_b = new System.Windows.Forms.Button();
            this.danceStatus_l = new System.Windows.Forms.Label();
            this.startDance_b = new System.Windows.Forms.Button();
            this.robots_gb = new System.Windows.Forms.GroupBox();
            this.RobotControlAllTogether_panel = new System.Windows.Forms.Panel();
            this.AddControlsForRobot_button = new System.Windows.Forms.Button();
            this.basicPosture_rAll_b = new System.Windows.Forms.Button();
            this.ForAll_l = new System.Windows.Forms.Label();
            this.files_gb = new System.Windows.Forms.GroupBox();
            this.AddControlsForFile_button = new System.Windows.Forms.Button();
            this.stopDance_b = new System.Windows.Forms.Button();
            this.pauseResumeDance_b = new System.Windows.Forms.Button();
            this.DanceTimer = new System.Windows.Forms.Timer(this.components);
            this.funny_p = new System.Windows.Forms.Panel();
            this.DanceControls_panel = new System.Windows.Forms.Panel();
            this.music_gb = new System.Windows.Forms.GroupBox();
            this.openMusic_b = new System.Windows.Forms.Button();
            this.isUsingMusic_chb = new System.Windows.Forms.CheckBox();
            this.music_tb = new System.Windows.Forms.TextBox();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.frames_trackBar = new System.Windows.Forms.TrackBar();
            this.frames_panel = new System.Windows.Forms.Panel();
            this.frames_label = new System.Windows.Forms.Label();
            this.robots_gb.SuspendLayout();
            this.RobotControlAllTogether_panel.SuspendLayout();
            this.files_gb.SuspendLayout();
            this.DanceControls_panel.SuspendLayout();
            this.music_gb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frames_trackBar)).BeginInit();
            this.frames_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // connect_rAll_b
            // 
            this.connect_rAll_b.Location = new System.Drawing.Point(318, 2);
            this.connect_rAll_b.Name = "connect_rAll_b";
            this.connect_rAll_b.Size = new System.Drawing.Size(55, 23);
            this.connect_rAll_b.TabIndex = 9;
            this.connect_rAll_b.Text = "Connect";
            this.connect_rAll_b.UseVisualStyleBackColor = true;
            this.connect_rAll_b.Click += new System.EventHandler(this.connect_rAll_b_Click);
            // 
            // DCOn_rAll_b
            // 
            this.DCOn_rAll_b.Location = new System.Drawing.Point(379, 2);
            this.DCOn_rAll_b.Name = "DCOn_rAll_b";
            this.DCOn_rAll_b.Size = new System.Drawing.Size(47, 23);
            this.DCOn_rAll_b.TabIndex = 10;
            this.DCOn_rAll_b.Text = "DC On";
            this.DCOn_rAll_b.UseVisualStyleBackColor = true;
            this.DCOn_rAll_b.Click += new System.EventHandler(this.DCOn_rAll_b_Click);
            // 
            // DCOff_rAll_b
            // 
            this.DCOff_rAll_b.Location = new System.Drawing.Point(521, 2);
            this.DCOff_rAll_b.Name = "DCOff_rAll_b";
            this.DCOff_rAll_b.Size = new System.Drawing.Size(47, 23);
            this.DCOff_rAll_b.TabIndex = 20;
            this.DCOff_rAll_b.Text = "DC Off";
            this.DCOff_rAll_b.UseVisualStyleBackColor = true;
            this.DCOff_rAll_b.Click += new System.EventHandler(this.DCOff_rAll_b_Click);
            // 
            // disconnect_rAll_b
            // 
            this.disconnect_rAll_b.Location = new System.Drawing.Point(574, 2);
            this.disconnect_rAll_b.Name = "disconnect_rAll_b";
            this.disconnect_rAll_b.Size = new System.Drawing.Size(69, 23);
            this.disconnect_rAll_b.TabIndex = 21;
            this.disconnect_rAll_b.Text = "Disconnect";
            this.disconnect_rAll_b.UseVisualStyleBackColor = true;
            this.disconnect_rAll_b.Click += new System.EventHandler(this.disconnect_rAll_b_Click);
            // 
            // danceStatus_l
            // 
            this.danceStatus_l.AutoSize = true;
            this.danceStatus_l.Location = new System.Drawing.Point(8, 5);
            this.danceStatus_l.Name = "danceStatus_l";
            this.danceStatus_l.Size = new System.Drawing.Size(90, 13);
            this.danceStatus_l.TabIndex = 55;
            this.danceStatus_l.Text = "Dance is stopped";
            // 
            // startDance_b
            // 
            this.startDance_b.Location = new System.Drawing.Point(104, 0);
            this.startDance_b.Name = "startDance_b";
            this.startDance_b.Size = new System.Drawing.Size(386, 23);
            this.startDance_b.TabIndex = 56;
            this.startDance_b.Text = "Start Dance";
            this.startDance_b.UseVisualStyleBackColor = true;
            this.startDance_b.Click += new System.EventHandler(this.startDance_b_Click);
            // 
            // robots_gb
            // 
            this.robots_gb.Controls.Add(this.RobotControlAllTogether_panel);
            this.robots_gb.Location = new System.Drawing.Point(12, 70);
            this.robots_gb.Name = "robots_gb";
            this.robots_gb.Size = new System.Drawing.Size(676, 61);
            this.robots_gb.TabIndex = 66;
            this.robots_gb.TabStop = false;
            this.robots_gb.Text = "Robots";
            // 
            // RobotControlAllTogether_panel
            // 
            this.RobotControlAllTogether_panel.Controls.Add(this.AddControlsForRobot_button);
            this.RobotControlAllTogether_panel.Controls.Add(this.basicPosture_rAll_b);
            this.RobotControlAllTogether_panel.Controls.Add(this.ForAll_l);
            this.RobotControlAllTogether_panel.Controls.Add(this.connect_rAll_b);
            this.RobotControlAllTogether_panel.Controls.Add(this.DCOn_rAll_b);
            this.RobotControlAllTogether_panel.Controls.Add(this.DCOff_rAll_b);
            this.RobotControlAllTogether_panel.Controls.Add(this.disconnect_rAll_b);
            this.RobotControlAllTogether_panel.Location = new System.Drawing.Point(12, 19);
            this.RobotControlAllTogether_panel.Name = "RobotControlAllTogether_panel";
            this.RobotControlAllTogether_panel.Size = new System.Drawing.Size(652, 27);
            this.RobotControlAllTogether_panel.TabIndex = 77;
            // 
            // AddControlsForRobot_button
            // 
            this.AddControlsForRobot_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddControlsForRobot_button.Image = global::DanceDance.Properties.Resources.plus;
            this.AddControlsForRobot_button.Location = new System.Drawing.Point(0, 0);
            this.AddControlsForRobot_button.Name = "AddControlsForRobot_button";
            this.AddControlsForRobot_button.Size = new System.Drawing.Size(26, 26);
            this.AddControlsForRobot_button.TabIndex = 76;
            this.AddControlsForRobot_button.UseVisualStyleBackColor = true;
            this.AddControlsForRobot_button.Click += new System.EventHandler(this.AddControlsForRobot_button_Click);
            // 
            // basicPosture_rAll_b
            // 
            this.basicPosture_rAll_b.Location = new System.Drawing.Point(433, 2);
            this.basicPosture_rAll_b.Name = "basicPosture_rAll_b";
            this.basicPosture_rAll_b.Size = new System.Drawing.Size(82, 23);
            this.basicPosture_rAll_b.TabIndex = 72;
            this.basicPosture_rAll_b.Text = "Basic Posture";
            this.basicPosture_rAll_b.UseVisualStyleBackColor = true;
            this.basicPosture_rAll_b.Click += new System.EventHandler(this.basicPosture_rAll_b_Click);
            // 
            // ForAll_l
            // 
            this.ForAll_l.AutoSize = true;
            this.ForAll_l.Location = new System.Drawing.Point(145, 7);
            this.ForAll_l.Name = "ForAll_l";
            this.ForAll_l.Size = new System.Drawing.Size(167, 13);
            this.ForAll_l.TabIndex = 60;
            this.ForAll_l.Text = "Controll all selected together ===>";
            // 
            // files_gb
            // 
            this.files_gb.Controls.Add(this.AddControlsForFile_button);
            this.files_gb.Location = new System.Drawing.Point(12, 137);
            this.files_gb.Name = "files_gb";
            this.files_gb.Size = new System.Drawing.Size(463, 54);
            this.files_gb.TabIndex = 67;
            this.files_gb.TabStop = false;
            this.files_gb.Text = "Files";
            // 
            // AddControlsForFile_button
            // 
            this.AddControlsForFile_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddControlsForFile_button.Image = global::DanceDance.Properties.Resources.plus;
            this.AddControlsForFile_button.Location = new System.Drawing.Point(6, 19);
            this.AddControlsForFile_button.Name = "AddControlsForFile_button";
            this.AddControlsForFile_button.Size = new System.Drawing.Size(26, 26);
            this.AddControlsForFile_button.TabIndex = 80;
            this.AddControlsForFile_button.UseVisualStyleBackColor = true;
            this.AddControlsForFile_button.Click += new System.EventHandler(this.AddControlsForFile_button_Click);
            // 
            // stopDance_b
            // 
            this.stopDance_b.Enabled = false;
            this.stopDance_b.Location = new System.Drawing.Point(581, 0);
            this.stopDance_b.Name = "stopDance_b";
            this.stopDance_b.Size = new System.Drawing.Size(44, 23);
            this.stopDance_b.TabIndex = 68;
            this.stopDance_b.Text = "Stop";
            this.stopDance_b.UseVisualStyleBackColor = true;
            this.stopDance_b.Click += new System.EventHandler(this.stopDance_b_Click);
            // 
            // pauseResumeDance_b
            // 
            this.pauseResumeDance_b.Enabled = false;
            this.pauseResumeDance_b.Location = new System.Drawing.Point(496, 0);
            this.pauseResumeDance_b.Name = "pauseResumeDance_b";
            this.pauseResumeDance_b.Size = new System.Drawing.Size(79, 23);
            this.pauseResumeDance_b.TabIndex = 69;
            this.pauseResumeDance_b.Text = "Pause";
            this.pauseResumeDance_b.UseVisualStyleBackColor = true;
            this.pauseResumeDance_b.Click += new System.EventHandler(this.pauseResumeDance_b_Click);
            // 
            // DanceTimer
            // 
            this.DanceTimer.Tick += new System.EventHandler(this.DanceTimer_Tick);
            // 
            // funny_p
            // 
            this.funny_p.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.funny_p.Location = new System.Drawing.Point(481, 137);
            this.funny_p.Name = "funny_p";
            this.funny_p.Size = new System.Drawing.Size(75, 75);
            this.funny_p.TabIndex = 70;
            this.funny_p.Paint += new System.Windows.Forms.PaintEventHandler(this.funny_p_Paint);
            // 
            // DanceControls_panel
            // 
            this.DanceControls_panel.Controls.Add(this.pauseResumeDance_b);
            this.DanceControls_panel.Controls.Add(this.stopDance_b);
            this.DanceControls_panel.Controls.Add(this.danceStatus_l);
            this.DanceControls_panel.Controls.Add(this.startDance_b);
            this.DanceControls_panel.Location = new System.Drawing.Point(18, 218);
            this.DanceControls_panel.Name = "DanceControls_panel";
            this.DanceControls_panel.Size = new System.Drawing.Size(625, 23);
            this.DanceControls_panel.TabIndex = 71;
            // 
            // music_gb
            // 
            this.music_gb.Controls.Add(this.openMusic_b);
            this.music_gb.Controls.Add(this.isUsingMusic_chb);
            this.music_gb.Controls.Add(this.music_tb);
            this.music_gb.Location = new System.Drawing.Point(12, 12);
            this.music_gb.Name = "music_gb";
            this.music_gb.Size = new System.Drawing.Size(676, 52);
            this.music_gb.TabIndex = 72;
            this.music_gb.TabStop = false;
            this.music_gb.Text = "Music";
            // 
            // openMusic_b
            // 
            this.openMusic_b.Location = new System.Drawing.Point(385, 17);
            this.openMusic_b.Name = "openMusic_b";
            this.openMusic_b.Size = new System.Drawing.Size(29, 23);
            this.openMusic_b.TabIndex = 74;
            this.openMusic_b.Text = "...";
            this.openMusic_b.UseVisualStyleBackColor = true;
            this.openMusic_b.Click += new System.EventHandler(this.openMusic_b_Click);
            // 
            // isUsingMusic_chb
            // 
            this.isUsingMusic_chb.AutoSize = true;
            this.isUsingMusic_chb.Location = new System.Drawing.Point(6, 22);
            this.isUsingMusic_chb.Name = "isUsingMusic_chb";
            this.isUsingMusic_chb.Size = new System.Drawing.Size(15, 14);
            this.isUsingMusic_chb.TabIndex = 73;
            this.isUsingMusic_chb.UseVisualStyleBackColor = true;
            // 
            // music_tb
            // 
            this.music_tb.Location = new System.Drawing.Point(27, 19);
            this.music_tb.Name = "music_tb";
            this.music_tb.Size = new System.Drawing.Size(358, 20);
            this.music_tb.TabIndex = 0;
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(678, 231);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(10, 10);
            this.axWindowsMediaPlayer1.TabIndex = 73;
            // 
            // frames_trackBar
            // 
            this.frames_trackBar.AutoSize = false;
            this.frames_trackBar.Location = new System.Drawing.Point(4, 3);
            this.frames_trackBar.Name = "frames_trackBar";
            this.frames_trackBar.Size = new System.Drawing.Size(532, 29);
            this.frames_trackBar.TabIndex = 74;
            this.frames_trackBar.ValueChanged += new System.EventHandler(this.frames_trackBar_ValueChanged);
            this.frames_trackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frames_trackBar_MouseUp);
            // 
            // frames_panel
            // 
            this.frames_panel.Controls.Add(this.frames_label);
            this.frames_panel.Controls.Add(this.frames_trackBar);
            this.frames_panel.Enabled = false;
            this.frames_panel.Location = new System.Drawing.Point(20, 253);
            this.frames_panel.Name = "frames_panel";
            this.frames_panel.Size = new System.Drawing.Size(664, 35);
            this.frames_panel.TabIndex = 74;
            // 
            // frames_label
            // 
            this.frames_label.AutoSize = true;
            this.frames_label.Location = new System.Drawing.Point(542, 13);
            this.frames_label.Name = "frames_label";
            this.frames_label.Size = new System.Drawing.Size(24, 13);
            this.frames_label.TabIndex = 75;
            this.frames_label.Text = "0/0";
            // 
            // main_f
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 290);
            this.Controls.Add(this.frames_panel);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Controls.Add(this.music_gb);
            this.Controls.Add(this.DanceControls_panel);
            this.Controls.Add(this.funny_p);
            this.Controls.Add(this.files_gb);
            this.Controls.Add(this.robots_gb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "main_f";
            this.Text = "RobobuilderDance";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.main_f_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.main_f_FormClosing);
            this.robots_gb.ResumeLayout(false);
            this.RobotControlAllTogether_panel.ResumeLayout(false);
            this.RobotControlAllTogether_panel.PerformLayout();
            this.files_gb.ResumeLayout(false);
            this.DanceControls_panel.ResumeLayout(false);
            this.DanceControls_panel.PerformLayout();
            this.music_gb.ResumeLayout(false);
            this.music_gb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frames_trackBar)).EndInit();
            this.frames_panel.ResumeLayout(false);
            this.frames_panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button connect_rAll_b;
        private System.Windows.Forms.Button DCOn_rAll_b;
        private System.Windows.Forms.Button DCOff_rAll_b;
        private System.Windows.Forms.Button disconnect_rAll_b;
        private System.Windows.Forms.Label danceStatus_l;
        private System.Windows.Forms.Button startDance_b;
        private System.Windows.Forms.GroupBox robots_gb;
        private System.Windows.Forms.GroupBox files_gb;
        private System.Windows.Forms.Label ForAll_l;
        private System.Windows.Forms.Button stopDance_b;
        private System.Windows.Forms.Button basicPosture_rAll_b;
        private System.Windows.Forms.Button pauseResumeDance_b;
        private System.Windows.Forms.Timer DanceTimer;
        private System.Windows.Forms.Panel funny_p;
        private System.Windows.Forms.Button AddControlsForRobot_button;
        private System.Windows.Forms.Button AddControlsForFile_button;
        private System.Windows.Forms.Panel RobotControlAllTogether_panel;
        private System.Windows.Forms.Panel DanceControls_panel;
        private System.Windows.Forms.GroupBox music_gb;
        private System.Windows.Forms.TextBox music_tb;
        private System.Windows.Forms.Button openMusic_b;
        private System.Windows.Forms.CheckBox isUsingMusic_chb;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.TrackBar frames_trackBar;
        private System.Windows.Forms.Panel frames_panel;
        private System.Windows.Forms.Label frames_label;
    }
}

