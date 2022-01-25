namespace WorkEvents.Models
{
    public class Message
    {
        public Message()
        {

        }
        private Message(string title, string description)
        {
            Title = title;
            Description = description;
            Id= Guid.NewGuid();
            Date = DateTime.Now;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public static async Task<Message> CreateMessage(string title, string description)
        {
            await Task.Delay(5000);

            return new Message(title, description);
        }

    }
}
