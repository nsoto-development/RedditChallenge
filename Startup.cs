using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedditChallenge.Services.RedditApi;
using dotenv.net;
using RedditChallenge.Hubs;

namespace RedditChallenge
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      var envVars = DotEnv.Read();

      if (envVars == null)
      {
          throw new InvalidOperationException("Failed to read environment variables from .env file.");
      } 

      foreach (var (key, value) in envVars)
      {
          Environment.SetEnvironmentVariable(key, value);
      }

      configuration = new ConfigurationBuilder()
          .AddEnvironmentVariables()
          .Build();

      _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllersWithViews();
      services.AddSignalR();
      
      services.AddSingleton<ApiService>(sp =>
      {
          // Retrieve Reddit API credentials from dotenv
          var clientId = _configuration["REDDIT_CLIENT_ID"] ;
          var clientSecret = _configuration["REDDIT_CLIENT_SECRET"];
          var userAgent = _configuration["REDDIT_USER_AGENT"];

          if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(userAgent))
          {
              throw new InvalidOperationException("Reddit API credentials are missing. Please check your configuration.");
          }
          
          return new ApiService(new HttpClient(), clientId, clientSecret, userAgent);
      });

      // Add WebSocket support
      // services.AddWebSocketManager();

      // Add other services as needed
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // Configure production-specific settings
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseWebSockets();
      app.UseAuthorization();


      app.UseEndpoints(endpoints =>
      {
          ConfigureHubs(endpoints);
          ConfigureRoutes(endpoints);
      });

    }

    public void ConfigureRoutes(IEndpointRouteBuilder endpoints)
    {
      endpoints.MapControllerRoute(
        name: "subreddit",
        pattern: "Subreddit/{subredditName}",
        defaults: new { controller = "Subreddit", action = "Index" });

      endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    }

    private void ConfigureHubs(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<PostsHub>("/postsHub");
    }
  }
}