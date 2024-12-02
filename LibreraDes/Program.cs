using LibreraDes.Data;
using LibreraDes.Services; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using LibreraDes.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registro de servicios
builder.Services.AddScoped<ServicioLibros>(); 
builder.Services.AddScoped<RegistroService>();
builder.Services.AddScoped<RecuperarCuentaService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ServicioInventario>();
builder.Services.AddScoped<ServicioCarrito>();
builder.Services.AddScoped<ServicioVentas>();
builder.Services.AddScoped<ServicioCategoriasAutores>();


// Configuraci�n de la base de datos
builder.Services.AddDbContext<LibreriaDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Configuraci�n de autenticaci�n con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login"; // Ruta para redirigir a la vista de login si no est� autenticado
        options.AccessDeniedPath = "/Home/AccessDenied"; // Ruta en caso de acceso denegado
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Manejo de errores en producci�n
    app.UseHsts(); // Seguridad para el manejo de HTTP
}

app.UseHttpsRedirection(); // Redirige HTTP a HTTPS
app.UseStaticFiles(); // Permite el uso de archivos est�ticos

app.UseRouting(); // Habilita el enrutamiento

// Agregar autenticaci�n y autorizaci�n al middleware
app.UseAuthentication(); // Middleware de autenticaci�n
app.UseAuthorization(); // Middleware de autorizaci�n

// Configurar rutas: asegura que la ruta por defecto apunte a Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



// Habilita el acceso a rutas espec�ficas si es necesario
// app.MapRazorPages();

app.Run(); // Inicia la aplicaci�n
