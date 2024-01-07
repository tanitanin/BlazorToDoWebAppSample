using BlazorToDoWebAppSample.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlazorToDoWebAppSample.Components.Data;

var builder = WebApplication.CreateBuilder(args);

// Add database context factory.
//builder.Services.AddDbContext<BlazorToDoWebAppSampleDbContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("BlazorToDoWebAppSampleDbContext")
//    ?? throw new InvalidOperationException("Connection string 'BlazorToDoWebAppSampleDbContext' not found.")));
#if DEBUG
builder.Services.AddDbContextFactory<BlazorToDoWebAppSampleDbContext>(opt =>
    opt.UseSqlite($"Data Source={BlazorToDoWebAppSampleDbContext.DbName}.db")
    .EnableSensitiveDataLogging());
#else
builder.Services.AddDbContextFactory<BlazorToDoWebAppSampleDbContext>(opt =>
    opt.UseSqlite($"Data Source={BlazorToDoWebAppSampleDbContext.DbName}.db")
#endif

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
