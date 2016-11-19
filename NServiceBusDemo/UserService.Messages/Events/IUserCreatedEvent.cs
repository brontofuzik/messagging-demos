using System;
using NServiceBus;

namespace UserService.Messages.Events
{
    public interface IUserCreatedEvent : IEvent
    {
        Guid UserId { get; set; }

        string EmailAddress { get; set; }
        
        string Name { get; set; }
    }
}
