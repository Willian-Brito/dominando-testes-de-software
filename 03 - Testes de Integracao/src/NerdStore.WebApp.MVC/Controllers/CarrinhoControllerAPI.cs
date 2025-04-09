

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Queries;
using NerdStore.WebApp.MVC.Models;

namespace NerdStore.WebApp.MVC.Controllers;

[Authorize]
public class CarrinhoControllerAPI : ControllerBase
{
    private readonly IProdutoService _produtoService;
    private readonly IPedidoQueries _pedidoQueries;
    private readonly IMediator _mediatorHandler;

    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly StartupApiTests.AppSettings _appSettings;

    public CarrinhoControllerAPI(
        INotificationHandler<DomainNotification> notifications,
        IProdutoService produtoService,
        IMediator mediatorHandler, 
        IPedidoQueries pedidoQueries,
        IHttpContextAccessor httpContextAccessor, 
        SignInManager<IdentityUser> signInManager, 
        UserManager<IdentityUser> userManager,
        IOptions<StartupApiTests.AppSettings> appSettings
    ) : base(notifications, mediatorHandler, httpContextAccessor)
    {
        _produtoService = produtoService;
        _mediatorHandler = mediatorHandler;
        _pedidoQueries = pedidoQueries;
        _signInManager = signInManager;
        _userManager = userManager;
        _appSettings = appSettings.Value;
    }

    [HttpGet]
    [Route("api/carrinho")]
    public async Task<IActionResult> Get()
    {
        return Response(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
    }

    [HttpPost]
    [Route("api/carrinho")]
    public async Task<IActionResult> Post([FromBody] ItemViewModel item)
    {
        var produto = await _produtoService.ObterPorId(item.Id);
        if (produto == null) return BadRequest();

        if (produto.QuantidadeEstoque < item.Quantidade)
        {
            NotificarErro("ErroValidacao","Produto com estoque insuficiente");
        }

        var command = new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome, item.Quantidade, produto.Valor);
        await _mediatorHandler.Send(command);

        return Response();
    }

    [HttpPut]
    [Route("api/carrinho/{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] ItemViewModel item)
    {
        var produto = await _produtoService.ObterPorId(id);
        if (produto == null) return BadRequest();

        var command = new AtualizarItemPedidoCommand(ClienteId, produto.Id, item.Quantidade);
        await _mediatorHandler.Send(command);

        return Response();
    }

    [HttpDelete]
    [Route("api/carrinho/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var produto = await _produtoService.ObterPorId(id);
        if (produto == null) return BadRequest();

        var command = new RemoverItemPedidoCommand(ClienteId, id);
        await _mediatorHandler.Send(command);
        
        return Response();
    }

    [AllowAnonymous]
    [HttpPost("api/login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel login)
    {
        var result = await _signInManager.PasswordSignInAsync(login.Email, login.Senha, false, true);

        if (result.Succeeded)
        {
            var token = await GerarJwt(login.Email);
            return Ok(token);
        }

        NotificarErro("login","Usuário ou Senha incorretos");
        return Response();
    }

    private async Task<string> GerarJwt(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _appSettings.Emissor,
            Audience = _appSettings.ValidoEm,
            NotBefore = DateTime.Now,
            Subject = identityClaims,
            Expires = DateTime.Now.AddHours(_appSettings.ExpiracaoHoras),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        });

        var tokenResult = tokenHandler.WriteToken(token);
        return tokenResult;
    }
}