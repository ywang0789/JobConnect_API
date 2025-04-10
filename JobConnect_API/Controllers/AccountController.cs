using JobConnect_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobConnect_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;

        public AccountController(UserManager<Account> userManager, SignInManager<Account> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Register a new account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            // get params from dto
            var account = new Account
            {
                UserName = model.Email,
                Email = model.Email,
                first_name = model.FirstName,
                last_name = model.LastName,
                role = model.Role
            };

            // create account
            var result = await _userManager.CreateAsync(account, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // add account to role 
            await _userManager.AddToRoleAsync(account, model.Role);

            // signin
            await _signInManager.SignInAsync(account, isPersistent: false);

            return Ok(new { message = "Register success" });
        }

        /// <summary>
        /// Login a account with email and password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            // sign in
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized("Login fail");
            }

            return Ok(new { message = "Login success" });
        }

        /// <summary>
        /// Logout the current logged-in account.s
        /// </summary>
        /// <returns></returns>
        // POST: api/account/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logout success" });
        }

        /// <summary>
        /// Delete the current logged-in account's account.
        /// </summary>
        /// <returns></returns>
        // DELETE: api/account/delete
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount()
        {
            // find account
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountId))
            {
                return BadRequest(new { message = "Account Id is null or empty." });
            }
            var account = await _userManager.FindByIdAsync(accountId);

            if (account == null)
            {
                return NotFound(new { message = "Account not found" });
            }

            // sign out first
            await _signInManager.SignOutAsync();

            // del
            var result = await _userManager.DeleteAsync(account);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "Delete account success" });
        }

        /// <summary>
        /// Get the current logged-in account's details.
        /// </summary>
        /// <returns></returns>
        // GET: api/account/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentAccount()
        {
            // find account
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(accountId))
            {
                return BadRequest(new { message = "Account Id is null or empty." });
            }
            var account = await _userManager.FindByIdAsync(accountId);

            if (account == null)
            {
                return NotFound(new { message = "Account not found" });
            }

            return Ok(new
            {
                account.Id,
                account.Email,
                account.first_name,
                account.last_name,
                account.role
            });
        }
    }
}
