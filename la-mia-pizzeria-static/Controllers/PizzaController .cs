//using la_mia_pizzeria_static.Migrations;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class PizzaController : Controller
    {

        private readonly ILogger<PizzaController> _logger;
        private readonly PizzaContext _context;

        public PizzaController(ILogger<PizzaController> logger, PizzaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            var pizzas = _context.Pizzas.Include(p => p.Category).Include(p => p.Ingredients).ToArray();

            return View(pizzas);
        }

        public IActionResult Detail(int id)
        {

             var pizza = _context.Pizzas.Include(p => p.Category).Include(p => p.Ingredients).SingleOrDefault(p => p.Id == id); 

            if (pizza is null)
            {
                return NotFound($"L'id {id} della pizza non è stato trovato");
            }

            return View(pizza);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var formModel = new PizzaFormModel
            {
                Categories = _context.Categories.ToArray(),
                Ingredients = _context.Ingredients.Select(p => new SelectListItem(p.Name, p.Id.ToString())).ToArray(),
            };
            return View(formModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel form)
        {
            if (!ModelState.IsValid)
            {
                form.Categories = _context.Categories.ToArray();
                form.Ingredients = _context.Ingredients.Select(p => new SelectListItem(p.Name, p.Id.ToString())).ToArray();

                return View(form);
            }

            form.SetImageFileFromFormFile();
            form.Pizza.Ingredients = form.SelectedIngredients.Select(st => _context.Ingredients.First(i => i.Id == Convert.ToInt32(st))).ToList();

            _context.Pizzas.Add(form.Pizza);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            var pizza = _context.Pizzas.Include(p => p.Ingredients).FirstOrDefault(p => p.Id == id);

            if (pizza is null)
            {
                return View("NotFound");
            }

            var formModel = new PizzaFormModel
            {
                Pizza = pizza,
                Categories = _context.Categories.ToArray(),
                Ingredients = _context.Ingredients.Select(p => new SelectListItem(p.Name, p.Id.ToString())).ToArray(),
               
            };

            return View(formModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaFormModel form)
        {
            if (!ModelState.IsValid)
            {
                form.Categories = _context.Categories.ToArray();
                form.Ingredients = _context.Ingredients.Select(p => new SelectListItem(p.Name, p.Id.ToString())).ToArray();

                return View(form);
            }

            var pizzaToUpdate = _context.Pizzas.Include(p => p.Ingredients).FirstOrDefault(p => p.Id == id);
            var ingredients = _context.Ingredients.ToArray();

            if (pizzaToUpdate is null)
            {
                return View("NotFound");
            }

            form.SetImageFileFromFormFile();

            pizzaToUpdate.Name = form.Pizza.Name;
            pizzaToUpdate.Description = form.Pizza.Description;
            pizzaToUpdate.Image = form.Pizza.Image;
            pizzaToUpdate.ImageFile = form.Pizza.ImageFile;
            pizzaToUpdate.Price = form.Pizza.Price;
            pizzaToUpdate.CategoryId = form.Pizza.CategoryId;
            pizzaToUpdate.Ingredients = form.SelectedIngredients.Select(st => _context.Ingredients.First(i => i.Id == Convert.ToInt32(st))).ToList();

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var pizzaToDelete = _context.Pizzas.Find(id);

            if (pizzaToDelete is null)
            {
                return View("NotFound");
            }

            _context.Pizzas.Remove(pizzaToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
