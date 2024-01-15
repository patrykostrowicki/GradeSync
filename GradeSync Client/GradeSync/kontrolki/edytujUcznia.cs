using System;
using System.Drawing;
using System.Windows.Forms;

namespace GradeSync.kontrolki
{
    internal class edytujUcznia : Form
    {
        private TextBox imieNazwiskoTextBox, klasaTextBox;
        private Button edytujButton, anulujButton;
        private Label labelImieNazwisko, labelKlasa;

        public string ImieNazwisko => imieNazwiskoTextBox.Text;
        public string Klasa => klasaTextBox.Text;

        public edytujUcznia(string imieNazwisko, string klasa)
        {
            InitializeComponent();
            imieNazwiskoTextBox.Text = imieNazwisko;
            klasaTextBox.Text = klasa;
        }

        private void InitializeComponent()
        {
            this.imieNazwiskoTextBox = new TextBox();
            this.klasaTextBox = new TextBox();
            this.edytujButton = new Button();
            this.anulujButton = new Button();
            this.labelImieNazwisko = new Label();
            this.labelKlasa = new Label();

            this.imieNazwiskoTextBox.Location = new Point(120, 10);
            this.imieNazwiskoTextBox.Size = new Size(160, 20);

            this.klasaTextBox.Location = new Point(120, 40);
            this.klasaTextBox.Size = new Size(160, 20);

            this.edytujButton.Location = new Point(10, 70);
            this.edytujButton.Text = "Edytuj";
            this.edytujButton.Click += new EventHandler(this.EdytujButton_Click);

            this.anulujButton.Location = new Point(205, 70);
            this.anulujButton.Text = "Anuluj";
            this.anulujButton.Click += new EventHandler(this.AnulujButton_Click);

            this.labelImieNazwisko.Location = new Point(10, 10);
            this.labelImieNazwisko.Text = "Imię i nazwisko:";

            this.labelKlasa.Location = new Point(10, 40);
            this.labelKlasa.Text = "Klasa:";

            this.Controls.Add(this.imieNazwiskoTextBox);
            this.Controls.Add(this.klasaTextBox);
            this.Controls.Add(this.edytujButton);
            this.Controls.Add(this.anulujButton);
            this.Controls.Add(this.labelImieNazwisko);
            this.Controls.Add(this.labelKlasa);

            this.ClientSize = new Size(290, 100);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "edytujUcznia";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edytuj Ucznia";
        }

        private void EdytujButton_Click(object sender, EventArgs e)
        {
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
