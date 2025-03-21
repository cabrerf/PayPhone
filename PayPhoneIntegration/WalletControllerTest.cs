using Entities;
using System.Net;
using System.Text;
using System.Text.Json;

namespace TodoList.IntegrationTests
{
    public class WalletControllerTests
    {
        private readonly HttpClient _client;

        public WalletControllerTests()
        {
            //NOT IMPLEMENTED YET!
        }

        [Fact]
        public async Task Get_ReturnsOkResult()
        {
            // Act
            var response = await _client.GetAsync("/api/wallet");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateWallet_ReturnsCreatedResult()
        {
            // Arrange
            var wallet = new Wallet
            {
                DocumentId = "DOC999",
                Name = "Test Wallet",
                Balance = 1000.00m
            };
            var content = new StringContent(JsonSerializer.Serialize(wallet), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/wallet", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
