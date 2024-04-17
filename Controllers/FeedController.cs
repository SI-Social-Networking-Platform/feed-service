using FeedService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeedService.Controllers;

[ApiController]
[Route("[controller]")]
public class FeedController : ControllerBase
{
    private readonly IUserServiceClient _userServiceClient;
    private readonly IPostServiceClient _postServiceClient;

    public FeedController(IUserServiceClient userServiceClient, IPostServiceClient postServiceClient)
    {
        _userServiceClient = userServiceClient;
        _postServiceClient = postServiceClient;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetFeed(int userId)
    {
        var followedUserIds = await _userServiceClient.GetFollowedUserIds(userId);
        var posts = await _postServiceClient.GetPostsByUserIds(followedUserIds);
        return Ok(posts);
    }
}
