using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Service.Services
{
	public class EmailService(IConfiguration config)
	{
		private readonly IConfiguration config = config;

		public async Task SendProposalNotification(string toEmail, string clientName, string jobTitle, string freelancerName)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("SkillBridge", config.GetValue<string>("EmailSettings:UserName")));
			message.To.Add(new MailboxAddress(clientName, toEmail));
			message.Subject = $"New proposal received for: {jobTitle}";
			message.Body = new TextPart("plain")
			{
				Text = $"Hi {clientName},\n\n{freelancerName} submitted a proposal for your job '{jobTitle}'.\n\nLogin to SkillBridge to review it."
			};

			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(
				config.GetValue<string>("EmailSettings:Host"),
				config.GetValue<int>("EmailSettings:Port"),
				false);
			await smtp.AuthenticateAsync(
				config.GetValue<string>("EmailSettings:UserName"),
				config.GetValue<string>("EmailSettings:Password"));
			await smtp.SendAsync(message);
			await smtp.DisconnectAsync(true);
		}
	}
}