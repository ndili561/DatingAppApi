using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Model
{
    //public abstract class BaseEntity
    //{
        


    //}
    public class User 
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public byte[] PassWordSalt { get; set; }
        public byte[] PassWordHash { get; set; }

        [NotMapped]
        public string Password { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime Created { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public string LookingFor { get; set; }
    }
}
