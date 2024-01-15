using System;
using System.Drawing;
using System.Windows.Forms;

namespace GradeSync.kontrolki
{
    internal class edytujUwage : Form
    {
        private RichTextBox opisTextBox;
        private ComboBox comboBoxTyp;
        private Button aktualizujButton;
        private Button anulujButton;
        private Label infoLabel;

        public string NowyOpis { get; private set; }
        public int NowyTyp { get; private set; }

        public edytujUwage(string uczen, string klasa, string opis, string data, int typ)
        {
            InitializeComponent();

            infoLabel.Text = $"Uczeń: {uczen}\nKlasa: {klasa}\nData: {data}";
            opisTextBox.Text = opis;
            comboBoxTyp.SelectedIndex = typ - 1;
        }

        private void InitializeComponent()
        {
            this.opisTextBox = new System.Windows.Forms.RichTextBox();
            this.comboBoxTyp = new System.Windows.Forms.ComboBox();
            this.aktualizujButton = new System.Windows.Forms.Button();
            this.anulujButton = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();

            this.opisTextBox.Location = new System.Drawing.Point(8, 114);
            this.opisTextBox.Name = "opisTextBox";
            this.opisTextBox.Size = new System.Drawing.Size(280, 100);
            this.opisTextBox.TabIndex = 0;
            this.opisTextBox.Text = "";

            this.comboBoxTyp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTyp.Items.AddRange(new object[] {
            "Uwaga",
            "Osiągnięcie"});
            this.comboBoxTyp.Location = new System.Drawing.Point(8, 87);
            this.comboBoxTyp.Name = "comboBoxTyp";
            this.comboBoxTyp.Size = new System.Drawing.Size(280, 21);
            this.comboBoxTyp.TabIndex = 1;

            this.aktualizujButton.Location = new System.Drawing.Point(10, 220);
            this.aktualizujButton.Name = "aktualizujButton";
            this.aktualizujButton.Size = new System.Drawing.Size(75, 23);
            this.aktualizujButton.TabIndex = 2;
            this.aktualizujButton.Text = "Aktualizuj";
            this.aktualizujButton.Click += new System.EventHandler(this.AktualizujButton_Click);

            this.anulujButton.Location = new System.Drawing.Point(213, 220);
            this.anulujButton.Name = "anulujButton";
            this.anulujButton.Size = new System.Drawing.Size(75, 23);
            this.anulujButton.TabIndex = 3;
            this.anulujButton.Text = "Anuluj";
            this.anulujButton.Click += new System.EventHandler(this.AnulujButton_Click);

            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(10, 10);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(0, 13);
            this.infoLabel.TabIndex = 4;

            this.ClientSize = new System.Drawing.Size(300, 260);
            this.Controls.Add(this.opisTextBox);
            this.Controls.Add(this.comboBoxTyp);
            this.Controls.Add(this.aktualizujButton);
            this.Controls.Add(this.anulujButton);
            this.Controls.Add(this.infoLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "edytujUwage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void AktualizujButton_Click(object sender, EventArgs e)
        {
            this.NowyOpis = opisTextBox.Text;
            this.NowyTyp = comboBoxTyp.SelectedIndex + 1;

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
