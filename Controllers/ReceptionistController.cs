using frontend_task_2.Models;
using frontend_task_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontend_task_2.Controllers
{
    [Authorize(Roles = "receptionist")]
    public class ReceptionistController : Controller
    {
    
        
       private readonly ApiService _apiService;

        public ReceptionistController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string date)
        {
            //  (If no date given, take today's)
      var selectedDate = string.IsNullOrEmpty(date) ? DateTime.Today.ToString("yyyy-MM-dd") : date;
        ViewBag.SelectedDate = selectedDate;

            var queue = await _apiService.GetAsync<List<QueueEntry>>($"/queue?date={selectedDate}");
            
            return View(queue ?? new List<QueueEntry>());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status, string date)
        {
            //  (To update queue status)
       var request = new QueueUpdateRequest { Status = status };
        
            
            
            var (success, error) = await _apiService.PatchAsync($"/queue/{id}", request);

            if (!success && !string.IsNullOrEmpty(error))
            {
                TempData["Error"] = error;
            }

            return RedirectToAction("Index", new { date });
        }
    }
}
