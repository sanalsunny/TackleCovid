using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CowinAvailabilityTracker.Models
{
    public class TelegramMessageModel
    {
        public List<Result> Result { get; set; }
    }

    public class Result
    {
        public Message Message { get; set; }
    }

    public class Message
    {
        public From From { get; set; }
    }

    public class From
    {
        public string Id { get; set; }
    }

}
