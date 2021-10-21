using System;

namespace Models.Responses
{
    public class ResponseDto
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Message { get; set; }
    }
}