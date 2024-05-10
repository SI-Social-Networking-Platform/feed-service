# Feed Service

The **Feed Service** aggregates posts from followed users to create a personalized feed for each authenticated user. This service interacts with other microservices, such as User and Post Services, to retrieve the relevant data.

## Controller

### FeedController

The `FeedController` aggregates posts from users that the currently authenticated user follows.

- **Endpoints:**

  - `GET /feed`  
    - **Description:** Retrieves a personalized feed for the authenticated user, containing posts from the users they follow.
    - **Authorization:** Requires a valid JWT token.
    - **Response Codes:**
      - `200`: Successfully retrieved the feed containing posts.
      - `401`: Unauthorized, user ID not found in the JWT claims.
      - `404`: No followed users found or no posts available.

## External Service Clients

The `FeedController` uses two service clients to aggregate data:

1. **IUserServiceClient**  
   Interacts with the User Service to fetch a list of followed user IDs for the authenticated user.

   - **Method:** `Task<List<int>> GetFollowedUserIds(int userId)`  
     - **Parameter:** `userId` representing the unique ID of the authenticated user.
     - **Returns:** A list of IDs of users followed by the specified user.

2. **IPostServiceClient**  
   Interacts with the Post Service to fetch posts by a list of user IDs.

   - **Method:** `Task<List<Post>> GetPostsByUserIds(List<int> userIds)`  
     - **Parameter:** `userIds` containing a list of unique user IDs.
     - **Returns:** A list of posts made by the users in the provided list.

## Security

- **JWT Authentication:** The `GET /feed` endpoint requires a valid JWT token to ensure only authenticated users can retrieve their feed.

