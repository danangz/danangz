using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.UserService.UserService
{
    public interface IUserProfileAD<TUserId> : IUserProfile<TUserId>
        where TUserId : struct
    {
        bool IsADUser { get; set; }
    }
}
