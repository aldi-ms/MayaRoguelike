using System.ComponentModel;

namespace WorldOfCSharp
{
    public enum GameMonth
    {
        [Description("Pop")]
        First = 1,
        [Description("Wo'")]
        Second,
        [Description("Sip")]
        Third,
        [Description("Sotz'")]
        Fourth,
        [Description("Sek")]
        Fifth,
        [Description("Xul")]
        Sixth,
        [Description("Yaxk'in'")]
        Seventh,
        [Description("Mol")]
        Eight,
        [Description("Ch'en")]
        Nineth,
        [Description("Yax")]
        Tenth,
        [Description("Sak'")]
        Eleventh,
        [Description("Keh")]
        Twelveth,
        [Description("Mak")]
        Thirteenth,
        [Description("K'ank'in")]
        Fourteenth,
        [Description("Muwan'")]
        Fiveteenth,
        [Description("Pax")]
        Sixteenth,
        [Description("K'ayab")]
        Seventeenth,
        [Description("Kumk'u")]
        Eighteenth,
        [Description("Wayeb'")]
        Nineteenth
	}

    public enum GameDay
    {
        [Description("Imix'")]
        First = 1,
        [Description("Ik'")]
        Second,
        [Description("Ak'b'al")]
        Third,
        [Description("K'an'")]
        Fourth,
        [Description("Chikchan")]
        Fifth,
        [Description("Kimi")]
        Sixth,
        [Description("Manik'")]
        Seventh,
        [Description("Lamat")]
        Eight,
        [Description("Muluk")]
        Nineth,
        [Description("Ok")]
        Tenth,
        [Description("Chuwen")]
        Eleventh,
        [Description("Eb'")]
        Twelveth,
        [Description("B'en")]
        Thirteenth,
        [Description("Ix")]
        Fourteenth,
        [Description("Men")]
        Fiveteenth,
        [Description("K'ib'")]
        Sixteenth,
        [Description("Kab'an")]
        Seventeenth,
        [Description("Etz'nab'")]
        Eighteenth,
        [Description("Kawak")]
        Nineteenth,
        [Description("Ajaw")]
        Twentieth
    }

    public static class GameTimeExtensions
    {
        public static string GetName(this GameMonth month)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])month.GetType().GetField(month.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string GetName(this GameDay day)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])day.GetType().GetField(day.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
