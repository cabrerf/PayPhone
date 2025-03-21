using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionHistoryController : ControllerBase
    {
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;

        public TransactionHistoryController(ITransactionHistoryRepository transactionHistoryRepository)
        {
            _transactionHistoryRepository = transactionHistoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(TransactionHistory), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IEnumerable<TransactionHistory> transactionHistories = await _transactionHistoryRepository.Get();

            return Ok(transactionHistories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TransactionHistory), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TransactionHistory? transactionHistory = await _transactionHistoryRepository.GetId(id);

            if (transactionHistory is null)
                return NotFound($"Transaction with ID {id} not found.");

            return Ok(transactionHistory);
        }

        [HttpGet("wallet/{walletId}")]
        [ProducesResponseType(typeof(IEnumerable<TransactionHistory>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByWalletId([FromRoute] int walletId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IEnumerable<TransactionHistory> transactionHistories = await _transactionHistoryRepository.GetByWalletId(walletId);

            if (!transactionHistories.Any())
                return NotFound($"No transactions found for Wallet ID {walletId}.");

            return Ok(transactionHistories);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TransactionHistory), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(TransactionHistory), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionHistory transactionHistory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                TransactionHistory newTransaction = await _transactionHistoryRepository.Create(transactionHistory.WalletId, transactionHistory.Amount, transactionHistory.Type);
                return Created("/TransactionHistory", newTransaction);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TransactionHistory), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TransactionHistory), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionHistory transactionHistory)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            try
            {
                TransactionHistory? updatedTransaction = await _transactionHistoryRepository.Update(id, transactionHistory.Amount, transactionHistory.Type);

                if (updatedTransaction is null)
                    return NotFound($"Transaction with ID {id} not found.");

                return Ok($"Transaction with ID {id} updated");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(TransactionHistory), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TransactionHistory), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(TransactionHistory), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool removed = await _transactionHistoryRepository.Delete(id);

            return !removed ? NotFound($"Transaction with ID {id} not found.") : Ok($"Transaction with ID {id} removed");
        }
    }
}