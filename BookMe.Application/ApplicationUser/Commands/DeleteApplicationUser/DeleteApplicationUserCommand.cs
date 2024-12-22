using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.ApplicationUser.Commands.DeleteApplicationUser
{
    public class DeleteApplicationUserCommand: IRequest<Unit>
    {
        public string Id { get; set; }

        public DeleteApplicationUserCommand(string id)
        {
            Id = id;
        }
    }
}
