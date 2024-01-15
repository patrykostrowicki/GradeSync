using GradeSync.klasy;
using GradeSync.kontrolki;
using MaterialSkin.Controls;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace GradeSync
{
    public partial class admin : MaterialForm
    {
        private AdminResponse adminResponse;
        private string wybranaKlasa;

        public admin(AdminResponse response)
        {
            InitializeComponent();
            adminResponse = response;

            wspólneMetody.StylizujDataGridView(uczniowie);
            wspólneMetody.StylizujDataGridView(nauczyciele);
            wspólneMetody.StylizujDataGridView(admini);
            wspólneMetody.StylizujDataGridView(plany_lekcji);
        }

        private void admin_Load(object sender, System.EventArgs e)
        {
            DodajUczniowDoDataGridView();
            ZapelnijKlasaUczniowieComboBox();

            DodajNauczycieliDoDataGridView();

            DodajAdminowDoDataGridView();

            WypelnijDataGridViewPlanamiLekcji();

            dzien_tygodnia.Enabled = false;
        }

        private void DodajUczniowDoDataGridView()
        {
            uczniowie.Rows.Clear();

            foreach (var uczen in adminResponse.Uczniowie)
            {
                var wychowawca = adminResponse.Nauczyciele.FirstOrDefault(n => n.Klasa == uczen.Klasa)?.ImieNazwisko ?? "Brak";

                int rowIndex = uczniowie.Rows.Add();
                uczniowie.Rows[rowIndex].Cells["imie_nazwisko_uczen"].Value = uczen.ImieNazwisko;
                uczniowie.Rows[rowIndex].Cells["login_uczen"].Value = uczen.Login;
                uczniowie.Rows[rowIndex].Cells["klasa_uczen"].Value = uczen.Klasa;
                uczniowie.Rows[rowIndex].Cells["wychowawca_uczen"].Value = wychowawca;

                var zmienButton = new DataGridViewButtonCell();
                zmienButton.Value = "Zmień dane";
                uczniowie.Rows[rowIndex].Cells["zmien_dane_uczen"] = zmienButton;

                var usunButton = new DataGridViewButtonCell();
                usunButton.Value = "Usuń konto";
                uczniowie.Rows[rowIndex].Cells["usun_uczen"] = usunButton;
            }
        }

        private void ZapelnijKlasaUczniowieComboBox()
        {
            var uniqueClasses = adminResponse.Uczniowie
                .Select(uczen => uczen.Klasa)
                .Distinct()
                .OrderBy(klasa => klasa)
                .ToList();

            klasa_uczniowie.Items.Clear();

            foreach (var klasa in uniqueClasses)
            {
                klasa_uczniowie.Items.Add(klasa);
            }

            klasa_uczniowie.Items.Add("WSZYSTKIE");

            klasa_uczniowie.SelectedIndex = klasa_uczniowie.Items.Count - 1;
        }

        private void klasa_uczniowie_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            FiltrowanieDanychDataGridViewPoKlasie();
        }

        private void FiltrowanieDanychDataGridViewPoKlasie()
        {
            string wybranaKlasa = klasa_uczniowie.SelectedItem.ToString();
            bool pokazWszystkie = wybranaKlasa == "WSZYSTKIE";

            uczniowie.Rows.Clear();

            foreach (var uczen in adminResponse.Uczniowie)
            {
                if (pokazWszystkie || uczen.Klasa == wybranaKlasa)
                {
                    DodajWierszDoDataGridView(uczen);
                }
            }
        }

        private void DodajWierszDoDataGridView(Uczen_a uczen)
        {
            var wychowawca = adminResponse.Nauczyciele.FirstOrDefault(n => n.Klasa == uczen.Klasa)?.ImieNazwisko ?? "Brak";

            int indeksWiersza = uczniowie.Rows.Add();
            uczniowie.Rows[indeksWiersza].Cells["imie_nazwisko_uczen"].Value = uczen.ImieNazwisko;
            uczniowie.Rows[indeksWiersza].Cells["login_uczen"].Value = uczen.Login;
            uczniowie.Rows[indeksWiersza].Cells["klasa_uczen"].Value = uczen.Klasa;
            uczniowie.Rows[indeksWiersza].Cells["wychowawca_uczen"].Value = wychowawca;

            var przyciskZmien = new DataGridViewButtonCell();
            przyciskZmien.Value = "Zmień dane";
            uczniowie.Rows[indeksWiersza].Cells["zmien_dane_uczen"] = przyciskZmien;

            var przyciskUsun = new DataGridViewButtonCell();
            przyciskUsun.Value = "Usuń konto";
            uczniowie.Rows[indeksWiersza].Cells["usun_uczen"] = przyciskUsun;
        }

        private void wyszukiwarka_uczniowie__TextChanged(object sender, System.EventArgs e)
        {
            FiltrujDaneDataGridViewPoWyszukiwarce();
        }

        private void uczniowie_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == uczniowie.Columns["usun_uczen"].Index && e.RowIndex >= 0)
            {
                var uczen = adminResponse.Uczniowie[e.RowIndex];
                var dialogResult = MessageBox.Show($"Czy na pewno chcesz usunąć konto ucznia {uczen.ImieNazwisko}?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    if (UsunKonto(uczen.Login))
                    {
                        MessageBox.Show("Konto zostało usunięte.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //usunięcie ucznia z listy
                        adminResponse.Uczniowie.RemoveAt(e.RowIndex);
                        //usunięcie wiersza z DataGridView
                        uczniowie.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        MessageBox.Show("Nie udało się usunąć konta.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (e.ColumnIndex == uczniowie.Columns["zmien_dane_uczen"].Index && e.RowIndex >= 0)
            {
                var uczen = adminResponse.Uczniowie[e.RowIndex];
                var edytujForm = new edytujUcznia(uczen.ImieNazwisko, uczen.Klasa);

                if (edytujForm.ShowDialog() == DialogResult.OK)
                {
                    string noweImieNazwisko = edytujForm.ImieNazwisko;
                    string nowaKlasa = edytujForm.Klasa;

                    if (EdytujDaneUcznia(uczen.Login, noweImieNazwisko, nowaKlasa))
                    {
                        MessageBox.Show("Dane ucznia zostały zaktualizowane.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //aktualizacja danych ucznia
                        uczen.ImieNazwisko = noweImieNazwisko;
                        uczen.Klasa = nowaKlasa;

                        //aktualizacja wiersza w DataGridView
                        uczniowie.Rows[e.RowIndex].Cells["imie_nazwisko_uczen"].Value = noweImieNazwisko;
                        uczniowie.Rows[e.RowIndex].Cells["klasa_uczen"].Value = nowaKlasa;
                    }
                    else
                    {
                        MessageBox.Show("Nie udało się zaktualizować danych ucznia.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool EdytujDaneUcznia(string login, string noweImieNazwisko, string nowaKlasa)
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{Properties.Resources.adres_api}/edytuj_ucznia";

                var values = new Dictionary<string, string>
                {
                    { "login", login },
                    { "imie_nazwisko", noweImieNazwisko },
                    { "klasa", nowaKlasa }
                };

                var content = new FormUrlEncodedContent(values);

                var response = client.PostAsync(requestUri, content).Result;

                return response.IsSuccessStatusCode;
            }
        }


        private void FiltrujDaneDataGridViewPoWyszukiwarce()
        {
            string searchText = wyszukiwarka_uczniowie.Texts.ToLower();

            uczniowie.Rows.Clear();

            foreach (var uczen in adminResponse.Uczniowie)
            {
                if (string.IsNullOrEmpty(searchText) || uczen.ImieNazwisko.ToLower().Contains(searchText))
                {
                    DodajWierszDoDataGridView(uczen);
                }
            }
        }

        private void nowy_uzytkownik_Click(object sender, System.EventArgs e)
        {
            using (var formNowyUzytkownik = new nowyUzytkownik())
            {
                if (formNowyUzytkownik.ShowDialog() == DialogResult.OK)
                {
                    string imieNazwisko = formNowyUzytkownik.ImieNazwisko;
                    string login = formNowyUzytkownik.Login;
                    string klasa = formNowyUzytkownik.Klasa;
                    string haslo = formNowyUzytkownik.Haslo;

                    try
                    {
                        using (var client = new HttpClient())
                        {
                            var requestUri = $"{Properties.Resources.adres_api}/utworz_ucznia";
                            var content = new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string, string>("login", login),
                                new KeyValuePair<string, string>("imie_nazwisko", imieNazwisko),
                                new KeyValuePair<string, string>("klasa", klasa),
                                new KeyValuePair<string, string>("haslo", haslo)
                            });

                            var response = client.PostAsync(requestUri, content).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Utworzono nowego użytkownika.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                var nowyUczen = new Uczen_a
                                {
                                    Login = login,
                                    ImieNazwisko = imieNazwisko,
                                    Klasa = klasa
                                };
                                adminResponse.Uczniowie.Add(nowyUczen);

                                //dodanie nowego uyztkownika do DataGridView
                                var wychowawca = adminResponse.Nauczyciele.FirstOrDefault(n => n.Klasa == klasa)?.ImieNazwisko ?? "Brak";
                                int rowIndex = uczniowie.Rows.Add();
                                uczniowie.Rows[rowIndex].Cells["imie_nazwisko_uczen"].Value = imieNazwisko;
                                uczniowie.Rows[rowIndex].Cells["login_uczen"].Value = login;
                                uczniowie.Rows[rowIndex].Cells["klasa_uczen"].Value = klasa;
                                uczniowie.Rows[rowIndex].Cells["wychowawca_uczen"].Value = wychowawca;

                                var zmienButton = new DataGridViewButtonCell();
                                zmienButton.Value = "Zmień dane";
                                uczniowie.Rows[rowIndex].Cells["zmien_dane_uczen"] = zmienButton;

                                var usunButton = new DataGridViewButtonCell();
                                usunButton.Value = "Usuń konto";
                                uczniowie.Rows[rowIndex].Cells["usun_uczen"] = usunButton;

                                uczniowie.Refresh();
                            }
                            else
                            {
                                MessageBox.Show("Nie udało się utworzyć nowego użytkownika.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DodajNauczycieliDoDataGridView()
        {
            nauczyciele.Rows.Clear();

            foreach (var nauczyciel in adminResponse.Nauczyciele)
            {
                int rowIndex = nauczyciele.Rows.Add();
                nauczyciele.Rows[rowIndex].Cells["imie_nazwisko_nauczyciel"].Value = nauczyciel.ImieNazwisko;
                nauczyciele.Rows[rowIndex].Cells["login_nauczyciel"].Value = nauczyciel.Login;
                nauczyciele.Rows[rowIndex].Cells["klasa_nauczyciel"].Value = nauczyciel.Klasa;

                //przetwarzanie i dodawanie przedmiotów
                var przedmioty = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(nauczyciel.Inne);
                nauczyciele.Rows[rowIndex].Cells["przedmioty_nauczyciel"].Value = string.Join(", ", przedmioty["przedmioty"]);

                //dodanie przycisków
                var zmienButton = new DataGridViewButtonCell();
                zmienButton.Value = "Zmień dane";
                nauczyciele.Rows[rowIndex].Cells["zmiendane_nauczyciel"] = zmienButton;

                var usunButton = new DataGridViewButtonCell();
                usunButton.Value = "Usuń";
                nauczyciele.Rows[rowIndex].Cells["usun_nauczyciel"] = usunButton;
            }
        }

        private void nauczyciele_wyszukiwarka__TextChanged(object sender, EventArgs e)
        {
            string searchText = nauczyciele_wyszukiwarka.Texts.ToLower();

            nauczyciele.Rows.Clear();

            foreach (var nauczyciel in adminResponse.Nauczyciele)
            {
                if (string.IsNullOrEmpty(searchText) || nauczyciel.ImieNazwisko.ToLower().Contains(searchText))
                {
                    DodajWierszDoDataGridView_n(nauczyciel);
                }
            }
        }

        private void DodajWierszDoDataGridView_n(Nauczyciel_a nauczyciel)
        {
            int indeksWiersza = nauczyciele.Rows.Add();
            nauczyciele.Rows[indeksWiersza].Cells["imie_nazwisko_nauczyciel"].Value = nauczyciel.ImieNazwisko;
            nauczyciele.Rows[indeksWiersza].Cells["login_nauczyciel"].Value = nauczyciel.Login;
            nauczyciele.Rows[indeksWiersza].Cells["klasa_nauczyciel"].Value = nauczyciel.Klasa;

            //konwersja JSON na listę przedmiotów
            var przedmioty = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(nauczyciel.Inne);
            nauczyciele.Rows[indeksWiersza].Cells["przedmioty_nauczyciel"].Value = string.Join(", ", przedmioty["przedmioty"]);

            var przyciskZmien = new DataGridViewButtonCell();
            przyciskZmien.Value = "Zmień dane";
            nauczyciele.Rows[indeksWiersza].Cells["zmiendane_nauczyciel"] = przyciskZmien;

            var przyciskUsun = new DataGridViewButtonCell();
            przyciskUsun.Value = "Usuń";
            nauczyciele.Rows[indeksWiersza].Cells["usun_nauczyciel"] = przyciskUsun;
        }

        private void nowy_nauczyciel_Click(object sender, EventArgs e)
        {
            using (var formNowyNauczyciel = new nowyNauczyciel())
            {
                if (formNowyNauczyciel.ShowDialog() == DialogResult.OK)
                {
                    string imieNazwisko = formNowyNauczyciel.ImieNazwisko;
                    string login = formNowyNauczyciel.Login;
                    string klasa = formNowyNauczyciel.Klasa;
                    string haslo = formNowyNauczyciel.Haslo;
                    var przedmioty = formNowyNauczyciel.WybranePrzedmioty;

                    string przedmiotyJson = JsonConvert.SerializeObject(new { przedmioty });

                    try
                    {
                        using (var client = new HttpClient())
                        {
                            var requestUri = $"{Properties.Resources.adres_api}/utworz_nauczyciela";
                            var content = new FormUrlEncodedContent(new[]
                            {
                        new KeyValuePair<string, string>("login", login),
                        new KeyValuePair<string, string>("imie_nazwisko", imieNazwisko),
                        new KeyValuePair<string, string>("klasa", klasa),
                        new KeyValuePair<string, string>("haslo", haslo),
                        new KeyValuePair<string, string>("przedmioty", przedmiotyJson)
                    });

                            var response = client.PostAsync(requestUri, content).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Utworzono nowego nauczyciela.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                
                                //dodanie nauczyciela do listy i odświeżenie datagridview
                                var nauczycielData = new List<string> { login, imieNazwisko, klasa, przedmiotyJson };
                                adminResponse.NauczycieleRaw.Add(nauczycielData);

                                DodajNauczycieliDoDataGridView();
                            }
                            else
                            {
                                MessageBox.Show("Nie udało się utworzyć nowego nauczyciela.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void nauczyciele_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == nauczyciele.Columns["usun_nauczyciel"].Index && e.RowIndex >= 0)
            {
                var nauczyciel = adminResponse.Nauczyciele[e.RowIndex];
                var dialogResult = MessageBox.Show($"Czy na pewno chcesz usunąć konto nauczyciela {nauczyciel.ImieNazwisko}?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    if (UsunKonto(nauczyciel.Login))
                    {
                        MessageBox.Show("Konto nauczyciela zostało usunięte.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //usuniecie nauczyciela z listy po pomyślnym usunięciu
                        adminResponse.Nauczyciele.RemoveAt(e.RowIndex);
                        nauczyciele.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        MessageBox.Show("Nie udało się usunąć konta nauczyciela.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            } else if (e.ColumnIndex == nauczyciele.Columns["zmiendane_nauczyciel"].Index && e.RowIndex >= 0) {
                var nauczyciel = adminResponse.Nauczyciele[e.RowIndex];
                var przedmioty = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(nauczyciel.Inne)["przedmioty"];
                var edytujForm = new edytujNauczyciela(nauczyciel.ImieNazwisko, nauczyciel.Klasa, przedmioty);

                if (edytujForm.ShowDialog() == DialogResult.OK)
                {
                    string noweImieNazwisko = edytujForm.ImieNazwisko;
                    string nowaKlasa = edytujForm.Klasa;
                    string nowePrzedmioty = JsonConvert.SerializeObject(new { przedmioty = edytujForm.WybranePrzedmioty });

                    if (EdytujDaneNauczyciela(nauczyciel.Login, noweImieNazwisko, nowaKlasa, nowePrzedmioty))
                    {
                        MessageBox.Show("Dane nauczyciela zostały zaktualizowane.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        var nauczycielRaw = adminResponse.NauczycieleRaw.FirstOrDefault(n => n[0] == nauczyciel.Login);
                        if (nauczycielRaw != null)
                        {
                            nauczycielRaw[1] = noweImieNazwisko;
                            nauczycielRaw[2] = nowaKlasa;
                            nauczycielRaw[3] = nowePrzedmioty;
                        }

                        DodajNauczycieliDoDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("Nie udało się zaktualizować danych nauczyciela.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool UsunKonto(string login)
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{Properties.Resources.adres_api}/usun_konto?login={login}";
                var response = client.DeleteAsync(requestUri).Result;

                return response.IsSuccessStatusCode;
            }
        }

        private bool EdytujDaneNauczyciela(string login, string imieNazwisko, string klasa, string przedmiotyJson)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var requestUri = $"{Properties.Resources.adres_api}/edytuj_nauczyciela";

                    var content = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("login", login),
                new KeyValuePair<string, string>("imie_nazwisko", imieNazwisko),
                new KeyValuePair<string, string>("klasa", klasa),
                new KeyValuePair<string, string>("przedmioty", przedmiotyJson)
            });

                    var response = client.PostAsync(requestUri, content).Result;

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void DodajAdminowDoDataGridView()
        {
            admini.Rows.Clear();

            foreach (var admin in adminResponse.Admini)
            {
                int rowIndex = admini.Rows.Add();
                admini.Rows[rowIndex].Cells["login_admin"].Value = admin[0];

                var deleteButton = new DataGridViewButtonCell
                {
                    Value = "Usuń"
                };
                admini.Rows[rowIndex].Cells["usun_admin"] = deleteButton;
            }
        }


        private void admini_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == admini.Columns["usun_admin"].Index && e.RowIndex >= 0)
            {
                string loginAdmina = admini.Rows[e.RowIndex].Cells["login_admin"].Value.ToString();

                var dialogResult = MessageBox.Show($"Czy na pewno chcesz usunąć konto admina {loginAdmina}?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    if (UsunKonto(loginAdmina)) 
                    {
                        MessageBox.Show("Konto admina zostało usunięte.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        adminResponse.Admini.RemoveAt(e.RowIndex);
                        admini.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        MessageBox.Show("Nie udało się usunąć konta admina.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void utworz_admina_Click(object sender, EventArgs e)
        {
            using (var formNowyAdmin = new nowyAdmin())
            {
                if (formNowyAdmin.ShowDialog() == DialogResult.OK)
                {
                    string login = formNowyAdmin.LoginAdmina;
                    string haslo = formNowyAdmin.HasloAdmina;

                    try
                    {
                        using (var client = new HttpClient())
                        {
                            var requestUri = $"{Properties.Resources.adres_api}/utworz_admina";
                            var content = new FormUrlEncodedContent(new[]
                            {
                        new KeyValuePair<string, string>("login", login),
                        new KeyValuePair<string, string>("haslo", haslo)
                    });

                            var response = client.PostAsync(requestUri, content).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Utworzono nowego admina.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                //dodanie admina do listy i odświeżenie datagridview
                                var adminData = new List<string> { login };
                                adminResponse.Admini.Add(adminData);
                                DodajAdminowDoDataGridView();
                            }
                            else
                            {
                                MessageBox.Show("Nie udało się utworzyć nowego admina.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void WypelnijDataGridViewPlanamiLekcji()
        {
            plany_lekcji.Rows.Clear();
            plan_klasy.Items.Clear();

            HashSet<string> unikalneKlasy = new HashSet<string>();

            foreach (var plan in adminResponse.PlanyLekcji)
            {
                int rowIndex = plany_lekcji.Rows.Add();
                plany_lekcji.Rows[rowIndex].Cells["klasa"].Value = plan.Klasa;

                var przyciskEdytuj = new DataGridViewButtonCell
                {
                    Value = "Edytuj"
                };
                plany_lekcji.Rows[rowIndex].Cells["edytuj"] = przyciskEdytuj;

                var przyciskWyswietl = new DataGridViewButtonCell
                {
                    Value = "Wyświetl ➡️"
                };
                plany_lekcji.Rows[rowIndex].Cells["wyswietl_plan"] = przyciskWyswietl;

                unikalneKlasy.Add(plan.Klasa);
            }

            foreach (var klasa in unikalneKlasy)
            {
                plan_klasy.Items.Add(klasa);
            }
            plan_klasy.Items.Add("WSZYSTKIE");
        }

        private void FiltrujDataGridViewPlanamiLekcji()
        {
            string wybranaKlasa = plan_klasy.SelectedItem?.ToString();

            foreach (DataGridViewRow row in plany_lekcji.Rows)
            {
                if (!row.IsNewRow)
                {
                    if (wybranaKlasa == null || wybranaKlasa == "WSZYSTKIE")
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = row.Cells["klasa"].Value?.ToString() == wybranaKlasa;
                    }
                }
            }
        }

        private void plan_klasy_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrujDataGridViewPlanamiLekcji();
        }

        private void plany_lekcji_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == plany_lekcji.Columns["wyswietl_plan"].Index && e.RowIndex >= 0)
            {
                dzien_tygodnia.Enabled = true;
                wyswietl_plan_lekcji.Clear();
                wybranaKlasa = plany_lekcji.Rows[e.RowIndex].Cells["klasa"].Value?.ToString();
            }
        }

        private void WyswietlPlanLekcji(string klasa, string dzienTygodnia)
        {
            var mappingDniTygodnia = new Dictionary<string, string>
            {
                {"Poniedziałek", "Poniedzialek"},
                {"Wtorek", "Wtorek"},
                {"Środa", "Sroda"},
                {"Czwartek", "Czwartek"},
                {"Piątek", "Piatek"}
            };


            string dzienTygodniaKey = mappingDniTygodnia[dzienTygodnia];

            var planLekcji = adminResponse.PlanyLekcji.FirstOrDefault(p => p.Klasa == klasa);
            if (planLekcji != null)
            {
                Dictionary<string, Lekcja> planDnia = typeof(PlanLekcji).GetProperty(dzienTygodniaKey).GetValue(planLekcji) as Dictionary<string, Lekcja>;
                WyswietlPlanDnia(planDnia);
            }
            else
            {
                wspólneMetody.Log("Nie znaleziono planu lekcji dla klasy " + klasa, wyswietl_plan_lekcji);
            }
        }

        private void WyswietlPlanDnia(Dictionary<string, Lekcja> planDnia)
        {
            wyswietl_plan_lekcji.Clear();
            if (planDnia != null)
            {
                foreach (var lekcja in planDnia)
                {
                    //ucięcie 3 pierwszych znaków np. po to aby zamiast lek0 było 0 itd.
                    string numerLekcji = lekcja.Key.Substring(3);

                    string text = $"Lekcja {numerLekcji}: {lekcja.Value.Przedmiot}, Prowadzący: {lekcja.Value.Prowadzacy}, Sala: {lekcja.Value.Sala}";
                    wspólneMetody.Log($"{text}\n", wyswietl_plan_lekcji);
                }
            }
            else
            {
                wspólneMetody.Log("Brak lekcji w tym dniu.", wyswietl_plan_lekcji);
            }
        }


        private void dzien_tygodnia_SelectedIndexChanged(object sender, EventArgs e)
        {
            WyswietlPlanLekcji(wybranaKlasa, dzien_tygodnia.SelectedItem.ToString());
        }
    }
}
