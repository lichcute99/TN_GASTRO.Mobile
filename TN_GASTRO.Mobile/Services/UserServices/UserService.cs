using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using TN_GASTRO.Mobile.Models;

namespace TN_GASTRO.Mobile.Services.UserServices
{
    public class UserService
    {
        private readonly SQLiteAsyncConnection _db;

        public UserService(SQLiteAsyncConnection db)
        {
            _db = db;
        }

        public async Task<User?> LoginAsync(string pin)
        {
            // Tìm user có password = pin
            var user = await _db.Table<User>()
                                .Where(u => u.Password == pin)
                                .FirstOrDefaultAsync();
            return user;
        }
    }
}
