using frontend_task_2.Models;
using frontend_task_2.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace frontend_task_2.Controllers
{
    public class AuthController : Controller
    {
               private readonly ApiService _apiService;

        public AuthController(ApiService apiService)
        {
                  _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, let them inside
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToRoleDashboard(User.FindFirst(ClaimTypes.Role)?.Value);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Sending data to API for login
            var (response, error) = await _apiService.PostAsync<LoginRequest, LoginResponse>("/auth/login", model);

            if (!string.IsNullOrEmpty(error) || response == null)
            {
                        ViewBag.Error = string.IsNullOrEmpty(error) ? "Login failed" : error;
                return View(model);
            }

            // (Make cookie when login successful)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, response.User.Id.ToString()),
                    new Claim(ClaimTypes.Name, response.User.Name),

                new Claim(ClaimTypes.Email, response.User.Email),


                   new Claim(ClaimTypes.Role, response.User.Role),
                   new Claim("Token", response.Token),



                  new Claim("ClinicId", response.User.ClinicId?.ToString() ?? ""),

                new Claim("ClinicName", response.User.ClinicName ?? "")
            };

                 var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
               await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToRoleDashboard(response.User.Role);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private IActionResult RedirectToRoleDashboard(string? role)
        {
            //  (Function to send to diff pages based on role)
            return role?.ToLower() switch
            {
                "admin" => RedirectToAction("Index", "Admin"),
                       "receptionist" => RedirectToAction("Index", "Receptionist"),
                       "doctor" => RedirectToAction("Index", "Doctor"),
                "patient" => RedirectToAction("Index", "Patient"),
                _ => RedirectToAction("Index", "Home")
            };
        }
    }
}
