using System;
using LinearAlgebra.Matrix;
using LinearAlgebra.Matrix.LinearEquations;
using LinearAlgebra.Vector;

namespace Lab1
{
    public static class Program
    {
        static void Main(string[] args)
        {
            decimal[,] matrixMainF = 
            {
                { 1.00m, 0.47m, -0.11m, 0.55m, 1.09m },
                { 0.42m, 1.00m, 0.35m, 0.17m, 2.87m },
                { -0.25m, 0.67m, 1.00m, 0.36m, 3.65m },
                { 0.54m, -0.32m, -0.74m, 1.00m, 4.43m }
            };
            
            decimal[,] matrixMain = 
            {
                { 1.00m, 0.47m, -0.11m, 0.55m },
                { 0.42m, 1.00m, 0.35m, 0.17m },
                { -0.25m, 0.67m, 1.00m, 0.36m },
                { 0.54m, -0.32m, -0.74m, 1.00m }
            };
            
            decimal[] VecF = { 1.09m, 2.87m, 3.65m, 4.43m };

            Matrix matrix = new Matrix(matrixMain);
            Console.WriteLine($"\nИсходная матрица:\n{matrix}");
            
            Vector f = new Vector(VecF);
            Console.WriteLine($"\nf:\n{f}");
            
            Vector X = GaussElimination.Solve(matrix, f);
            Console.WriteLine($"\nРешения X:\n{X}");

            Vector residual = new Vector(matrix * X - f); 
            Console.WriteLine($"\nВектор невязки(r = Ax - f):\n{residual}");

            decimal det = matrix.Determinant();
            Console.WriteLine($"\nОпределитель:\n{det}");
            
            Matrix invMat = matrix.Inv();
            Console.WriteLine($"\nОбратная матрица:\n{invMat}");

            Matrix res = GaussElimination.Check(matrix, invMat);
            Console.WriteLine($"\nМатрица проверки(A * A^(-1)):\n{res}");
        }
    }
}