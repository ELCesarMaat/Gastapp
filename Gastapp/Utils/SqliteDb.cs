
namespace Gastapp.Utils
{
    public static class SqliteDb
    {
        private const string DbName = "gastappDb";
        private static string _dbRoute = string.Empty;
        public static string GetDbRoute()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
                _dbRoute = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbName);
            else if(DeviceInfo.Platform == DevicePlatform.iOS)
                _dbRoute = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"..", "Library", DbName);

            return _dbRoute;
        }

    }
}
