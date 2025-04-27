namespace DaberlyProjet.DTO
{
    public class MessageRequestDTO
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Text { get; set; }
    }

    public class MarkAsReadDto
    {
        public int ConversationId { get; set; }
        public int UserId { get; set; }
    }

    public class MessageResponseDTO
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public int ConversationId { get; set; }
        public string SenderName { get; set; }
        public string RecipientName { get; set; }
    }


}
