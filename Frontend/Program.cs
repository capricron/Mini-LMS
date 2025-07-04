using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend;
using Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Setup HttpClient
builder.Services.AddScoped(sp => 
    new HttpClient { BaseAddress = new Uri("http://localhost:5083") });

// Daftarkan AssignmentService
builder.Services.AddScoped<AssignmentService>();

await builder.Build().RunAsync();
