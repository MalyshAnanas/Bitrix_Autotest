using System.Text.RegularExpressions;

namespace Demo.BaseFramework
{
    enum TransliterationType
    {
        Gost,
        ISO,
    }

    public static class Transliteration
    {
        private static Dictionary<string, string> gostDictionary = new Dictionary<string, string>();
        private static Dictionary<string, string> iso = new Dictionary<string, string>();

        public static string Front(string text)
        {
            return Front(text, TransliterationType.ISO);
        }

        static string Front(string text, TransliterationType type)
        {
            string output = text;

            output = Regex.Replace(output, @"\s|\.|\(", " ");
            output = Regex.Replace(output, @"\s+", " ");
            output = Regex.Replace(output, @"[^\s\w\d-]", "");
            output = output.Trim();

            Dictionary<string, string> tdict = GetDictionaryByType(type);

            foreach (KeyValuePair<string, string> key in tdict)
            {
                output = output.Replace(key.Key, key.Value);
            }
            return output;
        }

        private static Dictionary<string, string> GetDictionaryByType(TransliterationType type)
        {
            Dictionary<string, string> transliterationDictionary = iso;
            
            if (type == TransliterationType.Gost)
                transliterationDictionary = gostDictionary;
            return transliterationDictionary;
        }

        static Transliteration()
        {
            gostDictionary.Add("№", "#");
            gostDictionary.Add("А", "A");
            gostDictionary.Add("Б", "B");
            gostDictionary.Add("В", "V");
            gostDictionary.Add("Г", "G");
            gostDictionary.Add("Д", "D");
            gostDictionary.Add("Е", "E");
            gostDictionary.Add("Ё", "JO");
            gostDictionary.Add("Ж", "ZH");
            gostDictionary.Add("З", "Z");
            gostDictionary.Add("И", "I");
            gostDictionary.Add("Й", "JJ");
            gostDictionary.Add("К", "K");
            gostDictionary.Add("Л", "L");
            gostDictionary.Add("М", "M");
            gostDictionary.Add("Н", "N");
            gostDictionary.Add("О", "O");
            gostDictionary.Add("П", "P");
            gostDictionary.Add("Р", "R");
            gostDictionary.Add("С", "S");
            gostDictionary.Add("Т", "T");
            gostDictionary.Add("У", "U");
            gostDictionary.Add("Ф", "F");
            gostDictionary.Add("Х", "Kh");
            gostDictionary.Add("Ц", "C");
            gostDictionary.Add("Ч", "CH");
            gostDictionary.Add("Ш", "SH");
            gostDictionary.Add("Щ", "SHH");
            gostDictionary.Add("Ъ", "");
            gostDictionary.Add("Ы", "Y");
            gostDictionary.Add("Ь", "");
            gostDictionary.Add("Э", "EH");
            gostDictionary.Add("Ю", "YU");
            gostDictionary.Add("Я", "YA");
            gostDictionary.Add("а", "a");
            gostDictionary.Add("б", "b");
            gostDictionary.Add("в", "v");
            gostDictionary.Add("г", "g");
            gostDictionary.Add("д", "d");
            gostDictionary.Add("е", "e");
            gostDictionary.Add("ё", "jo");
            gostDictionary.Add("ж", "zh");
            gostDictionary.Add("з", "z");
            gostDictionary.Add("и", "i");
            gostDictionary.Add("й", "jj");
            gostDictionary.Add("к", "k");
            gostDictionary.Add("л", "l");
            gostDictionary.Add("м", "m");
            gostDictionary.Add("н", "n");
            gostDictionary.Add("о", "o");
            gostDictionary.Add("п", "p");
            gostDictionary.Add("р", "r");
            gostDictionary.Add("с", "s");
            gostDictionary.Add("т", "t");
            gostDictionary.Add("у", "u");

            gostDictionary.Add("ф", "f");
            gostDictionary.Add("х", "kh");
            gostDictionary.Add("ц", "c");
            gostDictionary.Add("ч", "ch");
            gostDictionary.Add("ш", "sh");
            gostDictionary.Add("щ", "shh");
            gostDictionary.Add("ъ", "");
            gostDictionary.Add("ы", "y");
            gostDictionary.Add("ь", "");
            gostDictionary.Add("э", "eh");
            gostDictionary.Add("ю", "yu");
            gostDictionary.Add("я", "ya");
            gostDictionary.Add("«", "");
            gostDictionary.Add("»", "");
            gostDictionary.Add("—", "-");

            iso.Add("Є", "YE");
            iso.Add("І", "I");
            iso.Add("Ѓ", "G");
            iso.Add("і", "i");
            iso.Add("№", "#");
            iso.Add("є", "ye");
            iso.Add("ѓ", "g");
            iso.Add("А", "A");
            iso.Add("Б", "B");
            iso.Add("В", "V");
            iso.Add("Г", "G");
            iso.Add("Д", "D");
            iso.Add("Е", "E");
            iso.Add("Ё", "YO");
            iso.Add("Ж", "ZH");
            iso.Add("З", "Z");
            iso.Add("И", "I");
            iso.Add("Й", "J");
            iso.Add("К", "K");
            iso.Add("Л", "L");
            iso.Add("М", "M");
            iso.Add("Н", "N");
            iso.Add("О", "O");
            iso.Add("П", "P");
            iso.Add("Р", "R");
            iso.Add("С", "S");
            iso.Add("Т", "T");
            iso.Add("У", "U");
            iso.Add("Ф", "F");
            iso.Add("Х", "X");
            iso.Add("Ц", "C");
            iso.Add("Ч", "CH");
            iso.Add("Ш", "SH");
            iso.Add("Щ", "SHH");
            iso.Add("Ъ", "'");
            iso.Add("Ы", "Y");
            iso.Add("Ь", "");
            iso.Add("Э", "E");
            iso.Add("Ю", "YU");
            iso.Add("Я", "YA");
            iso.Add("а", "a");
            iso.Add("б", "b");
            iso.Add("в", "v");
            iso.Add("г", "g");
            iso.Add("д", "d");
            iso.Add("е", "e");
            iso.Add("ё", "yo");
            iso.Add("ж", "zh");
            iso.Add("з", "z");
            iso.Add("и", "i");
            iso.Add("й", "j");
            iso.Add("к", "k");
            iso.Add("л", "l");
            iso.Add("м", "m");
            iso.Add("н", "n");
            iso.Add("о", "o");
            iso.Add("п", "p");
            iso.Add("р", "r");
            iso.Add("с", "s");
            iso.Add("т", "t");
            iso.Add("у", "u");
            iso.Add("ф", "f");
            iso.Add("х", "x");
            iso.Add("ц", "c");
            iso.Add("ч", "ch");
            iso.Add("ш", "sh");
            iso.Add("щ", "shh");
            iso.Add("ъ", "");
            iso.Add("ы", "y");
            iso.Add("ь", "");
            iso.Add("э", "e");
            iso.Add("ю", "yu");
            iso.Add("я", "ya");
            iso.Add("«", "");
            iso.Add("»", "");
            iso.Add("—", "-");
            iso.Add(" ", "-");
        }
    }
}


