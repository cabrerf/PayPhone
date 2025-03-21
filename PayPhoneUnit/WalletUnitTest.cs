using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Repository.Interfaces;
using TodoList.Controllers;

namespace WalletTestUnit
{
    public class WalletUnitTest
    {
        // ONLY TESTS FOR GET ENDPOINT
        private readonly WalletController _controller;
        private readonly Mock<IWalletRepository> _mockWalletRepository;
        private readonly Mock<ILogger<WalletController>> _loggerMock;

        public WalletUnitTest()
        {
            _mockWalletRepository = new Mock<IWalletRepository>();
            _loggerMock = new Mock<ILogger<WalletController>>();
            _controller = new WalletController(_mockWalletRepository.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WhenWalletsExist()
        {
            // Arrange
            var wallets = new List<Wallet>
            {
                new Wallet { Id = 1, DocumentId = "DOC123", Name = "Personal Wallet", Balance = 1000.00m, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Wallet { Id = 2, DocumentId = "DOC456", Name = "Business Wallet", Balance = 5000.00m, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            };
            _mockWalletRepository.Setup(repo => repo.Get()).Returns(Task.FromResult<IEnumerable<Wallet>>(wallets));

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedWallets = Assert.IsType<List<Wallet>>(okResult.Value);
            Assert.Equal(wallets.Count, returnedWallets.Count);
        }

        [Fact]
        public async Task Get_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Key", "An error occurs");

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetId_ReturnsOkResult_WhenWalletExists()
        {
            // Arrange
            var wallet = new Wallet { Id = 1, DocumentId = "DOC123", Name = "Personal Wallet", Balance = 1000.00m, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            _mockWalletRepository.Setup(repo => repo.GetId(1)).Returns(Task.FromResult<Wallet?>(wallet));

            // Act
            var result = await _controller.GetId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedWallet = Assert.IsType<Wallet>(okResult.Value);
            Assert.Equal(wallet.Id, returnedWallet.Id);
        }

        [Fact]
        public async Task GetId_ReturnsNotFound_WhenWalletDoesNotExist()
        {
            // Arrange
            _mockWalletRepository.Setup(repo => repo.GetId(1)).Returns(Task.FromResult<Wallet?>(null));

            // Act
            var result = await _controller.GetId(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}