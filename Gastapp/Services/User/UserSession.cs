using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gastapp.Services.User
{
    public static class UserSession
    {
        public static void SetSession(string token, string email, int userId)
        {
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(email))
            {
                Preferences.Set("token", token);
                Preferences.Set("email", email);
                Preferences.Set("userId", userId);
            }
        }

        public static string GetToken()
        {
            return Preferences.Get("token", string.Empty);
        }

        public static bool IsUserNameSaved()
        {
            return Preferences.Get("username", string.Empty) != string.Empty;
        }

        public static void ClearSession()
        {
            Preferences.Clear();
        }
    }
}
