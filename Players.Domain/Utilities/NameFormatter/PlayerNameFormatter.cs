namespace Players.Domain.Utilities.NameFormatter
{
    public abstract class PlayerNameFormatter
    {
        public abstract string GenerateNameBrief(string? firstName, string lastName);
    }
}
