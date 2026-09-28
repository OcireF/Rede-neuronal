using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rede_neuronal.Interfaces
{
    //interface para criar funções de ativação personalizadas, cada função de ativação deve implementar esta interface
    public interface IFuncoaAtivacao
    {
        public double FucaoAtivar(double entrada);
        public double Derivada(double entrada);
    }
}
