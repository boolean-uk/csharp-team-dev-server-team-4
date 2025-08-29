using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repository;

public interface IPostRepository
{
    public Task<IEnumerable<Post>> GetPostsByMostRecent();
    public Task<Post> CreatePost(Post post);
    public Task<Post?> UpdatePost(Post post);
    public Task<bool> DeletePost(Post post);
}