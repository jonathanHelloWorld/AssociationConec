/*using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
 
public class Send_Email : MonoBehaviour {

    public static void SendCompleteCallBack(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {

    }
    
    public void Send (string attachmentPath, string emailTo)
    {
        /*MailMessage mail = new MailMessage();
 
		mail.From = new MailAddress("renaultreileao@gmail.com");
        mail.To.Add(emailTo);
		mail.Bcc.Add("renault@interativaevents.com.br");
        mail.Subject = "Estive no Teatro Renault";
        mail.Body = "#RENAULTNOREILEÃO";
		
	// Create  the file attachment for this e-mail message.
        string fileName = attachmentPath;
		Attachment data = new System.Net.Mail.Attachment(fileName, MediaTypeNames.Application.Octet);
		// Add time stamp information for the file.
		ContentDisposition disposition = data.ContentDisposition;
        disposition.CreationDate = System.IO.File.GetCreationTime(fileName);
        disposition.ModificationDate = System.IO.File.GetLastWriteTime(fileName);
        disposition.ReadDate = System.IO.File.GetLastAccessTime(fileName);
		// Add the file attachment to this e-mail message.
		mail.Attachments.Add(data);
 		

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

        smtpServer.Port = 587;
        smtpServer.UseDefaultCredentials = false;
		smtpServer.Credentials = new System.Net.NetworkCredential("renaultreileao@gmail.com", "2re4na0ul2T") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = 
            delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
                { return true; };

        smtpServer.SendCompleted += new SendCompletedEventHandler(SendCompleteCallBack);
        string userState = "teste";
        smtpServer.SendAsync(mail, userState);
        //smtpServer.Send(mail);
        //Debug.Log("success");
        //

		MailMessage mail = new MailMessage();

        mail.From = new MailAddress("comercial@interativaevents.com.br");
		mail.To.Add(emailTo);
		mail.Subject = "Interativa Brasil Promotion 2014";
		mail.Body = "Obrigado por participar da interatividade\n\n Conheça mais em: www.interativaevents.com.br";
		
		// Create  the file attachment for this e-mail message.
		string fileName = attachmentPath;
		Attachment data = new System.Net.Mail.Attachment(fileName, MediaTypeNames.Application.Octet);
		// Add time stamp information for the file.
		ContentDisposition disposition = data.ContentDisposition;
		disposition.CreationDate = System.IO.File.GetCreationTime(fileName);
		disposition.ModificationDate = System.IO.File.GetLastWriteTime(fileName);
		disposition.ReadDate = System.IO.File.GetLastAccessTime(fileName);
		// Add the file attachment to this e-mail message.
		mail.Attachments.Add(data);

        SmtpClient smtpServer = new SmtpClient("smtp.interativaevents.com.br");
		
		smtpServer.Port = 587;
		smtpServer.UseDefaultCredentials = false;
        smtpServer.Credentials = new System.Net.NetworkCredential("comercial@interativaevents.com.br", "int3r4t1v42o14") as ICredentialsByHost;
		smtpServer.EnableSsl = false;
		ServicePointManager.ServerCertificateValidationCallback = 
			delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
		{ return true; };
		
		smtpServer.SendCompleted += new SendCompletedEventHandler(SendCompleteCallBack);
		string userState = "teste";
		smtpServer.SendAsync(mail, userState);
		//smtpServer.Send(mail);
		//Debug.Log("success");
    }
}
*/