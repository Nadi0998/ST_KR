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
            this.messageBox = new System.Windows.Forms.TextBox();
            this.onlineUsersList = new System.Windows.Forms.ListBox();
            this.msgInputField = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.switchConnectionButton = new System.Windows.Forms.Button();
            this.clearChatFieldBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.msgListView = new System.Windows.Forms.ListView();
            this.idHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fromHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.shortTextHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.msgFullHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.readBoolHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LogBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // messageBox
            // 
            this.messageBox.BackColor = System.Drawing.SystemColors.Window;
            this.messageBox.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.messageBox.Location = new System.Drawing.Point(455, 160);
            this.messageBox.Multiline = true;
            this.messageBox.Name = "messageBox";
            this.messageBox.ReadOnly = true;
            this.messageBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messageBox.Size = new System.Drawing.Size(419, 258);
            this.messageBox.TabIndex = 0;
            // 
            // onlineUsersList
            // 
            this.onlineUsersList.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.onlineUsersList.FormattingEnabled = true;
            this.onlineUsersList.ItemHeight = 14;
            this.onlineUsersList.Location = new System.Drawing.Point(12, 147);
            this.onlineUsersList.Name = "onlineUsersList";
            this.onlineUsersList.Size = new System.Drawing.Size(143, 102);
            this.onlineUsersList.TabIndex = 1;
            this.onlineUsersList.DoubleClick += new System.EventHandler(this.onlineUsersList_DoubleClick);
            // 
            // msgInputField
            // 
            this.msgInputField.Enabled = false;
            this.msgInputField.Font = new System.Drawing.Font("Garamond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.msgInputField.Location = new System.Drawing.Point(12, 306);
            this.msgInputField.Multiline = true;
            this.msgInputField.Name = "msgInputField";
            this.msgInputField.Size = new System.Drawing.Size(419, 112);
            this.msgInputField.TabIndex = 2;
            this.msgInputField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.msgInputField_KeyDown);
            // 
            // sendButton
            // 
            this.sendButton.Enabled = false;
            this.sendButton.Font = new System.Drawing.Font("Garamond", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.sendButton.Location = new System.Drawing.Point(326, 260);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(105, 40);
            this.sendButton.TabIndex = 3;
            this.sendButton.Text = "Отправить";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // switchConnectionButton
            // 
            this.switchConnectionButton.Font = new System.Drawing.Font("Garamond", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.switchConnectionButton.Location = new System.Drawing.Point(334, 159);
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
            this.clearChatFieldBtn.Location = new System.Drawing.Point(264, 260);
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
            this.label1.Location = new System.Drawing.Point(12, 282);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "Введите сообщение";
            // 
            // msgListView
            // 
            this.msgListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.idHeader,
            this.fromHeader,
            this.toHeader,
            this.shortTextHeader,
            this.msgFullHeader,
            this.readBoolHeader});
            this.msgListView.FullRowSelect = true;
            this.msgListView.GridLines = true;
            this.msgListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.msgListView.Location = new System.Drawing.Point(12, 12);
            this.msgListView.MultiSelect = false;
            this.msgListView.Name = "msgListView";
            this.msgListView.Size = new System.Drawing.Size(419, 129);
            this.msgListView.TabIndex = 8;
            this.msgListView.UseCompatibleStateImageBehavior = false;
            this.msgListView.View = System.Windows.Forms.View.Details;
            this.msgListView.ItemActivate += new System.EventHandler(this.msgListView_ItemActivate);
            this.msgListView.SelectedIndexChanged += new System.EventHandler(this.msgListView_SelectedIndexChanged);
            // 
            // idHeader
            // 
            this.idHeader.Text = "ID";
            this.idHeader.Width = 0;
            // 
            // fromHeader
            // 
            this.fromHeader.Text = "От:";
            this.fromHeader.Width = 100;
            // 
            // toHeader
            // 
            this.toHeader.Text = "К:";
            // 
            // shortTextHeader
            // 
            this.shortTextHeader.Text = "Сообщение";
            this.shortTextHeader.Width = 100;
            // 
            // msgFullHeader
            // 
            this.msgFullHeader.Width = 0;
            // 
            // readBoolHeader
            // 
            this.readBoolHeader.Text = "ПолученоПрочитано";
            // 
            // LogBox
            // 
            this.LogBox.Location = new System.Drawing.Point(455, 12);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.Size = new System.Drawing.Size(419, 129);
            this.LogBox.TabIndex = 9;
            this.LogBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(326, 203);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // FormChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ST_Cursach.Properties.Resources.background1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(896, 436);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.msgListView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clearChatFieldBtn);
            this.Controls.Add(this.switchConnectionButton);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.msgInputField);
            this.Controls.Add(this.onlineUsersList);
            this.Controls.Add(this.messageBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormChat";
            this.Text = "Chat";
            this.Load += new System.EventHandler(this.FormChat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox onlineUsersList;
        public System.Windows.Forms.TextBox messageBox;
        public System.Windows.Forms.TextBox msgInputField;
        public System.Windows.Forms.Button sendButton;
        public System.Windows.Forms.Button switchConnectionButton;
        private System.Windows.Forms.Button clearChatFieldBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView msgListView;
        private System.Windows.Forms.ColumnHeader fromHeader;
        private System.Windows.Forms.ColumnHeader shortTextHeader;
        public System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.ColumnHeader idHeader;
        private System.Windows.Forms.ColumnHeader msgFullHeader;
        private System.Windows.Forms.ColumnHeader readBoolHeader;
        private System.Windows.Forms.ColumnHeader toHeader;
        private System.Windows.Forms.Button button1;
    }
}