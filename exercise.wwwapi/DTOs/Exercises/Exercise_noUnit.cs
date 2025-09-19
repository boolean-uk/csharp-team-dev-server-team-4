using exercise.wwwapi.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.Models.Exercises;

public class Exercise_noUnit
{
    public int Id { get; set; }
    public int UnitId { get; set; }
    public string Name { get; set; }
    public string GitHubLink { get; set; }
    public string Description { get; set; }

    public Exercise_noUnit(){}

    public Exercise_noUnit(Exercise model)
    {
        Id = model.Id;
        UnitId = model.UnitId;
        Name = model.Name;
        GitHubLink = model.GitHubLink;
        Description = model.Description;
    }
}