using FluentAssertions;
using Newtonsoft.Json;
using CatalogService.Application.Entities;
using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace CatalogService.Tests
{
    public class CategoryControllerTests : IntegrationTest
    {
        private string _idToDelete;

        [Test]
        [Order(0)]
        public async Task CreateCategory_WithAllParameters_Returns200Code()
        {
            var response = await TestClient.PostAsync(
                "https://localhost:5001/api/Categories/category",
                new StringContent(JsonConvert.SerializeObject(new Category { CategoryName = "Test", Picture = null, Items = null, Description = "TestDesc" }),
                    Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            _idToDelete = await response.Content.ReadAsStringAsync();
        }

        [Test]
        [Order(1)]
        public async Task UpdateCategory_CategoryExists_Returns200Code()
        {
            if (string.IsNullOrEmpty(_idToDelete))
            {
                var updatedCategory = new Category { CategoryID = 1, CategoryName = "Beverages", Picture = null, Items = null, Description = "Soft drinks, coffees, teas, beers, and ales" };
                var requestContent = new StringContent(JsonConvert.SerializeObject(updatedCategory), Encoding.UTF8, "application/json");
                var updateResponse = await TestClient.PutAsync("https://localhost:5001/api/Categories/category", requestContent);

                updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }
            else
            {
                var updatedCategory = new Category { CategoryID = int.Parse(_idToDelete), CategoryName = "Test", Picture = null, Items = null, Description = "TestDesc" };
                var requestContent = new StringContent(JsonConvert.SerializeObject(updatedCategory), Encoding.UTF8, "application/json");
                var updateResponse = await TestClient.PutAsync("https://localhost:5001/api/Categories/category", requestContent);

                updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }
        }

        [Test]
        [Order(2)]
        public async Task DeleteCategory_DeleteCreatedEntity_Returns200CodeIfIdExistElse400Code()
        {
            var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:5001/api/Categories/category?id={_idToDelete}");

            var deleteResponse = await TestClient.SendAsync(deleteRequest);

            if (string.IsNullOrEmpty(_idToDelete))
            {
                deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            }
            else
            {
                deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            }
        }

        [Test]
        [Order(3)]
        public async Task UpdateCategory_CategoryNotExist_Returns400Code()
        {
            var updatedCategory = new Category { CategoryID = int.MaxValue, CategoryName = "Test", Picture = null, Items = null, Description = "TestDesc" };
            var category = System.Text.Json.JsonSerializer.Serialize(updatedCategory);
            var requestContent = new StringContent(category, Encoding.UTF8, "application/json");
            var updateResponse = await TestClient.PutAsync("https://localhost:5001/api/Categories/category", requestContent);

            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetCategories_WithoutCount_ReturnsAllExistingCategories()
        {
            var response = await TestClient.GetAsync("https://localhost:5001/api/Categories/categories");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async Task GetCategories_WithCount_ReturnsCountCategories()
        {
            var response = await TestClient.GetAsync("https://localhost:5001/api/Categories/categories?count=5");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async Task GetCategories_WithNoCount_Returns404Code()
        {
            var response = await TestClient.GetAsync("/Category/get");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetCategory_WrongUrl_Returns404Code()
        {
            var response = await TestClient.GetAsync("/Category/find/4");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetCategory_CategoryNotExist_Returns404Code()
        {
            var response = await TestClient.GetAsync("/Category/find?id=1000000000");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }

}
