using System;
using MassTransit;

namespace Messages
{
    public class BasicMessage
    {
        public string Text { get; set; }
    }

    public class BasicRequest : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public string Text { get; set; }
    }

    public class BasicResponse : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public string Text { get; set; }
    }
}
