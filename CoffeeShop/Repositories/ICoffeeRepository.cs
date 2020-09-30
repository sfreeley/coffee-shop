using CoffeeShop.Models;
using System.Collections.Generic;

namespace CoffeeShop.Repositories
{
    public interface ICoffeeRepository
    {
        List<Coffee> GetAllCoffees();
        Coffee GetCoffeeById(int id);
        void AddCoffee(Coffee coffee);
        void UpdateCoffee(Coffee coffee);
        void DeleteCoffee(int id);
    }
}