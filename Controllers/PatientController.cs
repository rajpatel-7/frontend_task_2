using frontend_task_2.Models;
using frontend_task_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontend_task_2.Controllers
{
    [Authorize(Roles = "patient")]
    public class PatientController : Controller
    {
        private readonly ApiService _apiService;

        public PatientController(ApiService apiService)
        {
            _apiService = apiService;
        }

        //(My all appointments)
        public async Task<IActionResult> Index()
        {
        var appointments = await _apiService.GetAsync<List<Appointment>>("/appointments/my");
            return View(appointments ?? new List<Appointment>());
        }

        [HttpGet]
        public IActionResult BookAppointment()
        {
      var model = new BookAppointmentRequest
            {
                AppointmentDate = DateTime.Today.ToString("yyyy-MM-dd")
            };
            return View(model);
        }

        [HttpPost]
    public async Task<IActionResult> BookAppointment(BookAppointmentRequest model)
        {
            if (!ModelState.IsValid) return View(model);

            // (Data will go to book new appt)
            var (response, error) = await _apiService.PostAsync<BookAppointmentRequest, Appointment>("/appointments", model);

            if (!string.IsNullOrEmpty(error))
            {
          ViewBag.Error = error;
                return View(model);
            }

            return RedirectToAction("Index");
        }

        //  (Full detailed info of appointment)
        public async Task<IActionResult> Details(int id)
        {
     var appointment = await _apiService.GetAsync<Appointment>($"/appointments/{id}");
            if (appointment == null)
            {
          return NotFound();
            }
            return View(appointment);
        }

        public async Task<IActionResult> Prescriptions()
        {
            // (My medicines prescriptions)
    var prescriptions = await _apiService.GetAsync<List<Prescription>>("/prescriptions/my");
            return View(prescriptions ?? new List<Prescription>());
        }

        public async Task<IActionResult> Reports()
        {
             // (View my medical reports)
      var reports = await _apiService.GetAsync<List<Report>>("/reports/my");
            return View(reports ?? new List<Report>());
        }
    }
}
