using GradeSync.klasy;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GradeSync.kontrolki
{
    public class NowyPlanLekcjiForm : Form
    {
        private DataGridView dataGridView;
        private Button btnUtworz;
        private Button btnAnuluj;
        private Label labelKlasa;
        private TextBox textBoxKlasa;
        private FlowLayoutPanel buttonsPanel;
        private TableLayoutPanel tableLayoutPanel;
        private AdminResponse adminResponse;

        public string json;

        public NowyPlanLekcjiForm(AdminResponse adminResponse)
        {
            this.adminResponse = adminResponse;
            InitializeComponent();
            CustomInitialize();
        }

        private void InitializeComponent()
        {
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.btnUtworz = new System.Windows.Forms.Button();
            this.btnAnuluj = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.buttonsPanel.SuspendLayout();
            this.SuspendLayout();

            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(240, 150);
            this.dataGridView.TabIndex = 0;

            this.btnUtworz.Location = new System.Drawing.Point(3, 3);
            this.btnUtworz.Name = "btnUtworz";
            this.btnUtworz.Size = new System.Drawing.Size(75, 23);
            this.btnUtworz.TabIndex = 0;
            this.btnUtworz.Text = "Utwórz";

            this.btnAnuluj.Location = new System.Drawing.Point(84, 3);
            this.btnAnuluj.Name = "btnAnuluj";
            this.btnAnuluj.Size = new System.Drawing.Size(75, 23);
            this.btnAnuluj.TabIndex = 1;
            this.btnAnuluj.Text = "Anuluj";

            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.dataGridView, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonsPanel, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(784, 761);
            this.tableLayoutPanel.TabIndex = 0;

            this.buttonsPanel.Controls.Add(this.btnUtworz);
            this.buttonsPanel.Controls.Add(this.btnAnuluj);
            this.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonsPanel.Location = new System.Drawing.Point(3, 658);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(778, 100);
            this.buttonsPanel.TabIndex = 1;

            this.ClientSize = new System.Drawing.Size(784, 761);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "NowyPlanLekcjiForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.buttonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void CustomInitialize()
        {
            this.dataGridView.ColumnCount = 5;
            this.dataGridView.RowCount = 10;
            this.dataGridView.RowHeadersWidth = 80;
            this.dataGridView.Columns[0].Name = "Poniedziałek";
            this.dataGridView.Columns[1].Name = "Wtorek";
            this.dataGridView.Columns[2].Name = "Środa";
            this.dataGridView.Columns[3].Name = "Czwartek";
            this.dataGridView.Columns[4].Name = "Piątek";
            this.dataGridView.Dock = DockStyle.Fill;
            this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            int rowHeight = 70;
            foreach (DataGridViewRow row in this.dataGridView.Rows)
            {
                row.Height = rowHeight;
                row.HeaderCell.Value = $"Lekcja {row.Index}";
                for (int i = 0; i < this.dataGridView.ColumnCount; i++)
                {
                    row.Cells[i] = new DataGridViewButtonCell()
                    {
                        Value = "Dodaj lekcję"
                    };
                }
            }

            labelKlasa = new Label();
            labelKlasa.Text = "Wybierz klasę:";
            labelKlasa.AutoSize = true;
            labelKlasa.Padding = new Padding(0, 10, 0, 0);

            textBoxKlasa = new TextBox();
            textBoxKlasa.Width = 200; 
            buttonsPanel.Controls.Add(labelKlasa);
            buttonsPanel.Controls.Add(textBoxKlasa);
            buttonsPanel.Controls.Add(btnUtworz);
            buttonsPanel.Controls.Add(btnAnuluj);

            this.btnUtworz.Click += new EventHandler(this.btnUtworz_Click);
            this.btnAnuluj.Click += new EventHandler(this.btnAnuluj_Click);

            this.dataGridView.CellClick += new DataGridViewCellEventHandler(this.dataGridView_CellClick);
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && this.dataGridView[e.ColumnIndex, e.RowIndex] is DataGridViewButtonCell && this.dataGridView[e.ColumnIndex, e.RowIndex].Value.ToString() == "Dodaj lekcję")
            {
                using (var dodajLekcjeForm = new DodajLekcjeForm(adminResponse))
                {
                    if (dodajLekcjeForm.ShowDialog() == DialogResult.OK)
                    {
                        string przedmiot = dodajLekcjeForm.przedmiot;
                        string nauczyciel = dodajLekcjeForm.nauczyciel;
                        string sala = dodajLekcjeForm.sala;
                        string loginNauczyciela = dodajLekcjeForm.loginNauczyciela;

                        AktualizujKomorke(e.RowIndex, e.ColumnIndex, sala, przedmiot, nauczyciel, loginNauczyciela);
                    }
                }
            }
        }


        private void AktualizujKomorke(int rowIndex, int columnIndex, string sala, string przedmiot, string nauczyciel, string login)
        {
            DataGridViewTextBoxCell textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = $"Sala: {sala} | Przedmiot: {przedmiot} | Prowadzacy: {nauczyciel} | Prowadzacy_login: {login}";

            this.dataGridView[columnIndex, rowIndex] = textBoxCell;
            textBoxCell.ReadOnly = true;
        }


        private void btnUtworz_Click(object sender, EventArgs e)
        {
            var planLekcji = new Dictionary<string, Dictionary<string, object>>();

            string[] dniTygodnia = { "poniedzialek", "wtorek", "sroda", "czwartek", "piatek" };

            for (int i = 0; i < dataGridView.RowCount; i++)
            {
                for (int j = 0; j < dataGridView.ColumnCount; j++)
                {
                    var cell = dataGridView[j, i];
                    if (cell is DataGridViewTextBoxCell && cell.Value != null && !(cell.Value.ToString().Contains("Dodaj lekcję")))
                    {
                        string[] lekcjaDane = cell.Value.ToString().Split('|');
                        var lekcjaInfo = new Dictionary<string, object>();

                        foreach (var dane in lekcjaDane)
                        {
                            string[] keyValue = dane.Split(new[] { ':' }, 2);
                            if (keyValue.Length == 2)
                            {
                                string key = keyValue[0].Trim().ToLower().Replace(" ", "_");
                                lekcjaInfo[key] = keyValue[1].Trim();
                            }
                        }

                        string dzien = dniTygodnia[j];
                        if (!planLekcji.ContainsKey(dzien))
                        {
                            planLekcji[dzien] = new Dictionary<string, object>();
                        }
                        planLekcji[dzien].Add($"lek{i}", lekcjaInfo);
                    }
                }
            }

            string klasa = textBoxKlasa.Text;
            int semestr = wspólneMetody.SprawdzSemestr();

            var wynikoweDane = new Dictionary<string, object>
    {
        {"klasa", klasa},
        {"semestr", semestr}
    };

            foreach (var dzien in dniTygodnia)
            {
                if (planLekcji.ContainsKey(dzien))
                {
                    wynikoweDane[dzien] = planLekcji[dzien];
                }
                else
                {
                    wynikoweDane[dzien] = new Dictionary<string, object>();
                }
            }

            json = Newtonsoft.Json.JsonConvert.SerializeObject(wynikoweDane, Newtonsoft.Json.Formatting.Indented);

            this.DialogResult = DialogResult.OK;
        }
    

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
