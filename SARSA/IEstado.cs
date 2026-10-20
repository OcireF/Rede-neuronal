using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSA
{
    public interface IEstado
    {
        public bool IsTerminal();
        public List<int> AcoesPossiveis();
    }
}
