using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST_diplom
{
    public partial class FormLogin : Form
    {
        GetLoginInfo getLoginInfo;

        public FormLogin(GetLoginInfo getLoginInfo)
        {
            InitializeComponent();

            this.getLoginInfo = getLoginInfo;

            loginField.Text = (sizeof(char)).ToString();
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
            }
            if (loginField.Text.IndexOf(' ') != -1)
            {
                MessageBox.Show("Пробелы в логине недопустимы", "Login");
                return false;
            }
            if(FormController.GetLogins().Contains(loginField.Text))
            {
                if(MessageBox.Show("This login does not exists. Register?", "Login", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    FormRegister formRegister = new FormRegister();
                    formRegister.ShowDialog();
                }
                else
                {
                    loginField.Focus();
                }
                return false;
            }

            getLoginInfo.login = loginField.Text;
            getLoginInfo.backComName = BackComPortName.Text;
            getLoginInfo.forwardComName = ForwardComPortName.Text;
            return true;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormLogin_Load_1(object sender, EventArgs e)
        {

        }

        private void BackComPortName_TextChanged(object sender, EventArgs e)
        {

        }

        private void BackComPortName_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
