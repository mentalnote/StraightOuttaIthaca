

namespace Assets.Scripts.God
{
    //base interface that all godpowers span from
    interface IGodPower
    {
        //how much it costs in GOD POWER PTs (tm) to cast this this power
        int GetCost();
        //cast your god power on your victim...err..followers
        void Cast();
    }
}
