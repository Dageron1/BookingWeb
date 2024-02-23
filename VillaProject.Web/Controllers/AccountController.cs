using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Common.Utility;
using VillaProject.Domain.Entities;
using VillaProject.Web.Models.ViewModels;

namespace VillaProject.Web.Controllers
{
    public class AccountController : Controller
    {
        //не буду добавлять в сервис т к весь контроллер уже и так выглядит как реализация сервиса. И создавать доп обертку нет смысла.
        private readonly UserManager<ApplicationUser> _userManager; //used to manage the users
        //that way we do not have to manage cookies or anything.
        //we will directly call the helper methods in sign in manager.
        private readonly SignInManager<ApplicationUser> _signInManager; //responsible for logging in a user and related operations.
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login(string returnUrl = null) //if not defined = null
        {
            //null coalescing operator
            //получает ссылку только если переадресация идет от вкладки с закрытым доступом без авторизации.
            //например при доступе к Amenity, нужна авторизация. Тут и получается эта ссылка returnUrl
            returnUrl ??= Url.Content("~/"); //if returnUrl is not empty, then it populate that with the content

            LoginVM loginVM = new()
            {
                RedirectUrl = returnUrl
            };
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(loginVM.Email);

                    if(await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            //LocalRedirect безопаснее чем RedirectToPage, он перенаправляет только на точно такой же домен.
                            //additional security measure
                            //примером RedirectUrl может быть /Amenity(по дефолту будет тут дальше Index который не пишется) or /Amenity/Create
                            return LocalRedirect(loginVM.RedirectUrl);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View(loginVM);
        }

        public async Task<IActionResult> Logout() //if not defined = null
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        //этот метод заранее прописан в cookie поэтому он сам вызывается, без участия.
        //и identity идещет AccountController и метод AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Register(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            

            RegisterVM registerVM = new()
            {
                //projections
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }),
                RedirectUrl = returnUrl
            };

            return View(registerVM);

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    Name = registerVM.Name,
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    EmailConfirmed = true,
                    UserName = registerVM.Email,
                    CreatedAt = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password); //GetAwaiter bacause CreateAsync is asycn method

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(registerVM.Role))
                    {
                        await _userManager.AddToRoleAsync(user, registerVM.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    }

                    //роль автоматом считывает EF из таблицы ролей.
                    await _signInManager.SignInAsync(user, isPersistent: false); //automatically sign in a user

                    if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        //LocalRedirect безопаснее чем RedirectToPage, он перенаправляет только на точно такой же домен.
                        //additional security measure
                        return LocalRedirect(registerVM.RedirectUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    //ключ не указываю чтобы отображать все ошибки через модель в Register.cshtml
                    ModelState.AddModelError("", error.Description);
                }
            }

            registerVM.RoleList = _roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            });
         
            /////

            return View(registerVM);
        }
    }
}
