using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.api
{
    [Route("api/user")]
    [ApiController]
    public class UsrController : Controller
    {
        private readonly SignInManager<IdentityUser> _signMngr;
        private readonly RoleManager<IdentityRole> _roleMngr;
        private readonly UserManager<IdentityUser> _userMngr;

        public UsrController(SignInManager<IdentityUser> sign, RoleManager<IdentityRole> role, UserManager<IdentityUser> user)
        {
            _roleMngr = role;
            _userMngr = user;
            _signMngr = sign;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated) {
                return View();
            }
            return RedirectToAction(nameof(Index), "H");
        }

        [HttpGet]
        public IActionResult Reg()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reg(string email, string passwordHash, string userName = null)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(passwordHash))
            {
                return BadRequest("Email or password are important ...");
            }
            var newUser = new IdentityUser
            {
                UserName = (userName == null) ? email : userName,
                Email = email,
                EmailConfirmed = true
            };
            var result = await _userMngr.CreateAsync(newUser, passwordHash);
            if (result.Succeeded)
            {
                var res = Log(email, passwordHash);
            }
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet]
        public async Task<IActionResult> Log()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Log(string email, string passwordHash)
        {
            Microsoft.AspNetCore.Identity.SignInResult result;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(passwordHash))
            {
                return BadRequest("Email and password are required.");
            }
            var resEmail = await _userMngr.FindByEmailAsync(email);
            if (resEmail != null)
            {
                result = await _signMngr.PasswordSignInAsync(resEmail, passwordHash, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index), "H");
                }
            }
            else
            {
                result = await _signMngr.PasswordSignInAsync(email, passwordHash, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index), "H");
                }
            }

            return BadRequest("Error auth ...");
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signMngr.SignOutAsync();
            return RedirectToAction("Index", "H");
        }

        [HttpGet]
        public async Task<IActionResult> Mngr()
        {
            List<UsrTbl> u = new List<UsrTbl>();
            foreach (var usr in _userMngr.Users)
            {
                var tmp = new UsrTbl { User = usr, Roles = new List<string>() { "" } };
                foreach (var role in _roleMngr.Roles)
                {
                    foreach (var us in await _userMngr.GetUsersInRoleAsync(role.Name))
                    {
                        if (us == usr)
                        {
                            if (tmp.Roles[0] == "")
                            {
                                tmp.Roles = new List<string>();
                            }
                            tmp.Roles?.Add(role.Name);
                        }
                    }
                }
                u.Add(tmp);
            }
            return View(u);
        }
    }
}
