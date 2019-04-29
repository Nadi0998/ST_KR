using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST_Cursach
{
    public partial class FormLogin : Form
    {
        GetLoginInfo getLoginInfo;

        public FormLogin(GetLoginInfo getLoginInfo)
        {
            InitializeComponent();

            this.getLoginInfo = getLoginInfo;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (GetLoginInfo())
                Close();
        }

        private void loginField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginButton_Click(sender, e);
            }
        }

        private bool GetLoginInfo()
        {
            if (loginField.Text == "")
            {
                MessageBox.Show("Логин не может быть пуст", "Login");
                loginField.SelectAll();
                loginField.Focus();
                return false;
            }
            if (BackComPortName.Text == "")
            {
                BackComPortName.SelectAll();
                BackComPortName.Focus();
                return false;
            }
            if (ForwardComPortName.Text == "")
            {
                ForwardComPortName.SelectAll();
                ForwardComPortName.Focus();
                return false;
            }
            if (loginField.Text.IndexOf(' ') != -1)
            {
                MessageBox.Show("Пробелы в логине недопустимы", "Login");
                loginField.SelectAll();
                loginField.Focus();
                return false;
            }
           

            getLoginInfo.login = loginField.Text;
            getLoginInfo.backComName = BackComPortName.Text;
            getLoginInfo.forwardComName = ForwardComPortName.Text;
            return true;
        }

        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("KeyDown on form");
        }
    }
}
