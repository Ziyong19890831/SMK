using Microsoft.EntityFrameworkCore;
using SMK.Data;
using System;

namespace GenPrivilege
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SMKWEBContext>();
            optionsBuilder.UseSqlServer("");
            using (SMKWEBContext context = new SMKWEBContext(optionsBuilder.Options))
            {
                PrivilegeFixutre privilegeFixutre = new PrivilegeFixutre(context);
                privilegeFixutre.Process();
            }
        }
    }
}
