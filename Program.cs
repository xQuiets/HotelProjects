using HotelProject.Data;
using Microsoft.EntityFrameworkCore;
using HotelProject.Components;
using HotelProject.Services;
using Plk.Blazor.DragDrop;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddScoped<AuthService>();//использует метод scoped, чтобы каждый раз создавать новый экземпл€р
builder.Services.AddScoped<PanelService>();
// Add services to the container.
builder.Services.AddControllers(); // ƒл€ API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // ƒл€ документации
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazorDragDrop();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers(); // ƒл€ API
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
