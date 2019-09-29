using Microsoft.EntityFrameworkCore;
using DAL.Model;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL
{
    public abstract class CoreDbContext : DbContext
    {
        protected CoreDbContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }

    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            // context.Database.EnsureCreated();
            // SqlScriptsHelper.ExecuteSqlScripts(context);// Used for dubug purpose only
            // 
        }
    }
    public class DataContext :  CoreDbContext, IDataContext
    {
       
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            DbInitializer.Initialize(this);
        }

        

        public DbSet<User> Users { get; set; }


    }

    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>,IDataContext
    {
       

        DataContext IDesignTimeDbContextFactory<DataContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(@"Server=.;Database=MyDatabase;Trusted_Connection=True;");

            return new DataContext(optionsBuilder.Options);

        }
    }

}
