using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class WebApplicationDbContext : IdentityDbContext
{
    public WebApplicationDbContext(DbContextOptions<WebApplicationDbContext> options)
        : base(options)
    {

    }
}
