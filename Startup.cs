using RedditChallenge.Services.RedditApi;
using dotenv.net;

namespace RedditChallenge
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      IDictionary<string, string> envVars = DotEnv.Read();

      if (envVars == null)
      {
        throw new InvalidOperationException("Failed to read environment variables from .env file.");
      }

      foreach ((string key, string value) in envVars)
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
        string? clientId = _configuration["REDDIT_CLIENT_ID"];
        string? clientSecret = _configuration["REDDIT_CLIENT_SECRET"];
        string? userAgent = _configuration["REDDIT_USER_AGENT"];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(userAgent))
        {
          throw new InvalidOperationException("Reddit API credentials are missing. Please check your configuration.");
        }

        return new ApiService(new HttpClient(), clientId, clientSecret, userAgent);
      });
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
      app.UseAuthorization();
      app.UseEndpoints(ConfigureRoutes);
      // ConfigureWebsockets(app); is a work in progress and does not work yet
    }

    private void ConfigureRoutes(IEndpointRouteBuilder endpoints)
    {
      endpoints.MapControllerRoute(
        name: "subreddit",
        pattern: "Subreddit/{subredditName}",
        defaults: new { controller = "Subreddit", action = "Index" });

      endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    }

    // private void ConfigureWebsockets(IApplicationBuilder app)
    // {
    //   app.UseWebSockets();
    //   app.Use(async (context, next) =>
    //   {
    //     if (context.Request.Path == "/redditUpdates")
    //     {
    //       string? subreddit = context.Request.Query["subreddit"];
    //       if (!string.IsNullOrEmpty(subreddit))
    //       {
    //         if (context.WebSockets.IsWebSocketRequest)
    //         {
    //           ApiService ApiService = app.ApplicationServices.GetRequiredService<ApiService>();
    //           WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
    //           await ApiService.HandleWebSocket(webSocket, subreddit);
    //         }
    //         else
    //         {
    //           context.Response.StatusCode = 400;
    //         }
    //       }
    //       else
    //       {
    //         context.Response.StatusCode = 400;
    //         await context.Response.WriteAsync("Subreddit parameter is missing.");
    //       }
    //     }
    //     else
    //     {
    //       await next();
    //     }
    //   });
    // }
  }
}
