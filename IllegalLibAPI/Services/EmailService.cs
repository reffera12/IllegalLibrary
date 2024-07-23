// using System.Net;
// using System.Net.Mail;
// using MimeKit;

// namespace IllegalLibAPI.Services
// {
//     public class EmailService : IEmailService
//     {
//         private readonly EmailConfiguration _emailConfiguration;
//         private readonly IConfiguration _configuration;

//         public EmailService(EmailConfiguration emailConfiguration, IConfiguration configuration)
//         {
//             _emailConfiguration = emailConfiguration;
//             _configuration = configuration;
//         }

//         public async Task SendConfirmationEmail(string email, string tokenUrl)
//         {
//             Message emailMessage = new Message(new MailboxAddress("IllegalLib", email), "Confirm  your account", $"Please confirm your account by clicking <a href='{tokenUrl}'>here");

//             await SendEmail(emailMessage);
//         }

//         public async Task SendEmail(Message message)
//         {
//         var emailSettings = _configuration.GetSection("EmailSettings");

//         using (var client = new SmtpClient(emailSettings["Host"]))
//         {
//             client.Port = int.Parse(emailSettings["Port"]);
//             client.UseDefaultCredentials = false;
//             client.Credentials = new NetworkCredential(emailSettings["Username"], emailSettings["Password"]);

//             try
//             {
//                 await client.SendMailAsync(message);
//             }
//             catch (System.Exception)
//             {
                
//                 throw;
//             }
//         }
//         }
//     }
// }