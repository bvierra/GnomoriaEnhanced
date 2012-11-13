using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnomoriaEnhanced
{
    public class GEException
    {
        [Serializable()]
        public class NotInstalledException : System.Exception
        {
            public NotInstalledException() : base() { }
            public NotInstalledException(string message) : base(message) { }
            public NotInstalledException(string message, System.Exception inner) : base(message, inner) { }

            // A constructor is needed for serialization when an 
            // exception propagates from a remoting server to the client.  
            protected NotInstalledException(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) { }
        }
        [Serializable()]
        public class GnomoriaNotInvoked : System.Exception
        {
            public GnomoriaNotInvoked() : base() { }
            public GnomoriaNotInvoked(string message) : base(message) { }
            public GnomoriaNotInvoked(string message, System.Exception inner) : base(message, inner) { }

            // A constructor is needed for serialization when an 
            // exception propagates from a remoting server to the client.  
            protected GnomoriaNotInvoked(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) { }
        }
    }
}
