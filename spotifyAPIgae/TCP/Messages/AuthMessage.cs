namespace spotifyAPIgae.TCP.Messages
{
    public class Authorization
    {
        public string Sender { get; set; }
        public string Key { get; set; }

        public Authorization(string sender, string key)
        {
            Sender = sender;
            Key = key;
        }
    }
}
