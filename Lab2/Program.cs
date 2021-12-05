/*
-------------------------------------------------------------------------------
    Лабораторная работа #2 / Рахманов Артур / Группа 47 / Вариант 17
    
    Исходная матрица (в)

    1) Решить СЛАУ методом квадратного корня(а).
    
    2) Вычислить невязку (Ax-b), где x - полученное решение.
    
    3) Уточнить полученное решение методом минимальных невязок(д)
    
    4) Вычислить число обусловленности матрицы система M = ||A|| * ||A^(-1)||
-------------------------------------------------------------------------------
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

    public static class MatrixExtension
    {
        public static Matrix ToMatrix(this Vector[] vectors)
        {
            var firstVectorN = vectors[0].Count;
            if (vectors.Skip(1).Any(vector => vector.Count != firstVectorN))
            {
                throw new Exception("БЛИН");
            }

            var result = new Matrix(firstVectorN, vectors.Length);

            for (int i = 0; i < vectors.Length; i++)
            {
                for (int j = 0; j < firstVectorN; j++)
                {
                    result[j, i] = vectors[i][j];
                }
            }

            return result;
        }
        
        public static Matrix ToTriangle(this Matrix matrix)
        {
            Matrix result = new Matrix(matrix);
            for (int i = 0; i < result.RowCount - 1; i++)
            {
                for (int j = i + 1; j < result.ColumnCount; j++)
                {
                    decimal koef = result[j, i] / result[i, i];
                    for (int k = i; k < result.RowCount; k++)
                        result[j, k] -= result[i, k] * koef;
                }
            }

            return result;
        }
    }
    
    public class Matrix
    {
        private readonly decimal[,] _matrx;
        
        public Matrix Transponse
        {
            get
            {
                var t = new Matrix(ColumnCount, RowCount);
                for(var i = 0; i < RowCount; i++)
                for (int j = 0; j < ColumnCount; j++)
                {
                    t[j, i] = this[i, j];
                }

                return t;
            }   
        }
        
        public Matrix(decimal[,] matrix)
        {
            _matrx = matrix;
        }

        private Matrix(Matrix matrix)
        {
            _matrx = new decimal[matrix.RowCount, matrix.ColumnCount];
            for (var i = 0; i < matrix.RowCount; i++)
            {
                for (var j = 0; j < matrix.ColumnCount; j++)
                {
                    _matrx[i, j] = matrix[i, j];
                }
            }
        }

        public Matrix(int N, int M)
        {
            _matrx = new decimal[N, M];
        }

        public decimal this[int i, int j]
        {
            get => _matrx[i, j];
            set => _matrx[i, j] = value;
        }
        
        
        public static implicit operator decimal[,](Matrix matrix) => new Matrix(matrix)._matrx;
        public int RowCount => _matrx.GetUpperBound(0) +1;

        public int ColumnCount => _matrx.GetUpperBound(1) + 1;
        
        public Matrix Inv()
        {
            Matrix _matrix = new Matrix(this);
            var vectors = ParallelEnumerable.Range(0, _matrix.ColumnCount).AsOrdered().Select(i =>
            {
                var tmpVector = new Vector(_matrix.ColumnCount)
                {
                    [i] = 1
                };

                return GaussElimination.Solve(_matrix, tmpVector);
            }).ToArray();

            return vectors.ToMatrix();
        }

        public Vector GetLastColumn()
        {
            var matrix = new Matrix(this);
            var vector = new Vector(ColumnCount - 1);
            for (int i = 0; i < ColumnCount - 1; i++)
            {
                vector[i] = matrix[i,ColumnCount - 1];
            }

            return vector;
        }

        public Vector GetRow(int Row)
        {
            var vector = new Vector(ColumnCount);
            for (int i = 0; i < ColumnCount; i++)
            {
                vector[i] = this[Row, i];
            }

            return vector;
        }
        
        public int MaxElementRow(int row)
        {
            var matrix = new Matrix(this);
            decimal temp = matrix[row, 0];
            int index = 0;
            for(int i = 0; i < ColumnCount; i++)
            {  
                if (Math.Abs(matrix[row, i]) > Math.Abs(temp))
                {
                    temp = matrix[row, i];
                    index = i;
                }
            }

            return index;
        }

        public decimal Determinant()
        {
            var matrix = new Matrix(this);
            const decimal eps = 1E-9m;
            decimal det = 1;
            if (matrix.ColumnCount != matrix.RowCount)
                throw new Exception("Determinant NOPE");

            for (int i = 0; i < matrix.RowCount; i++)
            {
                int k = i;
                for (int j = i + 1; j < matrix.ColumnCount; j++)
                {
                    if (Math.Abs(matrix[j, i]) > Math.Abs(matrix[k, i]))
                        k = j;
                }
                
                if (Math.Abs(matrix[k,i]) < eps)
                {
                    det = 0;
                    break;
                }

                if (i != k)
                    det = -det;
                det *= matrix[i, i];
                for (int j = i + 1; j < matrix.RowCount; j++)
                    matrix[i, j] /= matrix[i, i];
                for (int j = 0; j < matrix.RowCount; ++j)
                    if ((j != i) && (Math.Abs(matrix[j,i]) > eps))
                        for (k = i + 1; k < matrix.RowCount; ++k)
                            matrix[j,k] -= matrix[i,k] * matrix[j,i];
            }
            return det;
        }
        
        public void SwapColumn(int columnA, int columnB)
        {
            if(columnA == columnB)
                return;
            for(int i = 0; i < this.ColumnCount; i++)
                (this[i, columnA], this[i, columnB]) = (this[i, columnB], this[i, columnA]);
        }
        
        public void SwapRow(int rowA, int rowB)
        {
            if(rowA == rowB)
                return;
            for(int i = 0; i < this.RowCount; i++)
                (this[rowA, i], this[rowB, i]) = (this[rowB, i], this[rowA, i]);
        }

        public Matrix AddColumn(Vector vec)
        {
            if(vec.Count != this.RowCount)
                throw new Exception("GG");
            
            decimal[,] temp = new decimal[this.RowCount, this.ColumnCount + 1];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for(int j = 0; j < temp.GetLength(1); j++)
                    if (j < this.RowCount)
                        temp[i, j] = this[i, j];
                    else
                    {
                        temp[i, j] = vec[i];
                    }
            }

            return new Matrix(temp);
        }
        
        
        public override string ToString() => this.ToString(" #0.0000;-#0.0000; 0.0000");

        
        public string ToString(string format)
        {
            var builder = new StringBuilder();


            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    builder.Append(
                        $"{this[i, j].ToString(format, CultureInfo.InvariantCulture)} ");
                }

                if (i < RowCount - 1)
                {
                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }
        
        public static Matrix Concat(Matrix one, Vector other) {
            if (one.RowCount != other.Count) throw new Exception("Nope");

            var result = new Matrix(one.RowCount, one.ColumnCount + 1);

            for (var i = 0; i < result.RowCount; i++)
            for (var j = 0; j < result.ColumnCount; j++)
                result[i, j] = j == one.ColumnCount ? other[i] : one[i, j];

            return result;
        }
        
        public static Matrix Concat(Matrix one, Matrix other) {
            if (one.RowCount != other.RowCount) throw new Exception("Nope");

            var result = new Matrix(one.RowCount, one.ColumnCount + 1);

            for (var i = 0; i < result.RowCount; i++)
            for (var j = 0; j < result.ColumnCount; j++)
                result[i, j] = j == one.ColumnCount ? other[i, 0] : one[i, j];

            return result;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            var resultMatrix = new decimal[a._matrx.GetLength(0), b._matrx.GetLength(1)];
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < b.ColumnCount; j++)
                {
                    resultMatrix[i,j] = a._matrx[i,j] + b._matrx[i,j];
                }
            }
            return new Matrix(resultMatrix);
        }
        
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.ColumnCount != b.RowCount)
                throw new Exception("Multiplication NOPE");
            var resultMatrix = new decimal[a.RowCount, b.ColumnCount];
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < b.ColumnCount; j++)
                {
                    resultMatrix[i, j] = 0;
                    for (int k = 0; k < a.ColumnCount; k++)
                    {
                        resultMatrix[i,j] += a[i,k] * b[k,j];
                        //resultMatrix[i, j] = Decimal.Round(resultMatrix[i, j], 12);
                    }
                }
            }
            return new Matrix(resultMatrix);
        }
        
        public static Matrix operator *(Matrix matrx, int num)
        {
            var resultMatrix = new decimal[matrx.RowCount, matrx.ColumnCount];
            for (int i = 0; i < matrx.RowCount; i++)
            {
                for (int j = 0; j < matrx.ColumnCount; j++)
                {
                    resultMatrix[i, j] = matrx[i,j] * num;
                }
            }
            return new Matrix(resultMatrix);
        }
        
        
        public static Matrix operator -(Matrix a, Matrix b)
        {
            var resultMatrix = new decimal[a._matrx.GetLength(0), b._matrx.GetLength(1)];
            for (var i = 0; i < a.RowCount; i++)
            {
                for (var j = 0; j < b.ColumnCount; j++)
                {
                    resultMatrix[i,j] = a._matrx[i,j] - b._matrx[i,j];
                    //resultMatrix[i, j] = Decimal.Round(resultMatrix[i,j], 12);
                }
            }
            return new Matrix(resultMatrix);
        }
        
    }
    
    public class Vector : IEnumerable<decimal>
    {
        private decimal[] _elements;

        public Vector(decimal[] elements)
        {
            _elements = elements;
        }
        
        public Vector(Vector vec)
        {
            _elements = new decimal[vec.Count];

            for (int i = 0; i < vec.Count; i++)
            {
                _elements[i] = vec[i];
            }
        }
        
        public Vector(int count)
        {
            _elements = new decimal[count];

            for (int i = 0; i < Count; i++)
            {
                _elements[i] = 0;
            }
        }

        public int Count => _elements.Length;

        public decimal this[int i]
        {
            get => _elements[i];
            set => _elements[i] = value;
        }

       /* public override string ToString()
        {
            return string.Join("\n\r", _elements);
        }*/
       
       public override string ToString()
       {
           return ToString(" #0.0000;-#0.0000;0.0000");
       }

       public string ToString(string format) => string.Join('\n',
           this.Select(value => value.ToString(format, CultureInfo.InvariantCulture)));

       public Vector Truncate()
       {
           Vector resultVector = new Vector(this);
           for (int i = 0; i < this.Count; i++)
           {
               resultVector[i] = Math.Truncate(resultVector[i]);
           }

           return resultVector;
       }
       
        
        public static Vector operator *(Matrix a, Vector b)
        {
            if (a.ColumnCount != b.Count)
                throw new Exception("Multiplication NOPE");
            //var resultMatrix = new decimal[a.RowCount, b.ColumnCount];
            Vector resultVector = new Vector(a.ColumnCount);
            for (int i = 0; i < a.RowCount; i++)
            {
                for (int j = 0; j < b.Count; j++)
                {
                    resultVector[i] += a[i,j] * b[j];
                }
            }
            return resultVector;
        }
        
        public static decimal operator *(Vector a, Vector b)
        {
            if (a.Count != b.Count)
                throw new Exception("Multiplication NOPE");
            //var resultMatrix = new decimal[a.RowCount, b.ColumnCount];
            decimal resultVector = 0;
            for (int i = 0; i < a.Count; i++)
            {
                resultVector += a[i] * b[i];
            }
            return resultVector;
        }
        
        public static Vector operator *(Vector a, decimal b)
        {
            Vector resultVector = new Vector(a.Count);
            for (int i = 0; i < a.Count; i++)
            {
                resultVector[i] += a[i] * b;
            }
            return resultVector;
        }

        public static Vector operator -(Vector a, Vector b)
        {
            if (a.Count != b.Count)
                throw new Exception("Subtract NOPE");

            Vector resultVec = new Vector(a.Count);

            for (int i = 0; i < a.Count; i++)
            {
                resultVec[i] = a[i] - b[i];
            }

            return resultVec;
        }
        
        IEnumerator<decimal> IEnumerable<decimal>.GetEnumerator() => ((IEnumerable<decimal>)_elements).GetEnumerator();

        public IEnumerator GetEnumerator() => _elements.GetEnumerator();
        
    }
    
    public static class GaussElimination
    {
        public static Vector Solve(Matrix matrix, Vector vec)
        {
            Matrix _matrix = matrix.AddColumn(vec);
            Matrix forwaredMatrix = Forward(_matrix);
            Matrix backwaredMatrix = Backward(forwaredMatrix);
            _matrix = backwaredMatrix;

            Vector X = new Vector(_matrix.RowCount);
            for (int i = 0; i < X.Count; i++)
            {
                X[i] = _matrix[i, _matrix.RowCount];
            }

            return X;
        }
        
        public static Matrix Forward(Matrix matrixF)
        {
            Matrix copyMatrixF = new Matrix(matrixF);
            for (int k = 0; k < matrixF.RowCount; k++) 
            {
                for (int i = 0; i < matrixF.ColumnCount; i++) 
                    copyMatrixF[k, i] /= matrixF[k, k]; 
                for (int i = k + 1; i < matrixF.RowCount; i++) 
                {
                    decimal K = copyMatrixF[i, k] / copyMatrixF[k, k];
                    for (int j = 0; j < matrixF.ColumnCount; j++)
                    {
                        copyMatrixF[i, j] -= copyMatrixF[k, j] * K;
                    }
                }
            }
            return copyMatrixF;
        }
        
         public static Matrix Backward(Matrix matrixF)
        {
            Matrix copyMatrixF = new Matrix(matrixF);
            for (int k = matrixF.RowCount - 1; k > -1; k--)
            {
                for (int i = matrixF.ColumnCount - 1; i > -1; i--)
                    copyMatrixF[k, i] /= matrixF[k, k];
                for (int i = k - 1; i > -1; i--)
                {
                    decimal K = copyMatrixF[i, k] / copyMatrixF[k, k];
                    for (int j = matrixF.ColumnCount - 1; j > -1; j--)
                    {
                        copyMatrixF[i, j] -= copyMatrixF[k, j] * K;
                    }
                }
            }
            return copyMatrixF;
        }
         
         public static Matrix Check(Matrix matrix, Matrix invMatrix)
         {
             return new Matrix(matrix * invMatrix);
         }
    }
    
    
    public class SquareRoot
    {
        public Matrix Coefficient { get; set; }
        public Vector Answers { get; set; }
        
        public SquareRoot(Matrix coeff, Vector answers)
        {
            Coefficient = coeff;
            Answers = answers;
        }
        

        public Vector Solve()
        {
            var l = FindLDecomposition();
            Console.WriteLine($"\nМатрица S:\n{l}");
            var result = GaussElimination.Forward(Matrix.Concat(l, Answers));
            return GaussElimination.Backward(Matrix.Concat(l.Transponse, result.GetLastColumn())).GetLastColumn();
        }
        
        private Matrix FindLDecomposition() {
            var n = Coefficient.RowCount;
            var l = new Matrix(n, n);
            for (var i = 0; i < n; i++) {
                decimal sumLs = 0;
                for (var k = 0; k < i; k++)
                {
                    sumLs += l[i, k] * l[i, k];
                }

                l[i, i] = (decimal)Math.Sqrt((double)(Coefficient[i, i] - sumLs));

                for (var j = i; j < n; j++) {
                    decimal suml = 0;
                    for (int k = 0; k < i; k++)
                    {
                        suml += l[i, k] * l[j, k];
                    }
                    
                    l[j, i] = (Coefficient[j, i] - suml) / l[i, i];
                }
            }

            return l;
        }
    }
    
    public class Norma
    {
        public static decimal TwoVectorNorm(Vector vector)
        {
            return (decimal)Math.Sqrt((double)vector.Sum(item => item * item));
        }

        public static decimal InfMatrixNorm(Matrix matrix)
        {
            IEnumerable<decimal> RowNorm(Matrix rows) {
                for (var i = 0; i < rows.RowCount; i++)
                    yield return rows.GetRow(i).Sum(Math.Abs);
            }

            return RowNorm(matrix).Max();
        }
    }
    
    public class MinimumResidual
    {
        public Matrix Coefficient { get; }
        public Vector Answers { get; }
        public Vector Guess { get; set; }
        public decimal Tau { get; private set; }
        
        public MinimumResidual(Matrix coefficient, Vector answers, Vector guess) {
            Coefficient = coefficient;
            Answers = answers;
            Guess = guess;
            Tau = 1;
        }

        public Vector Solve()
        {
            var xk = Guess;
            var xkp = new Vector(Answers.Count);
            var iterCount = 0;
            do
            {
                xkp = new Vector(xk);
                TauNext(xk);
                xk = xk - Residual(xk) * Tau;
                iterCount++;
            } while (Norma.TwoVectorNorm(xk - xkp) > 1e-6m);
            
            Console.WriteLine(
                $"Номер итерации {iterCount}");
            return xk;
        }
        
        private Vector Residual(Vector vector) {
            return Coefficient * vector - Answers;
        }

        public void TauNext(Vector vector) {
            var res = Residual(vector);
            var ares = Coefficient * res;
            Tau = (ares * res) / (Norma.TwoVectorNorm(ares) * Norma.TwoVectorNorm(ares));
        }
    }
    
    
    class Program
    {
        static void Main(string[] args)
        {
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
            
            decimal[,] D = 
            {
                { 1m, 0m, -0m, 0m },
                { 0m, 1m, 0m, 0m },
                { 0m, 0m, 1m, 0m },
                { 0m, 0m, 0m, 1m }
            };

            Matrix matrixD = new Matrix(D);

            Matrix matrixS = new Matrix(matrixMain2);
            //matrixS = GaussElimination.Forward(matrixS);
            //Console.WriteLine($"\nМатрица S:\n{matrixS}");

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