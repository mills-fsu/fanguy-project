using Lib.CIS4930.Standard.Models;
using Microsoft.EntityFrameworkCore;
using Server.CIS4930.Database.Models;

namespace Server.CIS4930.Database
{
    public class DataContext : DbContext
    {
        public DbSet<ToDoModel> ToDos { get; set; }
        public DbSet<AppointmentModel> Appointments { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
