using System.ComponentModel.DataAnnotations;

namespace HairSalon.ModelViews.AppointmentModelViews
{
    public class CreateAppointmentModelView
    {
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "StylistId is required.")]
        public string StylistId { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string StatusForAppointment { get; set; }

        [Required(ErrorMessage = "PointsEarned is required.")]
        public int PointsEarned { get; set; }

        // [Required(ErrorMessage = "AppointmentDate is required.")]
        // public DateTime AppointmentDate { get; set; }
    }
}
