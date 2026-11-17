using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaciocinioAutomatico
{
    public interface IProblema
    {
        public double[] estado_inicial(); //estado inicial
        public List<double[]> sucessores(double[] estado); //get sucessores
        public Func<double[], double> avaliar(); //avaliador
        public double avaliar(double[] estado); //avaliar o estado
    }
}
