namespace WebApplication1.DTOs
{
    public class CountryDTO
    {
        public Name Name { get; set; }
        public List<string> Tld { get; set; }
        public string Cca2 { get; set; }
        public string Ccn3 { get; set; }
        public string Cca3 { get; set; }
        public bool Independent { get; set; }
        public string Status { get; set; }
        public bool UnMember { get; set; }
        public Dictionary<string, Currency> Currencies { get; set; }
        public IdD Idd { get; set; }
        public List<string> Capital { get; set; }
        public List<string> AltSpellings { get; set; }
        public string Region { get; set; }
        public Dictionary<string, string> Languages { get; set; }
        public Dictionary<string, Translation> Translations { get; set; }
        public List<double> Latlng { get; set; }
        public bool Landlocked { get; set; }
        public double Area { get; set; }
        public Dictionary<string, Demonym> Demonyms { get; set; }
        public string Flag { get; set; }
        public Maps Maps { get; set; }
        public int Population { get; set; }
        public Car Car { get; set; }
        public List<string> Timezones { get; set; }
        public List<string> Continents { get; set; }
        public Flags Flags { get; set; }
        public object CoatOfArms { get; set; } // Nếu không có thông tin, có thể dùng object
        public string StartOfWeek { get; set; }
        public CapitalInfo CapitalInfo { get; set; }
    }

    public class Name
    {
        public string Common { get; set; }
        public string Official { get; set; }
        public Dictionary<string, NativeName> NativeName { get; set; }
    }

    public class NativeName
    {
        public string Official { get; set; }
        public string Common { get; set; }
    }

    public class Currency
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
    }

    public class IdD
    {
        public string Root { get; set; }
        public List<string> Suffixes { get; set; }
    }

    public class Translation
    {
        public string Official { get; set; }
        public string Common { get; set; }
    }

    public class Demonym
    {
        public string F { get; set; }
        public string M { get; set; }
    }

    public class Maps
    {
        public string GoogleMaps { get; set; }
        public string OpenStreetMaps { get; set; }
    }

    public class Car
    {
        public List<string> Signs { get; set; }
        public string Side { get; set; }
    }

    public class Flags
    {
        public string Png { get; set; }
        public string Svg { get; set; }
    }

    public class CapitalInfo
    {
        public List<double> Latlng { get; set; }
    }
}