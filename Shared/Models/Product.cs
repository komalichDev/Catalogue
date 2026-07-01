using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public struct Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Description Description { get; set; }
        public string Category { get; set; }

    }
}
