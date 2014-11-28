using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAProject
{
    class Player
    {
        public string Name { get; set; }
        public double Att { get; set; }
        public double AttG { get; set; }
        public double Yds { get; set; }
        public double Avg { get; set; }
        public double YdsG { get; set; }
        public double Weight { get; set; }
        public double Lng { get; set; }
        public double First { get; set; }
        public double FirstPer { get; set; }
        public double Height { get; set; }
        
        public string ImageUrl { get; set; }
        public double CustomSum { get; set; }

        public void SetPlayerValues(short sAtt, short sAttG, short sYds, short sAvg, short sYdsG, short sWeight, short sLng, short sFirst, short sFirstPer, short sHeight)
        {
            CustomSum = (Att * sAtt) + (AttG * sAttG) + (Yds * sYds) + (Avg * sAvg) + (YdsG * sYdsG) + (Weight * sWeight) + (Lng * sLng) + (First * sFirst) + (FirstPer * sFirstPer) + (Height * sHeight);
        }
    }
}
