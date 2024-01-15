using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace GradeSync.kontrolki
{
    internal class edytujNauczyciela : Form
    {
        private TextBox imieNazwiskoTextBox, klasaTextBox;
        private CheckedListBox przedmiotyCheckedListBox;
        private Button edytujButton, anulujButton;
        private Label labelImieNazwisko, labelKlasa, labelPrzedmioty;

        public string ImieNazwisko => imieNazwiskoTextBox.Text;
        public string Klasa => klasaTextBox.Text;
        public List<string> WybranePrzedmioty
        {
            get
            {
                var przedmioty = new List<string>();
                foreach (var item in przedmiotyCheckedListBox.CheckedItems)
                {
                    przedmioty.Add(item.ToString());
                }
                return przedmioty;
            }
        }

        public edytujNauczyciela(string imieNazwisko, string klasa, List<string> przedmioty)
        {
            InitializeComponent();
            imieNazwiskoTextBox.Text = imieNazwisko;
            klasaTextBox.Text = klasa;
            SetCheckedPrzedmioty(przedmioty);
        }

        private void InitializeComponent()
        {
            imieNazwiskoTextBox = new TextBox();
            klasaTextBox = new TextBox();
            przedmiotyCheckedListBox = new CheckedListBox();
            edytujButton = new Button();
            anulujButton = new Button();
            labelImieNazwisko = new Label();
            labelKlasa = new Label();
            labelPrzedmioty = new Label();

            imieNazwiskoTextBox.Location = new Point(120, 10);
            imieNazwiskoTextBox.Size = new Size(160, 20);

            klasaTextBox.Location = new Point(120, 40);
            klasaTextBox.Size = new Size(160, 20);

            przedmiotyCheckedListBox.Location = new Point(120, 70);
            przedmiotyCheckedListBox.Size = new Size(160, 100);

            edytujButton.Location = new Point(10, 180);
            edytujButton.Text = "Edytuj";
            edytujButton.Click += new EventHandler(EdytujButton_Click);

            anulujButton.Location = new Point(205, 180);
            anulujButton.Text = "Anuluj";
            anulujButton.Click += new EventHandler(AnulujButton_Click);

           labelImieNazwisko.Location = new Point(10, 10);
            labelImieNazwisko.Text = "Imię i nazwisko:";

            labelKlasa.Location = new Point(10, 40);
            labelKlasa.Text = "Klasa:";

            labelPrzedmioty.Location = new Point(10, 70);
            labelPrzedmioty.Text = "Przedmioty:";

            var dostepnePrzedmioty = new List<string>
            {
                "Matematyka", "Wychowanie fizyczne", "Historia", "Fizyka",
                "Język Polski", "Język Angielski", "Język Niemiecki", "Biologia",
                "Chemia", "Geografia", "Informatyka", "Edukacja do bezpieczeństwa",
                "Podstawy przedsiębiorczości"
            };
            foreach (var przedmiot in dostepnePrzedmioty)
            {
                przedmiotyCheckedListBox.Items.Add(przedmiot);
            }

            Controls.Add(imieNazwiskoTextBox);
            Controls.Add(klasaTextBox);
            Controls.Add(przedmiotyCheckedListBox);
            Controls.Add(edytujButton);
            Controls.Add(anulujButton);
            Controls.Add(labelImieNazwisko);
            Controls.Add(labelKlasa);
            Controls.Add(labelPrzedmioty);

            ClientSize = new Size(290, 220);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edytuj Nauczyciela";
        }

        private void SetCheckedPrzedmioty(List<string> przedmioty)
        {
            foreach (string przedmiot in przedmioty)
            {
                int index = przedmiotyCheckedListBox.Items.IndexOf(przedmiot);
                if (index != -1)
                {
                    przedmiotyCheckedListBox.SetItemChecked(index, true);
                }
            }
        }

        private void EdytujButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void AnulujButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
