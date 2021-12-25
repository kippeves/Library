using System;
using System.Collections.Generic;

namespace DB.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Book = new HashSet<Books>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Books> Book { get; set; }
    }
}
