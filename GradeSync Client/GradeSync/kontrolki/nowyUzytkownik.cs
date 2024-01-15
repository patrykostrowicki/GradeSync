using System;
using System.Drawing;
using System.Windows.Forms;

namespace GradeSync.kontrolki
{
    internal class nowyUzytkownik : Form
    {
        private TextBox imieNazwiskoTextBox, loginTextBox, klasaTextBox, hasloTextBox;
        private Button utworzButton, anulujButton;
        private Label labelImieNazwisko, labelLogin, labelKlasa, labelHaslo;

        public string ImieNazwisko => imieNazwiskoTextBox.Text;
        public string Login => loginTextBox.Text;
        public string Klasa => klasaTextBox.Text;
        public string Haslo => hasloTextBox.Text;

        public nowyUzytkownik()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Inicjalizacja kontrolek
            imieNazwiskoTextBox = new TextBox();
            loginTextBox = new TextBox();
            klasaTextBox = new TextBox();
            hasloTextBox = new TextBox();
            utworzButton = new Button();
            anulujButton = new Button();
            labelImieNazwisko = new Label();
            labelLogin = new Label();
            labelKlasa = new Label();
            labelHaslo = new Label();

            imieNazwiskoTextBox.Location = new Point(120, 10);
            imieNazwiskoTextBox.Size = new Size(160, 20);

            loginTextBox.Location = new Point(120, 40);
            loginTextBox.Size = new Size(160, 20);

            klasaTextBox.Location = new Point(120, 70);
            klasaTextBox.Size = new Size(160, 20);

            hasloTextBox.Location = new Point(120, 100);
            hasloTextBox.Size = new Size(160, 20);
            hasloTextBox.PasswordChar = '*';

            utworzButton.Location = new Point(10, 130);
            utworzButton.Text = "Utwórz";
            utworzButton.Click += new EventHandler(UtworzButton_Click);

            anulujButton.Location = new Point(205, 130);
            anulujButton.Text = "Anuluj";
            anulujButton.Click += new EventHandler(AnulujButton_Click);

            labelImieNazwisko.Location = new Point(10, 10);
            labelImieNazwisko.Text = "Imię i nazwisko:";

            labelLogin.Location = new Point(10, 40);
            labelLogin.Text = "Login:";

            labelKlasa.Location = new Point(10, 70);
            labelKlasa.Text = "Klasa:";

            labelHaslo.Location = new Point(10, 100);
            labelHaslo.Text = "Hasło:";

            Controls.AddRange(new Control[] { imieNazwiskoTextBox, loginTextBox, klasaTextBox, hasloTextBox, utworzButton, anulujButton, labelImieNazwisko, labelLogin, labelKlasa, labelHaslo });

            ClientSize = new Size(290, 160);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nowy Użytkownik";
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
