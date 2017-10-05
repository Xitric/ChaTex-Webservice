using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Message
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Content { get; set; }
        public Person Sender { get; set; }

        public override string ToString()
        {
            return $"({CreationTime}) {Sender}: {Content}";
        }
    }
}
