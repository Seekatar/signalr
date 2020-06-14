using System;

namespace server.Models
{
    public class Message
    {
        public const string Warning = "WARNING";
        public const string Error = "ERROR";
        public const string Information = "INFO";

        public DateTimeOffset Timestamp { get; set; }
        public string SenderUsername { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
    }

}