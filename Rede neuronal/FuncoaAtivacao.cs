using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public class FuncoaAtivacao
    {
        public FuncoaAtivacao() { }
        public int FucaoAtivar(double entrada)
        {
            if(entrada < 0) //< 0
                return 0;
            else
            {
                return 1; //>= 1
            }
        }
        public int Dirivada(double entrada)
        { 
            return 0; 
        }
    }
}
