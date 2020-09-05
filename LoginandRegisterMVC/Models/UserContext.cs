using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoginandRegisterMVC.Models
{
    public class UserContext:DbContext
    {
        public UserContext() : base("name=Dbconfig")
        {

        }
        public DbSet<User> Users { get; set; }
    }
}