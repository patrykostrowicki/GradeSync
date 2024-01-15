using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GradeSync.klasy
{
    public class Ocena
    {
        [JsonProperty("przedmiot")]
        public string Przedmiot { get; set; }

        [JsonProperty("ocena")]
        public string Oceny { get; set; }

        [JsonProperty("wystawil")]
        public string Wystawil { get; set; }

        [JsonProperty("data_wystawienia")]
        public string DataWystawienia { get; set; }

        [JsonProperty("opis")]
        public string Opis { get; set; }
    }

    public class Przedmiot
    {
        [JsonProperty("przedmiot")]
        public string NazwaPrzedmiotu { get; set; }

        [JsonProperty("prowadzacy")]
        public string Prowadzacy { get; set; }

        [JsonProperty("sala")]
        public string Sala { get; set; }

        public static implicit operator Przedmiot(Dictionary<string, Przedmiot> v)
        {
            throw new NotImplementedException();
        }
    }

    public class PlanZajec
    {
        public Dictionary<string, Przedmiot> Poniedzialek { get; set; }
        public Dictionary<string, Przedmiot> Wtorek { get; set; }
        public Dictionary<string, Przedmiot> Sroda { get; set; }
        public Dictionary<string, Przedmiot> Czwartek { get; set; }
        public Dictionary<string, Przedmiot> Piatek { get; set; }
    }

    public class Frekwencja
    {
        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("przedmiot")]
        public string Przedmiot { get; set; }

        [JsonProperty("typ")]
        public int Typ { get; set; }
    }

    public class Uwaga
    {
        [JsonProperty("wystawil")]
        public string Wystawil { get; set; }

        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("tresc")]
        public string Tresc { get; set; }

        [JsonProperty("typ")]
        public int Typ { get; set; }
    }

    public class Wydarzenie
    {
        [JsonProperty("wystawil")]
        public string Wystawil { get; set; }

        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("termin")]
        public DateTime Termin { get; set; }

        [JsonProperty("typ")]
        public int Typ { get; set; }

        [JsonProperty("opis")]
        public string Opis { get; set; }

        [JsonProperty("przedmiot")]
        public string Przedmiot { get; set; }
    }

    public class UserResponse
    {
        [JsonProperty("imie_nazwisko")]
        public string ImieNazwisko { get; set; }

        [JsonProperty("klasa")]
        public string Klasa { get; set; }

        [JsonProperty("ocena_z_zachowania")]
        public string OcenaZZachowania { get; set; }

        [JsonProperty("oceny")]
        public List<Ocena> Oceny { get; set; }

        [JsonProperty("plan1")]
        public PlanZajec Plan1 { get; set; }

        [JsonProperty("plan2")]
        public PlanZajec Plan2 { get; set; }

        [JsonProperty("frekwencja")]
        public List<Frekwencja> Frekwencja { get; set; }

        [JsonProperty("uwagi")]
        public List<Uwaga> Uwagi { get; set; }

        public string Login { get; set; }

        [JsonProperty("wydarzenia")]
        public List<Wydarzenie> Wydarzenia { get; set; }

        public UserResponse()
        {
            Oceny = new List<Ocena>();
            Frekwencja = new List<Frekwencja>();
            Uwagi = new List<Uwaga>();
            Wydarzenia = new List<Wydarzenie>();
        }
    }
}
