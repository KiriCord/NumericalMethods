using System;

namespace LinearAlgebra.Matrix.LinearEquations
{
    public class MinimumResidual
    {
        public Matrix Coefficient { get; }
        public Vector.Vector Answers { get; }
        public Vector.Vector Guess { get; set; }
        public decimal Tau { get; private set; }
        
        public MinimumResidual(Matrix coefficient, Vector.Vector answers, Vector.Vector guess) {
            Coefficient = coefficient;
            Answers = answers;
            Guess = guess;
            Tau = 1;
        }

        public Vector.Vector Solve()
        {
            var xk = Guess;
            var xkp = new Vector.Vector(Answers.Count);
            var iterCount = 0;
            do
            {
                xkp = new Vector.Vector(xk);
                TauNext(xk);
                xk = xk - Residual(xk) * Tau;
                iterCount++;
            } while (Norma.TwoVectorNorm(xk - xkp) > 1e-6m);
            
            Console.WriteLine(
                $"Номер итерации {iterCount}");
            return xk;
        }
        
        private Vector.Vector Residual(Vector.Vector vector) {
            return Coefficient * vector - Answers;
        }

        public void TauNext(Vector.Vector vector) {
            var res = Residual(vector);
            var ares = Coefficient * res;
            Tau = (ares * res) / (Norma.TwoVectorNorm(ares) * Norma.TwoVectorNorm(ares));
        }
    }
}