namespace MyFirstMVCApp.Data
{
    public class UserCard
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int CardId { get; set; }
        public virtual Card Card { get; set; }
        public int Id { get; internal set; }
    }
}
