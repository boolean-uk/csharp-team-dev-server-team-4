using exercise.wwwapi.Enums;

namespace exercise.wwwapi.Utils
{
    public static class RoleToStringMap
    {
        private static readonly Dictionary<Role, string> _map = new Dictionary<Role, string>() 
        { 
            { Role.Student, "student" },
            { Role.Teacher, "teacher" }
        };

        public static string GetString(Role role)
        {
            return _map[role];
        }
    }
}
