using RedditChallenge;

var builder = WebApplication.CreateBuilder(args);

Startup startup = new Startup(builder.Configuration);
// Add services to the container.
startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);
startup.ConfigureRoutes(app);

app.Run();