namespace Players.Domain.Exceptions
{
    public class PlayerNotFoundException : Exception
    {
        public int Id { get; }

        public PlayerNotFoundException(int id)
            : base($"Player with ID {id} was not found.")
        {
            Id = id;
        }
    }
}
