using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repository;

public interface IUserRepository
{
    public Task<IEnumerable<Profile>> SearchUsersByName(string name);
    public Task<IEnumerable<Profile>> ListUsersInCohort(int cohortId);
}