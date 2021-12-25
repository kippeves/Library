using System;
using System.Collections.Generic;

namespace DB.Models
{
    public partial class Books
    {
        public Books()
        {
            Attributes = new HashSet<Attributes>();
            Author = new HashSet<Authors>();
            Category = new HashSet<Categories>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Cover { get; set; }
        public string? Descr { get; set; }

        public virtual ICollection<Attributes> Attributes { get; set; }

        public virtual ICollection<Authors> Author { get; set; } = null!;
        public virtual ICollection<Categories> Category { get; set; } = null!;
    }
}
