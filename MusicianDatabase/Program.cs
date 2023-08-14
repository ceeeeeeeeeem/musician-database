using Microsoft.EntityFrameworkCore;
using MusicianDatabase.Data;
using MusicianDatabase.Service;
using MusicianDatabase.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MusicianDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers();

builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IBandService, BandService>();
builder.Services.AddScoped<IConcertBandService, ConcertBandService>();
builder.Services.AddScoped<IConcertService, ConcertService>();
builder.Services.AddScoped<IInstrumentService, InstrumentService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleInstrumentService, RoleInstrumentService>();
builder.Services.AddScoped<IVenueService, VenueService>();


//AddScoped => Bulunduğu alana göre newleme işlemi.
//AddSingleton => Uygulama ayağa kalktığında  IArtistService, ArtistService üzerinden 1 adet nesne oluşturur. Çağırılan tüm yerlerde bu nesne kullanılır.
//AddTransient => Her istekte yeni bir nesne.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
