
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace SocialMedia.DTO.Users
{
    public class EmailApiDTO
    {
        [FromQuery(Name ="email")]
        public string Email { get; set; }

        [FromQuery(Name = "validations")]
        public ValidationDetails Validations { get; set; }

        [FromQuery(Name = "score")]
        public int Score { get; set; }

        [FromQuery(Name = "status")]
        public string Status { get; set; }
    }


    public class EmailApiQuery 
    {
        [FromQuery(Name ="email")]
        string Email { get; set; }
    }

    public class ValidationDetails
    {
        [FromQuery(Name = "syntax")]
        public bool Syntax { get; set; }

        [FromQuery(Name = "domain_exists")]
        public bool DomainExists { get; set; }

        [FromQuery(Name = "mx_records")]
        public bool MXRecords { get; set; }

        [FromQuery(Name = "mailbox_exists")]
        public bool MailBoxExists { get; set; }

        [FromQuery(Name = "is_disposable")]
        public bool Disposable { get; set; }

        [FromQuery(Name = "is_role_based")]
        public bool Rolebased { get; set; }
    }
}

