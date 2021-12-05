using System;
using System.Linq;

namespace LinearAlgebra.Matrix
{
    public class MatrixFunction {
        private Func<Vector.Vector, decimal>[,] _functions;
        private int _size;

        public MatrixFunction(Func<Vector.Vector, decimal>[,] functions) {
            _functions = functions;
            _size = functions.GetLength(0);
        }

        public Matrix InvokeWithArgs(Vector.Vector xs) {
            var res = new Matrix(_size, _size);
            for (var i = 0; i < _size; i++) {
                for (var j = 0; j < _size; j++) {
                    res[i, j] = _functions[i, j](xs);
                }
            }

            return res;
        }
    }
}