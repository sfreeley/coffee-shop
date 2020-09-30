using CoffeeShop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly string _connectionString;

        public CoffeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public SqlConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }

        public List<Coffee> GetAllCoffees()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Title, c.BeanVarietyId, b.[Name], b.Region, b.Notes
                                       FROM Coffee c
                                       LEFT JOIN BeanVariety b
                                       ON c.BeanVarietyId = b.Id";
                    var reader = cmd.ExecuteReader();
                    List<Coffee> coffees = new List<Coffee>();
                    while (reader.Read())
                    {
                        Coffee coffee = new Coffee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = new BeanVariety()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Region = reader.GetString(reader.GetOrdinal("Region"))
                            }

                        };

                        if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            coffee.BeanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        coffees.Add(coffee);
                    }
                    reader.Close();
                    return coffees;
                }
            }
        }

        public Coffee GetCoffeeById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT c.Id, c.Title, c.BeanVarietyId, b.[Name], b.Region, b.Notes
                        FROM Coffee c
                        LEFT JOIN BeanVariety b
                        ON c.BeanVarietyId = b.Id
                        WHERE c.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    Coffee coffee = null;
                    if (reader.Read())
                    {
                        coffee = new Coffee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = new BeanVariety()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Region = reader.GetString(reader.GetOrdinal("Region"))
                            }
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            coffee.BeanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        
                    }
                    reader.Close();

                    return coffee;
                }
            }
        }

        public void AddCoffee(Coffee coffee)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Coffee (Title, BeanVarietyId)
                        OUTPUT INSERTED.ID
                        VALUES (@title, @beanVarietyId)";
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);

                    coffee.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateCoffee(Coffee coffee)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Coffee
                        SET Title = @title,
                            BeanVarietyId = @beanVarietyId
                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", coffee.Id);
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCoffee(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Coffee 
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
