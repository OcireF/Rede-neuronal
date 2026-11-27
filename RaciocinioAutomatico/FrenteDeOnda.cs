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
        private double minVal;
        public FrenteDeOnda(double maxValue, double gamma, double minVal = double.MinValue)
        {
            this.maxValue = maxValue;
            this.gamma = gamma;
            this.minVal = minVal;
        }

        public void FazerOnda(Montes prob, List<double[]> estadosObjetivos)
        {
            List<double[]> frente_da_onda = new List<double[]>();
            foreach(double[] obj in estadosObjetivos) //max values nos objs
            {
                prob.SetStateValue(obj, maxValue);
                frente_da_onda.Add(obj);
            }
            while (frente_da_onda.Count > 0)
            {
                var estado = frente_da_onda[0]; //get first
                frente_da_onda.Remove(estado);  //remove para simular o "pop"
                foreach (double[] adjacente in prob.sucessores(estado))
                {
                    double v = prob.avaliar(estado) * gamma; // v = V[s] * pow(gamma,dist(s,s')) //@note not emplemated distancia assumir 1

                    if(minVal > v) //flatted in minValue 
                    {
                        v = minVal;
                    }

                    if(v > prob.avaliar(adjacente)) //@not emplemente the -infinite
                    {
                        prob.SetStateValue(adjacente, v); // V[s'] = v
                        frente_da_onda.Add(adjacente); // frente-de-onda.append(s’)
                    }
                }
            }
        }
    }
}
