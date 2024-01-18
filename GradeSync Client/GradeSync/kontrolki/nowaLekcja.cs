using System;
using System.Windows.Forms;
using System.Linq;
using GradeSync.klasy;
using System.Drawing;

namespace GradeSync.kontrolki
{
    public class DodajLekcjeForm : Form
    {
        private ComboBox comboBoxPrzedmioty;
        private ComboBox comboBoxNauczyciele;
        private TextBox textBoxSala;
        private Button btnDodaj;
        private Button btnAnuluj;
        private Label labelPrzedmiot;
        private Label labelNauczyciel;
        private Label labelSala;

        private AdminResponse adminResponse;

        public string przedmiot;
        public string nauczyciel;
        public string sala;
        public string loginNauczyciela;

        public DodajLekcjeForm(AdminResponse adminResponse)
        {
            this.adminResponse = adminResponse;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.labelPrzedmiot = new Label();
            this.labelNauczyciel = new Label();
            this.labelSala = new Label();
            this.comboBoxPrzedmioty = new ComboBox();
            this.comboBoxNauczyciele = new ComboBox();
            this.textBoxSala = new TextBox();
            this.btnDodaj = new Button();
            this.btnAnuluj = new Button();

            // Ustawienia FlowLayoutPanel
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.AutoSize = true;

            // Ustawienia Label
            this.labelPrzedmiot.Text = "Wybierz przedmiot:";
            this.labelNauczyciel.Text = "Wybierz nauczyciela:";
            this.labelSala.Text = "Wpisz salę:";

            // Ustawienia ComboBox dla przedmiotów
            this.comboBoxPrzedmioty.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxPrzedmioty.Items.AddRange(new string[]
            {
        "Matematyka", "Wychowanie fizyczne", "Historia", "Fizyka",
        "Język Polski", "Język Angielski", "Język Niemiecki", "Biologia",
        "Chemia", "Geografia", "Informatyka", "Edukacja do bezpieczeństwa",
        "Podstawy przedsiębiorczości"
            });

            // Ustawienia przycisków
            this.btnDodaj.Text = "Dodaj";
            this.btnAnuluj.Text = "Anuluj";

            // Dodawanie kontrolek do FlowLayoutPanel
            flowLayoutPanel.Controls.Add(this.labelPrzedmiot);
            flowLayoutPanel.Controls.Add(this.comboBoxPrzedmioty);
            flowLayoutPanel.Controls.Add(this.labelNauczyciel);
            flowLayoutPanel.Controls.Add(this.comboBoxNauczyciele);
            flowLayoutPanel.Controls.Add(this.labelSala);
            flowLayoutPanel.Controls.Add(this.textBoxSala);
            flowLayoutPanel.Controls.Add(this.btnDodaj);
            flowLayoutPanel.Controls.Add(this.btnAnuluj);

            // Ustawienie FlowLayoutPanel jako kontrolki podrzędnej dla Form
            this.Controls.Add(flowLayoutPanel);

            // Ustawienia Form
            this.Text = "Dodaj Lekcję";
            this.AutoSize = true; // Formularz dopasuje swój rozmiar do zawartości
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.StartPosition = FormStartPosition.CenterScreen; // Otwiera formularz na środku ekranu

            // Event handlers
            this.btnDodaj.Click += new EventHandler(this.btnDodaj_Click);
            this.btnAnuluj.Click += new EventHandler(this.btnAnuluj_Click);

            // Aby upewnić się, że wszystkie kontrolki są widoczne, możemy ustawić minimalne rozmiary dla Form
            this.MinimumSize = new Size(300, flowLayoutPanel.PreferredSize.Height + 50);
        }


        private void LoadData()
        {
            foreach (var nauczyciel in adminResponse.Nauczyciele)
            {
                this.comboBoxNauczyciele.Items.Add(nauczyciel.ImieNazwisko);
            }
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            przedmiot = this.comboBoxPrzedmioty.SelectedItem?.ToString() ?? "nie wybrano";
            nauczyciel = this.comboBoxNauczyciele.SelectedItem?.ToString() ?? "nie wybrano";
            sala = this.textBoxSala.Text;
            loginNauczyciela = ZnajdzLoginNauczyciela(nauczyciel);

            this.DialogResult = DialogResult.OK; // Ustawienie rezultatu dialogu na OK
            this.Close(); // Zamknięcie formularza
        }


        private string ZnajdzLoginNauczyciela(string imieNazwisko)
        {
            var nauczyciel = adminResponse.Nauczyciele.FirstOrDefault(n => n.ImieNazwisko == imieNazwisko);
            return nauczyciel?.Login ?? "nieznany";
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
