using Company.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Company.PL.Helper
{
	public class EmailSettings
	{
		public static void SendEmail(Email email) 
		{
			// hostname - port - Email_Sender, password- protocol_used
			// this service used to send link in mail
			// to send author use mailkit

			//var client = new SmtpClient(hostname, port);


			var client = new SmtpClient("smtp.gmail.com",587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("ozombedir@gmail.com", "kxilqbpgyyrejsvz"); //app password
			client.Send("ozombedir@gmail.com", email.To, email.Subject, email.Body);   		
		}
	}
}
