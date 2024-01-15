using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace GradeSync.kontrolki
{
    internal class nowyNauczyciel : Form
    {
        private TextBox imieNazwiskoTextBox, loginTextBox, klasaTextBox, hasloTextBox;
        private CheckedListBox przedmiotyCheckedListBox;
        private Button utworzButton, anulujButton;
        private Label labelImieNazwisko, labelLogin, labelKlasa, labelHaslo, labelPrzedmioty;

        public string ImieNazwisko => imieNazwiskoTextBox.Text;
        public string Login => loginTextBox.Text;
        public string Klasa => klasaTextBox.Text;
        public string Haslo => hasloTextBox.Text;
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

        public nowyNauczyciel()
        {
            InitializeComponent();
            DodajPrzedmiotyDoCheckedListBox();
        }

        private void InitializeComponent()
        {
            imieNazwiskoTextBox = new TextBox
            {
                Location = new Point(120, 10),
                Size = new Size(160, 20)
            };

            loginTextBox = new TextBox
            {
                Location = new Point(120, 40),
                Size = new Size(160, 20)
            };

            klasaTextBox = new TextBox
            {
                Location = new Point(120, 70),
                Size = new Size(160, 20)
            };

            hasloTextBox = new TextBox
            {
                Location = new Point(120, 100),
                Size = new Size(160, 20),
                PasswordChar = '*'
            };

            utworzButton = new Button
            {
                Location = new Point(10, 240),
                Size = new Size(75, 23),
                Text = "Utwórz",
                UseVisualStyleBackColor = true
            };
            utworzButton.Click += new EventHandler(UtworzButton_Click);

            anulujButton = new Button
            {
                Location = new Point(205, 240),
                Size = new Size(75, 23),
                Text = "Anuluj",
                UseVisualStyleBackColor = true
            };
            anulujButton.Click += new EventHandler(AnulujButton_Click);

            labelImieNazwisko = new Label
            {
                Location = new Point(10, 10),
                Text = "Imię i nazwisko:"
            };

            labelLogin = new Label
            {
                Location = new Point(10, 40),
                Text = "Login:"
            };

            labelKlasa = new Label
            {
                Location = new Point(10, 70),
                Text = "Klasa:"
            };

            labelHaslo = new Label
            {
                Location = new Point(10, 100),
                Text = "Hasło:"
            };

            labelPrzedmioty = new Label
            {
                Location = new Point(10, 130),
                Text = "Przedmioty:"
            };

            przedmiotyCheckedListBox = new CheckedListBox
            {
                Location = new Point(120, 130),
                Size = new Size(160, 100),
                CheckOnClick = true
            };

            Controls.AddRange(new Control[] {
                imieNazwiskoTextBox, loginTextBox, klasaTextBox, hasloTextBox,
                utworzButton, anulujButton, labelImieNazwisko, labelLogin,
                labelKlasa, labelHaslo, labelPrzedmioty, przedmiotyCheckedListBox
            });

            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(290, 280);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nowy Nauczyciel";
        }


        private void DodajPrzedmiotyDoCheckedListBox()
        {
            var przedmioty = new List<string>
            {
                "Matematyka",
                "Wychowanie fizyczne",
                "Historia",
                "Fizyka",
                "Język Polski",
                "Język Angielski",
                "Język Niemiecki",
                "Biologia",
                "Chemia",
                "Geografia",
                "Informatyka",
                "Edukacja do bezpieczeństwa",
                "Podstawy przedsiębiorczości"
            };

            foreach (var przedmiot in przedmioty)
            {
                przedmiotyCheckedListBox.Items.Add(przedmiot);
            }
        }

        private void UtworzButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(imieNazwiskoTextBox.Text) ||
                string.IsNullOrWhiteSpace(loginTextBox.Text) ||
                string.IsNullOrWhiteSpace(klasaTextBox.Text) ||
                string.IsNullOrWhiteSpace(hasloTextBox.Text))
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
