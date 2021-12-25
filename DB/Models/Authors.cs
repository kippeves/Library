using System;
using System.Collections.Generic;

namespace DB.Models
{
    public partial class Authors
    {
        public Authors()
        {
            Book = new HashSet<Books>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public virtual ICollection<Books> Book { get; set; }
    }
}
