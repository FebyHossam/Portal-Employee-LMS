namespace Leave_Mangement_System.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine("=== EMAIL SENT ===");
            Console.WriteLine($"To: {email}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body: {htmlMessage}");
            Console.WriteLine("==================");

            // TODO: Implement actual email sending using SMTP
            // var smtpClient = new SmtpClient("smtp.gmail.com")
            // {
            //     Port = 587,
            //     Credentials = new NetworkCredential("your-email@gmail.com", "your-password"),
            //     EnableSsl = true,
            // };

            await Task.CompletedTask;
        }
    }
}   


