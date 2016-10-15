using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Database.Model.Entities
{
    public class LocalUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AppInfo { get; set; }
    }
}
