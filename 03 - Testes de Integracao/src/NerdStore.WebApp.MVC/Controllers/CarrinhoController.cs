using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Queries;

namespace NerdStore.WebApp.MVC.Controllers;

[Authorize]
public class CarrinhoController : ControllerBase
{
    private readonly IProdutoService _produtoService;
    private readonly IPedidoQueries _pedidoQueries;
    private readonly IMediator _mediatorHandler;

    public CarrinhoController(
        INotificationHandler<DomainNotification> notifications,
        IProdutoService produtoService,
        IMediator mediatorHandler,
        IPedidoQueries pedidoQueries,
        IHttpContextAccessor httpContextAccessor
    ) : base(notifications, mediatorHandler, httpContextAccessor)
    {
        _produtoService = produtoService;
        _mediatorHandler = mediatorHandler;
        _pedidoQueries = pedidoQueries;
    }

    [HttpGet]
    [Route("meu-carrinho")]
    public async Task<IActionResult> Index()
    {
        return View(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
    }

    [HttpPost]
    [Route("meu-carrinho")]
    public async Task<IActionResult> AdicionarItem(Guid id, int quantidade)
    {
        try
        {
            var produto = await _produtoService.ObterPorId(id);
            if (produto == null) return BadRequest();

            if (produto.QuantidadeEstoque < quantidade)
            {
                TempData["Erro"] = "Produto com estoque insuficiente";
                return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
            }

            var command =
                new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome, quantidade, produto.Valor);
            await _mediatorHandler.Send(command);
            
            if (OperacaoValida())
            {
                return RedirectToAction("Index");
            }

            TempData["Erros"] = ObterMensagensErro();
            return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
        }
        catch (Exception e)
        {
            TempData["Erro"] = e.Message;
            return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
        }
    }

    [HttpPost]
    [Route("remover-item")]
    public async Task<IActionResult> RemoverItem(Guid id)
    {
        var produto = await _produtoService.ObterPorId(id);
        if (produto == null) return BadRequest();

        var command = new RemoverItemPedidoCommand(ClienteId, id);
        await _mediatorHandler.Send(command);

        if (OperacaoValida())
        {
            return RedirectToAction("Index");
        }

        return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
    }

    [HttpPost]
    [Route("atualizar-item")]
    public async Task<IActionResult> AtualizarItem(Guid id, int quantidade)
    {
        var produto = await _produtoService.ObterPorId(id);
        if (produto == null) return BadRequest();

        var command = new AtualizarItemPedidoCommand(ClienteId, id, quantidade);
        await _mediatorHandler.Send(command);

        if (OperacaoValida())
        {
            return RedirectToAction("Index");
        }

        return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
    }

    [HttpPost]
    [Route("aplicar-voucher")]
    public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
    {
        var command = new AplicarVoucherPedidoCommand(ClienteId, voucherCodigo);
        await _mediatorHandler.Send(command);

        if (OperacaoValida())
        {
            return RedirectToAction("Index");
        }

        return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
    }

    [HttpGet]
    [Route("resumo-da-compra")]
    public async Task<IActionResult> ResumoDaCompra()
    {
        return View(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
    }
}