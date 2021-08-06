using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMDContactsClient.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool Status { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Tel { get; set; }
        public string TelBusiness { get; set; }
        public string TelHome { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string BirthDate { get; set; }
        public string Note { get; set; }
    }
}