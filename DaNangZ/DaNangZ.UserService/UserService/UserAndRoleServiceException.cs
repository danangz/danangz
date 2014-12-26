using DaNangZ.CoreLib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.UserService.UserService
{
    class UserAndRoleServiceException : ServiceException
    {
        public UserAndRoleServiceException() : base() { }
        public UserAndRoleServiceException(string message) : base(message) { }
        public UserAndRoleServiceException(string message, Exception innerException) : base(message, innerException) { }
        protected UserAndRoleServiceException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
