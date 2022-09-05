using URL_Crawler.Models.Interfaces;
using URL_Crawler.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add Scoped - lifetime of a single request.
builder.Services.AddScoped<IViewRenderService, ViewRenderService>();

// Register generic type definition for document readers, allowing multiple document type injection.
System.Reflection.Assembly.GetExecutingAssembly()
      .GetTypes()
      .Where(item => item.GetInterfaces()
      .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDocumentReader<>)) && !item.IsAbstract && !item.IsInterface)
      .ToList()
      .ForEach(assignedTypes =>
      {
          var serviceType = assignedTypes.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IDocumentReader<>));
          builder.Services.AddScoped(serviceType, assignedTypes);
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

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();