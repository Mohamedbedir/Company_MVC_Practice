using Company.DAL.Models;
using Company.PL.Helper;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager ,RoleManager<IdentityRole> roleManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName=model.Email.Split('@')[0],
                    FName =model.FName,
                    LName =model.Email,
                   
                    IsAgree =model.IsAgree,
                };
                var res=await userManager.CreateAsync(user,model.Password);

                if(res.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                    return RedirectToAction(nameof(Login));
                }

                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

			return View(model);
		}

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
            if (ModelState.IsValid)
            {
                var user =await userManager.FindByEmailAsync(model.Email);
                if(user is not null) 
                {
                    bool flag =await userManager.CheckPasswordAsync(user, model.Password);
                    if(flag)
                    {
                        var res = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if(res.Succeeded)
                        {
                            return RedirectToAction("Index","Home");
                        }
                    }
					ModelState.AddModelError(string.Empty, "invalid Email or Password");
				}
                ModelState.AddModelError(string.Empty, "invalid Email or Password");
                
            }

            return View(model);
		}

        //SignOut
        public new async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        //get
        public IActionResult ForgetPassword()
        {
            return View();
        }

        //post
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user =await userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    //token
                    var Token =await userManager.GeneratePasswordResetTokenAsync(user); //just for one time
                    //link -> https://localhost:44341/Account/ResetPassword/ozom@gmail.com&token=21ads54ad64s
                    var passwordresetlink = Url.Action("ResetPassword", "Account", new { email = user.Email , token = Token }, Request.Scheme);
                    //create Email
                    var email = new Email()
                    {
                        Subject = "Reset Password",
                        To=user.Email,
                        Body= passwordresetlink                            // have a link to reset Password action
					};
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInBox));


                }

                ModelState.AddModelError(string.Empty, "Email not Found");

            }
            return View(model);

        }

		public IActionResult CheckYourInBox()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["Email"] = email;
            TempData["Token"]=token;
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
            {
				string email = TempData["Email"] as string;
				string token = TempData["Token"] as string;
				var user = await userManager.FindByEmailAsync(email);

                var res= await userManager.ResetPasswordAsync(user, token,model.NewPassword); 
                if (res.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }

                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

			return View(model);
		}
	}

}
