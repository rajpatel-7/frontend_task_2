using System.Text.Json.Serialization;

namespace frontend_task_2.Models
{
    // (Queue details)
    public class QueueEntry
    {
        public int Id { get; set; }
   
    public int TokenNumber { get; set; }
        public string Status { get; set; } = string.Empty; // waiting, in-progress, done, skipped
    public string QueueDate { get; set; } = string.Empty;
        public int AppointmentId { get; set; }
        public AppointmentDto? Appointment { get; set; }
    }

    public class AppointmentDto
    {
  public PatientDto? Patient { get; set; }
    }

    public class PatientDto
    {
       
        
     public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class QueueUpdateRequest
    {
                    
        
     [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }

    public class DoctorQueueItem
    {
        // (Today's queue for doctor)
        public int Id { get; set; }
    
    public int TokenNumber { get; set; }
      
 public string Status { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
    public int PatientId { get; set; }
        public int AppointmentId { get; set; }
    }
}
