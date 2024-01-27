using Gamebook.Interfaces;
using Gamebook.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache(); // Adds the default in-memory implementation of IDistributedCache

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(300); // Set session timeout to 300 days
});

//SERVICES injecting
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ISessionHelper, SessionHelper>();
builder.Services.AddScoped<IGameLocationService, GameLocationService>();
builder.Services.AddScoped<IGameplayService, GameplayService>();


var app = builder.Build();
app.UsePathBase("/HackAttack");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
