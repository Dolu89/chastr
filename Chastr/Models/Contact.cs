using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chastr.Models
{
    public class Contact
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string PubKey { get; set; }
    }
}
