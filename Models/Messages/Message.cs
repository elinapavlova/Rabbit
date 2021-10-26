using System;

namespace Models.Messages
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Text { get; set; }
    }
}