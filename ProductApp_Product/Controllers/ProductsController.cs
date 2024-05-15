using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApp_Product.Models;
using System.Data.SqlClient;

namespace ProductApp_Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly string connectionString;

        public ProductsController(IConfiguration configuration)

        {
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
        }



        [HttpGet]
        public IActionResult GetProduct()
        {
            List<Product> products = new List<Product>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM products";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product();

                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreatedAt = reader.GetDateTime(6);
                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "Sorry,But we Have an Exception");
                return BadRequest(ModelState);
            }
            return Ok(products);
        }

        [HttpPost]
        [Route("api/products/create")]
        public IActionResult CreateProduct(ProductDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"INSERT INTO products (name, brand, category, price, description)
                   VALUES (@name, @brand, @category, @price, @description)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", productDto.Name);
                        command.Parameters.AddWithValue("@brand", productDto.Brand);
                        command.Parameters.AddWithValue("@category", productDto.Category);
                        command.Parameters.AddWithValue("@price", productDto.Price);
                        command.Parameters.AddWithValue("@description", productDto.Description);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "Sorry,But we Have an Exception");
                return BadRequest(ModelState);
            }



            return Ok(productDto);

        }


        [HttpGet("{id}")]
        public IActionResult GetProductId(int id)
        {
            Product product = new Product();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM products WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreatedAt = reader.GetDateTime(6);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "Sorry, but we encountered an exception");
                return BadRequest(ModelState);
            }

            return Ok(product);
        }



        [HttpPut]
        [Route("api/product/update")]
        public IActionResult UpdateProduct(int id,ProductDto productDto)
        {

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE products SET name = @name,brand = @brand,  category = @category, price = @price, description = @description WHERE id = @id";

                    using (var command = new SqlCommand(sql,connection))
                    {
                        command.Parameters.AddWithValue("@name", productDto.Name);
                        command.Parameters.AddWithValue("@brand", productDto.Brand);
                        command.Parameters.AddWithValue("@category", productDto.Category);
                        command.Parameters.AddWithValue("@price", productDto.Price);
                        command.Parameters.AddWithValue("@description", productDto.Description);
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch
            {

            }

            return Ok();
        }



    }
}

