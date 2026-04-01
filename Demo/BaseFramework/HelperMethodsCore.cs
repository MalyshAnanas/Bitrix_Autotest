using System.Drawing;
using System.Text;

namespace Demo.BaseFramework
{
    public class HelperMethodsCore
    {
        public static string ConvertToHex(Color messageColor)
        {
            return "#" + messageColor.R.ToString("X2") + messageColor.G.ToString("X2") + messageColor.B.ToString("X2");
        }

        public static string GetDateTimeSalt(bool convertToPseudoRoman = false, int maxLength = 0)
        {
            string res = DateTime.Now.ToString("ddMMyyyyHHmmss");
            if (convertToPseudoRoman)
                res = ConvertNumberToPseudoRomanNumeral(long.Parse(res));
            if (maxLength > 0 && res.Length > maxLength)
                res = res[(res.Length - maxLength)..];
            return res;
        }

        /// <summary>
        /// Превращает арабскую цифру в псевдоримскую, каждые 2 разряда конвертит в римскую
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ConvertNumberToPseudoRomanNumeral(long number)
        {
            string sourceNumber = number.ToString();
            if (number < 0)
                sourceNumber = sourceNumber[1..];
            string result = default;

            if (number < 10)
                result = ConvertNumberToRomanNumeral(number);
            else
            {
                for (int i = 2; true; i += 2)
                {
                    string segment = sourceNumber[(i - 2)..];
                    if (segment.Length >= 2)
                        segment = segment[0..2];
                    result += ConvertNumberToRomanNumeral(int.Parse(segment));
                    if (segment.Length < 2 || i == sourceNumber.Length)
                        break;
                }
            }

            if (number < 0)
                result = "-" + result;
            return result;
        }

        /// <summary>
        /// Превращает арабскую цифру в римскую
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static string ConvertNumberToRomanNumeral(long number)
        {
            int[] roman_value_list = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            string[] roman_char_list = new string[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            StringBuilder res = new StringBuilder();

            for (int i = 0; i < roman_value_list.Length; i += 1)
            {
                while (number >= roman_value_list[i])
                {
                    number -= roman_value_list[i];
                    res.Append(roman_char_list[i]);
                }
            }

            return res.ToString();
        }
    }
}
