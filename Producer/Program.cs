using DataAccess;
using Microsoft.EntityFrameworkCore;
using Producer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<ThreadsAndTasksDbContext>(options => 
            options.UseNpgsql("User ID=postgres;Password=pwd;Host=localhost;Port=5432;Database=NotesDB;"));
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
