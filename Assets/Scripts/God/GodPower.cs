using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Assets.Scripts.God
{
    //base interface that all godpowers span from
    interface GodPower
    {
        //how much it costs in GOD POWER PTs (tm) to cast this this power
        int GetCost();
        //cast your god power on your victim...err..followers
        void Cast();
    }
}
