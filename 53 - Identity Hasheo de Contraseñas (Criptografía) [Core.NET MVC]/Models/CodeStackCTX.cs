using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleLogin.Models
{
    public class CodeStackCTX:DbContext
    {
        public CodeStackCTX(DbContextOptions<CodeStackCTX> options) : base(options)
        {

        }

        public DbSet<Usuarios> Usuarios { get; set; }
    }
}
