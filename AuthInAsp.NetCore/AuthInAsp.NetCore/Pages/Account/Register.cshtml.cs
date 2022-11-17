using AuthInAsp.NetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web.WebPages.Html;

namespace AuthInAsp.NetCore.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInmanager;
        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInmanager)
        {
            _userManager = userManager;
            _signInmanager = signInmanager;
        }

        [BindProperty]
        public RegisterViewModel UserRegister { get; set;}
        public void OnGet()
        {
            UserRegister=new RegisterViewModel();
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = UserRegister.Email, Email = UserRegister.Email };
                var result=await _userManager.CreateAsync(user,UserRegister.Password);
                if (result.Succeeded)
                {
                   await _signInmanager.SignInAsync(user, isPersistent: false);
                    TempData["message"] = $"You have been registred succefully {user.UserName}!";
                    return RedirectToPage("/Index");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
           
        }
    }
}
