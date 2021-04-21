using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SVCA.Data;
using SVCA.Models.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
namespace SVCA.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ChangePasswordModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100, MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña actual")]
            public string Password { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Nueva contraseña")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contraseña")]
            [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task<IActionResult> OnGetAsync()
        {
            if (!await _context.Users.AnyAsync(x => x.Id.Equals(new Guid(_userManager.GetUserId(User)))))
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["StatusMessage"] = "Error. Favor de verificar los datos. La contraseña debe de tener cómo mínimo un número, al menos 4 caracteres diferentes y una longitud mínima de 8.";
                return Page();
            }

            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));

            var result = await _userManager.ChangePasswordAsync(user, Input.Password, Input.NewPassword);

            if (result.Succeeded)
            {
                ViewData["StatusMessage"] = "Se han actualizado los datos satisfactoriamente.";

                return Page();
            }

            // If we got this far, something failed, redisplay form
            ViewData["StatusMessage"] = "Error. Favor de verificar los datos. La contraseña debe de tener cómo mínimo un número, al menos 4 caracteres diferentes y una longitud mínima de 8.";
            return Page();
        }
    }
}
