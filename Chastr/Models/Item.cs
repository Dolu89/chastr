using SQLite;
using System;

namespace Chastr.Models
{
    public class Item : IItem
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }
}