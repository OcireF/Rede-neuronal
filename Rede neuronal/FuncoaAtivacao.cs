using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal
{
    public interface FuncoaAtivacao
    {
        public int FucaoAtivar(double entrada);
        public int Dirivada(double entrada);
    }
}
