//using back_end.InputModel;
//using back_end.Services;
//using back_end.ViewModel;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Threading.Tasks;

//namespace back_end.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountsController : ControllerBase
//    {
//        private readonly IAccountService _accountService;

//        public AccountsController(IAccountService accountService)
//        {
//            _accountService = accountService;
//        }

//        /// <summary>
//        /// Buscar todas as accounts de forma paginada.
//        /// </summary>
//        /// <remarks>Todo: Paginação da query</remarks>
//        /// <param name="page">Indica qual pagina será consultada. (Min. 1)</param>
//        /// <param name="qtd">Indica a quantidade de contas por página (Min. 1 Max. 50)</param>
//        /// <response code="200">Retorna lista de accounts</response>
//        /// <response code="204">Não há accounts para serem listadas</response>
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<AccountViewModel>>> GetAccount([FromQuery, Range(1, int.MaxValue)] int page = 1, [FromQuery, Range(1, 50)] int qtd = 5)
//        {
//            var accounts = await _accountService.getAccount(page, qtd);

//            if (accounts.Count() == 0)
//                return NoContent();

//            return Ok(accounts);
//        }

//        /// <summary>
//        /// Retorna uma account por endereço hex da carteira
//        /// </summary>
//        /// <param name="address">Endereço da wallet vinculada</param>
//        /// <response code="200">Account encontrada com êxito</response>
//        /// <response code="204">Account não encontrada</response>
//        [HttpGet("{address}")]
//        public async Task<ActionResult<AccountViewModel>> GetAccount([FromRoute] string address)
//        {
//            var account = await _accountService.getAccount(address);

//            if (account == null)
//                return NoContent();

//            return Ok();
//        }

//        /// <summary>
//        /// Retorna uma account por id
//        /// </summary>
//        /// <param name="id">Id vinculado no banco de dados</param>
//        /// <response code="200">Account encontrada com êxito</response>
//        /// <response code="204">Account não encontrada</response>
//        [HttpGet("{id:int}")]
//        public async Task<ActionResult<AccountViewModel>> GetAccount([FromRoute] int id)
//        {
//            var account = await _accountService.getAccount(id);

//            if (account == null)
//                return NoContent();

//            return Ok(account);
//        }

//        /// <summary>
//        /// Adiciona uma nova conta ao banco de dados
//        /// </summary>
//        /// <param name="account">Objeto da conta</param>
//        /// <response code="200">Account criada com êxito</response>
//        /// <response code="422">Ocorreu algum erro no momento da criação</response>
//        [HttpPost]
//        public async Task<ActionResult<AccountViewModel>> AddAccount([FromBody] AccountInputModel account)
//        {
//            try
//            {
//                var _account = await _accountService.addAccount(account);
//                return Ok(_account);
//            }
//            catch (Exception e)
//            {
//                return UnprocessableEntity(e.Message);
//            }
//        }

//        /// <summary>
//        /// Altera os atributos de uma account através do endereço hex da carteira
//        /// </summary>
//        /// <param name="address">Endereço da wallet vinculada</param>
//        /// <param name="account">Objeto da account atualizado</param>
//        /// <response code="200">Account alterada com êxito</response>
//        /// <response code="404">Account não encontrada</response>
//        [HttpPut("{address}")]
//        public async Task<ActionResult> EditAccount([FromRoute] string address, [FromBody] AccountInputModel account)
//        {
//            try
//            {
//                await _accountService.editAccount(address, account);
//                return Ok();
//            }
//            catch (Exception e)
//            {
//                return NotFound(e.Message);
//            }
//        }

//        /// <summary>
//        /// Altera os atributos de uma account através do endereço hex da carteira
//        /// </summary>
//        /// <param name="id">Id vinculado no banco de dados</param>
//        /// <param name="account">Objeto da account atualizado</param>
//        /// <response code="200">Account alterada com êxito</response>
//        /// <response code="404">Account não encontrada</response>
//        [HttpPut("{id:int}")]
//        public async Task<ActionResult> EditAccount([FromRoute] int id, [FromBody] AccountInputModel account)
//        {
//            try
//            {
//                await _accountService.editAccount(id, account);
//                return Ok();
//            }
//            catch (Exception e)
//            {
//                return NotFound(e.Message);
//            }
//        }

//        /// <summary>
//        /// Altera o valor do atributo ban de uma account.
//        /// </summary>
//        /// <remarks>Também pode ser usado para desbanir accounts enviando o valor true apara o parâmetro "ban"
//        /// </remarks>
//        /// <param name="address">Endereço da wallet vinculada</param>
//        /// <param name="ban">Ban = true: conta banida.
//        /// Ban = false: conta ativa. (default)</param>
//        /// <response code="200">Ban aplicado com êxito</response>
//        /// <response code="404">Account não encontrada</response>
//        [HttpPatch("{address}/ban/{ban:bool}")]
//        public async Task<ActionResult> BanAccount(string address, bool ban)
//        {
//            try
//            {
//                await _accountService.banAccount(address, ban);
//                return Ok();
//            }
//            catch (Exception e)
//            {
//                return NotFound(e.Message);
//            }
//        }

//        /// <summary>
//        /// Altera o valor do atributo ban de uma account.
//        /// </summary>
//        /// <remarks>Também pode ser usado para desbanir accounts enviando o valor true apara o parâmetro "ban"
//        /// </remarks>
//        /// <param name="id">Id vinculado no banco de dados</param>
//        /// <param name="ban">Ban = true: conta banida.
//        /// Ban = false: conta ativa. (default)</param>
//        /// <response code="200">Ban aplicado com êxito</response>
//        /// <response code="404">Account não encontrada</response>
//        [HttpPatch("{id:int}/ban/{ban:bool}")]
//        public async Task<ActionResult> BanAccount(int id, bool ban)
//        {
//            try
//            {
//                await _accountService.banAccount(id, ban);
//                return Ok();
//            }
//            catch (Exception e)
//            {
//                return NotFound(e.Message);
//            }
//        }

//        /// <summary>
//        /// Deleta uma account no banco de dados através do endereço hex da wallet.
//        /// </summary>
//        /// <remarks>Foi criado só pra casos de extrema necessidade. Cuidado na sua utilização. </remarks>
//        /// <param name="address">Endereço da wallet vinculada</param>
//        /// <response code="200">Account deletada com êxito</response>
//        /// <response code="404">Account não encontrada</response>
//        [HttpDelete("{address}")]
//        public async Task<ActionResult> DeleteAccount(string address)
//        {
//            try
//            {
//                await _accountService.deleteAccount(address);
//                return Ok();
//            }
//            catch (Exception e)
//            {
//                return NotFound(e.Message);
//            }
//        }

//        /// <summary>
//        /// Deleta uma account no banco de dados através do id.
//        /// </summary>
//        /// <remarks>Foi criado só pra casos de extrema necessidade. Cuidado na sua utilização. </remarks>
//        /// <param name="id">Id vinculado no banco de dados</param>
//        /// <response code="200">Account deletada com êxito</response>
//        /// <response code="404">Account não encontrada</response>
//        [HttpDelete("{id:int}")]
//        public async Task<ActionResult> DeleteAccount(int id)
//        {
//            try
//            {
//                await _accountService.deleteAccount(id);
//                return Ok();
//            }
//            catch (Exception e)
//            {
//                return NotFound(e.Message);
//            }
//        }
//    }
//}
