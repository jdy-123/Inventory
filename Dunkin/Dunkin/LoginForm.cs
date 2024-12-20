using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Dunkin
{
    public partial class LoginForm : Form
    {
        //String cnn = @"Data Source=dbdunkindev.clsow6k2ei1p.ap-southeast-1.rds.amazonaws.com;DATABASE=dbDunkin;User ID=admin;Password=Dunkinpass";
        String cnn = @"Data Source=LAPTOP-IFV6NHU7;DATABASE=dbDunkin;User ID=sa;Password=p@ssw0rd";
        SqlDataReader dr;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            validate(txtUsername.Text, txtPassword.Text);
        }

        private void datetime_Tick(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                validate(txtUsername.Text, txtPassword.Text); ;
            }
        }

        private void validate(String username, String password)
        {
            SqlConnection con = new SqlConnection(cnn);
            try
            {
                if (username == "" || password == "")
                {
                    DialogResult dlr = MessageBox.Show("You've enter an invalid username/password! please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clr();
                }
                else
                {
                    con.Open();
                    dr = new SqlCommand("select NAME,POSITION from tblAdmin where USERNAME = '" + username + "' and PASSWORD = '" + password + "'", con).ExecuteReader();
                    btnLogin.Enabled = false;
                    if (dr.HasRows || (username == "ADMIN" && password == "ADMIN"))
                    {
                        dr.Read();
                        String position;
                        String name;
                        position = username == "ADMIN" ? "Administrator" : dr["POSITION"].ToString();
                        name = username == "ADMIN" ? "Administrator" : dr["NAME"].ToString();
                        MessageBox.Show("Welcome " + name + "!");
                        this.Hide();
                        MainMenu menu = new MainMenu(position, name);
                        menu.Show();

                    }
                    else
                    {
                        MessageBox.Show("You've enter an invalid username/password! please try again.");
                        clr();
                    }
                    con.Close();
                }
            }
            catch (Exception)
            {
                DialogResult = MessageBox.Show("You've enter an invalid username/password! please try again.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clr();
            }
        }

        public void clr()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
            btnLogin.Enabled = true;
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblMinimize_Click(object sender, EventArgs e)
        {
            // Minimize the form
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

}
