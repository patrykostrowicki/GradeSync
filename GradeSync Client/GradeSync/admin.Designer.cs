namespace GradeSync
{
    partial class admin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(admin));
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.nowy_uzytkownik = new MaterialSkin.Controls.MaterialButton();
            this.klasa_uczniowie = new MetroFramework.Controls.MetroComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.uczniowie = new System.Windows.Forms.DataGridView();
            this.imie_nazwisko_uczen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.login_uczen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.klasa_uczen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wychowawca_uczen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zmien_dane_uczen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usun_uczen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.nowy_nauczyciel = new MaterialSkin.Controls.MaterialButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.nauczyciele = new System.Windows.Forms.DataGridView();
            this.imie_nazwisko_nauczyciel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.login_nauczyciel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.klasa_nauczyciel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.przedmioty_nauczyciel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zmiendane_nauczyciel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usun_nauczyciel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.utworz_admina = new MaterialSkin.Controls.MaterialButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.admini = new System.Windows.Forms.DataGridView();
            this.login_admin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usun_admin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wyszukiwarka_uczniowie = new GradeSync.kontrolki.textbox();
            this.nauczyciele_wyszukiwarka = new GradeSync.kontrolki.textbox();
            this.admin_wyszukiwarka = new GradeSync.kontrolki.textbox();
            this.materialTabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uczniowie)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nauczyciele)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.admini)).BeginInit();
            this.SuspendLayout();
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Controls.Add(this.tabPage3);
            this.materialTabControl1.Controls.Add(this.tabPage4);
            this.materialTabControl1.Controls.Add(this.tabPage1);
            this.materialTabControl1.Controls.Add(this.tabPage5);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialTabControl1.Location = new System.Drawing.Point(3, 64);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Multiline = true;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(1091, 595);
            this.materialTabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.nowy_uzytkownik);
            this.tabPage3.Controls.Add(this.klasa_uczniowie);
            this.tabPage3.Controls.Add(this.panel1);
            this.tabPage3.Controls.Add(this.wyszukiwarka_uczniowie);
            this.tabPage3.Controls.Add(this.uczniowie);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1083, 569);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Zarządzaj uczniami";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // nowy_uzytkownik
            // 
            this.nowy_uzytkownik.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.nowy_uzytkownik.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.nowy_uzytkownik.Depth = 0;
            this.nowy_uzytkownik.HighEmphasis = true;
            this.nowy_uzytkownik.Icon = null;
            this.nowy_uzytkownik.Location = new System.Drawing.Point(561, 16);
            this.nowy_uzytkownik.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.nowy_uzytkownik.MouseState = MaterialSkin.MouseState.HOVER;
            this.nowy_uzytkownik.Name = "nowy_uzytkownik";
            this.nowy_uzytkownik.NoAccentTextColor = System.Drawing.Color.Empty;
            this.nowy_uzytkownik.Size = new System.Drawing.Size(161, 36);
            this.nowy_uzytkownik.TabIndex = 6;
            this.nowy_uzytkownik.Text = "Nowy użytkownik";
            this.nowy_uzytkownik.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.nowy_uzytkownik.UseAccentColor = false;
            this.nowy_uzytkownik.UseVisualStyleBackColor = true;
            this.nowy_uzytkownik.Click += new System.EventHandler(this.nowy_uzytkownik_Click);
            // 
            // klasa_uczniowie
            // 
            this.klasa_uczniowie.FormattingEnabled = true;
            this.klasa_uczniowie.ItemHeight = 23;
            this.klasa_uczniowie.Location = new System.Drawing.Point(377, 21);
            this.klasa_uczniowie.Name = "klasa_uczniowie";
            this.klasa_uczniowie.Size = new System.Drawing.Size(121, 29);
            this.klasa_uczniowie.TabIndex = 5;
            this.klasa_uczniowie.UseSelectable = true;
            this.klasa_uczniowie.SelectedIndexChanged += new System.EventHandler(this.klasa_uczniowie_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(5, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(32, 32);
            this.panel1.TabIndex = 4;
            // 
            // uczniowie
            // 
            this.uczniowie.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.uczniowie.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.uczniowie.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.imie_nazwisko_uczen,
            this.login_uczen,
            this.klasa_uczen,
            this.wychowawca_uczen,
            this.zmien_dane_uczen,
            this.usun_uczen});
            this.uczniowie.Location = new System.Drawing.Point(3, 61);
            this.uczniowie.Name = "uczniowie";
            this.uczniowie.Size = new System.Drawing.Size(716, 505);
            this.uczniowie.TabIndex = 0;
            this.uczniowie.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.uczniowie_CellClick);
            // 
            // imie_nazwisko_uczen
            // 
            this.imie_nazwisko_uczen.HeaderText = "Imię Nazwisko";
            this.imie_nazwisko_uczen.Name = "imie_nazwisko_uczen";
            this.imie_nazwisko_uczen.ReadOnly = true;
            // 
            // login_uczen
            // 
            this.login_uczen.HeaderText = "Login";
            this.login_uczen.Name = "login_uczen";
            this.login_uczen.ReadOnly = true;
            // 
            // klasa_uczen
            // 
            this.klasa_uczen.HeaderText = "Klasa";
            this.klasa_uczen.Name = "klasa_uczen";
            this.klasa_uczen.ReadOnly = true;
            // 
            // wychowawca_uczen
            // 
            this.wychowawca_uczen.HeaderText = "Wychowawca";
            this.wychowawca_uczen.Name = "wychowawca_uczen";
            // 
            // zmien_dane_uczen
            // 
            this.zmien_dane_uczen.HeaderText = "Zmień dane";
            this.zmien_dane_uczen.Name = "zmien_dane_uczen";
            this.zmien_dane_uczen.ReadOnly = true;
            // 
            // usun_uczen
            // 
            this.usun_uczen.HeaderText = "Usuń konto";
            this.usun_uczen.Name = "usun_uczen";
            this.usun_uczen.ReadOnly = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.nowy_nauczyciel);
            this.tabPage4.Controls.Add(this.panel2);
            this.tabPage4.Controls.Add(this.nauczyciele);
            this.tabPage4.Controls.Add(this.nauczyciele_wyszukiwarka);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1083, 569);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Zarządzaj nauczycielami";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.utworz_admina);
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Controls.Add(this.admin_wyszukiwarka);
            this.tabPage1.Controls.Add(this.admini);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1083, 569);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "Zarządzaj adminami";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1083, 569);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Zarządzaj pl. lekcji";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // nowy_nauczyciel
            // 
            this.nowy_nauczyciel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.nowy_nauczyciel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.nowy_nauczyciel.Depth = 0;
            this.nowy_nauczyciel.HighEmphasis = true;
            this.nowy_nauczyciel.Icon = null;
            this.nowy_nauczyciel.Location = new System.Drawing.Point(564, 6);
            this.nowy_nauczyciel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.nowy_nauczyciel.MouseState = MaterialSkin.MouseState.HOVER;
            this.nowy_nauczyciel.Name = "nowy_nauczyciel";
            this.nowy_nauczyciel.NoAccentTextColor = System.Drawing.Color.Empty;
            this.nowy_nauczyciel.Size = new System.Drawing.Size(155, 36);
            this.nowy_nauczyciel.TabIndex = 11;
            this.nowy_nauczyciel.Text = "Nowy nauczyciel";
            this.nowy_nauczyciel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.nowy_nauczyciel.UseAccentColor = false;
            this.nowy_nauczyciel.UseVisualStyleBackColor = true;
            this.nowy_nauczyciel.Click += new System.EventHandler(this.nowy_nauczyciel_Click);
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(5, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(32, 32);
            this.panel2.TabIndex = 9;
            // 
            // nauczyciele
            // 
            this.nauczyciele.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.nauczyciele.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.nauczyciele.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.imie_nazwisko_nauczyciel,
            this.login_nauczyciel,
            this.klasa_nauczyciel,
            this.przedmioty_nauczyciel,
            this.zmiendane_nauczyciel,
            this.usun_nauczyciel});
            this.nauczyciele.Location = new System.Drawing.Point(3, 47);
            this.nauczyciele.Name = "nauczyciele";
            this.nauczyciele.Size = new System.Drawing.Size(716, 505);
            this.nauczyciele.TabIndex = 7;
            this.nauczyciele.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.nauczyciele_CellClick);
            // 
            // imie_nazwisko_nauczyciel
            // 
            this.imie_nazwisko_nauczyciel.HeaderText = "Imię Nazwisko";
            this.imie_nazwisko_nauczyciel.Name = "imie_nazwisko_nauczyciel";
            this.imie_nazwisko_nauczyciel.ReadOnly = true;
            // 
            // login_nauczyciel
            // 
            this.login_nauczyciel.HeaderText = "Login";
            this.login_nauczyciel.Name = "login_nauczyciel";
            this.login_nauczyciel.ReadOnly = true;
            // 
            // klasa_nauczyciel
            // 
            this.klasa_nauczyciel.HeaderText = "Klasa";
            this.klasa_nauczyciel.Name = "klasa_nauczyciel";
            this.klasa_nauczyciel.ReadOnly = true;
            // 
            // przedmioty_nauczyciel
            // 
            this.przedmioty_nauczyciel.HeaderText = "Przedmioty";
            this.przedmioty_nauczyciel.Name = "przedmioty_nauczyciel";
            // 
            // zmiendane_nauczyciel
            // 
            this.zmiendane_nauczyciel.HeaderText = "Zmień dane";
            this.zmiendane_nauczyciel.Name = "zmiendane_nauczyciel";
            this.zmiendane_nauczyciel.ReadOnly = true;
            // 
            // usun_nauczyciel
            // 
            this.usun_nauczyciel.HeaderText = "Usuń konto";
            this.usun_nauczyciel.Name = "usun_nauczyciel";
            this.usun_nauczyciel.ReadOnly = true;
            // 
            // utworz_admina
            // 
            this.utworz_admina.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.utworz_admina.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.utworz_admina.Depth = 0;
            this.utworz_admina.HighEmphasis = true;
            this.utworz_admina.Icon = null;
            this.utworz_admina.Location = new System.Drawing.Point(604, 4);
            this.utworz_admina.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.utworz_admina.MouseState = MaterialSkin.MouseState.HOVER;
            this.utworz_admina.Name = "utworz_admina";
            this.utworz_admina.NoAccentTextColor = System.Drawing.Color.Empty;
            this.utworz_admina.Size = new System.Drawing.Size(115, 36);
            this.utworz_admina.TabIndex = 15;
            this.utworz_admina.Text = "Nowy admin";
            this.utworz_admina.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.utworz_admina.UseAccentColor = false;
            this.utworz_admina.UseVisualStyleBackColor = true;
            this.utworz_admina.Click += new System.EventHandler(this.utworz_admina_Click);
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Location = new System.Drawing.Point(5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(32, 32);
            this.panel3.TabIndex = 14;
            // 
            // admini
            // 
            this.admini.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.admini.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.admini.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.login_admin,
            this.usun_admin});
            this.admini.Location = new System.Drawing.Point(3, 45);
            this.admini.Name = "admini";
            this.admini.Size = new System.Drawing.Size(716, 505);
            this.admini.TabIndex = 12;
            this.admini.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.admini_CellClick);
            // 
            // login_admin
            // 
            this.login_admin.HeaderText = "Login";
            this.login_admin.Name = "login_admin";
            this.login_admin.ReadOnly = true;
            // 
            // usun_admin
            // 
            this.usun_admin.HeaderText = "Usuń konto";
            this.usun_admin.Name = "usun_admin";
            this.usun_admin.ReadOnly = true;
            // 
            // wyszukiwarka_uczniowie
            // 
            this.wyszukiwarka_uczniowie.BackColor = System.Drawing.SystemColors.Window;
            this.wyszukiwarka_uczniowie.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.wyszukiwarka_uczniowie.BorderFocusColor = System.Drawing.Color.HotPink;
            this.wyszukiwarka_uczniowie.BorderSize = 2;
            this.wyszukiwarka_uczniowie.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wyszukiwarka_uczniowie.ForeColor = System.Drawing.Color.DimGray;
            this.wyszukiwarka_uczniowie.Location = new System.Drawing.Point(44, 21);
            this.wyszukiwarka_uczniowie.Margin = new System.Windows.Forms.Padding(4);
            this.wyszukiwarka_uczniowie.Multiline = false;
            this.wyszukiwarka_uczniowie.Name = "wyszukiwarka_uczniowie";
            this.wyszukiwarka_uczniowie.Padding = new System.Windows.Forms.Padding(7);
            this.wyszukiwarka_uczniowie.PasswordChar = false;
            this.wyszukiwarka_uczniowie.Size = new System.Drawing.Size(313, 31);
            this.wyszukiwarka_uczniowie.TabIndex = 3;
            this.wyszukiwarka_uczniowie.Texts = "";
            this.wyszukiwarka_uczniowie.UnderlinedStyle = false;
            this.wyszukiwarka_uczniowie._TextChanged += new System.EventHandler(this.wyszukiwarka_uczniowie__TextChanged);
            // 
            // nauczyciele_wyszukiwarka
            // 
            this.nauczyciele_wyszukiwarka.BackColor = System.Drawing.SystemColors.Window;
            this.nauczyciele_wyszukiwarka.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.nauczyciele_wyszukiwarka.BorderFocusColor = System.Drawing.Color.HotPink;
            this.nauczyciele_wyszukiwarka.BorderSize = 2;
            this.nauczyciele_wyszukiwarka.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nauczyciele_wyszukiwarka.ForeColor = System.Drawing.Color.DimGray;
            this.nauczyciele_wyszukiwarka.Location = new System.Drawing.Point(44, 7);
            this.nauczyciele_wyszukiwarka.Margin = new System.Windows.Forms.Padding(4);
            this.nauczyciele_wyszukiwarka.Multiline = false;
            this.nauczyciele_wyszukiwarka.Name = "nauczyciele_wyszukiwarka";
            this.nauczyciele_wyszukiwarka.Padding = new System.Windows.Forms.Padding(7);
            this.nauczyciele_wyszukiwarka.PasswordChar = false;
            this.nauczyciele_wyszukiwarka.Size = new System.Drawing.Size(313, 31);
            this.nauczyciele_wyszukiwarka.TabIndex = 8;
            this.nauczyciele_wyszukiwarka.Texts = "";
            this.nauczyciele_wyszukiwarka.UnderlinedStyle = false;
            this.nauczyciele_wyszukiwarka._TextChanged += new System.EventHandler(this.nauczyciele_wyszukiwarka__TextChanged);
            // 
            // admin_wyszukiwarka
            // 
            this.admin_wyszukiwarka.BackColor = System.Drawing.SystemColors.Window;
            this.admin_wyszukiwarka.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.admin_wyszukiwarka.BorderFocusColor = System.Drawing.Color.HotPink;
            this.admin_wyszukiwarka.BorderSize = 2;
            this.admin_wyszukiwarka.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.admin_wyszukiwarka.ForeColor = System.Drawing.Color.DimGray;
            this.admin_wyszukiwarka.Location = new System.Drawing.Point(44, 5);
            this.admin_wyszukiwarka.Margin = new System.Windows.Forms.Padding(4);
            this.admin_wyszukiwarka.Multiline = false;
            this.admin_wyszukiwarka.Name = "admin_wyszukiwarka";
            this.admin_wyszukiwarka.Padding = new System.Windows.Forms.Padding(7);
            this.admin_wyszukiwarka.PasswordChar = false;
            this.admin_wyszukiwarka.Size = new System.Drawing.Size(313, 31);
            this.admin_wyszukiwarka.TabIndex = 13;
            this.admin_wyszukiwarka.Texts = "";
            this.admin_wyszukiwarka.UnderlinedStyle = false;
            // 
            // admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 662);
            this.Controls.Add(this.materialTabControl1);
            this.DrawerTabControl = this.materialTabControl1;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "admin";
            this.Text = "GradeSync - Administrator";
            this.Load += new System.EventHandler(this.admin_Load);
            this.materialTabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uczniowie)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nauczyciele)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.admini)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView uczniowie;
        private System.Windows.Forms.Panel panel1;
        private kontrolki.textbox wyszukiwarka_uczniowie;
        private System.Windows.Forms.DataGridViewTextBoxColumn imie_nazwisko_uczen;
        private System.Windows.Forms.DataGridViewTextBoxColumn login_uczen;
        private System.Windows.Forms.DataGridViewTextBoxColumn klasa_uczen;
        private System.Windows.Forms.DataGridViewTextBoxColumn wychowawca_uczen;
        private System.Windows.Forms.DataGridViewTextBoxColumn zmien_dane_uczen;
        private System.Windows.Forms.DataGridViewTextBoxColumn usun_uczen;
        private MetroFramework.Controls.MetroComboBox klasa_uczniowie;
        private MaterialSkin.Controls.MaterialButton nowy_uzytkownik;
        private MaterialSkin.Controls.MaterialButton nowy_nauczyciel;
        private System.Windows.Forms.Panel panel2;
        private kontrolki.textbox nauczyciele_wyszukiwarka;
        private System.Windows.Forms.DataGridView nauczyciele;
        private System.Windows.Forms.DataGridViewTextBoxColumn imie_nazwisko_nauczyciel;
        private System.Windows.Forms.DataGridViewTextBoxColumn login_nauczyciel;
        private System.Windows.Forms.DataGridViewTextBoxColumn klasa_nauczyciel;
        private System.Windows.Forms.DataGridViewTextBoxColumn przedmioty_nauczyciel;
        private System.Windows.Forms.DataGridViewTextBoxColumn zmiendane_nauczyciel;
        private System.Windows.Forms.DataGridViewTextBoxColumn usun_nauczyciel;
        private MaterialSkin.Controls.MaterialButton utworz_admina;
        private System.Windows.Forms.Panel panel3;
        private kontrolki.textbox admin_wyszukiwarka;
        private System.Windows.Forms.DataGridView admini;
        private System.Windows.Forms.DataGridViewTextBoxColumn login_admin;
        private System.Windows.Forms.DataGridViewTextBoxColumn usun_admin;
    }
}