using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace frontend_task_2.Models
{
    //  (Patient appointment data)
    public class Appointment
    {
        public int Id { get; set; }
        public string AppointmentDate { get; set; } = string.Empty;
     public string TimeSlot { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
     public int PatientId { get; set; }
     public int ClinicId { get; set; }
        public DateTime CreatedAt { get; set; }
  public QueueEntry? QueueEntry { get; set; }
        public Prescription? Prescription { get; set; }
       public Report? Report { get; set; }
    }

    // (To book new appt)
    public class BookAppointmentRequest
    {
        [Required(ErrorMessage = "Appointment Date is required")]
     [JsonPropertyName("appointmentDate")]
        public string AppointmentDate { get; set; } = string.Empty; // YYYY-MM-DD
        
        [Required(ErrorMessage = "Time Slot is required")]
     [JsonPropertyName("timeSlot")]
        public string TimeSlot { get; set; } = string.Empty;
    }

    public class Prescription
    {
        public int Id { get; set; }
    public List<PrescriptionMedicine> Medicines { get; set; } = new List<PrescriptionMedicine>();
        public string Notes { get; set; } = string.Empty;
    }

    public class PrescriptionMedicine
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
    [JsonPropertyName("dosage")]
        public string Dosage { get; set; } = string.Empty;
        
        [JsonPropertyName("duration")]
        public string Duration { get; set; } = string.Empty;
    }

    public class AddPrescriptionRequest
    {
        [Required(ErrorMessage = "Medicines are required")]
    
        
   [JsonPropertyName("medicines")]
        public List<PrescriptionMedicine> Medicines { get; set; } = new List<PrescriptionMedicine>();
        
        [JsonPropertyName("notes")]
        public string Notes { get; set; } = string.Empty;
    }

    public class Report
    {
        public int Id { get; set; }
       
        
     public string Diagnosis { get; set; } = string.Empty;
        public string TestRecommended { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }

    public class AddReportRequest
    {
       
        
        //  (Doctor's report)
        [Required(ErrorMessage = "Diagnosis is required")]
      [JsonPropertyName("diagnosis")]
    public string Diagnosis { get; set; } = string.Empty;
     
        [Required(ErrorMessage = "Test Recommendation is required")]
        [JsonPropertyName("testRecommended")]
        public string TestRecommended { get; set; } = string.Empty;
     
   [Required(ErrorMessage = "Remarks are required")]
        [JsonPropertyName("remarks")]
        public string Remarks { get; set; } = string.Empty;
    }
}
