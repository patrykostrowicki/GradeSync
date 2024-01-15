using System;
using System.Drawing;
using System.Windows.Forms;

namespace GradeSync.kontrolki
{
    internal class edytowanieWydarzenia : Form
    {
        private DateTimePicker terminPicker;
        private ComboBox comboBoxTyp;
        private RichTextBox opisTextBox;
        private Button aktualizujButton;
        private Button anulujButton;
        private Label infoLabel;

        public DateTime NowyTermin { get; private set; }
        public string NowyOpis { get; private set; }
        public int NowyTyp { get; private set; }

        public edytowanieWydarzenia(string klasa, string przedmiot, string opis, string data, string termin, string typ)
        {
            InitializeComponent();

            infoLabel.Text = $"Klasa: {klasa}\nPrzedmiot: {przedmiot}\nData: {data}\nTermin: {termin}\nTyp: {typ}";
            terminPicker.Value = DateTime.Parse(termin);
            comboBoxTyp.SelectedItem = typ;
            opisTextBox.Text = opis;
        }

        private void InitializeComponent()
        {
            this.terminPicker = new DateTimePicker();
            this.comboBoxTyp = new ComboBox();
            this.opisTextBox = new RichTextBox();
            this.aktualizujButton = new Button();
            this.anulujButton = new Button();
            this.infoLabel = new Label();

            this.infoLabel.Location = new Point(10, 10);
            this.infoLabel.Size = new Size(280, 60);
            this.infoLabel.AutoSize = true;

            this.terminPicker.Location = new Point(10, 80);
            this.terminPicker.Size = new Size(100, 20);

            this.comboBoxTyp.Items.AddRange(new object[] { "sprawdzian", "kartkówka", "zadanie", "projekt", "inne" });
            this.comboBoxTyp.Location = new Point(120, 80);
            this.comboBoxTyp.Size = new Size(100, 20);
            this.comboBoxTyp.DropDownStyle = ComboBoxStyle.DropDownList;

            this.opisTextBox.Location = new Point(10, 110);
            this.opisTextBox.Size = new Size(280, 130);

            this.aktualizujButton.Text = "Aktualizuj";
            this.aktualizujButton.Location = new Point(10, 250);
            this.aktualizujButton.Click += new EventHandler(this.AktualizujButton_Click);

            this.anulujButton.Text = "Anuluj";
            this.anulujButton.Location = new Point(120, 250);
            this.anulujButton.Click += new EventHandler(this.AnulujButton_Click);

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new Size(300, 290);
            this.Controls.Add(this.terminPicker);
            this.Controls.Add(this.comboBoxTyp);
            this.Controls.Add(this.opisTextBox);
            this.Controls.Add(this.aktualizujButton);
            this.Controls.Add(this.anulujButton);
            this.Controls.Add(this.infoLabel);
        }


        private void AktualizujButton_Click(object sender, EventArgs e)
        {
            this.NowyTermin = terminPicker.Value;
            this.NowyOpis = opisTextBox.Text;

            switch (comboBoxTyp.SelectedItem.ToString().ToLower())
            {
                case "sprawdzian":
                    this.NowyTyp = 1;
                    break;
                case "kartkówka":
                    this.NowyTyp = 2;
                    break;
                case "zadanie":
                    this.NowyTyp = 3;
                    break;
                case "projekt":
                    this.NowyTyp = 4;
                    break;
                case "inne":
                    this.NowyTyp = 5;
                    break;
                default:
                    this.NowyTyp = 0;
                    break;
            }

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
