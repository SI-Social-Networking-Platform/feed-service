namespace FeedService.Services;

public class UserServiceClient : IUserServiceClient
{
    private readonly HttpClient _httpClient;

    public UserServiceClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("UserServiceClient");
    }

    public async Task<List<int>> GetFollowedUserIds(int userId)
    {
        var response = await _httpClient.GetAsync($"/user-service/{userId}/follows");
        response.EnsureSuccessStatusCode();
        var followedUserIds = await response.Content.ReadFromJsonAsync<List<int>>();
        return followedUserIds ?? new List<int>();
    }
}

public interface IUserServiceClient
{
    Task<List<int>> GetFollowedUserIds(int userId);
}
