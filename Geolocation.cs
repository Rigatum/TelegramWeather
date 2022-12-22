using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramWeather
{
    public class Geolocation
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Chat
        {
            public int id { get; set; }
            public string type { get; set; }
            public string username { get; set; }
            public string first_name { get; set; }
        }

        public class From
        {
            public long  id { get; set; }
            public bool is_bot { get; set; }
            public string first_name { get; set; }
            public string username { get; set; }
            public string language_code { get; set; }
        }

        public class Location
        {
            public decimal longitude { get; set; }
            public decimal latitude { get; set; }
        }

        public class Message
        {
            public long message_id { get; set; }
            public From from { get; set; }
            public int date { get; set; }
            public Chat chat { get; set; }
            public ReplyToMessage reply_to_message { get; set; }
            public Location location { get; set; }
        }

        public class ReplyToMessage
        {
            public long  message_id { get; set; }
            public From from { get; set; }
            public int date { get; set; }
            public Chat chat { get; set; }
            public string text { get; set; }
        }

        public class Root
        {
            public int update_id { get; set; }
            public Message message { get; set; }
        }
    }
}