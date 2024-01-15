using System;
using System.Drawing;
using System.Windows.Forms;

namespace GradeSync.kontrolki
{
    internal class wystawianieOceny : Form
    {
        private ComboBox comboBoxOceny;
        private RichTextBox opisTextBox;
        private Button wystawButton;
        private Button anulujButton;
        private Label uczenLabel;
        private Label klasaLabel;
        private Label przedmiotLabel;
        private Label wybierzOceneLabel;

        public string WybranaOcena { get; private set; }
        public string Opis { get; private set; }

        public wystawianieOceny(string uczen, string klasa, string przedmiot)
        {
            InitializeComponent();

            uczenLabel.Text = $"Uczeń: {uczen}";
            klasaLabel.Text = $"Klasa: {klasa}";
            przedmiotLabel.Text = $"Przedmiot: {przedmiot}\n";

            if (przedmiot == "Zachowanie")
            {
                //dla przedmiotu zachowanie ogranicz wybór ocen od 1 do 6 bez plusów i minusów
                comboBoxOceny.Items.Clear();
                comboBoxOceny.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6" });

                opisTextBox.Visible = false;
                opisTextBox.Enabled = false;
                opisTextBox.Text = "";

                wystawButton.Location = new Point(wystawButton.Location.X, comboBoxOceny.Location.Y + comboBoxOceny.Height + 10);
                anulujButton.Location = new Point(anulujButton.Location.X, comboBoxOceny.Location.Y + comboBoxOceny.Height + 10);
                this.ClientSize = new Size(this.ClientSize.Width, wystawButton.Location.Y + wystawButton.Height + 30);
            }
            else
            {
                //dla innych przedmiotów umożliwia wybór wszystkich ocen
                opisTextBox.Visible = true;
                opisTextBox.Enabled = true;
                opisTextBox.Text = "Opis oceny";
            }
        }

        private void InitializeComponent()
        {
            this.comboBoxOceny = new ComboBox();
            this.opisTextBox = new RichTextBox();
            this.wystawButton = new Button();
            this.anulujButton = new Button();
            this.uczenLabel = new Label();
            this.klasaLabel = new Label();
            this.przedmiotLabel = new Label();
            this.wybierzOceneLabel = new Label();



            int verticalSpacing = 30;
            uczenLabel.Location = new Point(10, 10);
            klasaLabel.Location = new Point(10, uczenLabel.Location.Y + verticalSpacing);

            przedmiotLabel.Location = new Point(10, klasaLabel.Location.Y + verticalSpacing);
            przedmiotLabel.Size = new Size(260, 40); 
            przedmiotLabel.AutoSize = true; 
            przedmiotLabel.AutoEllipsis = true; 
            przedmiotLabel.MaximumSize = new Size(260, 0);

            wybierzOceneLabel.Text = "Wybierz ocenę:";
            wybierzOceneLabel.Location = new Point(10, przedmiotLabel.Location.Y + verticalSpacing);
            wybierzOceneLabel.AutoSize = true;

            comboBoxOceny.Items.AddRange(new object[] { "1", "1+", "2", "2-", "2+", "3", "3-", "3+", "4", "4-", "4+", "5", "5-", "5+", "6", "6-" });
            comboBoxOceny.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxOceny.Location = new Point(120, wybierzOceneLabel.Location.Y);
            comboBoxOceny.Size = new Size(150, 20);

            opisTextBox.Location = new Point(10, comboBoxOceny.Location.Y + comboBoxOceny.Height + 10);
            opisTextBox.Size = new Size(260, 100);

            wystawButton.Text = "Wystaw";
            wystawButton.Location = new Point(10, opisTextBox.Location.Y + opisTextBox.Height + 10);
            wystawButton.Click += new EventHandler(this.WstawOcene_Click);
            anulujButton.Text = "Anuluj";
            anulujButton.Location = new Point(180, wystawButton.Location.Y);
            anulujButton.Click += new EventHandler(this.Anuluj_Click);

            this.ClientSize = new Size(280, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Controls.Add(comboBoxOceny);
            this.Controls.Add(opisTextBox);
            this.Controls.Add(wystawButton);
            this.Controls.Add(anulujButton);
            this.Controls.Add(uczenLabel);
            this.Controls.Add(klasaLabel);
            this.Controls.Add(przedmiotLabel);
            this.Controls.Add(wybierzOceneLabel);
        }

        private void WstawOcene_Click(object sender, EventArgs e)
        {   
            if (comboBoxOceny.SelectedItem == null || string.IsNullOrEmpty(comboBoxOceny.SelectedItem.ToString()))
            {
                MessageBox.Show("Proszę wybrać ocenę.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                WybranaOcena = comboBoxOceny.SelectedItem.ToString();
                Opis = opisTextBox.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void Anuluj_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
