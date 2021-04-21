using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SVCA.Data;
using SVCA.Models.User;

namespace SVCA.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public SetPasswordModel(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool Changed { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Este campo es requerido")]
            [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} y máximo {1} caracteres de longitud.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Nueva contraseña")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar la nueva contraseña")]
            [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (await _userManager.IsInRoleAsync(user, "ChangePassword"))
            {
                return Page();
            }

            return LocalRedirect("~/Home/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                foreach (var error in removePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    switch (error.Description)
                    {
                        case "Passwords must have at least one digit ('0'-'9').":
                            ModelState.AddModelError(string.Empty, "La contraseña debe tener al menos 1 dígito (0-9)");
                            break;
                        case "Passwords must use at least 4 different characters.":
                            ModelState.AddModelError(string.Empty, "La contraseña debe tener al menos 4 caracteres diferentes.");
                            break;
                        default:
                            ModelState.AddModelError(string.Empty, error.Description);
                            break;
                    }
                }
                return Page();
            }

            await _userManager.RemoveFromRoleAsync(user, "ChangePassword");

            await _signInManager.RefreshSignInAsync(user);
            Changed = true;
            ViewData["StatusMessage"] = "Se ha actualizado la contraseña.";

            return Page();
        }
    }
}
