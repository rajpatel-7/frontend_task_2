using frontend_task_2.Models;
using frontend_task_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontend_task_2.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ApiService _apiService;
              

        public AdminController(ApiService apiService)
        {
            
            _apiService = apiService;
        }

        //admin dashboard
        public async Task<IActionResult> Index()
        {


            
    var clinicInfo = await _apiService.GetAsync<ClinicInfo>("/admin/clinic");
            var users = await _apiService.GetAsync<List<ClinicUser>>("/admin/users");

            ViewBag.Clinic = clinicInfo;
     // Sending users list
            return View(users ?? new List<ClinicUser>());
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
      return View(new AdminCreateUserRequest());
        }

        [HttpPost]
                        public async Task<IActionResult> CreateUser(AdminCreateUserRequest model)
        {
            if (!ModelState.IsValid)
            {
                      return View(model);
            }

                   var (response, error) = await _apiService.PostAsync<AdminCreateUserRequest, object>("/admin/users", model);

            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
                return View(model);
            }

            // Go back to dash after creating new user
                         return RedirectToAction("Index");
        }
    }
}
