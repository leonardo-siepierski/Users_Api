using Microsoft.EntityFrameworkCore;

namespace usersapi.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            // UsersQueries = usersQueries;
            // usersQueries.SetDbContext(this);
        }

        public DbSet<User> Users { get; set; }

        // public IUsersQueries UsersQueries { get; private set; }
    }
}