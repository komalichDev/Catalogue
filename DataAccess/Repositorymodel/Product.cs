using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositorymodel
{
    public struct Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public Description Description { get; set; }

        public Category Category { get; set; }
    }
}
