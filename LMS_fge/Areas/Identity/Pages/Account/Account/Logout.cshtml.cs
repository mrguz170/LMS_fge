using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SVCA.Models.User;
using System.Threading.Tasks;

namespace SVCA.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        public LogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            HttpContext.Session.Clear();
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await _signInManager.SignOutAsync();
            return LocalRedirect("~/Identity/Account/LogIn");
        }

        public async Task<IActionResult> OnPost()
        {
            HttpContext.Session.Clear();
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            await _signInManager.SignOutAsync();
            return LocalRedirect("~/Identity/Account/LogIn");
        }
    }
}
