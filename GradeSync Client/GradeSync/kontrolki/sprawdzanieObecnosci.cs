using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GradeSync.klasy;

namespace GradeSync.kontrolki
{
    internal class SprawdzanieObecnosciForm : Form
    {
        private List<Uczen> uczniowie;
        private TableLayoutPanel tableLayoutPanel;
        private Panel buttonsPanel;
        private Button btnAnuluj;
        private Button btnZamknij;

        public List<WynikObecnosci> WynikiObecnosci { get; private set; }

        public class WynikObecnosci
        {
            public string UczenLogin { get; set; }
            public string UczenImieNazwisko { get; set; }
            public int Typ { get; set; } //1 dla nieobecności3 dla spóźnienia
        }


        public SprawdzanieObecnosciForm(List<Uczen> uczniowieKlasy)
        {
            this.uczniowie = uczniowieKlasy;

            InitializeForm();
            InitializeUczniowieControls();
        }

        private void InitializeForm()
        {
            this.Width = 600;
            this.Height = 500;
            this.Text = "Sprawdzanie Obecności";

            tableLayoutPanel = new TableLayoutPanel
            {
                ColumnCount = 4,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(5),
                Dock = DockStyle.Fill
            };

            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66F));

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel.Controls.Add(new Label() { Text = "Imię Nazwisko", AutoSize = true }, 0, 0);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Obecny", AutoSize = true }, 1, 0);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Nieobecny", AutoSize = true }, 2, 0);
            tableLayoutPanel.Controls.Add(new Label() { Text = "Spóźniony", AutoSize = true }, 3, 0);

            buttonsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40
            };

            btnAnuluj = new Button { Text = "Anuluj" };
            btnAnuluj.Click += (s, e) => { this.Close(); };
            btnAnuluj.Location = new Point(10, 5);

            btnZamknij = new Button { Text = "Zamknij listę obecności" };
            btnZamknij.Click += BtnZamknij_Click;
            btnZamknij.Location = new Point(400, 5);

            buttonsPanel.Controls.Add(btnAnuluj);
            buttonsPanel.Controls.Add(btnZamknij);

            this.Controls.Add(tableLayoutPanel);
            this.Controls.Add(buttonsPanel);
        }

        private void InitializeUczniowieControls()
        {
            int row = 1;
            foreach (var uczen in uczniowie)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                Label lblUczen = new Label
                {
                    Text = uczen.ImieNazwisko,
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                tableLayoutPanel.Controls.Add(lblUczen, 0, row);

                CheckBox cbObecny = new CheckBox { Name = "obecny_" + uczen.Login };
                CheckBox cbNieobecny = new CheckBox { Name = "nieobecny_" + uczen.Login };
                CheckBox cbSpozniony = new CheckBox { Name = "spozniony_" + uczen.Login };

                cbObecny.CheckedChanged += (s, e) => CheckboxChanged(s, e, uczen.Login);
                cbNieobecny.CheckedChanged += (s, e) => CheckboxChanged(s, e, uczen.Login);
                cbSpozniony.CheckedChanged += (s, e) => CheckboxChanged(s, e, uczen.Login);

                tableLayoutPanel.Controls.Add(cbObecny, 1, row);
                tableLayoutPanel.Controls.Add(cbNieobecny, 2, row);
                tableLayoutPanel.Controls.Add(cbSpozniony, 3, row);

                row++;
            }
        }

        private void CheckboxChanged(object sender, EventArgs e, string uczenLogin)
        {
            CheckBox cbSender = (CheckBox)sender;
            int rowIndex = tableLayoutPanel.GetRow(cbSender);

            foreach (Control control in tableLayoutPanel.Controls)
            {
                if (control is CheckBox cb && cb != cbSender && tableLayoutPanel.GetRow(cb) == rowIndex)
                {
                    cb.Checked = false;
                }
            }
        }

        private void BtnZamknij_Click(object sender, EventArgs e)
        {
            //lista do przechowywania uczniów, dla których nie zaznaczono obecności
            List<string> uczniowieBezZaznaczenia = new List<string>();

            foreach (var uczen in uczniowie)
            {
                var cbObecny = tableLayoutPanel.Controls.Find("obecny_" + uczen.Login, true).FirstOrDefault() as CheckBox;
                var cbNieobecny = tableLayoutPanel.Controls.Find("nieobecny_" + uczen.Login, true).FirstOrDefault() as CheckBox;
                var cbSpozniony = tableLayoutPanel.Controls.Find("spozniony_" + uczen.Login, true).FirstOrDefault() as CheckBox;

                if (!(cbObecny?.Checked == true || cbNieobecny?.Checked == true || cbSpozniony?.Checked == true))
                {
                    uczniowieBezZaznaczenia.Add(uczen.ImieNazwisko);
                }
            }

            if (uczniowieBezZaznaczenia.Any())
            {
                MessageBox.Show("Nie zaznaczono obecności dla następujących uczniów:\n" + string.Join("\n", uczniowieBezZaznaczenia), "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("Czy na pewno chcesz zamknąć listę obecności?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ZapiszDaneObecnosci();
                    this.Close();
                }
            }
        }

        private void ZapiszDaneObecnosci()
        {
            WynikiObecnosci = new List<WynikObecnosci>();

            foreach (var uczen in uczniowie)
            {
                var cbNieobecny = tableLayoutPanel.Controls.Find("nieobecny_" + uczen.Login, true).FirstOrDefault() as CheckBox;
                var cbSpozniony = tableLayoutPanel.Controls.Find("spozniony_" + uczen.Login, true).FirstOrDefault() as CheckBox;

                if (cbNieobecny?.Checked == true || cbSpozniony?.Checked == true)
                {
                    WynikiObecnosci.Add(new WynikObecnosci
                    {
                        UczenLogin = uczen.Login,
                        UczenImieNazwisko = uczen.ImieNazwisko,
                        Typ = cbNieobecny?.Checked == true ? 1 : 3
                    });
                }
            }
        }
    }
}
