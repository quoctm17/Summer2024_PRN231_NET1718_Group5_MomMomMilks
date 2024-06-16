namespace MomMomMilks.EmailService
{
	public interface IEmailService
	{
		Task<string> SendAsync(string from, string to, string subject, string body);
		Task<bool> ValidateEmail(string email);
	}
}
