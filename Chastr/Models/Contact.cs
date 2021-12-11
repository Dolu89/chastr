using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chastr.Models
{
    public class Contact : IItem
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string PubKey { get; set; }
    }
}
