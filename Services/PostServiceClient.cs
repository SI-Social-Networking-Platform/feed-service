using System.Text;
using System.Text.Json;
using System.Web;
using FeedService.Models;

namespace FeedService.Services;

public class PostServiceClient : IPostServiceClient
{
    private readonly HttpClient _httpClient;

    public PostServiceClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("PostServiceClient");
    }

    public async Task<List<Post>> GetPostsByUserIds(List<int> userIds)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(userIds),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync("/post/byUserIds", content);
        response.EnsureSuccessStatusCode();
        var posts = await response.Content.ReadFromJsonAsync<List<Post>>();
        return posts ?? new List<Post>();
    }
}

public interface IPostServiceClient
{
    Task<List<Post>> GetPostsByUserIds(List<int> userIds);
}

