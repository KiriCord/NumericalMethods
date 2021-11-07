using System;
using LinearAlgebra;
using LinearAlgebra.Matrix;
using LinearAlgebra.Matrix.LinearEquations;
using LinearAlgebra.Vector;

namespace Lab1
{
    public static class Program
    {
        static void Main(string[] args)
        {
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
            Console.WriteLine($"\nВектор невязки(r = Ax - f):\n{residual.ToString(" #0.000000000000;-#0.000000000000;0.000000000000")}");

            decimal det = matrix.Determinant();
            Console.WriteLine($"\nОпределитель:\n{det}");
            
            Matrix invMat = matrix.Inv();
            Console.WriteLine($"\nОбратная матрица:\n{invMat}");

            Matrix res = GaussElimination.Check(matrix, invMat);
            Console.WriteLine($"\nМатрица проверки(A * A^(-1)):\n{res}");
            
            Console.WriteLine($"\n-------------------------------\n");
            
            decimal[,] matrixMain2 = 
            {
                { 24m, -7m, -4m, 4m },
                { -7m, 21m, 3m, -5m },
                { -4m, 3m, 19m, 7m },
                { 4m, -5m, 7m, 20m }
            };
            
            decimal[] VecF2 = { 20m, -16m, 14m, -81m };
            
            Matrix matrix2 = new Matrix(matrixMain2);
            Console.WriteLine($"\nИсходная матрица:\n{matrix2}");
            Vector f2 = new Vector(VecF2);
            Console.WriteLine($"\nf:\n{f2}");

            SquareRoot squareRoot = new SquareRoot(matrix2, f2);
            Vector result = squareRoot.Solve();
            Console.WriteLine($"\nРешение методом квадратного корня:\n{result}");
            
            Vector residual2 = new Vector(matrix2 * result - f2); 
            Console.WriteLine($"\nВектор невязки от квадратного корня:\n{residual2.ToString(" #0.000000000000;-#0.000000000000;0.000000000000")} \n");
            
            MinimumResidual minimumResidual = new MinimumResidual(matrix2, f2, result.Truncate());
            Vector vecMinimRes = minimumResidual.Solve();
            Console.WriteLine($"\nРешение минимальной невязки:\n{vecMinimRes}");
            
            Vector residual3 = new Vector(matrix2 * vecMinimRes - f2); 
            Console.WriteLine($"\nВектор невязки от минимальной невязки:\n{residual3.ToString(" #0.000000000000;-#0.000000000000;0.000000000000")} \n");

            decimal num = Norma.InfMatrixNorm(matrix2) * Norma.InfMatrixNorm(matrix2.Inv());
            Console.WriteLine($"\nЧисло обусловленности матрицы:\n{num}");
        }
    }
}