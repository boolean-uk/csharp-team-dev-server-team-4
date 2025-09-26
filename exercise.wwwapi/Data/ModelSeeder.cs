using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace exercise.wwwapi.Data;

public class ModelSeeder
{
    private static readonly DateTime _seedTime = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

    private readonly Random _random = new(123456);
    private static string[] _passwordHashes =
    [
        "$2a$11$NlNrSkH2Uop6Nl90BHeF9udj/s5N79m9j94htBwtiwPMzoJ5EXozW", // Test1test1%
        "$2a$11$MYFrTWP6v64imGdsbibutOW/DSZiu3wg5rWR1Nm5Zjb5XBNut5HKq", // Test2test2%
        "$2a$11$JyMDiDHwh8hrcjNmp0zb8uZGFettl5dyJ3FDa3S5iOCTYnDn6GZqm", // Test3test3%
        "$2a$11$.daNf2PApH/oqC8MGCQq5uHqw2zmjmIiIB8A6WZ/nLXjbI4iuQsEW", // Test4test4%
        "$2a$11$HmeURzynKz6PqTVeZxfDIeg6MRpzI/5ZAY1GyHW0hJuNUvv7ixOOO",  // Test5test5%
        "$2a$11$bUz6SRMx6L3gjVLtV7cKOO2R46tGJNrhF.myYyP2Odu5deLMRmfh6" // Password123!
    ];
    private List<string> _firstnames = new List<string>()
        {
            "Audrey",
            "Donald",
            "Elvis",
            "Barack",
            "Oprah",
            "Jimi",
            "Mick",
            "Kate",
            "Charles",
            "Kate",
            "Pepo",
            "Claus",
            "Fred"
        };
    private List<string> _lastnames = new List<string>()
        {
            "Hepburn",
            "Trump",
            "Presley",
            "Obama",
            "Winfrey",
            "Hendrix",
            "Jagger",
            "Winslet",
            "Windsor",
            "Middleton"

        };
    private List<string> _domain = new List<string>()
        {
            "bbc.co.uk",
            "google.com",
            "theworld.ca",
            "something.com",
            "tesla.com",
            "nasa.org.us",
            "gov.us",
            "gov.gr",
            "gov.nl",
            "gov.ru"
        };
    private List<string> _firstword = new List<string>()
        {
            "The",
            "Two",
            "Several",
            "Fifteen",
            "A bunch of",
            "An army of",
            "A herd of"


        };
    private List<string> _secondword = new List<string>()
        {
            "Orange",
            "Purple",
            "Large",
            "Microscopic",
            "Green",
            "Transparent",
            "Rose Smelling",
            "Bitter"
        };
    private List<string> _thirdword = new List<string>()
        {
            "Buildings",
            "Cars",
            "Planets",
            "Houses",
            "Flowers",
            "Leopards"
        };
    private List<Role> _roles = new List<Role>()
        {
            Role.Student,
            Role.Teacher
        };
    private List<Specialism> _specialisms = new List<Specialism>()
        {
            Specialism.Frontend,
            Specialism.Backend,
            Specialism.Fullstack,
            Specialism.None
        };

    private List<string> _firstPart = new List<string>()
        {
            "The curious fox dashed into the woods",
            "Rain tapped gently on the window glass",
            "She waited by the old lamp all night",
            "A shadow crossed the silent avenue",
            "He remembered the promise at dawn",
            "Stars blinked over the distant meadow",
            "The clock struck midnight with heavy sound",
            "Fresh bread scented the empty kitchen",
            "Hope lingered near the broken gate",
            "Birdsong rose above the misty lake",
            "The child clutched his favorite book",
            "Leaves fluttered around the cold bench",
            "A coin sparkled under the streetlight",
            "Bells rang softly through the sleepy town",
            "Warm sun dried yesterday’s heavy rain"
        };

    private List<string> _lastPart = new List<string>()
        {
            "until moonlight danced upon the forest floor.",
            "as stories unfolded behind quiet eyes.",
            "bringing dreams whispered by the falling leaves.",
            "while memories carved footprints in the snow.",
            "reminding all of hope’s persistent return.",
            "around secret paths only wanderers find.",
            "and time felt gentle for a fleeting moment.",
            "before laughter echoed down the empty hall.",
            "while courage grew beneath trembling hands.",
            "singing lost lullabies into the morning air.",
            "drowning sorrow under pages full of joy.",
            "welcoming travelers searching for home.",
            "evoking wishes only midnight could hear.",
            "breathing life into ordinary days.",
            "ending with promises of another tomorrow."
        };
    private List<string> _commentText = new List<string>()
{
    "What a magical night, full of hope and gentle surprises.",
    "The quiet rain inspired deep thoughts and sleepy smiles.",
    "Sometimes, the smallest coin glimmers brightest in the darkness.",
    "She found comfort in the book’s worn pages and simple stories.",
    "Even the silent streets carry echoes of laughter from days past.",
    "Moonlight always manages to reveal secrets hidden by the sun.",
    "I never realized how peaceful the kitchen can be before sunrise.",
    "Birdsong on the lake always reminds me of childhood adventures.",
    "Courage is found in moments when the gate appears locked.",
    "Each bell in this town rings with a promise of new beginnings.",
    "The forest floor feels alive during the twilight hours.",
    "There is something soothing about bread fresh from the oven.",
    "Every leaf that falls brings memories of autumn’s sweetest days.",
    "Sometimes, time slows and lets us enjoy the quiet miracles.",
    "Her wish was carried by stars shining over the distant meadow.",
    "Even in heavy rain, hope waits at every window.",
    "Travelers are welcomed here not just by open doors, but by open hearts.",
    "Promises whispered at dawn often come true by nightfall.",
    "The misty lake holds stories not yet spoken.",
    "Ordinary days can become extraordinary with just a smile."
};


    private List<User> _users = new List<User>();
    private List<Note> _notes = new List<Note>();
    private List<Post> _posts = new List<Post>();
    private List<Comment> _comments = new List<Comment>();
    private List<Like> _likes = new List<Like>();
    private List<Cohort> _cohorts = new List<Cohort>();
    private List<Course> _courses = new List<Course>();
    private List<CohortCourse> _cohortCourses = new List<CohortCourse>();
    private List<UserCC> _userCCs = new List<UserCC>();
    private List<Exercise> _exercises = new List<Exercise>();
    private List<Unit> _units = new List<Unit>();
    private List<Module> _modules = new List<Module>();
    private List<CourseModule> _courseModules = new List<CourseModule>();
    private List<UserExercise> _userExercises = new List<UserExercise>();



    public  void Seed(ModelBuilder modelBuilder)
    {
        
        User user1 = new User()
        {
            Id = 1,
            Username = "test1",
            Email = "test1@test1",
            PasswordHash = _passwordHashes[0],
            Role = Role.Student,
            FirstName = "Lionel",
            LastName = "Richie",
            Mobile = "1234567890",
            Github = "",
            Bio = "",
            Specialism = Specialism.Frontend,
            PhotoUrl = ""
        };
        _users.Add(user1);

        User user2 = new User()
        {
            Id = 2,
            Username = "test2",
            Email = "test2@test2",
            PasswordHash = _passwordHashes[1],
            Role = Role.Teacher,
            FirstName = "Michael",
            LastName = "Jordan",
            Mobile = "1234123",
            Github = "",
            Bio = "",
            Specialism = Specialism.Backend,
            PhotoUrl = ""
        };
        _users.Add(user2);

        User user3 = new User()
        {
            Id = 3,
            Username = "test3",
            Email = "test3@test3",
            PasswordHash = _passwordHashes[2],
            Role = Role.Student,
            FirstName = "Michael",
            LastName = "Johansen",
            Mobile = "55555555",
            Github = "",
            Bio = "",
            Specialism = Specialism.Frontend,
            PhotoUrl = ""
        };
        _users.Add(user3);

        User user4 = new User()
        {
            Id = 4,
            Username = "test4",
            Email = "test4@test4",
            PasswordHash = _passwordHashes[3],
            Role = Role.Student,
            FirstName = "Michael",
            LastName = "Jackson",
            Mobile = "98987878",
            Github = "",
            Bio = "",
            Specialism = Specialism.Backend,
            PhotoUrl = ""
        };
        _users.Add(user4);

        User user5 = new User()
        {
            Id = 5,
            Username = "test5",
            Email = "test5@test5",
            PasswordHash = _passwordHashes[4],
            Role = Role.Teacher,
            FirstName = "Johnny",
            LastName = "Cash",
            Mobile = "111222333",
            Github = "",
            Bio = "",
            Specialism = Specialism.Frontend,
            PhotoUrl = ""
        };
        _users.Add(user5);

        for (int i = 6; i < 50; i++)
        {
            var firstname = _firstnames[_random.Next(_firstnames.Count)];
            var lastname = _lastnames[_random.Next(_lastnames.Count)];
            var username = $"{firstname}{lastname}{i}";
            User user = new User()
            {
                Id = i,
                FirstName = firstname,
                LastName = lastname,
                Username = username,
                Email = $"{username}@{_domain[_random.Next(_domain.Count)]}",
                PasswordHash = _passwordHashes[5],
                Role = _roles[_random.Next(_roles.Count)],
                Mobile = _random.Next(12345678, 23456789).ToString(),
                Github = $"{username}git",
                Bio = $"{_firstword[_random.Next(_firstword.Count)]}{_secondword[_random.Next(_secondword.Count)]}{_thirdword[_random.Next(_thirdword.Count)]}",
                Specialism = _specialisms[_random.Next(_specialisms.Count)],
                PhotoUrl = ""
            };
            _users.Add(user);
        }

        Post post1 = new Post()
        {
            Id = 1,
            AuthorId = 1,
            Body = $"{_firstPart[0]} {_lastPart[0]}",
            CreatedAt = _seedTime
        };
        _posts.Add(post1);
        Post post2 = new Post()
        {
            Id = 2,
            AuthorId = 2,
            Body = $"{_firstPart[1]} {_lastPart[1]}",
            CreatedAt = _seedTime
        };
        _posts.Add(post2);
        Post post3 = new Post()
        {
            Id = 3,
            AuthorId = 2,
            Body = $"{_firstPart[2]} {_lastPart[2]}",
            CreatedAt = _seedTime
        };
        _posts.Add(post3);
        Post post4 = new Post()
        {
            Id = 4,
            AuthorId = 2,
            Body = $"{_firstPart[3]} {_lastPart[3]}",
            CreatedAt = _seedTime
        };
        _posts.Add(post4);
        Post post5 = new Post()
        {
            Id = 5,
            AuthorId = 1,
            Body = $"{_firstPart[4]} {_lastPart[4]}",
            CreatedAt = _seedTime
        };
        _posts.Add(post5);

        for (int i = 6; i < 20; i++)
        {
            Post p = new Post()
            {
                Id = i,
                AuthorId = _random.Next(_users.Count),
                Body = $"{_firstPart[_random.Next(_firstPart.Count)]} {_lastPart[_random.Next(_lastPart.Count)]}",
                CreatedAt = _seedTime
            };
            _posts.Add(p);
        }

        Comment comment1 = new Comment()
        {
            Id = 1,
            PostId = 1,
            UserId = 1,
            Body = "Comment 1 Body",
            CreatedAt = _seedTime,
        };
        _comments.Add(comment1);

        Comment comment2 = new Comment
        {
            Id = 2,
            PostId = 2,
            UserId = 2,
            Body = "Comment 2 Body",
            CreatedAt = _seedTime,
        };
        _comments.Add(comment2);

        Comment comment3 = new Comment
        {
            Id = 3,
            PostId = 2,
            UserId = 3,
            Body = "Comment 3 Body",
            CreatedAt = _seedTime,
        };
        _comments.Add(comment3);

        Comment comment4 = new Comment
        {
            Id = 4,
            PostId = 2,
            UserId = 1,
            Body = "Comment 4 Body",
            CreatedAt = _seedTime,
        };
        _comments.Add(comment4);

        Comment comment5 = new Comment
        {
            Id = 5,
            PostId = 3,
            UserId = 1,
            Body = "Comment 5 Body",
            CreatedAt = _seedTime,
        };
        _comments.Add(comment5);

        for (int i = 6; i < 50; i++)
        {
            int postId = _posts[_random.Next(_posts.Count)].Id;
            int userId = _users[_random.Next(_users.Count)].Id;
            Comment c = new Comment
            {
                Id = i,
                PostId = postId,
                UserId = userId,
                Body = _commentText[_random.Next(_commentText.Count)],
                CreatedAt = _seedTime,
            };
            _comments.Add(c);
        }

        Like like1 = new Like
        {
            Id = 1,
            PostId = 1,
            UserId = 1
        };
        _likes.Add(like1);

        Like like2 = new Like
        {
            Id = 2,
            PostId = 1,
            UserId = 2      
        };
        _likes.Add(like2);

        Like like3 = new Like
        {
            Id = 3,
            PostId = 1,
            UserId = 3
        };
        _likes.Add(like3);

        /*
        for (int i = 4;  i < 50; i++) {
            Random likeRandom = new Random();
            Like l = new Like
            {
                Id = i,
                Postid = 
            }
        */

        Course course1 = new Course
        {
            Id = 1,
            Name = "Java",
        };
        _courses.Add(course1);

        Course course2 = new Course
        {
            Id = 2,
            Name = ".NET",
        };
        _courses.Add(course2);

        Cohort cohort1 = new Cohort
        {
            Id = 1,
            CohortNumber = 1,
            CohortName = "August 2025",
            StartDate = new DateTime(2025, 8, 1),
            EndDate = new DateTime(2025, 9, 29),
        };
        _cohorts.Add(cohort1);

        Cohort cohort2 = new Cohort
        {
            Id = 2,
            CohortNumber = 2,
            CohortName = "February 2026",
            StartDate = new DateTime(2026, 2, 1),
            EndDate = new DateTime(2026, 3, 29),
        };
        _cohorts.Add(cohort2);

        CohortCourse cc1 = new CohortCourse
        {
            Id = 1,
            CohortId = 1,
            CourseId = 1,
        };
        _cohortCourses.Add(cc1);

        CohortCourse cc2 = new CohortCourse
        {
            Id = 2,
            CohortId = 1,
            CourseId = 2
        };
        _cohortCourses.Add(cc2);

        CohortCourse cc3 = new CohortCourse
        {
            Id = 3,
            CohortId = 2,
            CourseId = 1
        };
        _cohortCourses.Add(cc3);

        UserCC ucc1 = new UserCC
        {
            Id = 1,
            CcId = 1,
            UserId = 1
        };
        _userCCs.Add(ucc1);

        UserCC ucc2 = new UserCC
        {
            Id = 2,
            CcId = 1,
            UserId = 2,
        };
        _userCCs.Add(ucc2);

        UserCC ucc3 = new UserCC
        {
            Id = 3,
            CcId = 1,
            UserId = 3
        };
        _userCCs.Add(ucc3);


        for (int i = 4; i <= _users.Count; i++)
        {
            var userId = i;
            var ccId = _cohortCourses[_random.Next(_cohortCourses.Count)].Id;

            UserCC ucc = new UserCC
            {
                Id = i,
                UserId = userId,
                CcId = ccId
            };
            _userCCs.Add(ucc);
        }

        Module module1 = new Module
        {
            Id = 1,
            Title = "API"
        };
        _modules.Add(module1);

        Module module2 = new Module
        {
            Id = 2,
            Title = "UI"
        };
        _modules.Add(module2);

        Unit unit1 = new Unit
        {
            Id = 1,
            ModuleId = 1,
            Name = "Many2Many",
        };
        _units.Add(unit1);

        Unit unit2 = new Unit
        {
            Id = 2,
            ModuleId = 1,
            Name = "TDD",
        };
        _units.Add(unit2);

        Unit unit3 = new Unit
        {
            Id = 3,
            ModuleId = 2,
            Name = "Styling",
        };
        _units.Add(unit3);

        Unit unit4 = new Unit
        {
            Id = 4,
            ModuleId = 2,
            Name = "responsive UX",
        };
        _units.Add(unit4);

        Exercise exercise1 = new Exercise
        {
            Id = 1,
            UnitId = 1,
            Name = "pong_challenge",
            GitHubLink = "github.com/1",
            Description = "making pong game"
        };
        _exercises.Add(exercise1);

        Exercise exercise2 = new Exercise
        {
            Id = 2,
            UnitId = 2,
            Name = "testing exercise 2",
            GitHubLink = "github.com/2",
            Description = "exercise for testing"
        };
        _exercises.Add(exercise2);

        Exercise exercise3 = new Exercise
        {
            Id = 3,
            UnitId = 3,
            Name = "first_css",
            GitHubLink = "github.com/3",
            Description = "Styling html with css"
        };
        _exercises.Add(exercise3);

        Exercise exercise4 = new Exercise
        {
            Id = 4,
            UnitId = 3,
            GitHubLink = "github.com/4",
            Name = "css_javascript",
            Description = "modifying css with javascript"
        };
        _exercises.Add(exercise4);

        Exercise exercise5 = new Exercise
        {
            Id = 5,
            UnitId = 4,
            GitHubLink = "github.com/5",
            Name = "button_press",
            Description = "responsive UX with buttons"
        };
        _exercises.Add(exercise5);

        UserExercise ux1 = new UserExercise
        {
            Id = 1,
            SubmissionLink = "github.com/user1/1",
            SubmitionTime = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            Grade = 0,
            UserId = 1,
            Submitted = true,
            ExerciseId = 1
        };
        _userExercises.Add(ux1);

        UserExercise ux2 = new UserExercise
        {
            Id = 2,
            SubmissionLink = "github.com/user2/1",
            SubmitionTime = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            Grade = 3,
            UserId = 2,
            Submitted = true,
            ExerciseId = 1
        };
        _userExercises.Add(ux2);

        UserExercise ux3 = new UserExercise
        {
            Id = 3,
            SubmissionLink = "github.com/user3/1",
            SubmitionTime = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            Grade = 0,
            UserId = 3,
            Submitted = true,
            ExerciseId = 1
        };
        _userExercises.Add(ux3);


        CourseModule cm1 = new CourseModule
        {
            Id = 1,
            CourseId = 1,
            ModuleId = 1
        };
        _courseModules.Add(cm1);

        CourseModule cm2 = new CourseModule
        {
            Id = 2,
            CourseId = 2,
            ModuleId = 2
        };
        _courseModules.Add(cm2);

        CourseModule cm3 = new CourseModule
        {
            Id = 3,
            CourseId = 2,
            ModuleId = 1
        };
        _courseModules.Add(cm3);

        Note note1 = new Note
        {
            Id = 1,
            UserId = 1,
            Title = "Late",
            Content = "student was late",
            CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
        };
        _notes.Add(note1);

        Note note2 = new Note
        {
            Id = 2,
            UserId = 3,
            Title = "Late",
            Content = "student was late",
            CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
        };
        _notes.Add(note2);

        Note note3 = new Note
        {
            Id = 3,
            UserId = 1,
            Title = "Late",
            Content = "student was late",
            CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
        };
        _notes.Add(note3);

        Note note4 = new Note
        {
            Id = 4,
            UserId = 1,
            Title = "Late",
            Content = "student was late",
            CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
        };
        _notes.Add(note4);

        Note note5 = new Note
        {
            Id = 5,
            UserId = 1,
            Title = "Late",
            Content = "student was late",
            CreatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2025, 9, 5, 11, 2, 0, DateTimeKind.Utc),
        };
        _notes.Add(note5);



        int noteId = 6; // Start after existing notes
        for (int i = 0; i < _users.Count; i++)
        {
            var user = _users[i];
            if (user.Role == Role.Student)
            {
                Note n = new Note
                {
                    Id = noteId++,
                    UserId = user.Id,
                    Title = $"Note for {user.FirstName}",
                    Content = _commentText[_random.Next(_commentText.Count)],
                    CreatedAt = _seedTime,
                    UpdatedAt = _seedTime
                };
                _notes.Add(n);
            }
        }


        modelBuilder.Entity<User>().HasData(_users);
        modelBuilder.Entity<Post>().HasData(_posts);
        modelBuilder.Entity<Comment>().HasData(_comments);
        modelBuilder.Entity<Like>().HasData(_likes);
        modelBuilder.Entity<Course>().HasData(_courses);
        modelBuilder.Entity<Cohort>().HasData(_cohorts);
        modelBuilder.Entity<Module>().HasData(_modules);
        modelBuilder.Entity<Unit>().HasData(_units);
        modelBuilder.Entity<Exercise>().HasData(_exercises);
        modelBuilder.Entity<Note>().HasData(_notes);
        modelBuilder.Entity<CourseModule>().HasData(_courseModules);
        modelBuilder.Entity<UserExercise>().HasData(_userExercises);
        modelBuilder.Entity<CohortCourse>().HasData(_cohortCourses);
        modelBuilder.Entity<UserCC>().HasData(_userCCs);



    }

   

  


}