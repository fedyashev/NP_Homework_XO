﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Packets
{
    public class ActionSuccessPacket : Packet
    {
        public ActionSuccessPacket()
        {
            Type = "ACTIONSUCCESS";
        }
    }
}
