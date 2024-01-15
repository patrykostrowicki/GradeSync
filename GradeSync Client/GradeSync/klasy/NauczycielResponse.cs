using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GradeSync.klasy
{
    public class Inne
    {
        [JsonProperty("przedmioty")]
        public List<string> Przedmioty { get; set; }
    }

    public class Uczen
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("imie_nazwisko")]
        public string ImieNazwisko { get; set; }

        [JsonProperty("klasa")]
        public string Klasa { get; set; }
    }

    public class Wydarzenie_n
    {
        [JsonProperty("klasa")]
        public string Klasa { get; set; }

        [JsonProperty("wystawil")]
        public string Wystawil { get; set; }

        [JsonProperty("wystawil_login")]
        public string WystawilLogin { get; set; }

        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("termin")]
        public DateTime? Termin { get; set; }

        [JsonProperty("przedmiot")]
        public string Przedmiot { get; set; }

        [JsonProperty("opis")]
        public string Opis { get; set; }

        [JsonProperty("typ")]
        public int Typ { get; set; }
    }

    public class UwagaOsiagniecie
    {
        [JsonProperty("uczen")]
        public string Uczen { get; set; }

        [JsonProperty("uczen_login")]
        public string UczenLogin { get; set; }

        [JsonProperty("data")]
        public DateTime? Data { get; set; }

        [JsonProperty("tresc")]
        public string Tresc { get; set; }

        [JsonProperty("klasa")]
        public string Klasa { get; set; }

        [JsonProperty("typ")]
        public int Typ { get; set; }
    }

    public class Zajecia
    {
        [JsonProperty("klasa")]
        public string Klasa { get; set; }

        [JsonProperty("przedmiot")]
        public string Przedmiot { get; set; }
    }

    public class NauczycielResponse
    {
        [JsonProperty("imie_nazwisko")]
        public string ImieNazwisko { get; set; }

        [JsonProperty("klasa")]
        public string Klasa { get; set; }

        [JsonProperty("inne")]
        public Inne Inne { get; set; }

        [JsonProperty("uczniowie")]
        public List<Uczen> Uczniowie { get; set; }

        [JsonProperty("wydarzenia")]
        public List<Wydarzenie_n> Wydarzenia { get; set; }

        [JsonProperty("uwagi_i_osiagniecia")]
        public List<UwagaOsiagniecie> UwagiIOsiagniecia { get; set; }

        [JsonProperty("zajecia")]
        public List<Zajecia> Zajecia { get; set; }

        public string Login { get; set; }

        public NauczycielResponse()
        {
            Inne = new Inne();
            Uczniowie = new List<Uczen>();
            Wydarzenia = new List<Wydarzenie_n>();
            UwagiIOsiagniecia = new List<UwagaOsiagniecie>();
            Zajecia = new List<Zajecia>();
        }
    }
}
