namespace ST_diplom
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
            this.chatField = new System.Windows.Forms.TextBox();
            this.onlineUsersList = new System.Windows.Forms.ListBox();
            this.msgInputField = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.HistoryButton = new System.Windows.Forms.Button();
            this.switchConnectionButton = new System.Windows.Forms.Button();
            this.clearChatFieldBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chatField
            // 
            this.chatField.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.chatField.Location = new System.Drawing.Point(12, 12);
            this.chatField.Multiline = true;
            this.chatField.Name = "chatField";
            this.chatField.ReadOnly = true;
            this.chatField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.chatField.Size = new System.Drawing.Size(338, 277);
            this.chatField.TabIndex = 0;
            // 
            // onlineUsersList
            // 
            this.onlineUsersList.FormattingEnabled = true;
            this.onlineUsersList.Location = new System.Drawing.Point(356, 12);
            this.onlineUsersList.Name = "onlineUsersList";
            this.onlineUsersList.Size = new System.Drawing.Size(95, 277);
            this.onlineUsersList.TabIndex = 1;
            this.onlineUsersList.DoubleClick += new System.EventHandler(this.onlineUsersList_DoubleClick);
            // 
            // msgInputField
            // 
            this.msgInputField.Enabled = false;
            this.msgInputField.Location = new System.Drawing.Point(12, 302);
            this.msgInputField.Multiline = true;
            this.msgInputField.Name = "msgInputField";
            this.msgInputField.Size = new System.Drawing.Size(439, 57);
            this.msgInputField.TabIndex = 2;
            this.msgInputField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.msgInputField_KeyDown);
            // 
            // sendButton
            // 
            this.sendButton.Enabled = false;
            this.sendButton.Location = new System.Drawing.Point(356, 369);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(93, 28);
            this.sendButton.TabIndex = 3;
            this.sendButton.Text = "Отправить";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // HistoryButton
            // 
            this.HistoryButton.Location = new System.Drawing.Point(253, 369);
            this.HistoryButton.Name = "HistoryButton";
            this.HistoryButton.Size = new System.Drawing.Size(97, 28);
            this.HistoryButton.TabIndex = 4;
            this.HistoryButton.Text = "История";
            this.HistoryButton.UseVisualStyleBackColor = true;
            this.HistoryButton.Click += new System.EventHandler(this.HistoryButton_Click);
            // 
            // switchConnectionButton
            // 
            this.switchConnectionButton.Location = new System.Drawing.Point(12, 369);
            this.switchConnectionButton.Name = "switchConnectionButton";
            this.switchConnectionButton.Size = new System.Drawing.Size(97, 28);
            this.switchConnectionButton.TabIndex = 5;
            this.switchConnectionButton.Text = "Отключиться";
            this.switchConnectionButton.UseVisualStyleBackColor = true;
            this.switchConnectionButton.Click += new System.EventHandler(this.SwitchConnectionButton_Click);
            // 
            // clearChatFieldBtn
            // 
            this.clearChatFieldBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clearChatFieldBtn.Location = new System.Drawing.Point(300, 12);
            this.clearChatFieldBtn.Name = "clearChatFieldBtn";
            this.clearChatFieldBtn.Size = new System.Drawing.Size(31, 34);
            this.clearChatFieldBtn.TabIndex = 6;
            this.clearChatFieldBtn.Text = "X";
            this.clearChatFieldBtn.UseVisualStyleBackColor = true;
            this.clearChatFieldBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 406);
            this.Controls.Add(this.clearChatFieldBtn);
            this.Controls.Add(this.switchConnectionButton);
            this.Controls.Add(this.HistoryButton);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.msgInputField);
            this.Controls.Add(this.onlineUsersList);
            this.Controls.Add(this.chatField);
            this.Name = "FormChat";
            this.Text = "Chat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox onlineUsersList;
        private System.Windows.Forms.Button HistoryButton;
        public System.Windows.Forms.TextBox chatField;
        public System.Windows.Forms.TextBox msgInputField;
        public System.Windows.Forms.Button sendButton;
        public System.Windows.Forms.Button switchConnectionButton;
        private System.Windows.Forms.Button clearChatFieldBtn;
    }
}