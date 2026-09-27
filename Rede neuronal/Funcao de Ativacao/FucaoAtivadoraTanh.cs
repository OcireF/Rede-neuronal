using Rede_neuronal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public class FucaoAtivadoraTanh : IFuncoaAtivacao
    {
        public FucaoAtivadoraTanh() { }
        public double FucaoAtivar(double entrada)
        {
            return Math.Tanh(entrada);
        }
        public double Dirivada(double entrada)
        {
            return 1 - Math.Pow(Math.Tanh(entrada),2); // 1 - (tanh(x) elevado a 2)
        }
    }
}
