using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepository _walletRepository;

        public WalletController(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IEnumerable<Wallet> wallets = await _walletRepository.Get();

            return Ok(wallets);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Wallet? wallet = await _walletRepository.GetId(id);

            if (wallet is null)
                return NotFound($"Wallet with ID {id} not found.");

            return Ok(wallet);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateWallet([FromBody] Wallet wallet)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Wallet newWallet = await _walletRepository.Create(wallet.DocumentId, wallet.Name, wallet.Balance);
                return Created("/Wallet", newWallet);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] Wallet wallet)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            try
            {
                Wallet? updatedWallet = await _walletRepository.Put(id, wallet.DocumentId, wallet.Name, wallet.Balance);

                if (updatedWallet is null)
                    return NotFound($"Wallet with ID {id} not found.");

                return Ok($"Wallet with ID {id} updated");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Wallet), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool removed = await _walletRepository.Delete(id);

            return !removed ? NotFound($"Wallet with ID {id} not found.") : Ok($"Wallet with ID {id} removed");
        }
    }
}