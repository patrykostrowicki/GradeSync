using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GradeSync.kontrolki
{
    internal class noweWydarzenie : Form
    {
        private DateTimePicker terminPicker;
        private ComboBox comboBoxPrzedmiot, comboBoxTyp, comboBoxKlasa;
        private RichTextBox opisTextBox;
        private Button utworzButton, anulujButton;
        private Label labelTermin, labelPrzedmiot, labelTyp, labelKlasa, labelOpis;

        public DateTime WybranyTermin => terminPicker.Value;
        public string WybranyPrzedmiot => comboBoxPrzedmiot.SelectedItem?.ToString();
        public int WybranyTyp => GetTypIndex(comboBoxTyp.SelectedItem?.ToString());
        public string WybranaKlasa => comboBoxKlasa.SelectedItem?.ToString();
        public string Opis => opisTextBox.Text;

        public noweWydarzenie(IEnumerable<string> przedmioty, IEnumerable<string> klasy)
        {
            InitializeComponent();
            PopulateComboBoxes(przedmioty, klasy);
        }

        private void InitializeComponent()
        {
            this.terminPicker = new System.Windows.Forms.DateTimePicker();
            this.comboBoxPrzedmiot = new System.Windows.Forms.ComboBox();
            this.comboBoxTyp = new System.Windows.Forms.ComboBox();
            this.comboBoxKlasa = new System.Windows.Forms.ComboBox();
            this.opisTextBox = new System.Windows.Forms.RichTextBox();
            this.utworzButton = new System.Windows.Forms.Button();
            this.anulujButton = new System.Windows.Forms.Button();
            this.labelTermin = new System.Windows.Forms.Label();
            this.labelPrzedmiot = new System.Windows.Forms.Label();
            this.labelTyp = new System.Windows.Forms.Label();
            this.labelKlasa = new System.Windows.Forms.Label();
            this.labelOpis = new System.Windows.Forms.Label();
            this.SuspendLayout();

            this.terminPicker.Location = new System.Drawing.Point(120, 10);
            this.terminPicker.Name = "terminPicker";
            this.terminPicker.Size = new System.Drawing.Size(160, 20);
            this.terminPicker.TabIndex = 5;

            this.comboBoxPrzedmiot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPrzedmiot.Location = new System.Drawing.Point(120, 40);
            this.comboBoxPrzedmiot.Name = "comboBoxPrzedmiot";
            this.comboBoxPrzedmiot.Size = new System.Drawing.Size(160, 21);
            this.comboBoxPrzedmiot.TabIndex = 6;

            this.comboBoxTyp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTyp.Location = new System.Drawing.Point(120, 70);
            this.comboBoxTyp.Name = "comboBoxTyp";
            this.comboBoxTyp.Size = new System.Drawing.Size(160, 21);
            this.comboBoxTyp.TabIndex = 7;

            this.comboBoxKlasa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxKlasa.Location = new System.Drawing.Point(120, 100);
            this.comboBoxKlasa.Name = "comboBoxKlasa";
            this.comboBoxKlasa.Size = new System.Drawing.Size(160, 21);
            this.comboBoxKlasa.TabIndex = 8;

            this.opisTextBox.Location = new System.Drawing.Point(10, 160);
            this.opisTextBox.Name = "opisTextBox";
            this.opisTextBox.Size = new System.Drawing.Size(280, 100);
            this.opisTextBox.TabIndex = 9;
            this.opisTextBox.Text = "";

            this.utworzButton.Location = new System.Drawing.Point(10, 270);
            this.utworzButton.Name = "utworzButton";
            this.utworzButton.Size = new System.Drawing.Size(75, 23);
            this.utworzButton.TabIndex = 10;
            this.utworzButton.Text = "Utwórz";
            this.utworzButton.Click += new System.EventHandler(this.UtworzButton_Click);

            this.anulujButton.Location = new System.Drawing.Point(215, 270);
            this.anulujButton.Name = "anulujButton";
            this.anulujButton.Size = new System.Drawing.Size(75, 23);
            this.anulujButton.TabIndex = 11;
            this.anulujButton.Text = "Anuluj";
            this.anulujButton.Click += new System.EventHandler(this.AnulujButton_Click);

            this.labelTermin.Location = new System.Drawing.Point(10, 10);
            this.labelTermin.Name = "labelTermin";
            this.labelTermin.Size = new System.Drawing.Size(100, 23);
            this.labelTermin.TabIndex = 0;
            this.labelTermin.Text = "Wybierz termin:";

            this.labelPrzedmiot.Location = new System.Drawing.Point(10, 40);
            this.labelPrzedmiot.Name = "labelPrzedmiot";
            this.labelPrzedmiot.Size = new System.Drawing.Size(100, 23);
            this.labelPrzedmiot.TabIndex = 1;
            this.labelPrzedmiot.Text = "Wybierz przedmiot:";

            this.labelTyp.Location = new System.Drawing.Point(10, 70);
            this.labelTyp.Name = "labelTyp";
            this.labelTyp.Size = new System.Drawing.Size(100, 23);
            this.labelTyp.TabIndex = 2;
            this.labelTyp.Text = "Wybierz typ:";

            this.labelKlasa.Location = new System.Drawing.Point(10, 100);
            this.labelKlasa.Name = "labelKlasa";
            this.labelKlasa.Size = new System.Drawing.Size(100, 23);
            this.labelKlasa.TabIndex = 3;
            this.labelKlasa.Text = "Wybierz klasę:";
 
            this.labelOpis.Location = new System.Drawing.Point(10, 130);
            this.labelOpis.Name = "labelOpis";
            this.labelOpis.Size = new System.Drawing.Size(100, 23);
            this.labelOpis.TabIndex = 4;
            this.labelOpis.Text = "Opis:";

            this.ClientSize = new System.Drawing.Size(300, 310);
            this.Controls.Add(this.labelTermin);
            this.Controls.Add(this.labelPrzedmiot);
            this.Controls.Add(this.labelTyp);
            this.Controls.Add(this.labelKlasa);
            this.Controls.Add(this.labelOpis);
            this.Controls.Add(this.terminPicker);
            this.Controls.Add(this.comboBoxPrzedmiot);
            this.Controls.Add(this.comboBoxTyp);
            this.Controls.Add(this.comboBoxKlasa);
            this.Controls.Add(this.opisTextBox);
            this.Controls.Add(this.utworzButton);
            this.Controls.Add(this.anulujButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "noweWydarzenie";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nowe Wydarzenie";
            this.ResumeLayout(false);

        }

        private void PopulateComboBoxes(IEnumerable<string> przedmioty, IEnumerable<string> klasy)
        {
            comboBoxPrzedmiot.Items.AddRange(przedmioty.ToArray());
            comboBoxTyp.Items.AddRange(new object[] { "sprawdzian", "kartkówka", "zadanie", "projekt", "inne" });
            comboBoxKlasa.Items.AddRange(klasy.ToArray());
        }

        private int GetTypIndex(string typ)
        {
            switch (typ)
            {
                case "sprawdzian":
                    return 1;
                case "kartkówka":
                    return 2;
                case "zadanie":
                    return 3;
                case "projekt":
                    return 4;
                case "inne":
                    return 5;
                default:
                    return 0;
            }
        }


        private void UtworzButton_Click(object sender, EventArgs e)
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
