namespace DataAccess.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public List<Note> Notes { get; set; }
    }

    public class Note
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
