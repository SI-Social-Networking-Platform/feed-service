using System.Security.Claims;
using FeedService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedService.Controllers;
[Authorize]
[ApiController]
[Route("feed")]
public class FeedController : ControllerBase
{
    private readonly IUserServiceClient _userServiceClient;
    private readonly IPostServiceClient _postServiceClient;

    public FeedController(IUserServiceClient userServiceClient, IPostServiceClient postServiceClient)
    {
        _userServiceClient = userServiceClient;
        _postServiceClient = postServiceClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetFeed()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized("User ID not found in the JWT claims.");
        }

        var userId = int.Parse(userIdClaim.Value);

        var followedUserIds = await _userServiceClient.GetFollowedUserIds(userId);

        if (followedUserIds == null || followedUserIds.Count == 0)
        {
            return NotFound("No followed users found.");
        }

        var posts = await _postServiceClient.GetPostsByUserIds(followedUserIds);

        return Ok(posts);
    }
}
