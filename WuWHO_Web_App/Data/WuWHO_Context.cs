using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WuWHO_Web_App.Models;

namespace WuWHO_Web_App.Data
{
    public class WuWHO_Context : DbContext
    {
        public WuWHO_Context(DbContextOptions<WuWHO_Context> options)
            : base(options)
        {
        }
        public DbSet <WuWHO_db> tbl_environment_4 { get; set; }
    }
}
