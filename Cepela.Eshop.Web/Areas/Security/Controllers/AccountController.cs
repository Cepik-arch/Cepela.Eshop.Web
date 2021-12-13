using Cepela.Eshop.Web.Controllers;
using Cepela.Eshop.Web.Models.ApplicationServices.Abstraction;
using Cepela.Eshop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cepela.Eshop.Web.Areas.Security.Controllers
{
    [Area("Security")]
    public class AccountController : Controller
    {
        ISecurityApplicationService security;

        public AccountController(ISecurityApplicationService security)
        {
            this.security = security;
        }
         
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                string [] errors = await security.Register(registerViewModel, Models.Identity.Roles.Customer);
               if(errors != null)
               {
                    LoginViewModel loginViewModel = new LoginViewModel()
                    {
                        Username = registerViewModel.Username,
                        Password = registerViewModel.Password
                    };

                    bool isLogged = await security.Login(loginViewModel);

                    if (isLogged)
                        return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", String.Empty), new { area = String.Empty });
                    else
                        return RedirectToAction(nameof(Login));

                }

            }
            return View(registerViewModel);

        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isLogged = await security.Login(loginViewModel);

                if(isLogged)
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", String.Empty), new { area = String.Empty });

                loginViewModel.LoginFailed = true;


            }
            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await security.Logout();
            return RedirectToAction(nameof(Login));
        }

    }
}
