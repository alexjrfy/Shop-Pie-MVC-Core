using Shop.Interfaces;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Mock
{
    public class MockCategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> AllCategories 
            => new List<Category> 
            {
                new Category {Id=1, Name="Fruit pies", Description="All fruits pies"},
                new Category {Id=2, Name="Chesse cakes", Description="Cheesy all the way"},
                new Category {Id=3, Name="Seasonal pies", Description="Get in the mood for a seasonal pie"}
            };
    }
}
