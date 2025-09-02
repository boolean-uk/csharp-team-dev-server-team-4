using exercise.wwwapi.Models.UserInfo;

namespace exercise.wwwapi.Repository;

public interface IUserRepository
{
    public Task<IEnumerable<User>> GetAllUsers();
    public Task<User> CreateUser(User user);
    public Task<User?> GetUserById(int id);
    public Task<User> UpdateUser(User user);
    
    
    public Task<IEnumerable<Profile>> SearchUsersByName(string name);
    public Task<IEnumerable<Profile>> ListUsersInCohort(int cohortId);
}