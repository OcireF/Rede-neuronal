using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaciocinioAutomatico
{
    public interface IAlgoritmoHill
    {
        public void FaserPasso(); //fazer um passo
        public double[] GetEstado(); //get estado atual
        public void Restart(); //restart algorimo
        public void PrintResults();
    }
}
