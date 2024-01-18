using GradeSync.klasy;
using System;
using System.Windows.Forms;

namespace GradeSync.kontrolki
{
    public class NowyPlanLekcjiForm : Form
    {
        private DataGridView dataGridView;
        private Button btnUtworz;
        private Button btnAnuluj;
        private FlowLayoutPanel buttonsPanel;
        private TableLayoutPanel tableLayoutPanel;
        private AdminResponse adminResponse;

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
            // 
            // dataGridView
            // 
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(240, 150);
            this.dataGridView.TabIndex = 0;
            // 
            // btnUtworz
            // 
            this.btnUtworz.Location = new System.Drawing.Point(3, 3);
            this.btnUtworz.Name = "btnUtworz";
            this.btnUtworz.Size = new System.Drawing.Size(75, 23);
            this.btnUtworz.TabIndex = 0;
            this.btnUtworz.Text = "Utwórz";
            // 
            // btnAnuluj
            // 
            this.btnAnuluj.Location = new System.Drawing.Point(84, 3);
            this.btnAnuluj.Name = "btnAnuluj";
            this.btnAnuluj.Size = new System.Drawing.Size(75, 23);
            this.btnAnuluj.TabIndex = 1;
            this.btnAnuluj.Text = "Anuluj";
            // 
            // tableLayoutPanel
            // 
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
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Controls.Add(this.btnUtworz);
            this.buttonsPanel.Controls.Add(this.btnAnuluj);
            this.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonsPanel.Location = new System.Drawing.Point(3, 658);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(778, 100);
            this.buttonsPanel.TabIndex = 1;
            // 
            // NowyPlanLekcjiForm
            // 
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
            // Inicjalizacja DataGridView
            this.dataGridView.ColumnCount = 5;
            this.dataGridView.RowCount = 10;
            this.dataGridView.RowHeadersWidth = 80; // Zwiększona szerokość, aby pomieścić napisy "Lekcja"
            this.dataGridView.Columns[0].Name = "Poniedziałek";
            this.dataGridView.Columns[1].Name = "Wtorek";
            this.dataGridView.Columns[2].Name = "Środa";
            this.dataGridView.Columns[3].Name = "Czwartek";
            this.dataGridView.Columns[4].Name = "Piątek";
            this.dataGridView.Dock = DockStyle.Fill;
            this.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Ustawienie wysokości wierszy na 70 pikseli i dodanie przycisków "Dodaj lekcję"
            int rowHeight = 70;
            foreach (DataGridViewRow row in this.dataGridView.Rows)
            {
                row.Height = rowHeight;
                row.HeaderCell.Value = $"Lekcja {row.Index}"; // Dodanie napisów "Lekcja"
                for (int i = 0; i < this.dataGridView.ColumnCount; i++)
                {
                    row.Cells[i] = new DataGridViewButtonCell()
                    {
                        Value = "Dodaj lekcję"
                    };
                }
            }

            // Obsługa zdarzeń przycisków
            this.btnUtworz.Click += new EventHandler(this.btnUtworz_Click);
            this.btnAnuluj.Click += new EventHandler(this.btnAnuluj_Click);

            // Dodatkowo, jeśli chcesz obsłużyć kliknięcia przycisków w DataGridView
            this.dataGridView.CellClick += new DataGridViewCellEventHandler(this.dataGridView_CellClick);
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && this.dataGridView[e.ColumnIndex, e.RowIndex] is DataGridViewButtonCell && this.dataGridView[e.ColumnIndex, e.RowIndex].Value.ToString() == "Dodaj lekcję")
            {
                // Jeśli kliknięto przycisk "Dodaj lekcję", otwórz formularz DodajLekcjeForm
                using (var dodajLekcjeForm = new DodajLekcjeForm(adminResponse))
                {
                    if (dodajLekcjeForm.ShowDialog() == DialogResult.OK)
                    {
                        // Aktualizacja komórki danymi z formularza DodajLekcjeForm
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
            // Utwórz nową komórkę tekstową z danymi
            DataGridViewTextBoxCell textBoxCell = new DataGridViewTextBoxCell();
            textBoxCell.Value = $"Sala: {sala} | Przedmiot: {przedmiot} | Nauczyciel: {nauczyciel} | Login: {login}";

            // Zastąp obecną komórkę nową komórką tekstową
            this.dataGridView[columnIndex, rowIndex] = textBoxCell;
        }

        private void btnUtworz_Click(object sender, EventArgs e)
        {
            // Logika tworzenia nowego planu lekcji
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
