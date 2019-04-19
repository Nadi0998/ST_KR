namespace ST_diplom
{
    partial class FormLogin
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
            this.loginField = new System.Windows.Forms.TextBox();
            this.loginLabel = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.BackComPortName = new System.Windows.Forms.TextBox();
            this.ForwardComPortName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // loginField
            // 
            this.loginField.Location = new System.Drawing.Point(74, 79);
            this.loginField.Name = "loginField";
            this.loginField.Size = new System.Drawing.Size(122, 20);
            this.loginField.TabIndex = 0;
            this.loginField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.loginField_KeyDown);
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.Location = new System.Drawing.Point(71, 63);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(60, 13);
            this.loginLabel.TabIndex = 1;
            this.loginLabel.Text = "Ваш логин";
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(96, 105);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(73, 30);
            this.loginButton.TabIndex = 2;
            this.loginButton.Text = "Войти";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // BackComPortName
            // 
            this.BackComPortName.Location = new System.Drawing.Point(109, 166);
            this.BackComPortName.Name = "BackComPortName";
            this.BackComPortName.Size = new System.Drawing.Size(121, 20);
            this.BackComPortName.TabIndex = 3;
            this.BackComPortName.TextChanged += new System.EventHandler(this.BackComPortName_TextChanged_1);
            // 
            // ForwardComPortName
            // 
            this.ForwardComPortName.Location = new System.Drawing.Point(109, 193);
            this.ForwardComPortName.Name = "ForwardComPortName";
            this.ForwardComPortName.Size = new System.Drawing.Size(120, 20);
            this.ForwardComPortName.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Back COM";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Forward COM";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label3.Location = new System.Drawing.Point(29, 244);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "created by: ты да я и всё";
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ForwardComPortName);
            this.Controls.Add(this.BackComPortName);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.loginLabel);
            this.Controls.Add(this.loginField);
            this.Name = "FormLogin";
            this.Text = "Чат Волчат";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox loginField;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.TextBox BackComPortName;
        private System.Windows.Forms.TextBox ForwardComPortName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}