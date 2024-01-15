using System;
using System.Drawing;
using System.Windows.Forms;

namespace GradeSync.kontrolki
{
    internal class nowaUwaga : Form
    {
        private RichTextBox trescTextBox;
        private ComboBox comboBoxTyp;
        private Button utworzButton, anulujButton;
        private Label labelTresc, labelTyp;

        public string Tresc => trescTextBox.Text;
        public int Typ => GetTypIndex(comboBoxTyp.SelectedItem?.ToString());

        public nowaUwaga()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.trescTextBox = new System.Windows.Forms.RichTextBox();
            this.comboBoxTyp = new System.Windows.Forms.ComboBox();
            this.utworzButton = new System.Windows.Forms.Button();
            this.anulujButton = new System.Windows.Forms.Button();
            this.labelTresc = new System.Windows.Forms.Label();
            this.labelTyp = new System.Windows.Forms.Label();

            this.trescTextBox.Location = new System.Drawing.Point(120, 10);
            this.trescTextBox.Name = "trescTextBox";
            this.trescTextBox.Size = new System.Drawing.Size(160, 100);

            this.comboBoxTyp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTyp.Items.AddRange(new object[] { "Uwaga", "Osiągnięcie" });
            this.comboBoxTyp.Location = new System.Drawing.Point(120, 120);
            this.comboBoxTyp.Name = "comboBoxTyp";
            this.comboBoxTyp.Size = new System.Drawing.Size(160, 21);

            this.utworzButton.Location = new System.Drawing.Point(10, 160);
            this.utworzButton.Name = "utworzButton";
            this.utworzButton.Size = new System.Drawing.Size(75, 23);
            this.utworzButton.Text = "Utwórz";
            this.utworzButton.Click += new EventHandler(this.UtworzButton_Click);

            this.anulujButton.Location = new System.Drawing.Point(205, 160);
            this.anulujButton.Name = "anulujButton";
            this.anulujButton.Size = new System.Drawing.Size(75, 23);
            this.anulujButton.Text = "Anuluj";
            this.anulujButton.Click += new EventHandler(this.AnulujButton_Click);

            this.labelTresc.Location = new System.Drawing.Point(10, 10);
            this.labelTresc.Name = "labelTresc";
            this.labelTresc.Size = new System.Drawing.Size(100, 23);
            this.labelTresc.Text = "Treść uwagi:";

            this.labelTyp.Location = new System.Drawing.Point(10, 120);
            this.labelTyp.Name = "labelTyp";
            this.labelTyp.Size = new System.Drawing.Size(100, 23);
            this.labelTyp.Text = "Typ uwagi:";

            this.ClientSize = new System.Drawing.Size(290, 200);
            this.Controls.Add(this.trescTextBox);
            this.Controls.Add(this.comboBoxTyp);
            this.Controls.Add(this.utworzButton);
            this.Controls.Add(this.anulujButton);
            this.Controls.Add(this.labelTresc);
            this.Controls.Add(this.labelTyp);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "nowaUwaga";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Nowa Uwaga";
        }

        private int GetTypIndex(string typ)
        {
            return typ == "Uwaga" ? 1 : 2;
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
