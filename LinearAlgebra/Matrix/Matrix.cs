using System;
using System.Globalization;
using System.Linq;
using System.Text;
using LinearAlgebra.Matrix.LinearEquations;

namespace LinearAlgebra.Matrix
{
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
                var tmpVector = new Vector.Vector(_matrix.ColumnCount)
                {
                    [i] = 1
                };

                return GaussElimination.Solve(_matrix, tmpVector);
            }).ToArray();

            return vectors.ToMatrix();
        }

        public Vector.Vector GetLastColumn()
        {
            var matrix = new Matrix(this);
            var vector = new Vector.Vector(ColumnCount - 1);
            for (int i = 0; i < ColumnCount - 1; i++)
            {
                vector[i] = matrix[i,ColumnCount - 1];
            }

            return vector;
        }

        public Vector.Vector GetRow(int Row)
        {
            var vector = new Vector.Vector(ColumnCount);
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

        public Matrix AddColumn(Vector.Vector vec)
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
        
        public static Matrix Concat(Matrix one, Vector.Vector other) {
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
}