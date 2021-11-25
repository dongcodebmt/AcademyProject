using AcademyProject.Models;
using AcademyProject.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace AcademyProject.Controllers
{
    public class SharedComponent
    {
        private readonly IConfiguration configuration;
        private readonly IGenericService<UserRole> userRoleService;
        private readonly IGenericService<Picture> pictureService;
        public SharedComponent(IConfiguration configuration, IGenericService<UserRole> userRoleService, IGenericService<Picture> pictureService)
        {
            this.configuration = configuration;
            this.userRoleService = userRoleService;
            this.pictureService = pictureService;
        }

        public async Task<string> GetImageAsync(int? id)
        {
            string pic = "/";
            if (id == null)
            {
                return pic;
            }
            var picture = await pictureService.GetById((int)id);
            if (picture == null)
            {
                return pic;
            }
            if (picture.PicturePath != "/" && picture.PicturePath.Substring(0, 1) == "/")
            {
                pic = configuration["ServerHostName"] + picture.PicturePath;
            }
            else
            {
                pic = picture.PicturePath;
            }
            return pic;
        }

        public async Task<bool> HasRoleAsync(int id, int[] list)
        {
            var roles = await userRoleService.GetList(x => x.UserId == id);
            foreach (var item in list)
            {
                bool check = roles.Any(x => x.RoleId == item);
                if (check)
                {
                    return true;
                }
            }
            return false;
        }

        public string Truncate(string value, int maxLength)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
            {
                return value.Substring(0, maxLength);
            }

            return value;
        }

        public string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public bool SendMail(string to, string subject, string body)
        {
            try
            {
                string host = configuration["SMTPConfig:Host"];
                int port = Convert.ToInt32(configuration["SMTPConfig:Port"]);
                string user = configuration["SMTPConfig:Username"];
                string pass = configuration["SMTPConfig:Password"];
                string from = configuration["SMTPConfig:Email"];
                string name = configuration["SMTPConfig:Name"];

                SmtpClient smtpClient = new SmtpClient(host, port);

                smtpClient.Credentials = new NetworkCredential(user, pass);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                MailMessage message = new MailMessage();
                message.From = new MailAddress(from, name);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;

                smtpClient.Send(message);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
