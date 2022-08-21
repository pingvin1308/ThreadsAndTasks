using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ThreadsAndTasksDbContext : DbContext
{
    public ThreadsAndTasksDbContext(DbContextOptions<ThreadsAndTasksDbContext> options) : base(options)
    {

    }
}