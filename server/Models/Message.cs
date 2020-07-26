using System;

namespace server.Models
{
    public class Message
    {
        // this just "happen" to match up with
        // Vuetify colors for snackbar
        public const string Information = "info";
        public const string Success = "success";
        public const string Warning = "warning";
        public const string Error = "error";

        public DateTimeOffset Timestamp { get; set; }
        public string SenderUsername { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
    }

}