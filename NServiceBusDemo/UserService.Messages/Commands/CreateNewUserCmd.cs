using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace UserService.Messages.Commands
{
    public class CreateNewUserCmd : ICommand
    {
        public string Name { get; set; }

        public string EmailAddress { get; set; }
    }
}
