using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RealPolyclinic.HelpMethods
{
    public class HelpfulMethods
    {
        public static bool IsNumberContains(string str)
        {
            foreach (char c in str)
                if (char.IsNumber(c))
                    return true;
            return false;
        }
        public static bool Checkdata(object x)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(x)) && Convert.ToInt32(x) != 0)
            {
                return true;
            }
            return false;
        }
        public static bool CheckDigitsinStr(string str,int needdigits)
        {
            char[] ch = str.ToCharArray();
            var count = ch.Where((n) => n >= '0' && n <= '9').Count();
            return count != needdigits;
        }
        public static bool IsOnlyLetters(string str)
        {
            Regex rg = new Regex(@"^[a-zA-Zа-яА-Я\s,]*$");
            return rg.IsMatch(str);
        }
    }

}

