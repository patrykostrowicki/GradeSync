using System;
using System.Windows.Forms;

namespace GradeSync.kontrolki
{
    internal class edytujFrekwencje : Form
    {
        private ComboBox comboBoxTypFrekwencji;
        private Button btnAktualizuj;
        private Button btnAnuluj;

        public string WybranyTyp { get; private set; }

        public edytujFrekwencje()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.comboBoxTypFrekwencji = new System.Windows.Forms.ComboBox();
            this.btnAktualizuj = new System.Windows.Forms.Button();
            this.btnAnuluj = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.comboBoxTypFrekwencji.Items.AddRange(new object[] {
            "nieobecność",
            "nieobecność usprawiedliwiona",
            "spóźnienie",
            "obecność"});
            this.comboBoxTypFrekwencji.Location = new System.Drawing.Point(30, 30);
            this.comboBoxTypFrekwencji.Name = "comboBoxTypFrekwencji";
            this.comboBoxTypFrekwencji.Size = new System.Drawing.Size(200, 21);
            this.comboBoxTypFrekwencji.TabIndex = 0;
            this.comboBoxTypFrekwencji.DropDownStyle = ComboBoxStyle.DropDownList;

            this.btnAktualizuj.Location = new System.Drawing.Point(30, 70);
            this.btnAktualizuj.Name = "btnAktualizuj";
            this.btnAktualizuj.Size = new System.Drawing.Size(75, 23);
            this.btnAktualizuj.TabIndex = 1;
            this.btnAktualizuj.Text = "Aktualizuj";
            this.btnAktualizuj.Click += new System.EventHandler(this.btnAktualizuj_Click);

            this.btnAnuluj.Location = new System.Drawing.Point(150, 70);
            this.btnAnuluj.Name = "btnAnuluj";
            this.btnAnuluj.Size = new System.Drawing.Size(75, 23);
            this.btnAnuluj.TabIndex = 2;
            this.btnAnuluj.Text = "Anuluj";
            this.btnAnuluj.Click += new System.EventHandler(this.btnAnuluj_Click);

            this.ClientSize = new System.Drawing.Size(300, 120);
            this.Controls.Add(this.comboBoxTypFrekwencji);
            this.Controls.Add(this.btnAktualizuj);
            this.Controls.Add(this.btnAnuluj);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "edytujFrekwencje";
            this.Text = "Edytuj Typ Frekwencji";
            this.ResumeLayout(false);

        }

        private void btnAktualizuj_Click(object sender, EventArgs e)
        {
            WybranyTyp = comboBoxTypFrekwencji.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
