using GradeSync.klasy;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json;

namespace GradeSync
{
    public partial class uczen : MaterialForm
    {
        private UserResponse userResponse;
        private Dictionary<string, List<Ocena>> grupowaneOceny;

        public uczen(UserResponse response)
        {
            InitializeComponent();
            userResponse = response;
            GrupujOceny();

            WypelnijTabeleOcen();

            sem1.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            sem2.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

            f_sem1.CheckedChanged += new EventHandler(RadioButtonFrekwencja_CheckedChanged);
            f_sem2.CheckedChanged += new EventHandler(RadioButtonFrekwencja_CheckedChanged);
        }

        private void GrupujOceny()
        {
            grupowaneOceny = userResponse.Oceny
                .GroupBy(o => o.Przedmiot)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        private void WypelnijTabeleOcen()
        {
            foreach (var przedmiot in grupowaneOceny.Keys)
            {
                var ocenyPrzedmiotu = grupowaneOceny[przedmiot];
                var ocenyString = string.Join(", ", ocenyPrzedmiotu.Select(o => FormatujOcene(o.Oceny)));
                var srednia = ocenyPrzedmiotu.Average(o => double.TryParse(o.Oceny.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double num) ? num : 0);

                var item = new ListViewItem(new[] { przedmiot, ocenyString, srednia.ToString("0.00", CultureInfo.InvariantCulture) });
                oceny_tabela.Items.Add(item);
            }

            //dodanie wiersza zachowanie
            var itemZachowanie = new ListViewItem(new[] { "Zachowanie", FormatujOcene(userResponse.OcenaZZachowania), userResponse.OcenaZZachowania });
            oceny_tabela.Items.Add(itemZachowanie);
        }

        private int przesuniecieTygodnia = 0;

        private void UstawDatyTygodnia()
        {
            DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday + przesuniecieTygodnia * 7);
            var labels = new[] { dat_pon, dat_wtr, dat_srd, dat_czw, dat_ptk };

            for (int i = 0; i < labels.Length; i++)
            {
                DateTime day = startOfWeek.AddDays(i);
                labels[i].Text = day.ToString("dd.MM.yyyy");

                if (CzyWakacje(day))
                {
                    UstawWakacjeDlaKolumny(i);
                }
                else
                {
                    ResetujLabeleWKolumnie(i);
                    UstawPlanLekcjiDlaKolumny(i, day);
                }
            }
        }


        private bool CzyWakacje(DateTime date)
        {
            return (date.Month == 6 && date.Day >= 21) || (date.Month == 7 || date.Month == 8) || (date.Month == 9 && date.Day < 1);
        }

        private void UstawWakacjeDlaKolumny(int columnIndex)
        {
            for (int row = 0; row < planlekcji.RowCount; row++)
            {
                var control = planlekcji.GetControlFromPosition(columnIndex, row);
                if (control is Panel panel)
                {
                    foreach (Control subControl in panel.Controls)
                    {
                        if (subControl is Label label)
                        {
                            label.Text = "WAKACJE";
                        }
                    }
                }
            }
        }

        private void ResetujLabeleWKolumnie(int columnIndex)
        {
            for (int row = 0; row < planlekcji.RowCount; row++)
            {
                var control = planlekcji.GetControlFromPosition(columnIndex, row);
                if (control is Panel panel)
                {
                    Label[] labels = panel.Controls.OfType<Label>().ToArray();
                    if (labels.Length >= 3)
                    {
                        labels[0].Text = "";
                        labels[1].Text = "";
                        labels[2].Text = "";
                    }
                }
            }
        }



        private void UstawPlanLekcjiDlaKolumny(int columnIndex, DateTime date)
        {
            PlanZajec plan = UstalPlanLekcji(date);
            string dzienTygodnia = DayOfWeekToString(columnIndex);

            if (plan != null && plan.GetType().GetProperty(dzienTygodnia) != null)
            {
                var lekcjeDnia = (Dictionary<string, Przedmiot>)plan.GetType().GetProperty(dzienTygodnia).GetValue(plan, null);

                foreach (var lekcja in lekcjeDnia)
                {
                    int rowIndex = int.Parse(lekcja.Key.Replace("lek", ""));
                    var przedmiot = lekcja.Value;

                    var panel = planlekcji.GetControlFromPosition(columnIndex, rowIndex + 1) as Panel;
                    if (panel != null)
                    {
                        Label labelPrzedmiot = panel.Controls[0] as Label;
                        Label labelSala = panel.Controls[1] as Label;
                        Label labelProwadzacy = panel.Controls[2] as Label;

                        //zastosowanie skróconych nazw dla określonych przedmiotów (aby się nie ucinały w tablelayout)
                        string nazwaPrzedmiotu = przedmiot.NazwaPrzedmiotu;
                        if (nazwaPrzedmiotu == "Edukacja do bezpieczeństwa")
                            nazwaPrzedmiotu = "EDB";
                        else if (nazwaPrzedmiotu == "Podstawy przedsiębiorczości")
                            nazwaPrzedmiotu = "P. Przedsiębiorczości";

                        labelPrzedmiot.Text = nazwaPrzedmiotu;
                        labelSala.Text = przedmiot.Sala;
                        labelProwadzacy.Text = przedmiot.Prowadzacy;
                    }
                }
            }
        }


        private PlanZajec UstalPlanLekcji(DateTime date)
        {
            if (date >= new DateTime(date.Year, 9, 1) && date <= new DateTime(date.Year, 12, 15))
            {
                return userResponse.Plan1;
            }
            else if (date >= new DateTime(date.Year, 12, 16) || date <= new DateTime(date.Year, 6, 21))
            {
                return userResponse.Plan2;
            }
            return null;
        }

        private string DayOfWeekToString(int columnIndex)
        {
            switch (columnIndex)
            {
                case 0:
                    return "Poniedzialek";
                case 1:
                    return "Wtorek";
                case 2:
                    return "Sroda";
                case 3:
                    return "Czwartek";
                case 4:
                    return "Piatek";
                default:
                    return "";
            }
        }

        private static string FormatujOcene(string ocena)
        {
            if (double.TryParse(ocena.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
            {
                //sprawdzamy, czy część dziesiętna wynosi 0.8 (lub bliskoaby uwzględnić błędy zaokrąglania)
                if (Math.Abs(num % 1 - 0.8) < 0.01)
                {
                    return $"{Math.Floor(num) + 1}-";
                }
                //sprawdzamy, czy część dziesiętna wynosi 0.5 (lub blisko aby uwzględnić błędy zaokrąglania)
                else if (Math.Abs(num % 1 - 0.5) < 0.01)
                {
                    return $"{Math.Floor(num)}+";
                }
                //jeśli część dziesiętna wynosi 0 wyświetlamy tylko część całkowitą
                else if (Math.Abs(num % 1) < 0.01)
                {
                    return $"{Math.Floor(num)}";
                }
                return num.ToString("0.0", CultureInfo.InvariantCulture);
            }
            return ocena; //jeśli nie uda się przekonwertować, zwróć oryginalny string
        }

        private void UtworzPaneleWPlanieLekcji()
        {
            for (int row = 0; row < planlekcji.RowCount; row++)
            {
                for (int col = 0; col < planlekcji.ColumnCount; col++)
                {
                    Panel panel = new Panel();
                    panel.Dock = DockStyle.Fill;
                    panel.BackColor = Color.LightGray;

                    Label labelPrzedmiot = new Label();
                    labelPrzedmiot.Text = "";
                    labelPrzedmiot.Dock = DockStyle.Top;
                    labelPrzedmiot.TextAlign = ContentAlignment.MiddleCenter;
                    labelPrzedmiot.Height = 18;

                    Label labelSala = new Label();
                    labelSala.Text = "";
                    labelSala.Dock = DockStyle.Fill;
                    labelSala.TextAlign = ContentAlignment.MiddleCenter;
                    labelSala.Height = 18;

                    Label labelProwadzacy = new Label();
                    labelProwadzacy.Text = "";
                    labelProwadzacy.Dock = DockStyle.Bottom;
                    labelProwadzacy.TextAlign = ContentAlignment.MiddleCenter;
                    labelProwadzacy.Height = 18;

                    panel.Controls.Add(labelSala);
                    panel.Controls.Add(labelPrzedmiot);
                    panel.Controls.Add(labelProwadzacy);

                    planlekcji.Controls.Add(panel, col, row);
                }
            }
        }

        private void uczen_Load(object sender, EventArgs e)
        {
            dane_ucznia.Text = $"{userResponse.ImieNazwisko} {userResponse.Klasa}";

            UtworzPaneleWPlanieLekcji();
            UstawDatyTygodnia();
            UstalISprawdzSemestr();

            WyswietlUwagiOsiagniecia(userResponse.Uwagi.Where(u => u.Typ == 1).ToList(), uwagi_panel);
            WyswietlUwagiOsiagniecia(userResponse.Uwagi.Where(u => u.Typ == 2).ToList(), osiagniecia_panel);

            tabcontrol_wydarzenia.SelectedIndex = 0;
            WyswietlWydarzenia(userResponse.Wydarzenia, panel_spr, panel_kart, panel_zad, panel_prj, panel_inne);

            przedmioty.SelectedIndex = 0;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sem1.Checked)
            {
                FiltrujOcenyPoSemestrze(1);
            }
            else if (sem2.Checked)
            {
                FiltrujOcenyPoSemestrze(2);
            }
        }

        private void UstalISprawdzSemestr()
        {
            DateTime dzisiaj = DateTime.Today;
            if (dzisiaj >= new DateTime(dzisiaj.Year, 9, 1) && dzisiaj <= new DateTime(dzisiaj.Year, 12, 15))
            {
                sem1.Checked = true;
                f_sem1.Checked = true;
                FiltrujOcenyPoSemestrze(1);
            }
            else if ((dzisiaj >= new DateTime(dzisiaj.Year, 12, 16) && dzisiaj.Year == dzisiaj.Year) ||
                     (dzisiaj <= new DateTime(dzisiaj.Year, 6, 21) && dzisiaj.Year == dzisiaj.Year))
            {
                sem2.Checked = true;
                f_sem2.Checked = true;
                FiltrujOcenyPoSemestrze(2);
            }
        }

        private void FiltrujOcenyPoSemestrze(int semestr)
        {
            oceny_tabela.Items.Clear();

            foreach (var przedmiot in grupowaneOceny.Keys)
            {
                var ocenyPrzedmiotu = grupowaneOceny[przedmiot]
                    .Where(o => CzyOcenaWPrawidlowymSemestrze(o.DataWystawienia, semestr))
                    .ToList();

                if (ocenyPrzedmiotu.Count > 0)
                {
                    var ocenyString = string.Join(", ", ocenyPrzedmiotu.Select(o => FormatujOcene(o.Oceny)));
                    var srednia = ocenyPrzedmiotu
                        .Select(o => double.TryParse(o.Oceny.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double num) ? num : 0)
                        .Average();

                    var item = new ListViewItem(new[] { przedmiot, ocenyString, srednia.ToString("0.00", CultureInfo.InvariantCulture) });
                    oceny_tabela.Items.Add(item);
                }
            }
        }

        private bool CzyOcenaWPrawidlowymSemestrze(string dataOceny, int semestr)
        {
            DateTime data = DateTime.ParseExact(dataOceny, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
            if (semestr == 1)
            {
                return data >= new DateTime(data.Year, 9, 1) && data <= new DateTime(data.Year, 12, 15);
            }
            else if (semestr == 2)
            {
                return (data >= new DateTime(data.Year, 12, 16) && data.Year == data.Year) ||
                       (data <= new DateTime(data.Year, 6, 21) && data.Year == data.Year);
            }
            return false;
        }

        private void oceny_tabela_ItemActivate(object sender, EventArgs e)
        {
            if (oceny_tabela.SelectedItems.Count > 0)
            {
                var selectedItem = oceny_tabela.SelectedItems[0];
                var przedmiot = selectedItem.SubItems[0].Text;

                //wykluczamy wiersz z zachowaniem z pokazywania szczegółów
                if (przedmiot == "Zachowanie")
                {
                    return;
                }

                var ocenySzczegoly = grupowaneOceny[przedmiot]
                    .Where(o => CzyOcenaWNalezyDoSemestru(o.DataWystawienia))
                    .Select(o => $"Ocena: {FormatujOcene(o.Oceny)}\nWystawił: {o.Wystawil}\nData: {o.DataWystawienia}\nOpis: {o.Opis}")
                    .ToList();

                var message = string.Join("\n\n", ocenySzczegoly);
                MessageBox.Show(message, "Szczegóły ocen", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CzyOcenaWNalezyDoSemestru(string dataWystawienia)
        {
            DateTime dataOceny = DateTime.ParseExact(dataWystawienia, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
            if (sem1.Checked)
            {
                return dataOceny >= new DateTime(dataOceny.Year, 9, 1) && dataOceny <= new DateTime(dataOceny.Year, 12, 15);
            }
            else if (sem2.Checked)
            {
                return (dataOceny >= new DateTime(dataOceny.Year, 12, 16) || dataOceny <= new DateTime(dataOceny.Year, 6, 21));
            }
            return false;
        }

        private void RadioButtonFrekwencja_CheckedChanged(object sender, EventArgs e)
        {
            if (f_sem1.Checked)
            {
                FiltrujFrekwencjePoSemestrze(1);
            }
            else if (f_sem2.Checked)
            {
                FiltrujFrekwencjePoSemestrze(2);
            }
        }

        private void FiltrujFrekwencjePoSemestrze(int semestr)
        {
            frekwencja_tabela.Items.Clear();

            DateTime poczatekSemestru = semestr == 1 ? new DateTime(DateTime.Today.Year, 9, 1) : new DateTime(DateTime.Today.Year, 1, 1);
            DateTime koniecSemestru = semestr == 1 ? new DateTime(DateTime.Today.Year, 12, 15) : new DateTime(DateTime.Today.Year, 6, 21);

            //obliczanie liczby lekcji przedmiotu na semestr
            var liczbaLekcjiPrzedmiotu = ObliczLiczbeLekcjiPrzedmiotuNaSemestr(semestr == 1 ? userResponse.Plan1 : userResponse.Plan2, poczatekSemestru, koniecSemestru);

            //agregacja frekwencji
            var podsumowanieFrekwencji = new Dictionary<string, (int Nieobecnosci, int Usprawiedliwione, int Spoznienia)>();
            foreach (var wpis in userResponse.Frekwencja)
            {
                if (!podsumowanieFrekwencji.ContainsKey(wpis.Przedmiot))
                {
                    podsumowanieFrekwencji[wpis.Przedmiot] = (0, 0, 0);
                }

                if (!CzyFrekwencjaWPrawidlowymSemestrze(wpis.Data, semestr))
                {
                    continue;
                }

                var (nieobecnosci, usprawiedliwione, spoznienia) = podsumowanieFrekwencji[wpis.Przedmiot];

                switch (wpis.Typ)
                {
                    case 1:
                        nieobecnosci++;
                        break;
                    case 2:
                        usprawiedliwione++;
                        break;
                    case 3:
                        spoznienia++;
                        break;
                }

                podsumowanieFrekwencji[wpis.Przedmiot] = (nieobecnosci, usprawiedliwione, spoznienia);
            }

            //dodawanie danych do l;istView
            foreach (var kvp in podsumowanieFrekwencji)
            {
                var (nieobecnosci, usprawiedliwione, spoznienia) = kvp.Value;
                double procentFrekwencji = liczbaLekcjiPrzedmiotu.ContainsKey(kvp.Key) && liczbaLekcjiPrzedmiotu[kvp.Key] > 0
                                            ? (1 - (double)nieobecnosci / liczbaLekcjiPrzedmiotu[kvp.Key]) * 100
                                            : 0;

                var item = new ListViewItem(new[]
                {
            kvp.Key, //przedmiot
            nieobecnosci.ToString(),
            usprawiedliwione.ToString(),
            spoznienia.ToString(),
            $"{procentFrekwencji:0.00}%"
        });

                frekwencja_tabela.Items.Add(item);
            }
        }

        //pomocnicza metoda do obliczania liczby lekcji na semestr dla każdego przedmiotu
        private Dictionary<string, int> ObliczLiczbeLekcjiPrzedmiotuNaSemestr(PlanZajec planZajec, DateTime poczatekSemestru, DateTime koniecSemestru)
        {
            var liczbaLekcjiPrzedmiotu = new Dictionary<string, int>();

            //obliczanie liczby dni nauki w semestrze (z pomienieciem weekendow)
            int liczbaDniNauki = 0;
            for (DateTime dt = poczatekSemestru; dt <= koniecSemestru; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
                {
                    liczbaDniNauki++;
                }
            }

            //obliczanie liczby lekcji dla każdego przedmiotu
            var dniTygodnia = new Dictionary<string, Dictionary<string, Przedmiot>> {
        { "Poniedzialek", planZajec.Poniedzialek },
        { "Wtorek", planZajec.Wtorek },
        { "Sroda", planZajec.Sroda },
        { "Czwartek", planZajec.Czwartek },
        { "Piatek", planZajec.Piatek }
    };

            foreach (var dzien in dniTygodnia)
            {
                foreach (var lekcja in dzien.Value.Values)
                {
                    if (lekcja != null)
                    {
                        if (!liczbaLekcjiPrzedmiotu.ContainsKey(lekcja.NazwaPrzedmiotu))
                        {
                            liczbaLekcjiPrzedmiotu[lekcja.NazwaPrzedmiotu] = 0;
                        }
                        liczbaLekcjiPrzedmiotu[lekcja.NazwaPrzedmiotu]++;
                    }
                }
            }

            //skalowanie liczby lekcji przez liczbę dni nauki podzieloną przez 5 (dni nauki w tygodniu)
            foreach (var przedmiot in liczbaLekcjiPrzedmiotu.Keys.ToList())
            {
                liczbaLekcjiPrzedmiotu[przedmiot] *= liczbaDniNauki / 5;
            }

            return liczbaLekcjiPrzedmiotu;
        }

        private bool CzyFrekwencjaWPrawidlowymSemestrze(DateTime dataFrekwencji, int semestr)
        {
            if (semestr == 1)
            {
                return dataFrekwencji >= new DateTime(dataFrekwencji.Year, 9, 1) && dataFrekwencji <= new DateTime(dataFrekwencji.Year, 12, 15);
            }
            else if (semestr == 2)
            {
                return (dataFrekwencji >= new DateTime(dataFrekwencji.Year, 12, 16) || dataFrekwencji <= new DateTime(dataFrekwencji.Year, 6, 21));
            }
            return false;
        }

        private void WyswietlUwagiOsiagniecia(List<Uwaga> uwagi, TableLayoutPanel panel)
        {
            panel.Controls.Clear();
            panel.RowStyles.Clear();

            foreach (var uwaga in uwagi)
            {
                var labelData = new Label
                {
                    Text = uwaga.Data.ToString("dd.MM.yyyy"),
                    ForeColor = Color.Blue,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point),
                    Dock = DockStyle.Top,
                    AutoSize = true
                };

                var pustaLinia = new Label
                {
                    Text = "",
                    AutoSize = true
                };

                var labelWystawil = new Label
                {
                    Text = "Wystawił: " + uwaga.Wystawil,
                    Dock = DockStyle.Top,
                    AutoSize = true
                };

                var labelTresc = new Label
                {
                    Text = "Treść: " + uwaga.Tresc,
                    Dock = DockStyle.Top,
                    AutoSize = true
                };

                panel.Controls.Add(labelData);
                panel.Controls.Add(pustaLinia);
                panel.Controls.Add(labelWystawil); 
                panel.Controls.Add(labelTresc);

                var separator = new Label
                {
                    Height = 3,
                    BorderStyle = BorderStyle.Fixed3D,
                    Dock = DockStyle.Top
                };
                panel.Controls.Add(separator);

                var pustaLinia2 = new Label
                {
                    Text = "",
                    AutoSize = true
                };

                panel.Controls.Add(pustaLinia2);
                panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
        }

        private string PobierzDaneUczniaNaTleKlasy(string klasa, string przedmiot)
        {
            var httpClient = new HttpClient();
            var requestUrl = $"{Properties.Resources.adres_api}/uczen_na_tle_klasy?klasa={klasa}&przedmiot={przedmiot}&login={userResponse.Login}";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var responseTask = httpClient.SendAsync(request);
            responseTask.Wait();

            var response = responseTask.Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result; //.Result czeka na zakonczenie operacji
                return responseContent;
            }

            return null;
        }



        private void cofnij_Click(object sender, EventArgs e)
        {
            //pzesunięcie do nastenego tygodnia
            przesuniecieTygodnia--;
            UstawDatyTygodnia();
        }

        private void naprzod_Click(object sender, EventArgs e)
        {
            //pzesunięcie do poprzedniego tygodnia
            przesuniecieTygodnia++;
            UstawDatyTygodnia();
        }

        private void ZaladujDaneDoWykresu(Chart wykres, List<string> dane, string tytul)
        {
            //clearowanie wykresu 
            wykres.Series.Clear();
            wykres.Titles.Clear();
            wykres.Palette = ChartColorPalette.BrightPastel;

           
           wykres.Titles.Add(tytul);
            wykres.Titles[0].Font = new Font("Segoe UI", 16F, FontStyle.Bold);

            //grupowanie i sumowanie ocen
            var grupowaneOceny = dane
                .GroupBy(ocena => FormatujOceneDoLegnedy(ocena))
                .Select(group => new
                {
                    Etykieta = group.Key,
                    Liczba = group.Count()
                })
                .ToList();

            //dodanie nowej serii danych
            Series series = new Series
            {
                Name = "oceny",
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Pie
            };
            wykres.Series.Add(series);

            //dodawanie zgrupowanych danych do serii
            foreach (var grupa in grupowaneOceny)
            {
                series.Points.Add(new DataPoint(0, grupa.Liczba) { AxisLabel = grupa.Etykieta });
            }

            //formatowanie wykresu kolowego
            wykres.Series["oceny"]["PieLabelStyle"] = "Disabled";
            wykres.Invalidate();
        }


        private string FormatujOceneDoLegnedy(string ocena)
        {
            if (double.TryParse(ocena, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
            {
                if (Math.Abs(num % 1 - 0.8) < 0.01)
                {
                    return $"{Math.Floor(num) + 1}-";
                }else if (Math.Abs(num % 1 - 0.5) < 0.01)
                {
                    return $"{Math.Floor(num)}+";
                }
            }

            return $"{Math.Floor(num)}";
        }

        private bool CzyNalezyDoSemestru(string data, int semestr)
        {
            var dataOceny = DateTime.Parse(data);
            if (semestr == 1)
                return dataOceny >= new DateTime(dataOceny.Year, 9, 1) && dataOceny <= new DateTime(dataOceny.Year, 12, 15);
            else
                return dataOceny >= new DateTime(dataOceny.Year, 12, 16) || dataOceny <= new DateTime(dataOceny.Year, 6, 21);
        }

        private void przedmioty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (przedmioty.SelectedItem != null)
            {
                string wybranyPrzedmiot = przedmioty.SelectedItem.ToString();
                string klasa = userResponse.Klasa;

                string response = PobierzDaneUczniaNaTleKlasy(klasa, wybranyPrzedmiot);

                if (response != null)
                {
                    //deserializacja odpowiedzi z api
                    List<List<string>> ocenyZDatami = JsonConvert.DeserializeObject<List<List<string>>>(response);

                    //grupowanie ocen na semestry
                    var ocenySemestr1 = ocenyZDatami.Where(o => CzyNalezyDoSemestru(o[1], 1)).Select(o => o[0]).ToList();
                    var ocenySemestr2 = ocenyZDatami.Where(o => CzyNalezyDoSemestru(o[1], 2)).Select(o => o[0]).ToList();

                    ZaladujDaneDoWykresu(wykres_oceny_sem1, ocenySemestr1, "Oceny klasy - Semestr 1");
                    ZaladujDaneDoWykresu(wykres_oceny_sem2, ocenySemestr2, "Oceny klasy - Semestr 2");

                    //grupowanie ocen ucznia na semestry
                    var ocenyUczniaSem1 = userResponse.Oceny
                        .Where(o => o.Przedmiot.Equals(wybranyPrzedmiot, StringComparison.OrdinalIgnoreCase) && CzyNalezyDoSemestru(o.DataWystawienia, 1))
                        .Select(o => o.Oceny).ToList();
                    var ocenyUczniaSem2 = userResponse.Oceny
                        .Where(o => o.Przedmiot.Equals(wybranyPrzedmiot, StringComparison.OrdinalIgnoreCase) && CzyNalezyDoSemestru(o.DataWystawienia, 2))
                        .Select(o => o.Oceny).ToList();

                    ZaladujDaneDoWykresu(wykres_oceny_ucznia_sem1, ocenyUczniaSem1, "Twoje oceny - Semestr 1");
                    ZaladujDaneDoWykresu(wykres_oceny_ucznia_sem2, ocenyUczniaSem2, "Twoje oceny - Semestr 2");
                }
            }
        }

        private void WyswietlWydarzenia(List<Wydarzenie> wydarzenia, TableLayoutPanel panel_spr, TableLayoutPanel panel_kart, TableLayoutPanel panel_zad, TableLayoutPanel panel_prj, TableLayoutPanel panel_inne)
        {
            //grupowanie wydarzeń według typu i sortowanie ich według daty terminu
            var grupowaneWydarzenia = wydarzenia.GroupBy(w => w.Typ).ToDictionary(g => g.Key, g => g.OrderBy(w => w.Termin).ToList());

            panel_spr.Controls.Clear();
            panel_kart.Controls.Clear();
            panel_zad.Controls.Clear();
            panel_prj.Controls.Clear();
            panel_inne.Controls.Clear();

            panel_spr.RowStyles.Clear();
            panel_kart.RowStyles.Clear();
            panel_zad.RowStyles.Clear();
            panel_prj.RowStyles.Clear();
            panel_inne.RowStyles.Clear();

            //metoda do wyświetlania wydarzen w odpowiednich panelach
            void DodajDoPanelu(List<Wydarzenie> listaWydarzen, TableLayoutPanel panel, bool typInny = false)
            {
                foreach (var wydarzenie in listaWydarzen)
                {
                    //tworzenie i konfiguracja labeli
                    var labelTermin = new Label
                    {
                        Text = "Termin: " + wydarzenie.Termin.ToString("dd.MM.yyyy"),
                        ForeColor = Color.Blue,
                        Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point),
                        AutoSize = true
                    };

                    panel.Controls.Add(labelTermin);

                    var pustaLinia = new Label
                    {
                        Text = "",
                        AutoSize = true
                    };

                    panel.Controls.Add(pustaLinia);

                    if (!typInny)
                    {
                        var labelTyp = new Label
                        {
                            Text = "Typ: " + ZamienTypNaString(wydarzenie.Typ),
                            AutoSize = true
                        };

                        panel.Controls.Add(labelTyp);

                        var labelPrzedmiot = new Label
                        {
                            Text = "Przedmiot: " + wydarzenie.Przedmiot,
                            AutoSize = true
                        };

                        panel.Controls.Add(labelPrzedmiot);
                    }

                    var labelDataWystawienia = new Label
                    {
                        Text = "Data wystawienia: " + wydarzenie.Data.ToString("dd.MM.yyyy"),
                        AutoSize = true
                    };

                    panel.Controls.Add(labelDataWystawienia);

                    var labelWystawil = new Label
                    {
                        Text = "Wystawił: " + wydarzenie.Wystawil,
                        AutoSize = true
                    };

                    panel.Controls.Add(labelWystawil);

                    var labelOpis = new Label
                    {
                        Text = "Opis: " + wydarzenie.Opis,
                        AutoSize = true
                    };

                    panel.Controls.Add(labelOpis);

                    var separator = new Label
                    {
                        Height = 3,
                        BorderStyle = BorderStyle.Fixed3D,
                        AutoSize = false,
                        Width = panel.Width,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                    };
                    panel.Controls.Add(separator);

                    //pusta linia po labelu terminu
                    var pustaLinia2 = new Label
                    {
                        Text = "",
                        AutoSize = true
                    };

                    panel.Controls.Add(pustaLinia2);
                    panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                }
            }

            //wyświetlanie wydarzeń w odpowiednich panelach
            if (grupowaneWydarzenia.ContainsKey(1)) DodajDoPanelu(grupowaneWydarzenia[1], panel_spr);
            if (grupowaneWydarzenia.ContainsKey(2)) DodajDoPanelu(grupowaneWydarzenia[2], panel_kart);
            if (grupowaneWydarzenia.ContainsKey(3)) DodajDoPanelu(grupowaneWydarzenia[3], panel_zad);
            if (grupowaneWydarzenia.ContainsKey(4)) DodajDoPanelu(grupowaneWydarzenia[4], panel_prj);
            if (grupowaneWydarzenia.ContainsKey(5)) DodajDoPanelu(grupowaneWydarzenia[5], panel_inne, true);
        }

        //metoda do zamiany typu wydarzenia na stringa
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
    }
}
