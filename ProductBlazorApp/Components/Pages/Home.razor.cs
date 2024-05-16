using System.Text.Json;
using System.Text;
using System.Net;

namespace ProductBlazorApp.Components.Pages
{
    public partial class Home
    {
     

        public string apiUrl = "https://localhost:7295/api/Products/api/products/create";
        public string apiUrlUpdate = "https://localhost:7295/api/Products/api/products/update";
        protected ProductDto SelectedProduct { get; set; } = new ProductDto();

        public List<Product> Products { get; set; } = new List<Product>();

        public ProductDto NewProduct = new ProductDto();
        public bool showSuccessMessage = false;
        private bool showUpdateSuccessMessage = false;
        private string successMessage;

        public async Task CreateProduct()
        {
            try
            {
                var json = JsonSerializer.Serialize(NewProduct);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await HttpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {

                    showSuccessMessage = true;
                    NewProduct = new ProductDto();
                    await FetchData();
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();

                }
            }
            catch (Exception ex)
            {

            }
        }

  
 




        public async Task DeleteProduct(int id)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"https://localhost:7295/api/Products/api/product/delete/{id}");

                if (response.IsSuccessStatusCode)
                {
                    await FetchData();
                }
                else
                {
                    // Handle error
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }



        public async Task UpdateProduct()
        {
            try
            {
                 
                var json = JsonSerializer.Serialize(NewProduct);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
 
                var updatedProduct = JsonSerializer.Deserialize<ProductDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });               
                var response = await HttpClient.PutAsync($"https://localhost:7295/api/Products/api/product/update?id={NewProduct.Id}", content);


                if (response.IsSuccessStatusCode)
                {
                     
                    showUpdateSuccessMessage = true;

                    
                    await FetchData();
                }
                else
                {
                   
                    var errorMessage = await response.Content.ReadAsStringAsync();
                  
                }
            }
            catch (Exception ex)
            {
                 
            }
        }



        public async Task EditProduct(int productId)
        {
            try
            {
                var response = await HttpClient.GetAsync($"https://localhost:7295/api/products/{productId}");

                if (response.IsSuccessStatusCode)
                {
                    var productJson = await response.Content.ReadAsStringAsync();
                    SelectedProduct = JsonSerializer.Deserialize<ProductDto>(productJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    
                    NewProduct = new ProductDto
                    {
                        Id = productId,
                        Name = SelectedProduct.Name,
                        Brand = SelectedProduct.Brand,
                        Category = SelectedProduct.Category,
                        Price = SelectedProduct.Price,
                        Description = SelectedProduct.Description
                    };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    // Handle error
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }




        protected override async Task OnInitializedAsync()
        {
            await FetchData();
        }

        public HttpClient httpClient = new HttpClient();
        private string errorMessage;

        public async Task FetchData()
        {
            Products = await httpClient.GetFromJsonAsync<List<Product>>("https://localhost:7295/api/products");
        }





        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Brand { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string Brand { get; set; } = "";
            public string Category { get; set; } = "";
            public decimal Price { get; set; }
            public string Description { get; set; } = "";
            public DateTime CreatedAt { get; set; }
        }
    }
}
