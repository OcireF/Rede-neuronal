namespace SARSA
{
    public class SARSA
    {
        public Dictionary<(int, int), double> Politica = new Dictionary<(int estado, int acao), double>(); //Q(s,a)
        public double y;
        public double alfa;

        SARSA()
        {

        }

        public double Aprender(int s, int a, double r, int sLine, int aLine, bool isFinal) //s, a, r, s’, a’, final
        {
            double j = 0;
            if (isFinal)
            {
                j = r; // J = r
            }
            else
            {
                j = r + (y * Politica[(sLine, aLine)]); // J = r + γQ(s’, a’)
                Politica[(s, a)] += alfa * (j - Politica[(s, a)]); // Q(s, a) = Q(s, a) + alfa(J – Q(s, a))
            }
            return j;
        }
    }
}
