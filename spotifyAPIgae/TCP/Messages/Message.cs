using System;

namespace spotifyAPIgae.TCP.Messages
{
    public class Message
    {
        public const string Authorized = "Authorized";
        public const string Unauthorized = "Unauthorized";
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public Message(string sender, string text, DateTime time)
        {
            Sender = sender;
            Text = text;
            Time = time;
        }
        public override string ToString() => $"{Time:HH:mm} | {Sender}: {Text}";
    }
}