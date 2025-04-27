using DaberlyProjet.Data;
using Microsoft.EntityFrameworkCore;
using DaberlyProjet.Data;
using System;
using DaberlyProjet.Services;
using DaberlyProjet;
using DaberlyProjet.Hubs;



var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") 
                  .AllowAnyHeader()
                  //.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});








builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnetion"))
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PhotoUserService>();
builder.Services.AddScoped<AlbumService>();
builder.Services.AddScoped<PlaylistService>();
builder.Services.AddScoped<VideoService>();


builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); 
        });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseStaticFiles();


app.UseCors(MyAllowSpecificOrigins);


app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<AnnonceHub>("/annonceHub");
    endpoints.MapHub<ChatHub>("/chatHub");
});


app.MapControllers();

app.Run();
