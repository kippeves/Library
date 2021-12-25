using System;
using System.Collections.Generic;

namespace DB.Models
{
    public partial class AttributesNames
    {
        public AttributesNames()
        {
            Values = new HashSet<Attributes>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Attributes> Values { get; set; }
    }
}
