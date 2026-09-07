using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELBORAI.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        int? UserId { get; }

        bool IsAuthenticated { get; }

        bool IsInRole(string role);
    }
}
