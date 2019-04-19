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
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }

        private void registerBtn_Click(object sender, EventArgs e)
        {
            if(loginTB.Text.IndexOf(' ') != -1)
            {
                MessageBox.Show("Логин не может содержать пробелов", "Ошибка регистрации");
                loginTB.Text = "";
                loginTB.Focus();
                return;
            }
            if (FormController.GetLogins().Contains(loginTB.Text)) 
            {
                MessageBox.Show("Этот логин уже занят, выберите другой", "Ошибка регистрации");
                loginTB.Text = "";
                loginTB.Focus();
                return;
            }
            FormController.addLogin(loginTB.Text);
        }
    }
}
