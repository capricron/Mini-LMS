// File: Services/AuthService.cs
using Frontend.Dto;
using Frontend.DTOs;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _jsRuntime;

        public AuthService(HttpClient http, IJSRuntime jsRuntime)
        {
            _http = http;
            _jsRuntime = jsRuntime;
        }

        public async Task<LoginResponseDto?> LoginAsync(string email, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", new
            {
                email,
                password
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            }

            return null;
        }

        public async Task SaveTokenAsync(string token, string role)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "role", role);
        }
        public async Task<string?> GetRoleAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "role");
        }

    }
}