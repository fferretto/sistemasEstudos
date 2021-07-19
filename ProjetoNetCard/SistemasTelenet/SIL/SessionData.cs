using System;
using System.Collections.Generic;

namespace SIL
{
    public class SessionData
    {
        public DateTime Expiration { get; set; }
        public string Id { get; set; }
        public IEnumerable<SessionDataItem> Values { get; set; }
    }
}
