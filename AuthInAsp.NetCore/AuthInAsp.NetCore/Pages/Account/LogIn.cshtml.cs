using AuthInAsp.NetCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthInAsp.NetCore.Pages.Account
{
    public class LogInModel : PageModel
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public LogInModel(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [BindProperty]
        public LoginViewModel loginViewModel { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await signInManager.SignOutAsync();
            TempData["message"] = "You Have been  logged Out!";
            return RedirectToPage("/Index");
        }

       
        public  async  Task<IActionResult> OnPostLogin()
        {
           if(ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    TempData["message"] = "You Have Succefully loged In!";
                    return RedirectToPage("/index");
                }
                 ModelState.AddModelError(string.Empty, "Invalid Login Attempts");
            }
            return Page();
        }
    }
}
