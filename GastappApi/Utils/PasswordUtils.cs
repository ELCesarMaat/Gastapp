using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace GastappApi.Utils
{
    public static class PasswordUtils
    {
        public static string GenerateSalt()
        {
            var salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string salt)
        {
            using (var hmac = new HMACSHA256(Convert.FromBase64String(salt)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        public static bool IsPasswordSafe(string password)
        {
            if(password.Length < 8)
                return false;

            var regex = new Regex(@"^(?=.*[a-zA-Z])(?=.*\d).+$");
            return regex.IsMatch(password);
        }
    }
}
