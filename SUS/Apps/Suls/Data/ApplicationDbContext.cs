using Microsoft.EntityFrameworkCore;

namespace Suls.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            //pravq si prazen constructor, za da moje da go instancira moqt serviceCollection posle, zashtoto serviceCollection
            //shte vzeme pyrviq ctor s naj-malko argumenti i shte instancira nego, a ako go ostavq samo s optional 
            //parametrite ot dolniq ctor, serviceCollection ne znae ot kyde da gi vzeme.
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Problem> Problems { get; set; }

        public DbSet<Submission> Submissions { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Suls;Integrated Security=True;");
            }
        }
    }
}
