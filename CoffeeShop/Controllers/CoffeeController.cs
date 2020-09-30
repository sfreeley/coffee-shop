using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Models;
using CoffeeShop.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeController : ControllerBase
    {
        private readonly ICoffeeRepository _coffeeRepository;

        public CoffeeController(ICoffeeRepository coffeeRepository)
        {
            _coffeeRepository = coffeeRepository;
        }

        // https://localhost5001/api/coffee/
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_coffeeRepository.GetAllCoffees());
        }

        // https://localhost:5001/api/coffee/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Coffee coffee = _coffeeRepository.GetCoffeeById(id);
            if (coffee == null)
            {
                return NotFound();
            }
            //Ok() is used when data needs to be returned
            return Ok(coffee);
        }

        //https://localhost:5001/api/coffee/
        [HttpPost]
        public IActionResult Post(Coffee coffee)
        {
            _coffeeRepository.AddCoffee(coffee);
            return CreatedAtAction("Get", new { id = coffee.Id }, coffee);
        }

        // https://localhost:5001/api/coffee/id
        [HttpPut("{id}")]
        public IActionResult Put(int id, Coffee coffee)
        {
            //if when trying to edit this coffee and there is no match with the id of the coffee, return bad request
            if (id != coffee.Id)
            {
                return BadRequest();
            }

            //otherwise, update
            _coffeeRepository.UpdateCoffee(coffee);

            //used to indicate that the action was successful, but there is no data to return
            return NoContent();
        }

        // https://localhost:5001/api/coffee/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _coffeeRepository.DeleteCoffee(id);
            return NoContent();
        }

    }
}
