using frontend_task_2.Models;
using frontend_task_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace frontend_task_2.Controllers
{
    [Authorize(Roles = "doctor")]
    public class DoctorController : Controller
    {
        private readonly ApiService _apiService;

        public DoctorController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            //
            // Today's queue for Dr. sir)
           
            
     var queue = await _apiService.GetAsync<List<DoctorQueueItem>>("/doctor/queue");
            return View(queue ?? new List<DoctorQueueItem>());
        }

        [HttpGet]
        public IActionResult AddPrescription(int appointmentId)
        {
            ViewBag.AppointmentId = appointmentId;
        var model = new AddPrescriptionRequest
            {
                Medicines = new List<PrescriptionMedicine> { new PrescriptionMedicine() }
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription(int appointmentId, AddPrescriptionRequest model)
        {
            //(Remove empty medicines logically)
            model.Medicines.RemoveAll(m => string.IsNullOrWhiteSpace(m.Name));

    var (response, error) = await _apiService.PostAsync<AddPrescriptionRequest, object>($"/prescriptions/{appointmentId}", model);

            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
     ViewBag.AppointmentId = appointmentId;
         if(model.Medicines.Count == 0) model.Medicines.Add(new PrescriptionMedicine());
                return View(model);
            }

         return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddReport(int appointmentId)
        {
      ViewBag.AppointmentId = appointmentId;
            return View(new AddReportRequest());
        }

        [HttpPost]
        public async Task<IActionResult> AddReport(int appointmentId, AddReportRequest model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AppointmentId = appointmentId;
                return View(model);
            }

            //  (To add report)
            var (response, error) = await _apiService.PostAsync<AddReportRequest, object>($"/reports/{appointmentId}", model);

            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
          ViewBag.AppointmentId = appointmentId;
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}
