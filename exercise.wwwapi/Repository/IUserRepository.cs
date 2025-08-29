using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repository;

public interface IUserRepository
{
    public Task<IEnumerable<User>> SearchUsersByName(string name);
    public Task<IEnumerable<User>> ListUsersInCohort(int cohortId);
}