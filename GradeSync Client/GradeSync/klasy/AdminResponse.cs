using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GradeSync.klasy
{
    public class AdminResponse
    {
        [JsonProperty("admini")]
        public List<List<string>> Admini { get; set; }

        [JsonProperty("nauczyciele")]
        public List<List<string>> NauczycieleRaw { get; set; }

        public List<Nauczyciel_a> Nauczyciele => NauczycieleRaw.Select(Nauczyciel_a.FromArray).ToList();

        [JsonProperty("plany_lekcji")]
        public List<PlanLekcji> PlanyLekcji { get; set; }

        [JsonProperty("uczniowie")]
        public List<List<string>> UczniowieRaw { get; set; }

        public List<Uczen_a> Uczniowie { get; private set; }

        public string Login { get; set; }

        public AdminResponse()
        {
            Admini = new List<List<string>>();
            PlanyLekcji = new List<PlanLekcji>();
            Uczniowie = new List<Uczen_a>();
        }

        public void InitializeUczniowie()
        {
            Uczniowie = UczniowieRaw.Select(Uczen_a.FromArray).ToList();
        }
    }

    public class Nauczyciel_a
    {
        public string Login { get; set; }
        public string ImieNazwisko { get; set; }
        public string Klasa { get; set; }
        public string Inne { get; set; }

        public static Nauczyciel_a FromArray(List<string> array)
        {
            return new Nauczyciel_a
            {
                Login = array[0],
                ImieNazwisko = array[1],
                Klasa = array[2],
                Inne = array[3]
            };
        }
    }

    public class Uczen_a
    {
        public string Login { get; set; }
        public string ImieNazwisko { get; set; }
        public string Klasa { get; set; }

        public static Uczen_a FromArray(List<string> array)
        {
            return new Uczen_a
            {
                Login = array[0],
                ImieNazwisko = array[1],
                Klasa = array[2]
            };
        }
    }

        public class PlanLekcji
    {
        [JsonProperty("klasa")]
        public string Klasa { get; set; }

        [JsonProperty("poniedzialek")]
        public Dictionary<string, Lekcja> Poniedzialek { get; set; }

        [JsonProperty("wtorek")]
        public Dictionary<string, Lekcja> Wtorek { get; set; }

        [JsonProperty("sroda")]
        public Dictionary<string, Lekcja> Sroda { get; set; }

        [JsonProperty("czwartek")]
        public Dictionary<string, Lekcja> Czwartek { get; set; }

        [JsonProperty("piatek")]
        public Dictionary<string, Lekcja> Piatek { get; set; }
    }

    public class Lekcja
    {
        [JsonProperty("prowadzacy")]
        public string Prowadzacy { get; set; }

        [JsonProperty("prowadzacy_login")]
        public string ProwadzacyLogin { get; set; }

        [JsonProperty("przedmiot")]
        public string Przedmiot { get; set; }

        [JsonProperty("sala")]
        public string Sala { get; set; }
    }
}
