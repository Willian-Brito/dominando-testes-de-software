using Microsoft.AspNetCore.Identity;
using NerdStore.Catalogo.Domain;

namespace NerdStore.WebApp.MVC.Areas.Identity;

public class SeedUserInitial : ISeedUserInitial
{
    private readonly UserManager<IdentityUser> _userManager;

    public SeedUserInitial(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task SeedUser()
    {
        if (_userManager.FindByEmailAsync("teste@teste.com").Result == null)
        {
            var usuario = new IdentityUser();
            
            usuario.UserName = "teste@teste.com";
            usuario.Email = "teste@teste.com";
            usuario.NormalizedUserName = "TESTE@TESTE.COM";
            usuario.NormalizedEmail = "TESTE@TESTE.COM";
            usuario.EmailConfirmed = true;
            usuario.LockoutEnabled = false;
            usuario.SecurityStamp = Guid.NewGuid().ToString();

            await _userManager.CreateAsync(usuario, "#Teste@123");
        }
    }
}