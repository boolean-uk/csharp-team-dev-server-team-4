using exercise.wwwapi.Data;
using exercise.wwwapi.Models.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _db;

    public UserRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _db.Users
            .Include(user => user.Credential)
            .Include(user => user.Profile)
            .ToListAsync();
    }

    public async Task<User> CreateUser(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetUserById(int id)
    {
        return await _db.Users
            .Include(user => user.Credential)
            .Include(user => user.Profile)
            .FirstOrDefaultAsync(user => user.Id == id);
    }
    
    public async Task<User> UpdateUser(User user)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        return user;
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