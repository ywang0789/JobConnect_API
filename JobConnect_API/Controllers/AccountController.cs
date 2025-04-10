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
        /// Register a new user with email and password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new Account
            {
                UserName = model.Email,
                Email = model.Email,
                first_name = model.FirstName,
                last_name = model.LastName,
                role = model.Role
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, model.Role);
            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new { message = "User registered successfully." });
        }

        /// <summary>
        /// Login a user with email and password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid login attempt.");
            }

            return Ok(new { message = "Logged in successfully." });
        }

        /// <summary>
        /// Logout the current logged-in user.s
        /// </summary>
        /// <returns></returns>
        // POST: api/account/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully." });
        }

        /// <summary>
        /// Delete the current logged-in user's account.
        /// </summary>
        /// <returns></returns>
        // DELETE: api/account/delete
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
                
            await _signInManager.SignOutAsync(); 

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
                
            return Ok(new { message = "Account deleted successfully." });
        }

        /// <summary>
        /// Get the current logged-in user's details.
        /// </summary>
        /// <returns></returns>
        // GET: api/account/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            return Ok(new
            {
                user.Id,
                user.Email,
                user.first_name,
                user.last_name,
                user.role
            });
        }
    }
}
