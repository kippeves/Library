using System;
using System.Collections.Generic;

namespace DB.Models
{
    public partial class Attributes
    {
        public int BookId { get; set; }
        public int AttributeId { get; set; }
        public string Name => AttributeName.Name;
        public string Value { get; set; } = null!;

        public virtual AttributesNames AttributeName { get; set; } = null!;
        public virtual Books Book { get; set; } = null!;
    }
}
