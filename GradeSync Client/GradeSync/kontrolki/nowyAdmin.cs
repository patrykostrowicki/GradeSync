using System;
using System.Windows.Forms;

namespace GradeSync.kontrolki
{
    public class nowyAdmin : Form
    {
        private TextBox txtLogin;
        private TextBox txtHaslo;
        private Button btnAnuluj;
        private Button btnUtworz;
        private Label lblLogin;
        private Label lblHaslo;

        public string LoginAdmina { get; private set; }
        public string HasloAdmina { get; private set; }

        public nowyAdmin()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            txtLogin = new TextBox();
            txtHaslo = new TextBox();
            btnAnuluj = new Button();
            btnUtworz = new Button();
            lblLogin = new Label();
            lblHaslo = new Label();

            lblLogin.Text = "Login:";
            lblLogin.Location = new System.Drawing.Point(10, 20);
            lblLogin.AutoSize = true;

            txtLogin.Location = new System.Drawing.Point(70, 20);
            txtLogin.Size = new System.Drawing.Size(200, 20);

            lblHaslo.Text = "Hasło:";
            lblHaslo.Location = new System.Drawing.Point(10, 50);
            lblHaslo.AutoSize = true;

            txtHaslo.Location = new System.Drawing.Point(70, 50);
            txtHaslo.Size = new System.Drawing.Size(200, 20);

            btnAnuluj.Text = "Anuluj";
            btnAnuluj.Location = new System.Drawing.Point(10, 80);
            btnAnuluj.Click += new EventHandler(btnAnuluj_Click);

            btnUtworz.Text = "Utwórz";
            btnUtworz.Location = new System.Drawing.Point(120, 80);
            btnUtworz.Click += new EventHandler(btnUtworz_Click);

            this.Text = "Nowy Admin";
            this.Size = new System.Drawing.Size(300, 150);
            this.Controls.Add(lblLogin);
            this.Controls.Add(txtLogin);
            this.Controls.Add(lblHaslo);
            this.Controls.Add(txtHaslo);
            this.Controls.Add(btnAnuluj);
            this.Controls.Add(btnUtworz);
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnUtworz_Click(object sender, EventArgs e)
        {
            LoginAdmina = txtLogin.Text;
            HasloAdmina = txtHaslo.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
