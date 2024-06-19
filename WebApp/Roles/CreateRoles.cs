using Microsoft.AspNetCore.Identity;
using WebApp.Models;

namespace WebApp.Roles;

public static class CreateRoles
{
    
    public static async Task CreateCaesarAdmin(IServiceProvider serviceProvider, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
    
        string caesarRoleName = "admin_julius_caesar";
        IdentityRole caesarRole = await roleManager.FindByNameAsync(caesarRoleName);
        
        if (caesarRole == null)
        {
            await roleManager.CreateAsync(new IdentityRole(caesarRoleName));
        }
        
        string caesarEmailName = "julius.caesar@empire.spqr";
        AppUser caesarEmail = await userManager.FindByNameAsync(caesarEmailName);
        
        if (caesarEmail == null)
        {
            caesarEmail = new AppUser { UserName = caesarEmailName, Email = caesarEmailName };
            var res = await userManager.CreateAsync(caesarEmail, "Veni,Vidi,Vici!1");
            if (res.Succeeded)
            {
                Console.WriteLine(">>>>>>>>Caesar Admin is created");            }
            else
            {
                Console.WriteLine(res.ToString());
            }
            
            var idres = await userManager.AddToRoleAsync(caesarEmail, caesarEmailName);
            if (idres.Succeeded)
            {
                Console.WriteLine(">>>>>>>>Caesar Admin Role is applied");            }
            else
            {
                Console.WriteLine(idres.ToString());
            }
            
        }
    }
    
    
    public static async Task CreateVigenereAdmin(IServiceProvider serviceProvider, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
    
        string vigenereRoleName = "admin_blaise_de_vigenere";
        IdentityRole vigenereRole = await roleManager.FindByNameAsync(vigenereRoleName);
        
        if (vigenereRole == null)
        {
            await roleManager.CreateAsync(new IdentityRole(vigenereRoleName));
        }
        
        string vigenereEmailName = "blaise.vigenere@kingdom.fr";
        AppUser vigenereEmail = await userManager.FindByNameAsync(vigenereEmailName);
        
        if (vigenereEmail == null)
        {
            vigenereEmail = new AppUser { UserName = vigenereEmailName, Email = vigenereEmailName };
            var res = await userManager.CreateAsync(vigenereEmail, "Montjoie_Saint_Denis!1");
            if (res.Succeeded)
            {
                Console.WriteLine(">>>>>>>>Vigenere Admin is created");            }
            else
            {
                Console.WriteLine(res.ToString());
            }
            
            var idres = await userManager.AddToRoleAsync(vigenereEmail, vigenereRoleName);
            if (idres.Succeeded)
            {
                Console.WriteLine(">>>>>>>>Vigenere Admin Role is applied");            }
            else
            {
                Console.WriteLine(idres.ToString());
            }
            
        }
    }
}
