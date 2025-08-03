using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Helper
{
    public class DefaultValues
    {
        public string RemoveDiacritics(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            str = str.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in str)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public string GenerateRandomPassword(int length = 9)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var password = new char[length];
            Random _random = new Random();

            for (int i = 0; i < length; i++)
            {
                password[i] = validChars[_random.Next(validChars.Length)];
            }

            return new string(password);
        }

        public string GenerateUniqueCode(string accountName)
        {
            string input = accountName + DateTime.UtcNow.Ticks;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 30);
                return hashString;
            }
        }

        public string HashPassword(string password)
        {
            using (var hasher = new System.Security.Cryptography.SHA256Managed())
            {
                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hashedBytes = hasher.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public int GetWeekOfMonth(DateTime date)
        {
            var firstDay = new DateTime(date.Year, date.Month, 1);
            int firstDayDiff = (7 + (firstDay.DayOfWeek - DayOfWeek.Monday)) % 7;
            var firstWeekStart = firstDay.AddDays(-1 * firstDayDiff);
            int weekNumber = (int)Math.Floor((date - firstWeekStart).TotalDays / 7) + 1;
            return weekNumber;
        }
    }
}
