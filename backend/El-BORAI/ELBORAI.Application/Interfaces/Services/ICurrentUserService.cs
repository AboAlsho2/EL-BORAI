using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        string? KeycloakUserId { get; }

        bool IsAuthenticated { get; }

        bool IsInRole(string role);
    }
}
