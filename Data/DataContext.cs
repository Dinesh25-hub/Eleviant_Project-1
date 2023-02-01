using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using User_Application.Models;
using User_Application.Models.Service;

namespace User_Application.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {        
        }

        public DbSet<Data_List> data_Lists { get; set; }
        public DbSet<Authentication> authentication { get; set; }
        
    }
}
