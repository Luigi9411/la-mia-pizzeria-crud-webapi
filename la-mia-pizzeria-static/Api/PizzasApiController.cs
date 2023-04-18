using la_mia_pizzeria_static.Controllers;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace la_mia_pizzeria_static.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasApiController : ControllerBase
    {
        private readonly ILogger<PizzaController> _logger;
        private readonly PizzaContext _context;

        public PizzasApiController(ILogger<PizzaController> logger, PizzaContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPizzas([FromQuery] string? name)
        {
            var pizzas = _context.Pizzas
                .Where(p => name == null || p.Name.ToLower().Contains(name.ToLower()))
                .ToList();

            return Ok(pizzas);
        }

        [HttpGet("{id}")]
        public IActionResult GetPizza(int id)
        {
            var pizza = _context.Pizzas.FirstOrDefault(p => p.Id == id);

            if (pizza is null)
            {
                return NotFound();
            }

            return Ok(pizza);
        }

        [HttpPost]
        public IActionResult CreatePizza(Pizza pizza)
        {
            _context.Pizzas.Add(pizza);
            _context.SaveChanges();

            return Ok(pizza);
        }

        [HttpPut("{id}")]
        public IActionResult PutPizza(int id, [FromBody] Pizza pizza)
        {
            var savedPizza = _context.Pizzas.FirstOrDefault(p => p.Id == id);

            if (savedPizza is null)
            {
                return NotFound();
            }

            savedPizza.Name = pizza.Name;
            savedPizza.Description = pizza.Description;
            savedPizza.Image = pizza.Image;
            savedPizza.Price = pizza.Price;
            savedPizza.CategoryId = pizza.CategoryId;

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePizza(int id)
        {
            var savedPizza = _context.Pizzas.FirstOrDefault(p => p.Id == id);

            if (savedPizza is null)
            {
                return NotFound();
            }

            _context.Pizzas.Remove(savedPizza);
            _context.SaveChanges();

            return Ok();
        }
    }
}
