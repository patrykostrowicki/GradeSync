using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using GradeSync.klasy;

namespace GradeSync
{
    public partial class login : Form
    {
        private HttpClient httpClient = new HttpClient();

        public login()
        {
            InitializeComponent();
            this.FormClosed += (s, e) => Application.Exit();
        }

        bool pokaz = true;

        private void metroPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (pokaz) { 
                haslo.PasswordChar = false;
                pokaz = false;
            } else {
                haslo.PasswordChar = true;
                pokaz = true;
            }
        }

        private void login_bttn_Click(object sender, EventArgs e)
        {
            string login = login_.Texts;
            string haslo_ = haslo.Texts;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(haslo_))
            {
                MessageBox.Show("Login i hasło nie mogą być puste.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string url = $"{Properties.Resources.adres_api}/login?login={login}&haslo={haslo_}";

            Task.Run(async () =>
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        //deserializacja tylko części odpowiedzi zawierającej typ użytkownika
                        var typeResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        var userType = (string)typeResponse.type;

                        this.Invoke(new Action(() =>
                        {
                            //szyfrowanie hasla i zapisywanie danych logowania
                            if (zap_dane.Checked)
                            {
                                Properties.Settings.Default.Login = login;
                                Properties.Settings.Default.Haslo = CryptoHelper.Encrypt(haslo_);
                                Properties.Settings.Default.Save();
                            }

                            //otwieranie odpowiedniego formularza na podstawie typu użytkownika
                            switch (userType)
                            {
                                case "uczen":
                                    var uczenResponse = JsonConvert.DeserializeObject<UserResponse>((string)typeResponse.data.ToString());
                                    uczenResponse.Login = login_.Texts;
                                    uczen uczenForm = new uczen(uczenResponse, this);
                                    uczenForm.FormClosed += (s, args) => this.Show();
                                    uczenForm.Show();
                                    break;
                                case "nauczyciel":
                                    var nauczycielResponse = JsonConvert.DeserializeObject<NauczycielResponse>((string)typeResponse.data.ToString());
                                    nauczycielResponse.Login = login_.Texts;
                                    nauczyciel nauczycielForm = new nauczyciel(nauczycielResponse, this);
                                    nauczycielForm.FormClosed += (s, args) => this.Show();
                                    nauczycielForm.Show();
                                    break;
                                case "admin":
                                    var adminResponse = JsonConvert.DeserializeObject<AdminResponse>((string)typeResponse.data.ToString());
                                    adminResponse.InitializeUczniowie();
                                    adminResponse.Login = login_.Texts;
                                    admin adminForm = new admin(adminResponse, this);
                                    adminForm.FormClosed += (s, args) => this.Show();
                                    adminForm.Show();
                                    this.Hide();
                                    break;
                                default:
                                    MessageBox.Show("Nieznany typ użytkownika.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    break;
                            }

                            this.Hide();
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show($"Błąd logowania: {response.ReasonPhrase}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                }
                catch (HttpRequestException ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"Problem z połączeniem: {ex.Message}", "Wyjątek", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                }
            });
        }

        private void login_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Login) && !string.IsNullOrEmpty(Properties.Settings.Default.Haslo))
            {
                login_.Texts = Properties.Settings.Default.Login;
                haslo.Texts = CryptoHelper.Decrypt(Properties.Settings.Default.Haslo);
            }
        }
    }
}
