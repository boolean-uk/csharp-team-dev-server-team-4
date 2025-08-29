using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository;

public class UserRepository : IUserRepository
{
    private DataContext _db;

    public UserRepository(DataContext db)
    {
        _db = db;
    }

    public Task<IEnumerable<User>> SearchUsersByName(string name)
    {
        var split = name.Split(' ');

        List<User> users = new List<User>();

        if (split.Length > 1)
        {
            users.AddRange(_db.Users.Where(user =>
                (user.FirstName == split[0] || user.FirstName == split[0]) &&
                (user.LastName == split[1] || user.FirstName == split[1])));
        }

        if (users.Count == 0)
        {
            users.AddRange(_db.Users.Where(user => user.FirstName == name));
            users.AddRange(_db.Users.Where(user => user.LastName == name));
        }

        return Task.FromResult<IEnumerable<User>>(users);
    }

    public Task<IEnumerable<User>> ListUsersInCohort(int cohortId)
    {
        throw new NotImplementedException();
    }
}