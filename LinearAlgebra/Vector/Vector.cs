using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LinearAlgebra.Vector
{
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
       
        
        public static Vector operator *(Matrix.Matrix a, Vector b)
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
}