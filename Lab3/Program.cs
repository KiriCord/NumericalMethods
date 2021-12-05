using System;
using LinearAlgebra.Matrix;
using LinearAlgebra.Matrix.LinearEquations;
using LinearAlgebra.Vector;


namespace Lab3
{
    public static class Program
    {

        public static decimal Sqr(decimal num)
        {
            return num * num;
        }
        
        static void Main(string[] args)
        {
            Func<Vector, decimal>[] originalVector = {
                item => item[0] + Sqr(item[0]) - 2 * item[1] * item[2] - 0.1m,
                item => item[1] - Sqr(item[1]) + 3 * item[0] * item[2] + 0.2m,
                item => item[2] + Sqr(item[2]) + 2 * item[0] * item[1] - 0.3m
            };
            
            Func<Vector, decimal>[,] originalJacobi = {
                { item => 1 + 2 * item[0], item => -2 * item[2], item => -2 * item[1] },
                { item => 3 * item[2], item => 1 - 2 * item[1], item => 3 * item[0] },
                { item => 2 * item[1], item => 2 * item[0], item => 1 + 2 * item[2] },
            };
            
            Func<Vector, decimal>[] reducedVector = {
                item => -Sqr(item[0]) + 2 * item[1] * item[2] + 0.1m,
                item => Sqr(item[1]) - 3 * item[0] * item[2] - 0.2m,
                item => -Sqr(item[2]) - 2 * item[0] * item[1] + 0.3m
            };
            
            var guess = new Vector(new []{0m, 0m, 0m });
            
            var reducedVectorFunction = new VectorFunction(reducedVector);
            var vecFun = new VectorFunction(originalVector);
            var matrixFun = new MatrixFunction(originalJacobi);

            var s1mIter = new SimpleIter(reducedVectorFunction, guess);
            var sim = s1mIter.Solve();
            Console.WriteLine($"\nРешение методом простых итераций:\n{sim}");
            Console.WriteLine($"\nПроверка:\n{vecFun.InvokeWithArgs(sim)}");
            
            var newTon = new Newton(matrixFun, vecFun, sim);
            var solveNewton = newTon.Solve();
            Console.WriteLine($"\nРешение методом Ньютона:\n{solveNewton}");
            Console.WriteLine($"\nПроверка:\n{vecFun.InvokeWithArgs(solveNewton)}");

        }
    }
}