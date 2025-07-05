using System.Net.Http.Json;
using System.Net;
using Backend.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Text.Json;

namespace MiniLMSTests;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_User_Success()
    {
        var client = _factory.CreateClient();

        var loginDto = new LoginRequestDto
        {
            Email = "andi@example.com",
            Password = "password123"
        };

        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        Assert.NotNull(result);
        Assert.Equal("Manager", result.Role);
    }

    [Fact]
    public async Task GetAssignments_Manager_ReturnsList()
    {
        var client = _factory.CreateClient();
        var token = await GetJwtTokenForUser("manager");
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);

        var response = await client.GetAsync("/api/assignments");
        response.EnsureSuccessStatusCode();

        var assignments = await response.Content.ReadFromJsonAsync<List<GetAssignmentDto>>();
        Assert.NotNull(assignments);
        Assert.True(assignments.Count >= 1);
    }

    [Fact]
    public async Task CreateAssignment_WithMCQ_Question()
    {
        var client = _factory.CreateClient();
        var token = await GetJwtTokenForUser("manager");
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);

        var dto = new CreateAssignmentDto
        {
            Title = "Test Assignment",
            Description = "Description",
            IsActive = true,
            Media = "https://youtube.com/embed/test ",
            Questions = new List<MCQDto>
        {
            new MCQDto
            {
                QuestionText = "What is 2+2?",
                OptionA = "3",
                OptionB = "4",
                OptionC = "5",
                OptionD = "6",
                CorrectAnswer = "B"
            }
        }
        };

        var response = await client.PostAsJsonAsync("/api/assignments", dto);
        response.EnsureSuccessStatusCode();

        // ✅ Validasi bahwa responsenya JSON
        Assert.Contains("application/json", response.Content.Headers.ContentType?.ToString());

        // ✅ Baca sebagai JSON dan validasi isinya
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("oke", result.GetProperty("message").GetString());
    }

    [Fact]
    public async Task SubmitAnswer_Learner_Success()
    {
        var client = _factory.CreateClient();
        var token = await GetJwtTokenForUser("learner");
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);

        // Hapus submission jika sudah ada
        await client.DeleteAsync("/api/Learner/submission?assignmentId=1");

        var dto = new SubmitAnswerRequestDto
        {
            AssignmentId = 1,
            Answers = new Dictionary<int, char>
            {
                { 1, 'C' },
                { 2, 'D' }
            }
        };

        var response = await client.PostAsJsonAsync("/api/Learner/submit", dto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<SubmissionResultDto>();
        Assert.NotNull(result);
        Assert.Equal(2, result.TotalQuestions);
        Assert.InRange(result.Score, 0, 100); // Harus antara 0 - 100%
    }

    [Fact]
    public async Task GetSubmissionReview_Manager_Success()
    {
        var client = _factory.CreateClient();
        var token = await GetJwtTokenForUser("learner");
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);

        var submitDto = new SubmitAnswerRequestDto
        {
            AssignmentId = 1,
            Answers = new Dictionary<int, char>
            {
                { 1, 'C' }, // Benar
                { 2, 'D' }  // Benar
            }
        };

        await client.PostAsJsonAsync("/api/Learner/submit", submitDto);

        var response = await client.GetAsync("/api/assignments/review/1/user1");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AssignmentReviewDto>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Answers);
        Assert.Equal(100.00, result.Score); // Harus 100% benar
    }

    private async Task<string> GetJwtTokenForUser(string userType)
    {
        var client = _factory.CreateClient();
        var loginDto = userType switch
        {
            "manager" => new LoginRequestDto
            {
                Email = "andi@example.com",
                Password = "password123"
            },
            "learner" => new LoginRequestDto
            {
                Email = "budi@example.com",
                Password = "password123"
            },
            _ => throw new ArgumentException("Invalid user type")
        };

        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);
        response.EnsureSuccessStatusCode();

        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        Assert.NotNull(auth?.Token);
        return auth.Token;
    }
}