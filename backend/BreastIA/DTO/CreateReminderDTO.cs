namespace BreastIA.DTOs
{
    public class CreateReminderDTO
    {


        public int UserId { get; set; }

        // Título del recordatorio

        public string Title { get; set; }

        // Fecha del recordatorio
        public DateTime DateReminders { get; set; }
    }
}
