using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaciocinioAutomatico
{
    public class FrenteDeOnda
    {
        private double maxValue;
        private double gamma;
        public FrenteDeOnda(double maxValue, double gamma)
        {
            this.maxValue = maxValue;
            this.gamma = gamma;
        }
    }
}
