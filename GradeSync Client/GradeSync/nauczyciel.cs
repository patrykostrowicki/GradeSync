    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using GradeSync.klasy;
    using MaterialSkin.Controls;
    using System.Net;
    using Newtonsoft.Json;
    using System.Net.Http;
    using GradeSync.kontrolki;
    using System.Drawing.Printing;

    namespace GradeSync
    {
        public partial class nauczyciel : MaterialForm
        {
            private NauczycielResponse nauczycielResponse;

            private PrintDocument printDocument1 = new PrintDocument();
            private PrintDialog printDialog1 = new PrintDialog();

            private Form loginForm;

            public class Frekwencja
            {
                public string Data { get; set; }
                public string Przedmiot { get; set; }
                public int Typ { get; set; }
            }

            public nauczyciel(NauczycielResponse response, Form loginFormInstance)
            {
                InitializeComponent();
                nauczycielResponse = response;

                //przypisz zdarzenie PrintPage do printDocument1
                printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

                //przypisz printDocument1 do printDialog1
                printDialog1.Document = printDocument1;

                this.wydarzenia.CellClick += new DataGridViewCellEventHandler(wydarzenia_CellClick);
                this.loginForm = loginFormInstance;
            }

            private void nauczyciel_Load(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(nauczycielResponse.Klasa))
                {
                    dane_nauczyciela.Text = $"{nauczycielResponse.ImieNazwisko}";
                    
                    nauczyciel_tabcontrol_main.TabPages.Remove(tabPage1);
                    nauczyciel_tabcontrol_main.TabPages.Remove(tabPage3);     
                }
                else
                {
                    dane_nauczyciela.Text = $"{nauczycielResponse.ImieNazwisko} | Wychowawca klasy {nauczycielResponse.Klasa}";
                    przeg_klasy_lbl.Text = $"Przegląd klasy {nauczycielResponse.Klasa}";

                    wspólneMetody.StylizujDataGridView(przeglad_klasy);
                    wspólneMetody.StylizujDataGridView(frekwencja_tabela);

                    DodajUczniowDoPrzegladuKlasy();
                    WczytajUczniowDoComboBox();
                }

                wystaw_ocene.Enabled = false;

                wspólneMetody.StylizujDataGridView(lista_uczniow);
                wspólneMetody.StylizujDataGridView(lista_ocen);

                wspólneMetody.StylizujDataGridView(wydarzenia);
                wspólneMetody.StylizujDataGridView(uwagi);
                wspólneMetody.StylizujDataGridView(uczniowie_lista);

                DodajUczniowDoDatagridview();

                WypelnijComboBoxPrzedmiotami();

                DodajWydarzeniaDoDataGridView();

                DodajUwagiDoDataGridView();

                DodajUczniowDoDataGridViewUwagi();

                ImportujZajeciaDoListView();
            }

            public void WypelnijComboBoxPrzedmiotami()
            {
                ocena_przedmioty.Items.Clear();

                //jeśli nauczyciel jest wychowawcą klasy, dodaj opcję "Zachowanie" na początku listy
                if (!string.IsNullOrEmpty(nauczycielResponse.Klasa))
                {
                    ocena_przedmioty.Items.Add("Zachowanie");
                }

                if (nauczycielResponse.Inne != null && nauczycielResponse.Inne.Przedmioty != null)
                {
                    foreach (var przedmiot in nauczycielResponse.Inne.Przedmioty)
                    {
                        ocena_przedmioty.Items.Add(przedmiot);
                    }
                }
            }

            private void DodajUczniowDoPrzegladuKlasy()
            {
                przeglad_klasy.Columns.Clear();
                przeglad_klasy.Rows.Clear();

                // Dodanie kolumn
                przeglad_klasy.Columns.Add("imie_nazwisko", "Imię i Nazwisko");
                DodajPrzyciskDoDataGridView(przeglad_klasy, "oceny", "Oceny");
                DodajPrzyciskDoDataGridView(przeglad_klasy, "uwagi", "Uwagi i osiągnięcia");
                DodajPrzyciskDoDataGridView(przeglad_klasy, "frekwencja", "Frekwencja");

                // Dodanie wierszy
                foreach (var uczen in nauczycielResponse.Uczniowie)
                {
                    przeglad_klasy.Rows.Add(uczen.ImieNazwisko);
                }

                // Dodanie obsługi zdarzeń kliknięcia
                przeglad_klasy.CellClick += przeglad_klasy_CellClick;
            }

            private void DodajPrzyciskDoDataGridView(DataGridView dgv, string nazwaKolumny, string tekst)
            {
                DataGridViewButtonColumn przyciskColumn = new DataGridViewButtonColumn();
                przyciskColumn.Name = nazwaKolumny;
                przyciskColumn.HeaderText = "";
                przyciskColumn.Text = tekst;
                przyciskColumn.UseColumnTextForButtonValue = true;
                dgv.Columns.Add(przyciskColumn);
            }


            private void przeglad_klasy_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    var uczenLogin = nauczycielResponse.Uczniowie[e.RowIndex].Login;
                    var uczenKlasa = nauczycielResponse.Uczniowie[e.RowIndex].Klasa;

                    var clickedColumn = przeglad_klasy.Columns[e.ColumnIndex].Name;
                    switch (clickedColumn)
                    {
                        case "oceny":
                            output_przeg_klasy.Text = null;
                            var oceny = PobierzOcenyUcznia(uczenKlasa, uczenLogin, "wszyscy");
                            WyswietlOceny(oceny);
                            break;
                        case "uwagi":
                            output_przeg_klasy.Text = null;
                            var uwagi = PobierzUwagiUcznia(uczenLogin);
                            WyswietlUwagi(uwagi);
                            break;
                        case "frekwencja":
                            output_przeg_klasy.Text = null;
                            var frekwencja = PobierzFrekwencjeUcznia(uczenLogin);
                            WyswietlFrekwencje(frekwencja);
                            break;
                    }
                }
            }

        private void WyswietlFrekwencje(List<Dictionary<string, string>> frekwencja)
        {
            if (frekwencja == null || frekwencja.Count == 0)
            {
                wspólneMetody.Log("Brak danych frekwencji dla tego ucznia.", output_przeg_klasy);
                return;
            }

            //grupowanie danych frekwencji według przedmiotu
            var grupowaneFrekwencje = frekwencja
                .GroupBy(f => f["przedmiot"])
                .ToDictionary(group => group.Key, group => group.ToList());

            foreach (var przedmiot in grupowaneFrekwencje.Keys)
            {
                int nieobecnosci = grupowaneFrekwencje[przedmiot].Count(f => f["typ"] == "1");
                int usprawiedliwione = grupowaneFrekwencje[przedmiot].Count(f => f["typ"] == "2");
                int spoznienia = grupowaneFrekwencje[przedmiot].Count(f => f["typ"] == "3");

                string formattedWpis = $"Przedmiot: {przedmiot}\nNieobecności: {nieobecnosci}\nUsprawiedliwione: {usprawiedliwione}\nSpóźnienia: {spoznienia}\n";
                wspólneMetody.Log(formattedWpis, output_przeg_klasy);
            }
        }

        private void WyswietlUwagi(List<Dictionary<string, string>> uwagi)
        {
            if (uwagi == null || uwagi.Count == 0)
            {
                wspólneMetody.Log("Brak uwag dla tego ucznia.", output_przeg_klasy);
                return;
            }

            //sortowanie uwag od najnowszych do najstarszych
            uwagi.Sort((a, b) => DateTime.Parse(b["data"]).CompareTo(DateTime.Parse(a["data"])));

            foreach (var uwaga in uwagi)
            {
                string typTekstowy = uwaga["typ"] == "1" ? "Uwaga" : "Osiągnięcie";
                string formattedUwaga = $"Wystawił: {uwaga["wystawil"]}\nData: {uwaga["data"]}\nTyp: {typTekstowy}\nTreść: {uwaga["tresc"]}\n";

                wspólneMetody.Log(formattedUwaga, output_przeg_klasy);
            }
        }


        private void WyswietlOceny(List<List<string>> oceny)
        {
            if (oceny == null || !oceny.Any())
            {
                wspólneMetody.Log("Brak ocen dla tego ucznia.", output_przeg_klasy);
                return;
            }

            foreach (var ocena in oceny)
            {
                string formattedOcena;

                //specjalne formatowanie dla oceny z zachowania
                if (ocena[0] == "Ocena z zachowania")
                {
                    formattedOcena = ocena[0] + ": " + FormatujOcene(ocena[3]);
                }
                else
                {
                    //standardowe formatowanie dla innych ocen
                    formattedOcena = FormatujOcene(ocena[0]) + " | " + string.Join(" | ", ocena.Skip(1));
                }

                wspólneMetody.Log(formattedOcena, output_przeg_klasy);
            }
        }

            private void DodajUczniowDoDatagridview()
            {
                lista_uczniow.Columns.Clear();
                lista_uczniow.Rows.Clear();

                lista_uczniow.Columns.Add("imie_nazwisko", "Imię i Nazwisko");
                lista_uczniow.Columns.Add("klasa", "Klasa");
                DataGridViewButtonColumn akcjeColumn = new DataGridViewButtonColumn();
                akcjeColumn.Name = "akcje";
                akcjeColumn.HeaderText = "Akcje";
                akcjeColumn.Text = "Oceny ➡️";
                akcjeColumn.UseColumnTextForButtonValue = true;
                lista_uczniow.Columns.Add(akcjeColumn);

                lista_uczniow.Columns["imie_nazwisko"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                lista_uczniow.Columns["imie_nazwisko"].FillWeight = 60;
                lista_uczniow.Columns["klasa"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                lista_uczniow.Columns["klasa"].FillWeight = 20;
                lista_uczniow.Columns["akcje"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                lista_uczniow.Columns["akcje"].FillWeight = 20;

                foreach (var uczen in nauczycielResponse.Uczniowie)
                {
                    lista_uczniow.Rows.Add(uczen.ImieNazwisko, uczen.Klasa);
                }

                lista_uczniow.CellClick += lista_uczniow_CellClick;
            }

            private void lista_uczniow_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                if (e.ColumnIndex == lista_uczniow.Columns["akcje"].Index && e.RowIndex >= 0)
                {                 
                    var uczenLogin = nauczycielResponse.Uczniowie[e.RowIndex].Login;
                    var uczenImie = nauczycielResponse.Uczniowie[e.RowIndex].ImieNazwisko;
                    var uczenKlasa = nauczycielResponse.Uczniowie[e.RowIndex].Klasa;

                    uczen_lbl.Text = uczenImie;
                    login_lbl.Text = uczenLogin;
                    klasa_lbl.Text = uczenKlasa;

                    var oceny = PobierzOcenyUcznia(uczenKlasa, uczenLogin, nauczycielResponse.Login);

                    if (oceny != null)
                    {
                        ZaładujOcenyDoDataGridView(oceny);
                        var ocenaZachowania = oceny.FirstOrDefault(ocena => ocena[0] == "Ocena z zachowania");
                        if (ocenaZachowania != null)
                        {
                            zachowanie_lbl.Text = FormatujOcene(ocenaZachowania[3]);
                        }
                        else
                        {
                            zachowanie_lbl.Text = "Brak oceny z zachowania";
                        }
                    }
                }
            }

        private List<Dictionary<string, string>> PobierzFrekwencjeUcznia(string uczenLogin)
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                try
                {
                    var url = $"{Properties.Resources.adres_api}/zwroc_frekwencje_ucznia?login={uczenLogin}";
                    var jsonResponse = client.DownloadString(url);
                    return JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonResponse);
                }
                catch (WebException ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas łączenia z API: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }


        private void wyszukiwarka__TextChanged(object sender, EventArgs e)
            {
                lista_uczniow.CurrentCell = null; //odznaczanie bieżącej komórki, aby uniknąć błędów przy zmianie widoczności

                string searchText = wyszukiwarka.Texts.ToLower();

                //iterowanie przez wszystkie wiersze i ustawianie ich widoczności
                foreach (DataGridViewRow row in lista_uczniow.Rows)
                {
                    bool visible = string.IsNullOrEmpty(searchText) || row.Cells["imie_nazwisko"].Value.ToString().ToLower().Contains(searchText);
                    row.Visible = visible;
                }

                lista_uczniow.Refresh();
            }

            private List<List<string>> PobierzOcenyUcznia(string klasa, string uczenLogin, string nauczycielLogin)
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    try
                    {
                        var url = $"{Properties.Resources.adres_api}/zwroc_oceny_ucznia?klasa={klasa}&login={uczenLogin}&nauczyciel={nauczycielLogin}";
                        var jsonResponse = client.DownloadString(url);
                        return JsonConvert.DeserializeObject<List<List<string>>>(jsonResponse);
                    }
                    catch (WebException ex)
                    {
                        //obsługa błędów związanych z zapytaniem do API
                        MessageBox.Show($"Wystąpił błąd podczas łączenia z API: {ex.Message}","Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
            }

        private List<Dictionary<string, string>> PobierzUwagiUcznia(string uczenLogin)
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                try
                {
                    var url = $"{Properties.Resources.adres_api}/zwroc_uwagi_ucznia?login={uczenLogin}";
                    var jsonResponse = client.DownloadString(url);
                    return JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonResponse);
                }
                catch (WebException ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas łączenia z API: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        private void ZaładujOcenyDoDataGridView(List<List<string>> oceny)
        {
            lista_ocen.CellClick -= lista_ocen_CellClick;

            lista_ocen.Columns.Clear();
            lista_ocen.Rows.Clear();

            lista_ocen.Columns.Add("ocena", "Ocena");
            lista_ocen.Columns.Add("przedmiot", "Przedmiot");
            lista_ocen.Columns.Add("opis", "Opis");
            lista_ocen.Columns.Add("data_wystawienia", "Data Wystawienia");

            DodajKolumnyPrzyciskow(lista_ocen);

            foreach (var ocena in oceny)
            {
                if (ocena[0] != "Ocena z zachowania")
                {
                    string ocenaFormatowana = FormatujOcene(ocena[0]);
                    lista_ocen.Rows.Add(ocenaFormatowana, ocena[2], ocena[3], ocena[1]);
                }
            }

            lista_ocen.CellClick += lista_ocen_CellClick;
        }


        private void DodajKolumnyPrzyciskow(DataGridView dgv)
            {
                DataGridViewButtonColumn edytujColumn = new DataGridViewButtonColumn();
                edytujColumn.Name = "edytuj";
                edytujColumn.HeaderText = "Edytuj";
                edytujColumn.Text = "Edytuj";
                edytujColumn.UseColumnTextForButtonValue = true; // To sprawia, że przycisk pokazuje tekst "Edytuj"
                dgv.Columns.Add(edytujColumn);

                DataGridViewButtonColumn usunColumn = new DataGridViewButtonColumn();
                usunColumn.Name = "usun";
                usunColumn.HeaderText = "Usuń";
                usunColumn.Text = "Usuń";
                usunColumn.UseColumnTextForButtonValue = true; // To sprawia, że przycisk pokazuje tekst "Usuń"
                dgv.Columns.Add(usunColumn);
            }

            private void ZaktualizujOceny()
            {
                var oceny = PobierzOcenyUcznia(klasa_lbl.Text, login_lbl.Text, nauczycielResponse.Login);
                if (oceny != null && oceny.Count > 0)
                {
                    ZaładujOcenyDoDataGridView(oceny);
                }
                else
                {
                    lista_ocen.Rows.Clear();
                }
            }

            private void lista_ocen_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == lista_ocen.Columns["edytuj"].Index)
                {
                    //pobieranie danych z klikniętego wierszza
                    string ocena = lista_ocen.Rows[e.RowIndex].Cells["ocena"].Value.ToString();
                    string przedmiot = lista_ocen.Rows[e.RowIndex].Cells["przedmiot"].Value.ToString();
                    string opis = lista_ocen.Rows[e.RowIndex].Cells["opis"].Value.ToString();
                    string dataWystawienia = lista_ocen.Rows[e.RowIndex].Cells["data_wystawienia"].Value.ToString();

                    var edytujOceneForm = new kontrolki.edytowanieOceny(ocena, przedmiot, opis, dataWystawienia);
                    if (edytujOceneForm.ShowDialog() == DialogResult.OK)
                    {
                        string nowaOcena = edytujOceneForm.WybranaOcena;

                        using (var client = new HttpClient())
                        {
                            var requestUri = $"{Properties.Resources.adres_api}/edytuj_ocene";
                            var requestData = new
                            {
                                nowa_ocena = FormatujOcene(nowaOcena),
                                opis = opis,
                                ocena = FormatujOcene(ocena),
                                klasa = klasa_lbl.Text,
                                login = login_lbl.Text,
                                nauczyciel = nauczycielResponse.Login,
                                przedmiot = przedmiot,
                                data = dataWystawienia
                            };

                            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                            var response = client.PutAsync(requestUri, jsonContent).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = response.Content.ReadAsStringAsync().Result;
                                var resultData = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseContent);

                                if (resultData != null && resultData.ContainsKey("success") && resultData["success"])
                                {
                                    MessageBox.Show("Ocena została pomyślnie zedytowana", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ZaktualizujOceny();
                                }
                                else
                                {
                                    MessageBox.Show("Wystąpił błąd przy edycji oceny", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ZaktualizujOceny();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Wystąpił błąd przy komunikacji z serwerem", "Błąd Komunikacji", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                else if (e.RowIndex >= 0 && e.ColumnIndex == lista_ocen.Columns["usun"].Index)
                {
                    string ocena = lista_ocen.Rows[e.RowIndex].Cells["ocena"].Value.ToString();
                    string przedmiot = lista_ocen.Rows[e.RowIndex].Cells["przedmiot"].Value.ToString();
                    string opis = lista_ocen.Rows[e.RowIndex].Cells["opis"].Value.ToString();
                    string dataWystawienia = lista_ocen.Rows[e.RowIndex].Cells["data_wystawienia"].Value.ToString();

                    string confirmMessage = $"Czy na pewno chcesz usunąć ocenę?\nOcena: {ocena}\nPrzedmiot: {przedmiot}\nOpis: {opis}\nData Wystawienia: {dataWystawienia}";

                    var result = MessageBox.Show(confirmMessage, "Potwierdzenie usunięcia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        using (var client = new HttpClient())
                        {
                            var requestUri = $"{Properties.Resources.adres_api}/usun_ocene?opis={opis}&ocena={FormatujOcene(ocena)}&klasa={klasa_lbl.Text}&login={login_lbl.Text}&nauczyciel={nauczycielResponse.Login}&przedmiot={przedmiot}&data={dataWystawienia}";
                            var response = client.DeleteAsync(requestUri).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = response.Content.ReadAsStringAsync().Result;
                                var resultData = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseContent);

                                if (resultData != null && resultData.ContainsKey("success") && resultData["success"])
                                {
                                    MessageBox.Show("Ocena została pomyślnie usunięta", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ZaktualizujOceny();
                                }
                                else
                                {
                                    MessageBox.Show("Wystąpił błąd przy usuwaniu oceny", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ZaktualizujOceny();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Wystąpił błąd przy komunikacji z serwerem", "Błąd Komunikacji", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }


            private string FormatujOcene(string ocena)
            {
                double liczbaOcena;

                if (ocena.EndsWith(".5") && double.TryParse(ocena.TrimEnd('.', '5'), out liczbaOcena))
                {
                    return liczbaOcena + "+";
                }

                else if (ocena.EndsWith(".8") && double.TryParse(ocena.TrimEnd('.', '8'), out liczbaOcena))
                {
                    return (liczbaOcena + 1) + "-";
                }

                else if (ocena.EndsWith("+"))
                {
                    return ocena.Replace("+", ".5");
                }

                else if (ocena.EndsWith("-") && double.TryParse(ocena.TrimEnd('-'), out liczbaOcena))
                {
                    return (liczbaOcena - 1) + ".8";
                }

                else if (int.TryParse(ocena, out int liczbaCalkowita))
                {
                    return liczbaCalkowita + ".0";
                }

                else if (ocena.EndsWith(".0"))
                {
                    return ocena.Replace(".0", "");
                }

                return ocena;
            }

            private void wystaw_ocene_Click(object sender, EventArgs e)
            {
                if (ocena_przedmioty.SelectedItem == null || string.IsNullOrEmpty(ocena_przedmioty.SelectedItem.ToString()))
                {
                    MessageBox.Show("Proszę wybrać przedmiot.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var wystawianieOcenyForm = new wystawianieOceny(uczen_lbl.Text, klasa_lbl.Text, ocena_przedmioty.SelectedItem.ToString());

                    if (wystawianieOcenyForm.ShowDialog() == DialogResult.OK)
                    {
                        string wybranaOcena = wystawianieOcenyForm.WybranaOcena;
                        string opis = wystawianieOcenyForm.Opis;
                        using (var client = new HttpClient())
                        {
                            var requestUri = $"{Properties.Resources.adres_api}/wystaw_ocene";
                            var requestData = new
                            {
                                uczen_login = login_lbl.Text,
                                wystawil = nauczycielResponse.ImieNazwisko,
                                ocena = FormatujOcene(wybranaOcena),
                                opis = opis,
                                klasa = klasa_lbl.Text,
                                przedmiot = ocena_przedmioty.SelectedItem.ToString(),
                                wystawil_login = nauczycielResponse.Login
                            };

                            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(requestUri, jsonContent).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = response.Content.ReadAsStringAsync().Result;
                                var resultData = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseContent);

                                if (resultData != null && resultData.ContainsKey("success") && resultData["success"])
                                {
                                    if(ocena_przedmioty.SelectedItem.ToString() == "Zachowanie")
                                    {
                                        MessageBox.Show("Ocena została pomyślnie wystawiona", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        zachowanie_lbl.Text = FormatujOcene(wybranaOcena);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Ocena została pomyślnie wystawiona", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        ZaktualizujOceny(); 
                                    }                                   
                                }
                                else
                                {
                                    if (ocena_przedmioty.SelectedItem.ToString() == "Zachowanie")
                                    {
                                        MessageBox.Show("Wystąpił błąd przy wystawianiu oceny", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Wystąpił błąd przy wystawianiu oceny", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        ZaktualizujOceny();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Wystąpił błąd przy komunikacji z serwerem", "Błąd Komunikacji", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }

            private void uczen_lbl_TextChanged(object sender, EventArgs e)
            {
                if (uczen_lbl.Text == "brak")
                {
                    wystaw_ocene.Enabled = false;
                }
                else
                {
                    wystaw_ocene.Enabled = true;
                }
            }

        private void textbox_przeg_klasy__TextChanged(object sender, EventArgs e)
        {
            przeglad_klasy.CurrentCell = null; //odznaczanie bieżącej komórki, aby uniknąć błędów przy zmianie widoczności

            string searchText = textbox_przeg_klasy.Texts.ToLower();

            //iterowanie przez wszystkie wiersze i ustawianie ich widoczności
            foreach (DataGridViewRow row in przeglad_klasy.Rows)
            {
                bool visible = string.IsNullOrEmpty(searchText) || row.Cells["imie_nazwisko"].Value.ToString().ToLower().Contains(searchText);
                row.Visible = visible;
            }

            przeglad_klasy.Refresh();
        }

        private void drukowanie_Click(object sender, EventArgs e)
        {
            //wyświetl dialog wyboru drukarki
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            //wydrukuj zawartość RichTextBox
            e.Graphics.DrawString(output_przeg_klasy.Text, output_przeg_klasy.Font, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top);
        }

        private void DodajWydarzeniaDoDataGridView()
        {
            // Assuming wydarzenia, nauczycielResponse, and filtr_klasa have been initialized elsewhere
            if (wydarzenia == null || nauczycielResponse == null)
            {
                // Handle the case where wydarzenia or nauczycielResponse are not initialized
                return;
            }

            HashSet<string> unikatoweKlasy = new HashSet<string>();

            wydarzenia.Rows.Clear();

            foreach (var wydarzenie in nauczycielResponse.Wydarzenia)
            {
                int rowIndex = wydarzenia.Rows.Add();
                wydarzenia.Rows[rowIndex].Cells["wystawil"].Value = wydarzenie.Wystawil;
                wydarzenia.Rows[rowIndex].Cells["klasa_wydarzenie"].Value = wydarzenie.Klasa;
                wydarzenia.Rows[rowIndex].Cells["data"].Value = wydarzenie.Data.ToShortDateString();
                wydarzenia.Rows[rowIndex].Cells["termin"].Value = wydarzenie.Termin?.ToShortDateString() ?? "";
                wydarzenia.Rows[rowIndex].Cells["przedmiot_wyd"].Value = wydarzenie.Przedmiot;
                wydarzenia.Rows[rowIndex].Cells["opis_wyd"].Value = wydarzenie.Opis;
                wydarzenia.Rows[rowIndex].Cells["typ"].Value = ZamienTypNaString(wydarzenie.Typ);

                // Add buttons only if wydarzenie.WystawilLogin matches nauczycielResponse.Login
                if (wydarzenie.WystawilLogin == nauczycielResponse.Login)
                {
                    var usunButton = new DataGridViewButtonCell();
                    usunButton.Value = "Usuń";
                    wydarzenia.Rows[rowIndex].Cells["usun_wyd"] = usunButton;

                    var edytujButton = new DataGridViewButtonCell();
                    edytujButton.Value = "Edytuj";
                    wydarzenia.Rows[rowIndex].Cells["edytuj_wyd"] = edytujButton;
                }

                // Add class name to the HashSet to ensure uniqueness
                unikatoweKlasy.Add(wydarzenie.Klasa);
            }

            // Update filtr_klasa ComboBox with unique class names
            filtr_klasa.Items.Clear();
            foreach (var klasa in unikatoweKlasy)
            {
                filtr_klasa.Items.Add(klasa);
            }
            filtr_klasa.Items.Add("WSZYSTKIE");
            filtr_klasa.SelectedItem = "WSZYSTKIE";
            filtr_typ.SelectedItem = "WSZYSTKIE";
            filtr_twoje.SelectedItem = "WSZYSTKIE";
        }

        private string ZamienTypNaString(int typ)
        {
            switch (typ)
            {
                case 1: return "sprawdzian";
                case 2: return "kartkówka";
                case 3: return "zadanie";
                case 4: return "projekt";
                default: return "inny";
            }
        }

        private void FiltrujWydarzenia()
        {
            if (wydarzenia == null || nauczycielResponse == null)
            {
                return;
            }

            string wybranyTyp = filtr_typ.SelectedIndex != -1 ? filtr_typ.SelectedItem.ToString() : "WSZYSTKIE";
            string wybranaKlasa = filtr_klasa.SelectedIndex != -1 ? filtr_klasa.SelectedItem.ToString() : "WSZYSTKIE";
            string wybraneWydarzenia = filtr_twoje.SelectedIndex != -1 ? filtr_twoje.SelectedItem.ToString() : "WSZYSTKIE";

            wydarzenia.Rows.Clear();

            foreach (var wydarzenie in nauczycielResponse.Wydarzenia)
            {
                bool pasujeDoFiltruTypu = (wybranyTyp == "WSZYSTKIE" || ZamienTypNaString(wydarzenie.Typ) == wybranyTyp);
                bool pasujeDoFiltruKlasy = (wybranaKlasa == "WSZYSTKIE" || wydarzenie.Klasa == wybranaKlasa);
                bool pasujeDoFiltruTwoje = (wybraneWydarzenia == "WSZYSTKIE" || wydarzenie.WystawilLogin == nauczycielResponse.Login);

                if (pasujeDoFiltruTypu && pasujeDoFiltruKlasy && pasujeDoFiltruTwoje)
                {
                    int rowIndex = wydarzenia.Rows.Add();
                    wydarzenia.Rows[rowIndex].Cells["wystawil"].Value = wydarzenie.Wystawil;
                    wydarzenia.Rows[rowIndex].Cells["klasa_wydarzenie"].Value = wydarzenie.Klasa;
                    wydarzenia.Rows[rowIndex].Cells["data"].Value = wydarzenie.Data.ToShortDateString();
                    wydarzenia.Rows[rowIndex].Cells["termin"].Value = wydarzenie.Termin?.ToShortDateString() ?? "";
                    wydarzenia.Rows[rowIndex].Cells["przedmiot_wyd"].Value = wydarzenie.Przedmiot;
                    wydarzenia.Rows[rowIndex].Cells["opis_wyd"].Value = wydarzenie.Opis;
                    wydarzenia.Rows[rowIndex].Cells["typ"].Value = ZamienTypNaString(wydarzenie.Typ);

                    if (wydarzenie.WystawilLogin == nauczycielResponse.Login)
                    {
                        var usunButton = new DataGridViewButtonCell();
                        usunButton.Value = "Usuń";
                        wydarzenia.Rows[rowIndex].Cells["usun_wyd"] = usunButton;

                        var edytujButton = new DataGridViewButtonCell();
                        edytujButton.Value = "Edytuj";
                        wydarzenia.Rows[rowIndex].Cells["edytuj_wyd"] = edytujButton;
                    }
                }
            }

            DodajKolumnyPrzyciskowWydarzenia(wydarzenia);
        }

        private void filtr_typ_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrujWydarzenia();
        }

        private void filtr_klasa_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrujWydarzenia();
        }

        private void filtr_opis__TextChanged(object sender, EventArgs e)
        {
            wydarzenia.CurrentCell = null;

            string searchText = filtr_opis.Texts.ToLower();

            foreach (DataGridViewRow row in wydarzenia.Rows)
            {
                if (!row.IsNewRow)
                {
                    string opis = row.Cells["opis_wyd"].Value?.ToString().ToLower() ?? "";

                    bool visible = string.IsNullOrEmpty(searchText) || opis.Contains(searchText);
                    row.Visible = visible;
                }
            }

            wydarzenia.Refresh();
        }

        private void filtr_twoje_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrujWydarzenia();
                
        }

        private void DodajKolumnyPrzyciskowWydarzenia(DataGridView dgv)
        {
            if (!dgv.Columns.Contains("edytuj_wyd"))
            {
                DataGridViewButtonColumn edytujColumn = new DataGridViewButtonColumn();
                edytujColumn.Name = "edytuj_wyd";
                edytujColumn.HeaderText = "Edytuj";
                edytujColumn.Text = "Edytuj";
                edytujColumn.UseColumnTextForButtonValue = true;
                dgv.Columns.Add(edytujColumn);
            }

            if (!dgv.Columns.Contains("usun_wyd"))
            {
                DataGridViewButtonColumn usunColumn = new DataGridViewButtonColumn();
                usunColumn.Name = "usun_wyd";
                usunColumn.HeaderText = "Usuń";
                usunColumn.Text = "Usuń";
                usunColumn.UseColumnTextForButtonValue = true;
                dgv.Columns.Add(usunColumn);
            }
        }

        private Wydarzenie_n PobierzWydarzenieZDataGridView(int rowIndex)
        {
            string data = wydarzenia.Rows[rowIndex].Cells["data"].Value.ToString();
            string przedmiot = wydarzenia.Rows[rowIndex].Cells["przedmiot_wyd"].Value.ToString();
            string klasa = wydarzenia.Rows[rowIndex].Cells["klasa_wydarzenie"].Value.ToString();
            string opis = wydarzenia.Rows[rowIndex].Cells["opis_wyd"].Value.ToString();

            return nauczycielResponse.Wydarzenia.FirstOrDefault(w => w.Data.ToShortDateString() == data && w.Przedmiot == przedmiot && w.Klasa == klasa && w.Opis == opis);
        }

        private void wydarzenia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == wydarzenia.Columns["usun_wyd"].Index)
            {
                var wydarzenie = PobierzWydarzenieZDataGridView(e.RowIndex);

                if (wydarzenie != null)
                {
                    string confirmMessage = $"Czy na pewno chcesz usunąć wydarzenie?\nKlasa: {wydarzenie.Klasa}\nPrzedmiot: {wydarzenie.Przedmiot}\nOpis: {wydarzenie.Opis}\nData: {wydarzenie.Data.ToShortDateString()}\nTermin: {(wydarzenie.Termin.HasValue ? wydarzenie.Termin.Value.ToShortDateString() : "brak")}";
                    var result = MessageBox.Show(confirmMessage, "Potwierdzenie usunięcia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        UsunWydarzenie(wydarzenie);
                    }
                }
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == wydarzenia.Columns["edytuj_wyd"].Index)
            {
                var wydarzenie = PobierzWydarzenieZDataGridView(e.RowIndex);
                if (wydarzenie != null)
                {
                    var edytujForm = new edytowanieWydarzenia(wydarzenie.Klasa, wydarzenie.Przedmiot, wydarzenie.Opis, wydarzenie.Data.ToString(), wydarzenie.Termin.ToString(), ZamienTypNaString(wydarzenie.Typ));
                    if (edytujForm.ShowDialog() == DialogResult.OK)
                    {
                        EdytujWydarzenie(wydarzenie, edytujForm.NowyTermin, edytujForm.NowyOpis, edytujForm.NowyTyp);
                    }
                }
            }
        }

        private void EdytujWydarzenie(Wydarzenie_n wydarzenie, DateTime nowyTermin, string nowyOpis, int nowyTyp)
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{Properties.Resources.adres_api}/edytuj_wydarzenie?opis={wydarzenie.Opis}&typ={wydarzenie.Typ}&klasa={wydarzenie.Klasa}&wystawil_login={wydarzenie.WystawilLogin}&przedmiot={wydarzenie.Przedmiot}&data={wydarzenie.Data.ToString("yyyy-MM-dd")}&termin={(wydarzenie.Termin.HasValue ? wydarzenie.Termin.Value.ToString("yyyy-MM-dd") : "")}&nowy_termin={nowyTermin.ToString("yyyy-MM-dd")}&nowy_opis={nowyOpis}&nowy_typ={nowyTyp}";
                var response = client.PutAsync(requestUri, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    var eventToUpdate = nauczycielResponse.Wydarzenia.FirstOrDefault(e => e.Data.ToShortDateString() == wydarzenie.Data.ToShortDateString() && e.Przedmiot == wydarzenie.Przedmiot && e.Klasa == wydarzenie.Klasa && e.Opis == wydarzenie.Opis);
                    if (eventToUpdate != null)
                    {
                        eventToUpdate.Termin = nowyTermin;
                        eventToUpdate.Opis = nowyOpis;
                        eventToUpdate.Typ = nowyTyp;
                    }

                    MessageBox.Show("Wydarzenie zostało pomyślnie zedytowane", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DodajWydarzeniaDoDataGridView();
                }
                else
                {
                    MessageBox.Show("Wystąpił błąd przy edycji wydarzenia", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UsunWydarzenie(Wydarzenie_n wydarzenie)
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{Properties.Resources.adres_api}/usun_wydarzenie?opis={wydarzenie.Opis}&typ={wydarzenie.Typ}&klasa={wydarzenie.Klasa}&wystawil_login={wydarzenie.WystawilLogin}&przedmiot={wydarzenie.Przedmiot}&data={wydarzenie.Data.ToString("yyyy-MM-dd")}&termin={(wydarzenie.Termin.HasValue ? wydarzenie.Termin.Value.ToString("yyyy-MM-dd") : "")}";
                var response = client.DeleteAsync(requestUri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var resultData = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseContent);

                    if (resultData != null && resultData.ContainsKey("success") && resultData["success"])
                    {
                        MessageBox.Show("Wydarzenie zostało pomyślnie usunięte", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        nauczycielResponse.Wydarzenia.Remove(wydarzenie);
                        DodajWydarzeniaDoDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił błąd przy usuwaniu wydarzenia", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Wystąpił błąd przy komunikacji z serwerem", "Błąd Komunikacji", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private async void nowe_wydarzenie_Click(object sender, EventArgs e)
        {
            var unikatoweKlasy = new HashSet<string>();
            foreach (var wydarzenie in nauczycielResponse.Wydarzenia)
            {
                unikatoweKlasy.Add(wydarzenie.Klasa);
            }

            var przedmioty = nauczycielResponse.Inne?.Przedmioty ?? new List<string>();
            var form = new noweWydarzenie(przedmioty, unikatoweKlasy);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var termin = form.WybranyTermin.ToString("yyyy-MM-dd");
                var przedmiot = form.WybranyPrzedmiot;
                var opis = form.Opis;
                var typ = form.WybranyTyp.ToString();
                var klasa = form.WybranaKlasa;

                using (var client = new HttpClient())
                {
                    var requestUri = $"{Properties.Resources.adres_api}/utworz_wydarzenie";
                    var postData = new Dictionary<string, string>
                    {
                        { "wystawil_login", nauczycielResponse.Login },
                        { "wystawil", nauczycielResponse.ImieNazwisko },
                        { "termin", termin },
                        { "przedmiot", przedmiot },
                        { "opis", opis },
                        { "typ", typ },
                        { "klasa", klasa }
                    };

                    var encodedContent = new FormUrlEncodedContent(postData);
                    var response = await client.PostAsync(requestUri, encodedContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var resultData = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseContent);

                        if (resultData != null && resultData.ContainsKey("success") && resultData["success"])
                        {
                            MessageBox.Show("Wydarzenie zostało pomyślnie utworzone", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            var newEvent = new Wydarzenie_n
                            {
                                Klasa = klasa,
                                Wystawil = nauczycielResponse.ImieNazwisko,
                                WystawilLogin = nauczycielResponse.Login,
                                Data = DateTime.Now,
                                Termin = DateTime.Parse(termin),
                                Przedmiot = przedmiot,
                                Opis = opis,
                                Typ = int.Parse(typ)
                            };
                            nauczycielResponse.Wydarzenia.Add(newEvent);

                            DodajWydarzeniaDoDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Wystąpił błąd przy tworzeniu wydarzenia", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił błąd przy komunikacji z serwerem", "Błąd Komunikacji", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void DodajUwagiDoDataGridView()
        {
            if (uwagi == null || nauczycielResponse == null)
            {
                return;
            }

            HashSet<string> unikatoweKlasy = new HashSet<string>();

            uwagi.Rows.Clear();

            foreach (var uwaga in nauczycielResponse.UwagiIOsiagniecia)
            {
                int rowIndex = uwagi.Rows.Add();
                uwagi.Rows[rowIndex].Cells["imie_nazwisko_uwaga"].Value = uwaga.Uczen;
                uwagi.Rows[rowIndex].Cells["klasa_uwaga"].Value = uwaga.Klasa;
                uwagi.Rows[rowIndex].Cells["typ_uwaga"].Value = ZamienTypNaString(uwaga.Typ);
                uwagi.Rows[rowIndex].Cells["opis_uwaga"].Value = uwaga.Tresc;

                var edytujButton = new DataGridViewButtonCell();
                edytujButton.Value = "Edytuj";
                uwagi.Rows[rowIndex].Cells["edytuj_uwaga"] = edytujButton;

                var usunButton = new DataGridViewButtonCell();
                usunButton.Value = "Usuń";
                uwagi.Rows[rowIndex].Cells["usun_uwaga"] = usunButton;

                unikatoweKlasy.Add(uwaga.Klasa);
            }

            uwagi_filtr_klasa.Items.Clear();
            foreach (var klasa in unikatoweKlasy)
            {
                uwagi_filtr_klasa.Items.Add(klasa);
            }
            uwagi_filtr_klasa.Items.Add("WSZYSTKIE");
            uwagi_filtr_klasa.SelectedItem = "WSZYSTKIE";

            uwaga_filtr_typ.Items.Clear();
            uwaga_filtr_typ.Items.AddRange(new object[] { "uwaga", "osiągnięcie", "WSZYSTKIE" });
            uwaga_filtr_typ.SelectedItem = "WSZYSTKIE";
        }

        private string ZamienTypNaString_uwaga(int typ)
        {
            switch (typ)
            {
                case 1:
                    return "uwaga";
                case 2:
                    return "osiągnięcie";
                default:
                    return "";
            }
        }

        private void FiltrujUwagi()
        {
            if (uwagi == null || nauczycielResponse == null)
            {
                return;
            }

            string wybranyTyp = uwaga_filtr_typ.SelectedIndex != -1 ? uwaga_filtr_typ.SelectedItem.ToString() : "WSZYSTKIE";
            string wybranaKlasa = uwagi_filtr_klasa.SelectedIndex != -1 ? uwagi_filtr_klasa.SelectedItem.ToString() : "WSZYSTKIE";

            uwagi.Rows.Clear();

            foreach (var uwaga in nauczycielResponse.UwagiIOsiagniecia)
            {
                bool pasujeDoFiltruTypu = (wybranyTyp == "WSZYSTKIE" || ZamienTypNaString_uwaga(uwaga.Typ) == wybranyTyp);
                bool pasujeDoFiltruKlasy = (wybranaKlasa == "WSZYSTKIE" || uwaga.Klasa == wybranaKlasa);

                if (pasujeDoFiltruTypu && pasujeDoFiltruKlasy)
                {
                    int rowIndex = uwagi.Rows.Add();
                    uwagi.Rows[rowIndex].Cells["imie_nazwisko_uwaga"].Value = uwaga.Uczen;
                    uwagi.Rows[rowIndex].Cells["klasa_uwaga"].Value = uwaga.Klasa;
                    uwagi.Rows[rowIndex].Cells["typ_uwaga"].Value = ZamienTypNaString_uwaga(uwaga.Typ);
                    uwagi.Rows[rowIndex].Cells["opis_uwaga"].Value = uwaga.Tresc;

                    var edytujButton = new DataGridViewButtonCell();
                    edytujButton.Value = "Edytuj";
                    uwagi.Rows[rowIndex].Cells["edytuj_uwaga"] = edytujButton;

                    var usunButton = new DataGridViewButtonCell();
                    usunButton.Value = "Usuń";
                    uwagi.Rows[rowIndex].Cells["usun_uwaga"] = usunButton;
                }
            }
        }

        private void uwagi_filtr_klasa_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrujUwagi();
        }

        private void uwaga_filtr_typ_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrujUwagi();
        }

        private void UsunUwage(UwagaOsiagniecie uwaga, int rowIndex)
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{Properties.Resources.adres_api}/usun_uwage?uczen_login={uwaga.UczenLogin}&data={uwaga.Data?.ToString("yyyy-MM-dd")}&tresc={uwaga.Tresc}&typ={uwaga.Typ}&klasa={uwaga.Klasa}&wystawil_login={nauczycielResponse.Login}";
                var response = client.DeleteAsync(requestUri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var resultData = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseContent);

                    if (resultData != null && resultData.ContainsKey("success") && resultData["success"])
                    {
                        MessageBox.Show("Uwaga/osiągnięcie zostało pomyślnie usunięte", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        nauczycielResponse.UwagiIOsiagniecia.RemoveAt(rowIndex);
                        DodajUwagiDoDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił błąd przy usuwaniu uwagi/osiągnięcia", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Wystąpił błąd przy komunikacji z serwerem", "Błąd Komunikacji", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void uwagi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var uwaga = nauczycielResponse.UwagiIOsiagniecia[e.RowIndex];

                if (e.ColumnIndex == uwagi.Columns["usun_uwaga"].Index)
                {
                    string confirmMessage = $"Czy na pewno chcesz usunąć uwagę/osiągnięcie?\nUczeń: {uwaga.Uczen}\nKlasa: {uwaga.Klasa}\nOpis: {uwaga.Tresc}";
                    var result = MessageBox.Show(confirmMessage, "Potwierdzenie usunięcia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        UsunUwage(uwaga, e.RowIndex);
                    }
                }
                else if (e.ColumnIndex == uwagi.Columns["edytuj_uwaga"].Index)
                {
                    var edytujForm = new edytujUwage(uwaga.Uczen, uwaga.Klasa, uwaga.Tresc, uwaga.Data?.ToShortDateString(), uwaga.Typ);
                    if (edytujForm.ShowDialog() == DialogResult.OK)
                    {
                        EdytujUwage(uwaga, edytujForm.NowyOpis, edytujForm.NowyTyp);
                    }
                }
            }
        }

        private void EdytujUwage(UwagaOsiagniecie uwaga, string nowyOpis, int nowyTyp)
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{Properties.Resources.adres_api}/edytuj_uwage?uczen_login={uwaga.UczenLogin}&data={uwaga.Data?.ToString("yyyy-MM-dd")}&tresc={uwaga.Tresc}&typ={uwaga.Typ}&klasa={uwaga.Klasa}&nowa_tresc={nowyOpis}&nowy_typ={nowyTyp}&wystawil_login={nauczycielResponse.Login}";
                var response = client.PutAsync(requestUri, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    var uwagaToUpdate = nauczycielResponse.UwagiIOsiagniecia.FirstOrDefault(u => u.Data == uwaga.Data && u.UczenLogin == uwaga.UczenLogin && u.Tresc == uwaga.Tresc);
                    if (uwagaToUpdate != null)
                    {
                        uwagaToUpdate.Tresc = nowyOpis;
                        uwagaToUpdate.Typ = nowyTyp;
                    }

                    MessageBox.Show("Uwaga/osiągnięcie zostało pomyślnie zedytowane", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DodajUwagiDoDataGridView();
                }
                else
                {
                    MessageBox.Show("Wystąpił błąd przy edycji uwagi/osiągnięcia", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DodajUczniowDoDataGridViewUwagi()
        {
            if (uczniowie_lista == null || nauczycielResponse == null)
            {
                return;
            }

            uczniowie_lista.Rows.Clear();

            foreach (var uczen in nauczycielResponse.Uczniowie)
            {
                int rowIndex = uczniowie_lista.Rows.Add();
                uczniowie_lista.Rows[rowIndex].Cells["imie_nazwisko_uczniowie"].Value = uczen.ImieNazwisko;
                uczniowie_lista.Rows[rowIndex].Cells["klasa_uczniowie"].Value = uczen.Klasa;

                var wystawButton = new DataGridViewButtonCell();
                wystawButton.Value = "Wstaw";
                uczniowie_lista.Rows[rowIndex].Cells["wstaw_uwage_uczniowie"] = wystawButton;
            }
        }

        private async void uczniowie_lista_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == uczniowie_lista.Columns["wstaw_uwage_uczniowie"].Index)
            {
                var uczen = nauczycielResponse.Uczniowie[e.RowIndex];
                var form = new nowaUwaga();

                if (form.ShowDialog() == DialogResult.OK)
                {
                    using (var client = new HttpClient())
                    {
                        var requestUri = $"{Properties.Resources.adres_api}/utworz_uwage";
                        var postData = new Dictionary<string, string>
                        {
                            { "wystawil_login", nauczycielResponse.Login },
                            { "wystawil", nauczycielResponse.ImieNazwisko},
                            { "uczen_login", uczen.Login },
                            { "uczen", uczen.ImieNazwisko },
                            { "tresc", form.Tresc },
                            { "typ", form.Typ.ToString() },
                            { "klasa", uczen.Klasa }
                        };

                        var encodedContent = new FormUrlEncodedContent(postData);
                        var response = await client.PostAsync(requestUri, encodedContent);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            var resultData = JsonConvert.DeserializeObject<Dictionary<string, bool>>(responseContent);

                            if (resultData != null && resultData.ContainsKey("success") && resultData["success"])
                            {
                                MessageBox.Show("Uwaga została pomyślnie utworzona", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                var nowaUwaga = new UwagaOsiagniecie
                                {
                                    Uczen = uczen.ImieNazwisko,
                                    UczenLogin = uczen.Login,
                                    Tresc = form.Tresc,
                                    Typ = form.Typ,
                                    Klasa = uczen.Klasa,
                                    Data = DateTime.Now
                                };

                                nauczycielResponse.UwagiIOsiagniecia.Add(nowaUwaga);

                                DodajUwagiDoDataGridView();
                            }
                            else
                            {
                                MessageBox.Show("Wystąpił błąd przy tworzeniu uwagi", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Wystąpił błąd przy komunikacji z serwerem", "Błąd Komunikacji", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void uczniowie_wyszukiwarka__TextChanged(object sender, EventArgs e)
        {
            uczniowie_lista.CurrentCell = null; // odznaczanie bieżącej komórki, aby uniknąć błędów przy zmianie widoczności

            string searchText = uczniowie_wyszukiwarka.Texts.ToLower();

            // iterowanie przez wszystkie wiersze i ustawianie ich widoczności
            foreach (DataGridViewRow row in uczniowie_lista.Rows)
            {
                if (row.Index == uczniowie_lista.NewRowIndex)
                {
                    continue; //pomijanie "wiersza do dodania"
                }

                var cellValue = row.Cells["imie_nazwisko_uczniowie"].Value;
                bool visible = string.IsNullOrEmpty(searchText) || (cellValue != null && cellValue.ToString().ToLower().Contains(searchText));
                row.Visible = visible;
            }

            uczniowie_lista.Refresh();
        }

        private void uwagi_wyszukwiarka__TextChanged(object sender, EventArgs e)
        {
            uwagi.CurrentCell = null; // odznaczanie bieżącej komórki, aby uniknąć błędów przy zmianie widoczności

            string searchText = uwagi_wyszukwiarka.Texts.ToLower();

            // iterowanie przez wszystkie wiersze i ustawianie ich widoczności
            foreach (DataGridViewRow row in uwagi.Rows)
            {
                if (row.Index == uwagi.NewRowIndex)
                {
                    continue; //pomijanie "wiersza do dodania"
                }

                var cellValue = row.Cells["imie_nazwisko_uwaga"].Value;
                bool visible = string.IsNullOrEmpty(searchText) || (cellValue != null && cellValue.ToString().ToLower().Contains(searchText));
                row.Visible = visible;
            }

            uwagi.Refresh();
        }

        private void ImportujZajeciaDoListView()
        {
            obecnosc_tabela.Items.Clear();

            foreach (var zajecia in nauczycielResponse.Zajecia)
            {
                ListViewItem listViewItem;
                if (zajecia.Klasa == "brak" && zajecia.Przedmiot == "brak")
                {
                    listViewItem = new ListViewItem(new[] { "BRAK", "BRAK", "" });
                }
                else
                {
                    listViewItem = new ListViewItem(new[] { zajecia.Przedmiot, zajecia.Klasa, "     📝" });
                }
                obecnosc_tabela.Items.Add(listViewItem);
            }
        }

        private void obecnosc_tabela_ItemActivate(object sender, EventArgs e)
        {
            if (obecnosc_tabela.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = obecnosc_tabela.SelectedItems[0];
                string przedmiot = selectedItem.SubItems[0].Text;
                string klasa = selectedItem.SubItems[1].Text;

                List<Uczen> uczniowieKlasy = nauczycielResponse.Uczniowie.Where(u => u.Klasa == klasa).ToList();
                SprawdzanieObecnosciForm formObecnosci = new SprawdzanieObecnosciForm(uczniowieKlasy);
                formObecnosci.ShowDialog();
                if (formObecnosci.WynikiObecnosci != null && formObecnosci.WynikiObecnosci.Any())
                {
                    foreach (var wynik in formObecnosci.WynikiObecnosci)
                    {
                        WyslijFrekwencje(przedmiot, wynik.UczenLogin, wynik.Typ, wynik.UczenImieNazwisko, klasa);
                    }
                    obecnosc_tabela.Items.Remove(selectedItem);
                }
            }
        }

        private void WyslijFrekwencje(string przedmiot, string loginUcznia, int typFrekwencji, string ImieNazwisko, string klasa)
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{Properties.Resources.adres_api}/dodaj_frekwencje";

                var values = new Dictionary<string, string>
                {
                    { "przedmiot", przedmiot },
                    { "typ", typFrekwencji.ToString() },
                    { "login", loginUcznia },
                    { "uczen",  ImieNazwisko },
                    { "klasa", klasa }
                };

                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync(requestUri, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Wystąpił błąd przy dodawaniu frekwencji", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void WczytajUczniowDoComboBox()
        {
            filtr_uczniowie_frekwencja.Items.Clear();

            var uczniowieWKlasie = nauczycielResponse.Uczniowie
                .Where(u => u.Klasa == nauczycielResponse.Klasa)
                .ToList();

            foreach (var uczen in uczniowieWKlasie)
            {
                filtr_uczniowie_frekwencja.Items.Add(uczen.ImieNazwisko);
            }

            filtr_uczniowie_frekwencja.SelectedIndexChanged += Filtr_uczniowie_frekwencja_SelectedIndexChanged;
        }

        private void Filtr_uczniowie_frekwencja_SelectedIndexChanged(object sender, EventArgs e)
        {
            var wybranyUczenImieNazwisko = filtr_uczniowie_frekwencja.SelectedItem.ToString();
            var wybranyUczen = nauczycielResponse.Uczniowie.FirstOrDefault(u => u.ImieNazwisko == wybranyUczenImieNazwisko);

            if (wybranyUczen != null)
            {
                var frekwencja = PobierzFrekwencje(wybranyUczen.Login);
                WypelnijDataGridView(frekwencja, wybranyUczen.ImieNazwisko);

                // Odblokowanie pozostałych ComboBoxów i resetowanie filtrów
                filtr_przedmiot_frekwencja.Enabled = true;
                filtr_typ_frekwencja.Enabled = true;
                filtr_przedmiot_frekwencja.SelectedIndex = filtr_przedmiot_frekwencja.Items.Count - 1;
                filtr_typ_frekwencja.SelectedIndex = filtr_typ_frekwencja.Items.Count - 1;
            }
        }

        private List<Frekwencja> PobierzFrekwencje(string login)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Properties.Resources.adres_api}/pobierz_frekwencje?login={login}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var responseData = JsonConvert.DeserializeObject<Dictionary<string, List<Frekwencja>>>(responseString);
                    return responseData["frekwencja"];
                }
                else
                {
                    MessageBox.Show("Wystąpił błąd przy pobieraniu frekwencji", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new List<Frekwencja>();
                }
            }
        }

        private void WypelnijDataGridView(List<Frekwencja> frekwencja, string imieNazwisko)
        {
            frekwencja_tabela.Rows.Clear();

            foreach (var f in frekwencja)
            {
                DodajWierszDoDataGridView(f, imieNazwisko);
            }
        }

        private void DodajWierszDoDataGridView(Frekwencja f, string imieNazwisko)
        {
            string typTekst = ZamienTypNaStringF(f.Typ);

            int rowIndex = frekwencja_tabela.Rows.Add();
            frekwencja_tabela.Rows[rowIndex].Cells["imie_nazwisko_frekwencja"].Value = imieNazwisko;
            frekwencja_tabela.Rows[rowIndex].Cells["przedmiot_frekwencja"].Value = f.Przedmiot;
            frekwencja_tabela.Rows[rowIndex].Cells["typ_frekwencja"].Value = typTekst;
            frekwencja_tabela.Rows[rowIndex].Cells["data_frekwencja"].Value = f.Data;

            var btnUsun = new DataGridViewButtonCell();
            btnUsun.Value = "Usuń";
            frekwencja_tabela.Rows[rowIndex].Cells["usun_frekwencja"] = btnUsun;

            var btnEdytuj = new DataGridViewButtonCell();
            btnEdytuj.Value = "Edytuj";
            frekwencja_tabela.Rows[rowIndex].Cells["edytuj_frekwencja"] = btnEdytuj;
        }

        private void frekwencja_tabela_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = frekwencja_tabela.Rows[e.RowIndex];

                string imieNazwisko = selectedRow.Cells["imie_nazwisko_frekwencja"].Value.ToString();
                string przedmiot = selectedRow.Cells["przedmiot_frekwencja"].Value.ToString();
                string typ = selectedRow.Cells["typ_frekwencja"].Value.ToString();
                string data = selectedRow.Cells["data_frekwencja"].Value.ToString();

                string wybranyUczenImieNazwisko = filtr_uczniowie_frekwencja.SelectedItem?.ToString();
                var wybranyUczen = nauczycielResponse.Uczniowie.FirstOrDefault(u => u.ImieNazwisko == wybranyUczenImieNazwisko);

                if (e.ColumnIndex == frekwencja_tabela.Columns["usun_frekwencja"].Index)
                {
                    if(UsunFrekwencje(przedmiot, typ, wybranyUczen.Login, wybranyUczenImieNazwisko, nauczycielResponse.Klasa, data))
                    {
                        frekwencja_tabela.Rows.RemoveAt(e.RowIndex);
                    }
                }
                else if (e.ColumnIndex == frekwencja_tabela.Columns["edytuj_frekwencja"].Index)
                {
                    var edytujFrekwencjeForm = new edytujFrekwencje();
                    if (edytujFrekwencjeForm.ShowDialog() == DialogResult.OK)
                    {
                        string nowyTyp = edytujFrekwencjeForm.WybranyTyp;
                        if (nowyTyp == "obecność")
                        {
                            if (UsunFrekwencje(przedmiot, typ, wybranyUczen.Login, wybranyUczenImieNazwisko, nauczycielResponse.Klasa, data))
                            {
                                frekwencja_tabela.Rows.RemoveAt(e.RowIndex);
                            }
                        }
                        else
                        {
                            using (var client = new HttpClient())
                            {
                                var requestUri = $"{Properties.Resources.adres_api}/edytuj_frekwencje";
                                var values = new Dictionary<string, string>
                                {
                                    { "przedmiot", przedmiot },
                                    { "typ", Convert.ToString(ZamienStringNaTypF(typ)) },
                                    { "nowy_typ", Convert.ToString(ZamienStringNaTypF(nowyTyp)) },
                                    { "login", wybranyUczen.Login },
                                    { "uczen", wybranyUczenImieNazwisko },
                                    { "klasa", nauczycielResponse.Klasa },
                                    { "data", data }
                                };

                                var content = new FormUrlEncodedContent(values);
                                var response = client.PutAsync(requestUri, content).Result;

                                if (response.IsSuccessStatusCode)
                                {
                                    MessageBox.Show("Frekwencja została zaktualizowana.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    var zaktualizowana_frekwencja = PobierzFrekwencje(wybranyUczen.Login);
                                    WypelnijDataGridView(zaktualizowana_frekwencja, wybranyUczenImieNazwisko);
                                }
                                else
                                {
                                    MessageBox.Show("Nie udało się zaktualizować frekwencji.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool UsunFrekwencje(string przedmiot, string typ, string uczen_login, string uczen_in, string klasa, string data)
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{Properties.Resources.adres_api}/usun_frekwencje?przedmiot={przedmiot}&typ={ZamienStringNaTypF(typ)}&login={uczen_login}&data={data}&uczen={uczen_in}&klasa={klasa}";
                var response = client.DeleteAsync(requestUri).Result;

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Frekwencja została usunięta.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Nie udało się usunąć frekwencji.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private void FiltrujFrekwencje()
        {
            string wybranyPrzedmiot = filtr_przedmiot_frekwencja.SelectedItem?.ToString() ?? "WSZYSTKIE";
            string wybranyTyp = filtr_typ_frekwencja.SelectedItem?.ToString() ?? "WSZYSTKIE";
            string wybranyUczenImieNazwisko = filtr_uczniowie_frekwencja.SelectedItem?.ToString();

            frekwencja_tabela.Rows.Clear();

            if (wybranyUczenImieNazwisko != null)
            {
                var wybranyUczen = nauczycielResponse.Uczniowie.FirstOrDefault(u => u.ImieNazwisko == wybranyUczenImieNazwisko);

                if (wybranyUczen != null)
                {
                    var frekwencja = PobierzFrekwencje(wybranyUczen.Login);

                    foreach (var f in frekwencja)
                    {
                        bool pasujeDoPrzedmiotu = (wybranyPrzedmiot == "WSZYSTKIE" || f.Przedmiot == wybranyPrzedmiot);
                        bool pasujeDoTypu = (wybranyTyp == "WSZYSTKIE" || ZamienTypNaStringF(f.Typ) == wybranyTyp);

                        if (pasujeDoPrzedmiotu && pasujeDoTypu)
                        {
                            DodajWierszDoDataGridView(f, wybranyUczenImieNazwisko);
                        }
                    }
                }
            }
        }

        private void filtr_przedmiot_frekwencja_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrujFrekwencje();
        }

        private void filtr_typ_frekwencja_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrujFrekwencje();
        }

        private string ZamienTypNaStringF(int typ)
        {
            switch (typ)
            {
                case 1:
                    return "nieobecność";
                case 2:
                    return "nieobecność usprawiedliwiona";
                case 3:
                    return "spóźnienie";
                default:
                    return "nieznany";
            }
        }

        private int ZamienStringNaTypF(string typTekst)
        {
            switch (typTekst)
            {
                case "nieobecność":
                    return 1;
                case "nieobecność usprawiedliwiona":
                    return 2;
                case "spóźnienie":
                    return 3;
                default:
                    return -1;
            }
        }

        private void wyloguj_Click(object sender, EventArgs e)
        {
            this.Hide();
            loginForm.Show();
        }
    }
}