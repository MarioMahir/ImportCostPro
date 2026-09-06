using ImportCostPro.Core.Data;
using Microsoft.EntityFrameworkCore;
using ImportCostPro.Core.Interfaces;
using ImportCostPro.Core.Services;
using System.Buffers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IMonedaService, MonedaService>();
builder.Services.AddScoped<IImportadorService, ImportadorService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoriaArancelariaService, CategoriaArancelariaService>();
builder.Services.AddScoped<ITasaCambioService, TasaCambioService>();
builder.Services.AddScoped<IOrdenImportacionService, OrdenImportacionService>();
builder.Services.AddScoped<IDetalleOrdenImportacionService, DetalleOrdenImportacionService>();
builder.Services.AddScoped<IGastoImportacionService, GastoImportacionService>();
builder.Services.AddScoped<IConfiguracionImpuestosService, ConfiguracionImpuestosService>();
builder.Services.AddScoped<ILandedCostService, LandedCostService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
