using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yapper.Client
{
    /// <summary>
    /// This list can be iterated upon with new message types.
    /// Events for new message types can be defined in NetworkManager
    /// and called in the HandleCustomMessages method.
    /// </summary>
    public enum CustomMessages
    {
        CHAT,
        NICKREQ
    }
}
