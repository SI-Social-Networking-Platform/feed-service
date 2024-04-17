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
        var query = HttpUtility.ParseQueryString(string.Empty);
        foreach (var id in userIds)
        {
            query["userIds"] = id.ToString();
        }
        var url = $"api/posts?{query}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var posts = await response.Content.ReadFromJsonAsync<List<Post>>();
        return posts ?? new List<Post>();
    }
}

public interface IPostServiceClient
{
    Task<List<Post>> GetPostsByUserIds(List<int> userIds);
}

