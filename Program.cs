using dotenv.net;
using Reddit;
using RedditChallenge.Services.RedditApi;

// read .env file
var envVars = DotEnv.Read();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ApiService>(sp =>
{
    // Retrieve Reddit API credentials from dotenv
    var clientId = envVars["REDDIT_CLIENT_ID"];
    var clientSecret = envVars["REDDIT_CLIENT_SECRET"];
    var userAgent = envVars["REDDIT_USER_AGENT"];
    
    return new ApiService(new HttpClient(), clientId, clientSecret, userAgent);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// controller routes
app.MapControllerRoute(
    name: "subreddit",
    pattern: "Subreddit/{subredditName}",
    defaults: new { controller = "Subreddit", action = "Index" }); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();