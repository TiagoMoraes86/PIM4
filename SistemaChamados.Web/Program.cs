using SistemaChamados.Web.Data;
using SistemaChamados.Web.Data.Repositories;
using SistemaChamados.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Configurar session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Registrar DatabaseConnection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=localhost;Port=5432;Database=pim;User Id=postgres;Password=2004;";
builder.Services.AddSingleton(new DatabaseConnection(connectionString));

// Registrar Repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IChamadoRepository, ChamadoRepository>();
builder.Services.AddScoped<IFAQRepository, FAQRepository>();
builder.Services.AddScoped<ILogLGPDRepository, LogLGPDRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IRelatorioRepository, RelatorioRepository>();

// Registrar Services
builder.Services.AddScoped<ILGPDService, LGPDService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChamadoService, ChamadoService>();
builder.Services.AddScoped<IFAQService, FAQService>();
builder.Services.AddScoped<IRelatorioService, RelatorioService>(sp => new RelatorioService(
        sp.GetRequiredService<IRelatorioRepository>(),
        sp.GetRequiredService<IChamadoRepository>()
    ));

// Adicionar HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

// Redirecionar root para login
app.MapGet("/", context =>
{
    context.Response.Redirect("/Login");
    return Task.CompletedTask;
});

app.Run();
