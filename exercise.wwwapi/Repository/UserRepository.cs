using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
namespace exercise.wwwapi.Repository;

public class UserRepository : IUserRepository
{
    private DataContext _db;

    public UserRepository(DataContext db)
    {
        _db = db;
    }

    public Task<IEnumerable<Profile>> SearchUsersByName(string name)
    {
        var split = name.Split(' ');

        List<Profile> profiles = new List<Profile>();

        if (split.Length > 1)
        {
            profiles.AddRange(_db.Profiles.Where(profile =>
                (profile.FirstName == split[0] || profile.FirstName == split[0]) &&
                (profile.LastName == split[1] || profile.FirstName == split[1])));
        }

        if (profiles.Count == 0)
        {
            profiles.AddRange(_db.Profiles.Where(profile => profile.FirstName == name));
            profiles.AddRange(_db.Profiles.Where(profile => profile.LastName == name));
        }

        return Task.FromResult<IEnumerable<Profile>>(profiles);
    }

    public Task<IEnumerable<Profile>> ListUsersInCohort(int cohortId)
    {
        throw new NotImplementedException();
    }
}