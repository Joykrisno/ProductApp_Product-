using System.Text.Json;
using System.Text;

namespace ProductBlazorApp.Components.Pages
{
    public partial class Home
    {

        public string apiUrl = "https://localhost:7295/api/Products/api/products/create";
        public List<Product> Products { get; set; } = new List<Product>();
        public ProductDto NewProduct = new ProductDto();
        public bool showSuccessMessage = false;

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

        protected override async Task OnInitializedAsync()
        {
            await FetchData();
        }

        public HttpClient httpClient = new HttpClient();

        public async Task FetchData()
        {
            Products = await httpClient.GetFromJsonAsync<List<Product>>("https://localhost:7295/api/products");
        }





        public class ProductDto
        {
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
