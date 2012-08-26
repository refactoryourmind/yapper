using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yapper.Server
{
    /// <summary>
    /// This list can be iterated upon with new message types.
    /// Logic for new message types can be handled directly in Network Manager's 
    /// HandleCustomMessage method, or in the case of more complex messages,
    /// generate a struct that handles all relevant logic and simply call its
    /// OnReceive method.
    /// </summary>
    public enum CustomMessages
    {
        CHAT,
        NICKREQ
    }
}
