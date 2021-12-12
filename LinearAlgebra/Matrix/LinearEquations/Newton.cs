using System;
using LinearAlgebra.Vector;

namespace LinearAlgebra.Matrix.LinearEquations
{
    public class Newton
    {
        public Newton(MatrixFunction jacobiMatrix, VectorFunction system, Vector.Vector guess)
        {
            JacobiMatrix = jacobiMatrix;
            System = system;
            Guess = guess;
        }
        public MatrixFunction JacobiMatrix { get; }
        
        public VectorFunction System { get; }
        
        public Vector.Vector Guess { get; }

        public int CounterIteration { get; private set; }
        
        public Vector.Vector Solve()
        {
            var xk = Guess;

            Vector.Vector xkp;
            do
            {
                xkp = xk;
                var jac = JacobiMatrix.InvokeWithArgs(xk);
                var f = System.InvokeWithArgs(xk);
                var gauss = GaussElimination.Solve(jac, -f);
                xk += gauss;
                CounterIteration++;
            } while (Norma.TwoVectorNorm(xk - xkp) > 1e-6m);
            
            Console.WriteLine($"Номер итерации: {CounterIteration}");

            return xk;
        }
    }
}