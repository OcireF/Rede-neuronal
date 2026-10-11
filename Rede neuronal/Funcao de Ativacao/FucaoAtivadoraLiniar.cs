using Rede_neuronal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public class FucaoAtivadoraLiniar:IFuncoaAtivacao
    {
        public FucaoAtivadoraLiniar() { }
        public double FucaoAtivar(double entrada)
        {
            return entrada;
        }
        public double Derivada(double entrada)
        {
            return 1;
        }
    }
}
