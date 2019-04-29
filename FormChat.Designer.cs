namespace ST_Cursach
{
    partial class FormChat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChat));
            this.chatField = new System.Windows.Forms.TextBox();
            this.onlineUsersList = new System.Windows.Forms.ListBox();
            this.msgInputField = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.switchConnectionButton = new System.Windows.Forms.Button();
            this.clearChatFieldBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chatField
            // 
            this.chatField.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.chatField.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chatField.Location = new System.Drawing.Point(12, 12);
            this.chatField.Multiline = true;
            this.chatField.Name = "chatField";
            this.chatField.ReadOnly = true;
            this.chatField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.chatField.Size = new System.Drawing.Size(419, 277);
            this.chatField.TabIndex = 0;
            // 
            // onlineUsersList
            // 
            this.onlineUsersList.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.onlineUsersList.FormattingEnabled = true;
            this.onlineUsersList.ItemHeight = 14;
            this.onlineUsersList.Location = new System.Drawing.Point(445, 12);
            this.onlineUsersList.Name = "onlineUsersList";
            this.onlineUsersList.Size = new System.Drawing.Size(95, 102);
            this.onlineUsersList.TabIndex = 1;
            this.onlineUsersList.DoubleClick += new System.EventHandler(this.onlineUsersList_DoubleClick);
            // 
            // msgInputField
            // 
            this.msgInputField.Enabled = false;
            this.msgInputField.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.msgInputField.Location = new System.Drawing.Point(12, 331);
            this.msgInputField.Multiline = true;
            this.msgInputField.Name = "msgInputField";
            this.msgInputField.Size = new System.Drawing.Size(419, 87);
            this.msgInputField.TabIndex = 2;
            this.msgInputField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.msgInputField_KeyDown);
            // 
            // sendButton
            // 
            this.sendButton.Enabled = false;
            this.sendButton.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.sendButton.Location = new System.Drawing.Point(437, 331);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(105, 40);
            this.sendButton.TabIndex = 3;
            this.sendButton.Text = "Отправить";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // switchConnectionButton
            // 
            this.switchConnectionButton.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.switchConnectionButton.Location = new System.Drawing.Point(445, 126);
            this.switchConnectionButton.Name = "switchConnectionButton";
            this.switchConnectionButton.Size = new System.Drawing.Size(97, 28);
            this.switchConnectionButton.TabIndex = 5;
            this.switchConnectionButton.Text = "Отключиться";
            this.switchConnectionButton.UseVisualStyleBackColor = true;
            this.switchConnectionButton.Click += new System.EventHandler(this.SwitchConnectionButton_Click);
            // 
            // clearChatFieldBtn
            // 
            this.clearChatFieldBtn.BackgroundImage = global::ST_Cursach.Properties.Resources.edit_clear_7605;
            this.clearChatFieldBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.clearChatFieldBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clearChatFieldBtn.Location = new System.Drawing.Point(445, 255);
            this.clearChatFieldBtn.Name = "clearChatFieldBtn";
            this.clearChatFieldBtn.Size = new System.Drawing.Size(34, 34);
            this.clearChatFieldBtn.TabIndex = 6;
            this.clearChatFieldBtn.UseVisualStyleBackColor = true;
            this.clearChatFieldBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Lavender;
            this.label1.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 310);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "Введите сообщение";
            // 
            // FormChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ST_Cursach.Properties.Resources.background1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(552, 430);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clearChatFieldBtn);
            this.Controls.Add(this.switchConnectionButton);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.msgInputField);
            this.Controls.Add(this.onlineUsersList);
            this.Controls.Add(this.chatField);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormChat";
            this.Text = "Chat";
            this.Load += new System.EventHandler(this.FormChat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox onlineUsersList;
        public System.Windows.Forms.TextBox chatField;
        public System.Windows.Forms.TextBox msgInputField;
        public System.Windows.Forms.Button sendButton;
        public System.Windows.Forms.Button switchConnectionButton;
        private System.Windows.Forms.Button clearChatFieldBtn;
        private System.Windows.Forms.Label label1;
    }
}