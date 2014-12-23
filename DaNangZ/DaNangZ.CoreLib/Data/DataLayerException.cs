using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.CoreLib.Data
{
    [Serializable]
    public class DataLayerException : Exception
    {
        public DataLayerException() : base() { }
        public DataLayerException(string message) : base(message) { }
        public DataLayerException(string message, Exception innerException) : base(message, innerException) { }
        protected DataLayerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
