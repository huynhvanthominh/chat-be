namespace chat_be.Models.Responses
{
    public class MessageGroupResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MessageGroupResponse(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}