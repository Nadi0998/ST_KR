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
    public partial class FormController : Form
    {
        public FormController()
        {
            InitializeComponent();
        }

        private void FormController_Load(object sender, EventArgs e)
        {
            GetLoginInfo getLogin = new GetLoginInfo();
            FormLogin formLogin = new FormLogin(getLogin);
            formLogin.ShowDialog();
            if (getLogin.login == null || getLogin.login == "")
            {
                Application.Exit();
            }
            FormChat formChat = new FormChat(getLogin);
            formChat.ShowDialog();
            Application.Exit();
        }
    }

    public class GetLoginInfo
    {
        public String login { get; set; }
        public String backComName { get; set; }
        public String forwardComName { get; set; }
    }
}
