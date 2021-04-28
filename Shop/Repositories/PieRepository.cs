using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Interfaces;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Repositories
{
    public class PieRepository : IPieRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PieRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Pie> AllPies {
            get{ 
                return _applicationDbContext.Pies.Include(c => c.Category);
            }
        }

        public IEnumerable<Pie> PiesOfTheWeek
        {
            get { 
                return _applicationDbContext.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);
            }
        }

        public Pie GetPieById(int Id)
        {
            return _applicationDbContext.Pies.FirstOrDefault(p => p.Id == Id);
        }
    }
}
