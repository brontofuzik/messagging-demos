using System;
using NServiceBus;
using NServiceBus.Logging;
using UserService.Messages.Commands;
using UserService.Messages.Events;

namespace UserService
{
    public class UserCreator : IHandleMessages<CreateNewUserCmd>
    {
        public IBus Bus { get; set; }

        private static readonly ILog log = LogManager.GetLogger(typeof(UserCreator));

        public void Handle(CreateNewUserCmd message)
        {
            log.InfoFormat("Creating user '{0}' with email '{1}'", message.Name, message.EmailAddress);

            // This is where the user would be added to the database.
            // The database command would auto-enlist in the ambient
            // transaction and either succeed or fail along with
            // the message being processed.

            Bus.Publish<IUserCreatedEvent>(evt =>
            {
                evt.UserId = Guid.NewGuid();
                evt.Name = message.Name;
                evt.EmailAddress = message.EmailAddress;
            });
        }
    }
}
