using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TN_GASTRO.Mobile.Models
{
    [Table("user")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("salt")]
        public string Salt { get; set; }

        [Column("required_change_password")]
        public int? RequiredChangePassword { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("is_active")]
        public int? IsActive { get; set; }

        [Column("is_login")]
        public int? IsLogin { get; set; }

        [Column("created_at")]
        public string CreatedAt { get; set; }

        [Column("updated_at")]
        public string UpdatedAt { get; set; }
    }
}
