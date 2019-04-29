namespace ST_Cursach
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
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
            this.loginField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.loginField.Location = new System.Drawing.Point(165, 75);
            this.loginField.Name = "loginField";
            this.loginField.Size = new System.Drawing.Size(122, 22);
            this.loginField.TabIndex = 0;
            this.loginField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.loginField_KeyDown);
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.BackColor = System.Drawing.Color.Lavender;
            this.loginLabel.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginLabel.Location = new System.Drawing.Point(38, 76);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(96, 21);
            this.loginLabel.TabIndex = 1;
            this.loginLabel.Text = "Ваш логин";
            // 
            // loginButton
            // 
            this.loginButton.BackColor = System.Drawing.Color.LightSkyBlue;
            this.loginButton.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginButton.Location = new System.Drawing.Point(129, 205);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(76, 34);
            this.loginButton.TabIndex = 4;
            this.loginButton.Text = "Войти";
            this.loginButton.UseVisualStyleBackColor = false;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            this.loginButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.loginField_KeyDown);
            // 
            // BackComPortName
            // 
            this.BackComPortName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BackComPortName.Location = new System.Drawing.Point(165, 113);
            this.BackComPortName.Name = "BackComPortName";
            this.BackComPortName.Size = new System.Drawing.Size(121, 22);
            this.BackComPortName.TabIndex = 2;
            this.BackComPortName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.loginField_KeyDown);
            // 
            // ForwardComPortName
            // 
            this.ForwardComPortName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForwardComPortName.Location = new System.Drawing.Point(165, 151);
            this.ForwardComPortName.Name = "ForwardComPortName";
            this.ForwardComPortName.Size = new System.Drawing.Size(120, 22);
            this.ForwardComPortName.TabIndex = 3;
            this.ForwardComPortName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.loginField_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Lavender;
            this.label1.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(39, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "Back COM";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Lavender;
            this.label2.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "Forward COM";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Lavender;
            this.label3.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(111, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "Вход в чат";
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImage = global::ST_Cursach.Properties.Resources.background1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(333, 302);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ForwardComPortName);
            this.Controls.Add(this.BackComPortName);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.loginLabel);
            this.Controls.Add(this.loginField);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLogin";
            this.Text = "Chat";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormLogin_KeyDown);
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