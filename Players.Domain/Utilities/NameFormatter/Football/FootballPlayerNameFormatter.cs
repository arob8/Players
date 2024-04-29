namespace Players.Domain.Utilities.NameFormatter.Football
{
    public class FootballPlayerNameFormatter : PlayerNameFormatter
    {
        public override string GenerateNameBrief(string? firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && firstName.Length > 0 && lastName.Length > 0)
            {
                return $"{firstName[0]}. {lastName}";
            }
            return null;
        }
    }
}
