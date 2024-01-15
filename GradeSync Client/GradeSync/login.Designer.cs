namespace GradeSync
{
    partial class login
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(login));
            this.panel1 = new System.Windows.Forms.Panel();
            this.login_bttn = new MaterialSkin.Controls.MaterialButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.zap_dane = new System.Windows.Forms.CheckBox();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.haslo = new GradeSync.kontrolki.textbox();
            this.login_ = new GradeSync.kontrolki.textbox();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(49, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(198, 170);
            this.panel1.TabIndex = 2;
            // 
            // login_bttn
            // 
            this.login_bttn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.login_bttn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.login_bttn.Depth = 0;
            this.login_bttn.HighEmphasis = true;
            this.login_bttn.Icon = null;
            this.login_bttn.Location = new System.Drawing.Point(168, 306);
            this.login_bttn.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.login_bttn.MouseState = MaterialSkin.MouseState.HOVER;
            this.login_bttn.Name = "login_bttn";
            this.login_bttn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.login_bttn.Size = new System.Drawing.Size(111, 36);
            this.login_bttn.TabIndex = 3;
            this.login_bttn.Text = "Zaloguj się";
            this.login_bttn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.login_bttn.UseAccentColor = false;
            this.login_bttn.UseVisualStyleBackColor = true;
            this.login_bttn.Click += new System.EventHandler(this.login_bttn_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(10, 199);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(35, 35);
            this.panel2.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Location = new System.Drawing.Point(10, 243);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(32, 32);
            this.panel3.TabIndex = 7;
            // 
            // zap_dane
            // 
            this.zap_dane.AutoSize = true;
            this.zap_dane.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.zap_dane.Location = new System.Drawing.Point(50, 280);
            this.zap_dane.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.zap_dane.Name = "zap_dane";
            this.zap_dane.Size = new System.Drawing.Size(184, 17);
            this.zap_dane.TabIndex = 9;
            this.zap_dane.Text = "Zapamiętaj dane logowania";
            this.zap_dane.UseVisualStyleBackColor = true;
            // 
            // metroPanel1
            // 
            this.metroPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(43)))), ((int)(((byte)(74)))));
            this.metroPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("metroPanel1.BackgroundImage")));
            this.metroPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(250, 245);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(27, 27);
            this.metroPanel1.TabIndex = 10;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            this.metroPanel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.metroPanel1_MouseClick);
            // 
            // haslo
            // 
            this.haslo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(43)))), ((int)(((byte)(74)))));
            this.haslo.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.haslo.BorderFocusColor = System.Drawing.Color.HotPink;
            this.haslo.BorderSize = 2;
            this.haslo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.haslo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.haslo.Location = new System.Drawing.Point(50, 243);
            this.haslo.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.haslo.Multiline = false;
            this.haslo.Name = "haslo";
            this.haslo.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.haslo.PasswordChar = true;
            this.haslo.Size = new System.Drawing.Size(229, 31);
            this.haslo.TabIndex = 5;
            this.haslo.Texts = "";
            this.haslo.UnderlinedStyle = false;
            // 
            // login_
            // 
            this.login_.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(43)))), ((int)(((byte)(74)))));
            this.login_.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.login_.BorderFocusColor = System.Drawing.Color.HotPink;
            this.login_.BorderSize = 2;
            this.login_.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login_.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.login_.Location = new System.Drawing.Point(50, 204);
            this.login_.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.login_.Multiline = false;
            this.login_.Name = "login_";
            this.login_.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.login_.PasswordChar = false;
            this.login_.Size = new System.Drawing.Size(229, 31);
            this.login_.TabIndex = 4;
            this.login_.Texts = "";
            this.login_.UnderlinedStyle = false;
            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(21)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(293, 363);
            this.Controls.Add(this.metroPanel1);
            this.Controls.Add(this.zap_dane);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.haslo);
            this.Controls.Add(this.login_);
            this.Controls.Add(this.login_bttn);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "login";
            this.Text = "GradeSync - Logowanie";
            this.Load += new System.EventHandler(this.login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private MaterialSkin.Controls.MaterialButton login_bttn;
        private kontrolki.textbox login_;
        private kontrolki.textbox haslo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox zap_dane;
        private MetroFramework.Controls.MetroPanel metroPanel1;
    }
}

