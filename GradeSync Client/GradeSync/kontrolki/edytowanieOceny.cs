using System;
using System.Drawing;
using System.Windows.Forms;

namespace GradeSync.kontrolki
{
    internal class edytowanieOceny : Form
    {
        private ComboBox comboBoxOceny;
        private Button aktualizujButton;
        private Button anulujButton;
        private Label infoLabel;

        public string WybranaOcena { get; private set; }

        public edytowanieOceny(string ocena, string przedmiot, string opis, string dataWystawienia)
        {
            InitializeComponent();

            infoLabel.Text = $"Ocena: {ocena}\nPrzedmiot: {przedmiot}\nOpis: {opis}\nData Wystawienia: {dataWystawienia}";
            comboBoxOceny.SelectedItem = ocena;
        }

        private void InitializeComponent()
        {
            this.comboBoxOceny = new ComboBox();
            this.aktualizujButton = new Button();
            this.anulujButton = new Button();
            this.infoLabel = new Label();

            this.comboBoxOceny.Items.AddRange(new object[] { "1","1+", "2","2-", "2+", "3","3-", "3+", "4","4-", "4+", "5", "5-", "5+", "6", "6-" });
            this.comboBoxOceny.Location = new Point(10, 70);
            this.comboBoxOceny.Size = new Size(100, 20);
            this.comboBoxOceny.DropDownStyle = ComboBoxStyle.DropDownList;

            this.aktualizujButton.Text = "Aktualizuj";
            this.aktualizujButton.Location = new Point(120, 70);
            this.aktualizujButton.Click += new EventHandler(this.AktualizujButton_Click);

            this.anulujButton.Text = "Anuluj";
            this.anulujButton.Location = new Point(230, 70);
            this.anulujButton.Click += new EventHandler(this.AnulujButton_Click);

            this.infoLabel.Location = new Point(10, 10);
            this.infoLabel.Size = new Size(280, 50);
            this.infoLabel.AutoSize = true;

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new Size(320, 120);
            this.Controls.Add(this.comboBoxOceny);
            this.Controls.Add(this.aktualizujButton);
            this.Controls.Add(this.anulujButton);
            this.Controls.Add(this.infoLabel);
        }

        private void AktualizujButton_Click(object sender, EventArgs e)
        {
            this.WybranaOcena = comboBoxOceny.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AnulujButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
