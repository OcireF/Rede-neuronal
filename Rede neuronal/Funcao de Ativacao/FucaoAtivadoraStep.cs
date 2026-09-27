using Rede_neuronal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public class FucaoAtivadoraStep:IFuncoaAtivacao
    {
        public FucaoAtivadoraStep() { }
        public double FucaoAtivar(double entrada)
        {
            if (entrada < 0) //< 0
                return 0;
            else
            {
                return 1; //>= 1
            }
        }
        public double Dirivada(double entrada)
        {
            return 0;
        }
    }
}
