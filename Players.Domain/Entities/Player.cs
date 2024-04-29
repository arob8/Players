using Players.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Players.Domain.Entities
{
    public abstract class Player
    {
        [Key]
        public string Id { get; set; }

        public string NameBrief { get; set; }

        public string? FirstName { get; set; }

        public string LastName { get; set; }

        public string Position { get; set; }

        public int? Age { get; set; }

        public int? AveragePositionAgeDiff { get; set; }

        public SportType Sport { get; set; }

        public Player(string id, string firstName, string lastName, string position, int? age, int? averagePositionAgeDiff, SportType sport)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Position = position;
            Age = age;
            AveragePositionAgeDiff = averagePositionAgeDiff;
            Sport = sport;
        }
    }
}