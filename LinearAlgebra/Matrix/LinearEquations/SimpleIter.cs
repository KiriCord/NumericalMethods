using System;
using LinearAlgebra.Vector;

namespace LinearAlgebra.Matrix.LinearEquations
{
    public class SimpleIter
    {
        public VectorFunction VectorFunction { get; }
        public Vector.Vector Guess { get; set; }

        public SimpleIter(VectorFunction vectorFunction, Vector.Vector guess) {
            VectorFunction = vectorFunction;
            Guess = guess;
        }
        
        public Vector.Vector Solve() {
            var xk = Guess;
            var xkp = new Vector.Vector(xk.Count);
            var countIteration = 0;
            do {
                xkp.From(xk);
                xk = VectorFunction.InvokeWithArgs(xk);
                countIteration++;
            } while (Norma.TwoVectorNorm(xk - xkp) > 1e-3m);

            Console.WriteLine(
                $"Номер итерации {countIteration}\nПредыдущий вектор\n{xkp}\nСледующий\n{xk}\nРазница между предыдущим и следующим\n{xk - xkp}\nНорма разницы равна {Norma.TwoVectorNorm(xk - xkp)}\n");
            return xk;
        }
        
        
    }
}