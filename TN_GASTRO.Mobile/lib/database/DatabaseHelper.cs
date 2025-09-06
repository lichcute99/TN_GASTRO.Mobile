
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using SQLite;

namespace TN_GASTRO.Mobile.lib.database
{
    public class DatabaseHelper
    {
        // Singleton
        private static readonly Lazy<DatabaseHelper> _instance =
            new Lazy<DatabaseHelper>(() => new DatabaseHelper());

        public static DatabaseHelper Instance => _instance.Value;

        private SQLiteAsyncConnection _database;
        private readonly string _dbPath = Path.Combine(
     Windows.Storage.ApplicationData.Current.LocalFolder.Path,
     "order_db.db"
 );


        private DatabaseHelper()
        {
           
        }

        public async Task<SQLiteAsyncConnection> GetDatabaseAsync()
        {
            if (_database != null)
                return _database;

            _database = await InitDatabaseAsync();
            return _database;
        }

        private async Task<SQLiteAsyncConnection> InitDatabaseAsync() {
            if (!File.Exists(_dbPath))
            {
                try
                {
                    // Đọc file từ gói cài đặt (Assets)
                    var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
                        new Uri("ms-appx:///Assets/data.sqlite"));



                    var buffer = await Windows.Storage.FileIO.ReadBufferAsync(file);

                    using (var stream = File.Create(_dbPath))
                    using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
                    {
                        var bytes = new byte[buffer.Length];
                        dataReader.ReadBytes(bytes);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi copy database mẫu: " + ex.Message);
                }
            }

            return new SQLiteAsyncConnection(_dbPath);
        }



    }
}
