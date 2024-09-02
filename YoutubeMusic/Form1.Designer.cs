namespace YoutubeMusic
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.login_check_timer = new System.Windows.Forms.Timer(this.components);
            this.memory_manager_timer = new System.Windows.Forms.Timer(this.components);
            this.BrowserPanel = new System.Windows.Forms.Panel();
            this.TitlePanel = new System.Windows.Forms.Panel();
            this.SimpleModeButton = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.Minimize_Button = new System.Windows.Forms.Button();
            this.MaximizeButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.SpacePanel = new System.Windows.Forms.Panel();
            this.TitlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // login_check_timer
            // 
            this.login_check_timer.Interval = 200;
            this.login_check_timer.Tick += new System.EventHandler(this.login_check_timer_Tick);
            // 
            // memory_manager_timer
            // 
            this.memory_manager_timer.Interval = 500;
            this.memory_manager_timer.Tick += new System.EventHandler(this.memory_manager_timer_Tick);
            // 
            // BrowserPanel
            // 
            this.BrowserPanel.BackColor = System.Drawing.SystemColors.Control;
            this.BrowserPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowserPanel.Location = new System.Drawing.Point(1, 34);
            this.BrowserPanel.Name = "BrowserPanel";
            this.BrowserPanel.Size = new System.Drawing.Size(798, 415);
            this.BrowserPanel.TabIndex = 0;
            // 
            // TitlePanel
            // 
            this.TitlePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.TitlePanel.Controls.Add(this.SimpleModeButton);
            this.TitlePanel.Controls.Add(this.BackButton);
            this.TitlePanel.Controls.Add(this.Minimize_Button);
            this.TitlePanel.Controls.Add(this.MaximizeButton);
            this.TitlePanel.Controls.Add(this.ExitButton);
            this.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitlePanel.Location = new System.Drawing.Point(1, 1);
            this.TitlePanel.Name = "TitlePanel";
            this.TitlePanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.TitlePanel.Size = new System.Drawing.Size(798, 33);
            this.TitlePanel.TabIndex = 1;
            this.TitlePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitlePanel_MouseDown);
            // 
            // SimpleModeButton
            // 
            this.SimpleModeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.SimpleModeButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.SimpleModeButton.FlatAppearance.BorderSize = 0;
            this.SimpleModeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SimpleModeButton.Font = new System.Drawing.Font("Segoe UI Symbol", 14F);
            this.SimpleModeButton.ForeColor = System.Drawing.Color.White;
            this.SimpleModeButton.Location = new System.Drawing.Point(32, 0);
            this.SimpleModeButton.Name = "SimpleModeButton";
            this.SimpleModeButton.Size = new System.Drawing.Size(32, 32);
            this.SimpleModeButton.TabIndex = 5;
            this.SimpleModeButton.Text = "♪";
            this.SimpleModeButton.UseVisualStyleBackColor = false;
            this.SimpleModeButton.Click += new System.EventHandler(this.SimpleModeButton_Click);
            // 
            // BackButton
            // 
            this.BackButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.BackButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.BackButton.FlatAppearance.BorderSize = 0;
            this.BackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BackButton.Font = new System.Drawing.Font("Calibri", 14F);
            this.BackButton.ForeColor = System.Drawing.Color.White;
            this.BackButton.Location = new System.Drawing.Point(0, 0);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(32, 32);
            this.BackButton.TabIndex = 4;
            this.BackButton.Text = "←";
            this.BackButton.UseVisualStyleBackColor = false;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // Minimize_Button
            // 
            this.Minimize_Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.Minimize_Button.Dock = System.Windows.Forms.DockStyle.Right;
            this.Minimize_Button.FlatAppearance.BorderSize = 0;
            this.Minimize_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Minimize_Button.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.Minimize_Button.ForeColor = System.Drawing.Color.White;
            this.Minimize_Button.Location = new System.Drawing.Point(702, 0);
            this.Minimize_Button.Name = "Minimize_Button";
            this.Minimize_Button.Size = new System.Drawing.Size(32, 32);
            this.Minimize_Button.TabIndex = 3;
            this.Minimize_Button.Text = "─";
            this.Minimize_Button.UseVisualStyleBackColor = false;
            this.Minimize_Button.Click += new System.EventHandler(this.Minimize_Button_Click);
            // 
            // MaximizeButton
            // 
            this.MaximizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.MaximizeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.MaximizeButton.FlatAppearance.BorderSize = 0;
            this.MaximizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MaximizeButton.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeButton.ForeColor = System.Drawing.Color.White;
            this.MaximizeButton.Location = new System.Drawing.Point(734, 0);
            this.MaximizeButton.Name = "MaximizeButton";
            this.MaximizeButton.Size = new System.Drawing.Size(32, 32);
            this.MaximizeButton.TabIndex = 2;
            this.MaximizeButton.Text = "⬜";
            this.MaximizeButton.UseVisualStyleBackColor = false;
            this.MaximizeButton.Click += new System.EventHandler(this.MaximizeButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ExitButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.ExitButton.FlatAppearance.BorderSize = 0;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.Font = new System.Drawing.Font("Segoe UI", 13.25F);
            this.ExitButton.ForeColor = System.Drawing.Color.White;
            this.ExitButton.Location = new System.Drawing.Point(766, 0);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(32, 32);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "✕";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // SpacePanel
            // 
            this.SpacePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(19)))));
            this.SpacePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SpacePanel.Location = new System.Drawing.Point(1, 34);
            this.SpacePanel.Name = "SpacePanel";
            this.SpacePanel.Size = new System.Drawing.Size(798, 1);
            this.SpacePanel.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SpacePanel);
            this.Controls.Add(this.BrowserPanel);
            this.Controls.Add(this.TitlePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Youtube Music";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.YoutubeMusic_FormClosing);
            this.Load += new System.EventHandler(this.YoutubeMusic_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.MouseHover += new System.EventHandler(this.Form1_MouseHover);
            this.TitlePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer login_check_timer;
        private System.Windows.Forms.Timer memory_manager_timer;
        private System.Windows.Forms.Panel BrowserPanel;
        private System.Windows.Forms.Panel TitlePanel;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button Minimize_Button;
        private System.Windows.Forms.Button MaximizeButton;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Panel SpacePanel;
        private System.Windows.Forms.Button SimpleModeButton;
    }
}

