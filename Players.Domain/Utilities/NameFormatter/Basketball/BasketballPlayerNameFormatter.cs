namespace Players.Domain.Utilities.NameFormatter.Basketball
{
    public class BasketballPlayerNameFormatter : PlayerNameFormatter
    {
        public override string GenerateNameBrief(string? firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && firstName.Length > 0 && lastName.Length > 0)
            {
                return $"{firstName} {lastName[0]}.";
            }
            return null;
        }
    }
}
