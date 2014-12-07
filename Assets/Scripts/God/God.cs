using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.God
{
    class God
    {

        /////////////////////////
        ///////FIELDS////////////
        /////////////////////////

        //followers is the stat which determines how quickly you gain power & whether you lose or not
        private int _nFollowers = 0;

        //the stat which controls whether you can use your god powers or not
        private float _godGlory = 100;

        //the list of GodPowerType that you have available to you at this level of godliness
        private HashSet<IGodPower> _godPowers;

        //your current level of godliness
        private GodLevels _godLevel = GodLevels.FalseProhphet;

        //your godly name
        private String _godlyName = "Lil 'B";

        //TO NICK: no accessors added yet, getter and setters might not be relevant for some of these

        /////////////////////////
        ///////CONSTANTS/////////
        /////////////////////////
        private const float GloryPerSecondPerFollower = 0.1f;
        

        /////////////////////////
        ///////PRIVATE///////////
        /////////////////////////

        /////////////////////////
        ///////PUBLIC////////////
        /////////////////////////
        
        /**
         * Tick this method to accrue your glory
         */
        public void BaskInWorship(float deltaTime)
        {
            var gloryAccrued = deltaTime * (GloryPerSecondPerFollower * _nFollowers);
            _godGlory += gloryAccrued;
        }

        /**
         * Try to use one of your god powers 
         */
        public void UsePower(IGodPower godPower)
        {
            //see if you can afford this power and also that you actually have this power
            if (godPower.GetCost() <= _godGlory && _godPowers.Contains(godPower))
            {
                godPower.Cast();
            }
        }

    }
}
