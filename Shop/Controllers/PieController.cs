using Microsoft.AspNetCore.Mvc;
using Shop.Interfaces;
using Shop.Models;
using Shop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PieController(IPieRepository pieRepository, ICategoryRepository category)
        {
            _pieRepository = pieRepository;
            _categoryRepository = category;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(string category)
        {
            IEnumerable<Pie> pies;
            string currentCategory;

            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.AllPies.OrderBy(p => p.Id);
                currentCategory = "All pies";
            }
            else
            {
                pies = _pieRepository.AllPies.Where(c => c.Category.Name == category).OrderBy(p => p.Id);
                currentCategory = _categoryRepository.AllCategories.FirstOrDefault(c => c.Name == category)?.Name;
            }

            PieListViewModel pieListViewModel = new PieListViewModel 
            { 
                Pies = pies,
                CurrentCategory = currentCategory
            };
            
            return View(pieListViewModel);
        }

        public IActionResult Details(int Id)
        {
            var pie = _pieRepository.GetPieById(Id);
            if (pie == null)
            {
                return NotFound();
            }
            return View(pie);
        }
    }
}
