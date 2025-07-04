using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend;
using Frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Setup HttpClient
builder.Services.AddScoped(sp => 
    new HttpClient { BaseAddress = new Uri("http://localhost:5083") });

// Daftarkan service
builder.Services.AddScoped<AssignmentService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<Frontend.Services.AuthStateProvider>();

// 🔥 Tambahkan ini:
builder.Services.AddAuthorizationCore(); // <--- Penting untuk AuthorizeView

// HttpClient untuk API
builder.Services.AddScoped(sp => 
    new HttpClient { BaseAddress = new Uri("http://localhost:5083") });

// Services
builder.Services.AddScoped<AuthService>();

// 🔥 Tambahkan baris ini:
builder.Services.AddScoped<Frontend.Services.AuthStateProvider>(); // <-- Ini penting agar bisa inject langsung

// Untuk AuthorizeView dan AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider>(sp => 
    sp.GetRequiredService<Frontend.Services.AuthStateProvider>());

// Untuk support <AuthorizeView>
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();