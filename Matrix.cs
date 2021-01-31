using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiscUtils
{
    namespace Matrix
    {
        /// <summary>
        /// Базовый класс
        /// </summary>
        public class MatrixBase
        {
            /// <summary>
            /// Создание матрицы
            /// </summary>
            /// <param name="rows">Количество строк</param>
            /// <param name="cols">Количество столбцов</param>
            /// <returns>Новая матрица</returns>
            public static double[,] Create(int rows, int cols)
            {
                // Создаем матрицу, полностью инициализированную
                // значениями 0.0. Проверка входных параметров опущена.
                double[,] result = new double[rows, cols];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        result[i, j] = 0;
                return result;
            }

            // ======================================
            //    МАТРИЦА В ВИДЕ СТРОКИ
            // ======================================
            public static string ToString(double[,] matrix)
            {
                string s = "";
                for (int i = 0; i < matrix.GetLength(0); ++i)
                {
                    for (int j = 0; j < matrix.GetLength(1); ++j)
                        s += matrix[i, j].ToString("F3").PadLeft(8) + " ";
                    s += Environment.NewLine;
                }
                return s;
            }

            // ======================================
            //    УМНОЖЕНИЕ
            // ======================================

            /// <summary>
            /// Умножение двух матриц
            /// </summary>
            /// <param name="matrixA">Первая матрица</param>
            /// <param name="matrixB">Вторая матрица</param>
            /// <returns>Новая матрица</returns>
            public static double[,] Mult(double[,] matrixA, double[,] matrixB)
            {
                int aRows = matrixA.GetLength(0); int aCols = matrixA.GetLength(1);
                int bRows = matrixB.GetLength(0); int bCols = matrixB.GetLength(1);
                if (aCols != bRows)
                    throw new Exception("Non-conformable matrices in MatrixProduct");
                double[,] result = Create(aRows, bCols);
                Parallel.For(0, aRows, i =>
                {
                    for (int j = 0; j < bCols; ++j)
                        for (int k = 0; k < aCols; ++k)
                            result[i, j] += matrixA[i, k] * matrixB[k, j];
                }
                );
                return result;
            }

            /// <summary>
            /// Умножение двумерной матрицы на вектор-столбец
            /// </summary>
            /// <param name="matrixA">Двумерная матрица</param>
            /// <param name="matrixB">Вектор-столбец</param>
            /// <returns>Новая матрица</returns>
            public static double[] Mult(double[,] matrixA, double[] matrixB)
            {
                int aRows = matrixA.GetLength(0); int aCols = matrixA.GetLength(1);
                int bRows = matrixB.GetLength(0); int bCols = 1;
                if (aCols != bRows)
                    throw new Exception("Non-conformable matrices in MatrixProduct");
                double[] result = new double[4] { 0, 0, 0, 0 };
                Parallel.For(0, aRows, i =>
                {
                    for (int j = 0; j < bCols; ++j)
                        for (int k = 0; k < aCols; ++k)
                            result[i] += matrixA[i, k] * matrixB[k];
                }
                );
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class Matrix : MatrixBase
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public Matrix()
            {

            }

            /// <summary>
            /// Создание копии матрицы вида double[,]
            /// </summary>
            /// <param name="matrix">Копируемая матрица</param>
            /// <returns>Новая матрица</returns>
            public static double[,] Copy(double[,] matrix)
            {
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1);

                double[,] result = Create(rows, cols);
                for (int i = 0; i < rows; ++i) // copy the values
                    for (int j = 0; j < cols; ++j)
                        result[i, j] = matrix[i, j];
                return result;
            }


            // =================================================
            // ЕДИНИЧНАЯ МАТРИЦА
            // =================================================
            /// <summary>
            /// Создание единичной квадратной матрицы double[,]
            /// </summary>
            /// <param name="n">Количество строк и столбцов</param>
            /// <returns>Новая матрица</returns>
            public static double[,] Identity(int n)
            {
                // return an n x n Identity matrix
                double[,] result = Create(n, n);
                for (int i = 0; i < n; ++i)
                    result[i, i] = 1.0;

                return result;
            }

            // ======================================
            //    ТРАНСПОНИРОВАНИЕ
            // ======================================

            /// <summary>
            /// Транспонирование ОДНОМЕРНОГО массива
            /// </summary>
            /// <param name="m"></param>
            /// <returns></returns>
            public static int[,] T(int[] m)
            {
                var result = new int[m.GetLength(0), 1];

                for (int y = 0; y < m.GetLength(0); y++)
                    result[y, 0] = m[y];

                return result;
            }
            public static double[,] T(double[] m)
            {
                var result = new double[m.GetLength(0), 1];

                for (int y = 0; y < m.GetLength(0); y++)
                    result[y, 0] = m[y];

                return result;
            }

            // Транспонирование МНОГОМЕРНОГО массива
            public static int[,] T(int[,] m)
            {
                var result = new int[m.GetLength(1), m.GetLength(0)];

                for (int y = 0; y < m.GetLength(1); y++)
                    for (int x = 0; x < m.GetLength(0); x++)
                        result[y, x] = m[x, y];

                return result;
            }

            // ======================================
            //    ВЫЧИТАНИЕ
            // ======================================
            // This function subtracts B[,] from A[,]
            public static double[,] Subtract(double[,] matrixA, double[,] matrixB)
            {
                int aRows = matrixA.GetLength(0); int aCols = matrixA.GetLength(1);
                int bRows = matrixB.GetLength(0); int bCols = matrixB.GetLength(1);
                if (aCols != bCols || aRows != bRows)
                    throw new Exception("Размерность матриц не одинакова");
                double[,] result = Create(aRows, bCols);

                for (int i = 0; i < aRows; i++)
                    for (int j = 0; j < aCols; j++)
                        result[i, j] += matrixA[i, j] - matrixB[i, j];

                return result;
            }

            // ======================================
            //    СЛОЖЕНИЕ
            // ======================================
            // This function adding B[,] and A[,]
            public static double[,] Add(double[,] matrixA, double[,] matrixB)
            {
                int aRows = matrixA.GetLength(0); int aCols = matrixA.GetLength(1);
                int bRows = matrixB.GetLength(0); int bCols = matrixB.GetLength(1);
                if (aCols != bCols || aRows != bRows)
                    throw new Exception("Размерность матриц не одинакова");
                double[,] result = Create(aRows, bCols);

                for (int i = 0; i < aRows; i++)
                    for (int j = 0; j < aCols; j++)
                        result[i, j] += matrixA[i, j] + matrixB[i, j];

                return result;
            }

            // ======================================
            //    МАТРИЦЫ ЭКВИВАЛЕНТНЫ?
            // ======================================
            public static bool IsEqual(double[,] matrixA, double[,] matrixB, double epsilon)
            {
                // true if all values in matrixA == corresponding values in matrixB
                int aRows = matrixA.Length; int aCols = matrixA.GetLength(0);
                int bRows = matrixB.Length; int bCols = matrixB.GetLength(0);
                if (aRows != bRows || aCols != bCols)
                    throw new Exception("Non-conformable matrices in MatrixAreEqual");

                for (int i = 0; i < aRows; ++i) // each row of A and B
                    for (int j = 0; j < aCols; ++j) // each col of A and B
                                                    //if (matrixA[i][j] != matrixB[i][j])
                        if (Math.Abs(matrixA[i, j] - matrixB[i, j]) > epsilon)
                            return false;
                return true;
            }

            // ======================================
            //    ОПРЕДЕЛИТЕЛЬ (ДЕТЕРМИНАНТ)
            // ======================================
            public static double Determinant(double[,] matrix)
            {
                int[] perm;
                int toggle;
                double[,] lum = Decompose(matrix, out perm, out toggle);
                if (lum == null)
                    throw new Exception("Unable to compute MatrixDeterminant");
                double result = toggle;
                for (int i = 0; i < lum.GetLength(0); ++i)
                    result *= lum[i, i];
                return result;
            }

            // ======================================
            //    РАЗЛОЖЕНИЕ МАТРИЦЫ - МЕТОД ДУЛЛИТЛА
            // ======================================
            public static double[,] Decompose(double[,] matrix, out int[] perm, out int toggle)
            {
                // Doolittle LUP decomposition with partial pivoting.
                // rerturns: result is L (with 1s on diagonal) and U; perm holds row permutations; toggle is +1 or -1 (even or odd)
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1); // assume all rows have the same number of columns so just use row [0].
                if (rows != cols)
                    throw new Exception("Attempt to MatrixDecompose a non-square mattrix");

                int n = rows; // convenience

                double[,] result = Copy(matrix); // make a copy of the input matrix

                perm = new int[n]; // set up row permutation result
                for (int i = 0; i < n; ++i) { perm[i] = i; }

                toggle = 1; // toggle tracks row swaps. +1 -> even, -1 -> odd. used by MatrixDeterminant

                for (int j = 0; j < n - 1; ++j) // each column
                {
                    double colMax = Math.Abs(result[j, j]); // find largest value in col j
                    int pRow = j;
                    for (int i = j + 1; i < n; ++i)
                    {
                        if (result[i, j] > colMax)
                        {
                            colMax = result[i, j];
                            pRow = i;
                        }
                    }

                    if (pRow != j) // if largest value not on pivot, swap rows
                    {
                        //double[] rowPtr = result[pRow];
                        double[] rowPtr = GetColsInRow(result, pRow);
                        //result[pRow] = result[j];
                        result = SetColsInRow(result, pRow, GetColsInRow(result, j));
                        //result[j] = rowPtr;
                        result = SetColsInRow(result, j, rowPtr);

                        int tmp = perm[pRow]; // and swap perm info
                        perm[pRow] = perm[j];
                        perm[j] = tmp;

                        toggle = -toggle; // adjust the row-swap toggle
                    }

                    if (Math.Abs(result[j, j]) < 1.0E-20) // if diagonal after swap is zero . . .
                        return null; // consider a throw

                    for (int i = j + 1; i < n; ++i)
                    {
                        result[i, j] /= result[j, j];
                        for (int k = j + 1; k < n; ++k)
                        {
                            result[i, k] -= result[i, j] * result[j, k];
                        }
                    }
                } // main j column loop

                return result;
            } // MatrixDecompose

            /*
               МОИ ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ДЛЯ ДЕКОМПОЗИЦИИ (ОБРАЩЕНИЯ)
            */
            // Вспомогательный метод получения столбцов двумерного массива в строке
            private static double[] GetColsInRow(double[,] m, int row)
            {
                double[] result = new double[m.GetLength(1)];
                for (int i = 0; i < m.GetLength(1); i++)
                    result[i] = m[row, i];
                return result;
            }

            // Вспомогательный метод записи столбцов двумерного массива в строку
            private static double[,] SetColsInRow(double[,] m, int row, double[] cols)
            {
                double[,] duplm = Copy(m);
                for (int i = 0; i < duplm.GetLength(1); i++)
                    duplm[row, i] = cols[i];
                return duplm;
            }

            // ======================================
            //    ОБРАЩЕНИЕ МАТРИЦЫ
            // ======================================

            public static double[,] Inverse(double[,] matrix)
            {
                int n = matrix.GetLength(0);
                double[,] result = Copy(matrix);

                int[] perm;
                int toggle;
                double[,] lum = Decompose(matrix, out perm, out toggle);
                if (lum == null)
                    throw new Exception("Unable to compute inverse");

                double[] b = new double[n];
                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < n; ++j)
                    {
                        if (i == perm[j])
                            b[j] = 1.0;
                        else
                            b[j] = 0.0;
                    }

                    double[] x = HelperSolve(lum, b); // 

                    for (int j = 0; j < n; ++j)
                        result[j, i] = x[j];
                }
                return result;
            }

            // Вспомогательный метод для обращения матрицы
            private static double[] HelperSolve(double[,] luMatrix, double[] b) // helper
            {
                // before calling this helper, permute b using the perm array from MatrixDecompose that generated luMatrix
                int n = luMatrix.GetLength(0);
                double[] x = new double[n];
                b.CopyTo(x, 0);

                for (int i = 1; i < n; ++i)
                {
                    double sum = x[i];
                    for (int j = 0; j < i; ++j)
                        sum -= luMatrix[i, j] * x[j];
                    x[i] = sum;
                }

                x[n - 1] /= luMatrix[n - 1, n - 1];
                for (int i = n - 2; i >= 0; --i)
                {
                    double sum = x[i];
                    for (int j = i + 1; j < n; ++j)
                        sum -= luMatrix[i, j] * x[j];
                    x[i] = sum / luMatrix[i, i];
                }

                return x;
            }
        }

        // ======================================
        //    2D МАТРИЦА
        // ======================================

        /// <summary>
        /// Класс для 2D матричных трансформаций
        /// </summary>
        public class Matrix2D : MatrixBase
        {
            public double M11;
            public double M12;
            public double M13;
            public double M21;
            public double M22;
            public double M23;
            public double M31;
            public double M32;
            public double M33;


            /// <summary>
            /// Конструктор класса
            /// </summary>
            public Matrix2D()
            {

            }

            /// <summary>
            /// Создание пустой 2D матрицы (3х3) типа double[,]
            /// </summary>
            /// <returns>Новая матрица</returns>
            public static double[,] Create()
            {
                return new double[3, 3] {
                { 0,   0,   0 },
                { 0,   0,   0 },
                { 0,   0,   0 }
            };
            }

            /// <summary>
            /// Конвертация в двумерный масиив double[3,3]
            /// </summary>
            /// <returns>Двумерный массив double[3,3]</returns>
            public double[,] ToArray()
            {
                return new double[3, 3] {
                { M11,   M12,   M13 },
                { M21,   M22,   M23 },
                { M31,   M32,   M33 }
            };
            }

            /// <summary>
            /// Текстовый вид матрицы
            /// </summary>
            /// <returns>Строка</returns>
            public new string ToString()
            {
                return $"[ {M11}, {M12}, {M13} \n" +
                        $" {M21}, {M22}, {M23} \n" +
                        $" {M31}, {M32}, {M33} ]";
            }


            /// <summary>
            /// Матрица 2D трансформации: rotation + scale + offset
            /// </summary>
            /// <param name="angle">Угол поворота в градусах</param>
            /// <param name="scaleX">Растяжение по X</param>
            /// <param name="scaleY">Растяжение по Y</param>
            /// <param name="offsX">Смещение по Х</param>
            /// <param name="offsY">Смещение по Y</param>
            /// <returns>Результирующая матрица</returns>
            public double[,] FullTransform(double angle, double scaleX, double scaleY, double offsX, double offsY)
            {
                /* 
                    Матрица поворота:
                    cosQ    sinQ    0
                    -sinQ   cosQ    0
                    offX    offY    1
                */

                double angleRadian = angle * Math.PI / 180; //переводим угол в радианты

                double ma = Math.Cos(angleRadian) + scaleX;
                double mb = Math.Sin(angleRadian);
                double mc = -Math.Sin(angleRadian);
                double md = Math.Cos(angleRadian) + scaleY;

                return new double[3, 3] {
                { ma,   mb,     0 },
                { mc,   md,     0 },
                { offsX, offsY, 1 }
            };
            }
            /// <summary>
            /// Матрица 2D трансформации: rotation + scale + offset
            /// </summary>
            /// <param name="angle">Угол поворота в градусах</param>
            /// <param name="scaleX">Растяжение по X</param>
            /// <param name="scaleY">Растяжение по Y</param>
            /// <param name="offsX">Смещение по Х</param>
            /// <param name="offsY">Смещение по Y</param>
            /// <returns>Результирующая матрица</returns>
            public static double[,] FullTransform2D(double angle, double scaleX, double scaleY, double offsX, double offsY)
            {
                /* 
                    Матрица поворота:
                    cosQ    sinQ    0
                    -sinQ   cosQ    0
                    offX    offY    1
                */

                double angleRadian = angle * Math.PI / 180; //переводим угол в радианы

                double ma = Math.Cos(angleRadian) + scaleX;
                double mb = Math.Sin(angleRadian);
                double mc = -Math.Sin(angleRadian);
                double md = Math.Cos(angleRadian) + scaleY;

                return new double[3, 3] {
                { ma,   mb,     0 },
                { mc,   md,     0 },
                { offsX, offsY, 1 }
            };
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="angle"></param>
            public void Rotate(double angle)
            {
                /* 
                    Матрица поворота:
                    cosQ    sinQ    0
                    -sinQ   cosQ    0
                    0       0       1
                */

                double angleRadian = angle * Math.PI / 180; //переводим угол в радианы

                M11 = Math.Cos(angleRadian); M12 = Math.Sin(angleRadian); M13 = 0;
                M21 = -Math.Sin(angleRadian); M22 = Math.Cos(angleRadian); M23 = 0;
                M31 = 0; M32 = 0; M33 = 1;
            }
            /// <summary>
            /// Матрица вращения 2D
            /// </summary>
            /// <param name="angle">Угол поворота в градусах</param>
            /// <returns>Новая матрица поворота</returns>
            public static double[,] Rotate2D(double angle)
            {
                /* 
                    Матрица поворота:
                    cosQ    sinQ    0
                    -sinQ   cosQ    0
                    0       0       1
                */

                double angleRadian = angle * Math.PI / 180; //переводим угол в радианты

                double ma = Math.Cos(angleRadian);
                double mb = Math.Sin(angleRadian);
                double mc = -Math.Sin(angleRadian);
                double md = Math.Cos(angleRadian);

                return new double[3, 3] {
                { ma,   mb,     0 },
                { mc,   md,     0 },
                { 0,    0,      1 }
            };
            }

            /// <summary>
            /// Матрица растяжения 2D
            /// </summary>
            /// <param name="Sx">X</param>
            /// <param name="Sy">Y</param>
            /// <returns></returns>
            public double[,] Scale(double Sx, double Sy)
            {
                /* 
                    Матрица поворота:
                    Sx   0    0
                    0    Sy   0
                    0    0    1
                */

                return new double[3, 3] {
                { Sx,   0,  0 },
                { 0,    Sy, 0 },
                { 0,    0,  1 }
            };
            }
            /// <summary>
            /// Матрица растяжения 2D
            /// </summary>
            /// <param name="Sx">X</param>
            /// <param name="Sy">Y</param>
            /// <returns></returns>
            public static double[,] Scale2D(double Sx, double Sy)
            {
                /* 
                    Матрица поворота:
                    Sx   0    0
                    0    Sy   0
                    0    0    1
                */

                return new double[3, 3] {
                { Sx,   0,  0 },
                { 0,    Sy, 0 },
                { 0,    0,  1 }
            };
            }

            /// <summary>
            /// Матрица перемещения 2D
            /// </summary>
            /// <param name="Tx">X</param>
            /// <param name="Ty">Y</param>
            /// <returns></returns>
            public void Translate(double Tx, double Ty)
            {
                /* 
                    Матрица поворота:
                    1   0   0
                    0   1   0
                    Tx  Ty  1
                */

                M11 = 1; M12 = 0; M13 = 0;
                M21 = 0; M22 = 1; M23 = 0;
                M31 = Tx; M32 = Ty; M33 = 1;
            }
            /// <summary>
            /// Матрица перемещения 2D
            /// </summary>
            /// <param name="Tx">X</param>
            /// <param name="Ty">Y</param>
            /// <returns></returns>
            public static double[,] Translate2D(double Tx, double Ty)
            {
                /* 
                    Матрица поворота:
                    1   0   0
                    0   1   0
                    Tx  Ty  1
                */

                return new double[3, 3] {
                { 1,    0,   0 },
                { 0,    1,   0 },
                { Tx,   Ty,  1 }
            };
            }


            // ПЕРЕГРУЗКА ОПЕРАТОРОВ

            public static Matrix2D operator *(Matrix2D a, Matrix2D b)
            {
                var m11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31;
                var m12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32;
                var m13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33;

                var m21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31;
                var m22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32;
                var m23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33;

                var m31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31;
                var m32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32;
                var m33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33;

                return new Matrix2D()
                {
                    M11 = m11,
                    M12 = m12,
                    M13 = m13,
                    M21 = m21,
                    M22 = m22,
                    M23 = m23,
                    M31 = m31,
                    M32 = m32,
                    M33 = m33
                };
            }
        }


        // ======================================
        //    3D МАТРИЦА
        // ======================================

        /// <summary>
        /// Класс для 3D матричных трансформаций
        /// </summary>
        public class Matrix3D : MatrixBase
        {
            public double M11;
            public double M12;
            public double M13;
            public double M14;
            public double M21;
            public double M22;
            public double M23;
            public double M24;
            public double M31;
            public double M32;
            public double M33;
            public double M34;
            public double M41;
            public double M42;
            public double M43;
            public double M44;


            /// <summary>
            /// Конструктор
            /// </summary>
            public Matrix3D()
            {

            }

            public void Create(double m11 = 0, double m12 = 0, double m13 = 0, double m14 = 0,
                                    double m21 = 0, double m22 = 0, double m23 = 0, double m24 = 0,
                                    double m31 = 0, double m32 = 0, double m33 = 0, double m34 = 0,
                                    double m41 = 0, double m42 = 0, double m43 = 0, double m44 = 0)
            {
                this.M11 = m11; this.M12 = m12; this.M13 = m13; this.M14 = m14;
                this.M21 = m21; this.M22 = m22; this.M23 = m23; this.M24 = m24;
                this.M31 = m31; this.M32 = m32; this.M33 = m33; this.M34 = m34;
                this.M41 = m41; this.M42 = m42; this.M43 = m43; this.M44 = m44;
            }

            /// <summary>
            /// Создание пустой матрицы double[4,4]
            /// </summary>
            /// <returns>Новая матрица</returns>
            public static double[,] Create()
            {
                return new double[4, 4] {
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 }
            };
            }

            public static Matrix3D Copy(Matrix3D m)
            {
                Matrix3D tmp = new Matrix3D();

                tmp.M11 = m.M11; tmp.M12 = m.M12; tmp.M13 = m.M13; tmp.M14 = m.M14;
                tmp.M21 = m.M21; tmp.M22 = m.M22; tmp.M23 = m.M23; tmp.M24 = m.M24;
                tmp.M31 = m.M31; tmp.M32 = m.M32; tmp.M33 = m.M33; tmp.M34 = m.M34;
                tmp.M41 = m.M41; tmp.M42 = m.M42; tmp.M43 = m.M43; tmp.M44 = m.M44;

                return tmp;
            }

            public Matrix3D Copy()
            {
                Matrix3D tmp = new Matrix3D();

                tmp.M11 = this.M11; tmp.M12 = this.M12; tmp.M13 = this.M13; tmp.M14 = this.M14;
                tmp.M21 = this.M21; tmp.M22 = this.M22; tmp.M23 = this.M23; tmp.M24 = this.M24;
                tmp.M31 = this.M31; tmp.M32 = this.M32; tmp.M33 = this.M33; tmp.M34 = this.M34;
                tmp.M41 = this.M41; tmp.M42 = this.M42; tmp.M43 = this.M43; tmp.M44 = this.M44;

                return tmp;
            }

            public static void Copy(Matrix3D from, Matrix3D to)
            {
                to.M11 = from.M11; to.M12 = from.M12; to.M13 = from.M13; to.M14 = from.M14;
                to.M21 = from.M21; to.M22 = from.M22; to.M23 = from.M23; to.M24 = from.M24;
                to.M31 = from.M31; to.M32 = from.M32; to.M33 = from.M33; to.M34 = from.M34;
                to.M41 = from.M41; to.M42 = from.M42; to.M43 = from.M43; to.M44 = from.M44;
            }

            /// <summary>
            /// Преобразование матрицы в массив double[4,4]
            /// </summary>
            /// <returns>Новая матрица</returns>
            public double[,] ToArray()
            {
                return new double[4, 4] {
                { M11, M12, M13, M14 },
                { M21, M22, M23, M24 },
                { M31, M32, M33, M34 },
                { M41, M42, M43, M44 }
            };
            }

            /// <summary>
            /// Текстовый вид матрицы
            /// </summary>
            /// <returns>Строка</returns>
            public new string ToString()
            {
                return $"[ {M11}, {M12}, {M13}, {M14} \n" +
                        $" {M21}, {M22}, {M23}, {M24} \n" +
                        $" {M31}, {M32}, {M33}, {M34} \n" +
                        $" {M41}, {M42}, {M43}, {M44} ]";
            }


            public void Scale(double Sx, double Sy, double Sz)
            {
                /*
                    Матрица масштабирования:
                        Sx  0   0   0
                        0   Sy  0   0
                        0   0   Sz  0
                        0   0   0   1
                 */

                Matrix3D sm = new Matrix3D();
                sm.Create(m11: Sx, m22: Sy, m33: Sz);

                Copy(sm, this);
            }

            public static double[,] Scale3D(double Sx, double Sy, double Sz)
            {
                /*
                    Матрица масштабирования:
                        Sx  0   0   0
                        0   Sy  0   0
                        0   0   Sz  0
                        0   0   0   1
                 */

                var sx = 1 * Sx;
                var sy = 1 * Sy;
                var sz = 1 * Sz;

                return new double[4, 4] {
                { sx, 0,  0,  0 },
                { 0,  sy, 0,  0 },
                { 0,  0,  sz, 0 },
                { 0,  0,  0,  1 }
            };
            }



            public void Translate(double Tx, double Ty, double Tz)
            {
                /*
                    Матрица перемещения:
                        1   0   0   Tx
                        0   1   0   Ty
                        0   0   1   Tz
                        0   0   0   1
                 */

                Matrix3D tm = new Matrix3D();
                tm.Create(m41: Tx, m42: Ty, m43: Tz);

                Copy(tm, this);
            }

            public static double[,] Translate3D(double Tx, double Ty, double Tz)
            {
                /*
                    Матрица перемещения:
                        1   0   0   Tx
                        0   1   0   Ty
                        0   0   1   Tz
                        0   0   0   1
                 */

                return new double[4, 4] {
                { 1, 0, 0, Tx },
                { 0, 1, 0, Ty },
                { 0, 0, 1, Tz },
                { 0, 0, 0, 1 }
            };
            }



            public void Rotate(double xa, double ya, double za)
            {
                /* 
                    Rx Матрица поворота:
                        1   0       0       0
                        0   cosQ   -sinQ    0
                        0   sinQ   cosQ     0
                        0   0       0       1

                    Ry Матрица поворота:
                        cosQ    0   sinQ    0
                        0       1   0       0
                        -sinQ   0   cosQ    0
                        0       0   0       1

                    Rz Матрица поворота:
                        cosQ   -sinQ    0   0
                        sinQ   cosQ     0   0
                        0       0       1   0
                        0       0       0   1
                */

                Matrix3D xr = new Matrix3D();
                Matrix3D yr = new Matrix3D();
                Matrix3D zr = new Matrix3D();

                // Матрица поворота вокруг оси Ох
                xr.M11 = 1; xr.M12 = 0; xr.M13 = 0; xr.M14 = 0;
                xr.M21 = 0; xr.M22 = cos(xa); xr.M23 = -sin(xa); xr.M24 = 0;
                xr.M31 = 0; xr.M32 = sin(xa); xr.M33 = cos(xa); xr.M34 = 0;
                xr.M41 = 0; xr.M42 = 0; xr.M43 = 0; xr.M44 = 1;

                // Матрица поворота вокруг оси Оy
                yr.M11 = cos(ya); yr.M12 = 0; yr.M13 = sin(ya); yr.M14 = 0;
                yr.M21 = 0; yr.M22 = 1; yr.M23 = 0; yr.M24 = 0;
                yr.M31 = -sin(ya); yr.M32 = 0; yr.M33 = cos(ya); yr.M34 = 0;
                yr.M41 = 0; yr.M42 = 0; yr.M43 = 0; yr.M44 = 1;

                // Матрица поворота вокруг оси Оz
                zr.M11 = cos(za); zr.M12 = -sin(za); zr.M13 = 0; zr.M14 = 0;
                zr.M21 = sin(za); zr.M22 = cos(za); zr.M23 = 0; zr.M24 = 0;
                zr.M31 = 0; zr.M32 = 0; zr.M33 = 1; zr.M34 = 0;
                zr.M41 = 0; zr.M42 = 0; zr.M43 = 0; zr.M44 = 1;

                var resMatrix = xr * yr * zr; // Получаем результирующую матрицу вращения

                Copy(resMatrix, this); // Копирование результирующей матрицы в текущую
            }

            /// <summary>
            /// Создание 3D матрицы вращения
            /// </summary>
            /// <param name="xa">Вращение вокруг оси Х</param>
            /// <param name="ya">Вращение вокруг оси Y</param>
            /// <param name="za">Вращение вокруг оси Z</param>
            /// <returns></returns>
            public static double[,] Rotate3D(double xa, double ya, double za)
            {
                double[,] xr = Create();
                double[,] yr = Create();
                double[,] zr = Create();

                // Матрица поворота вокруг оси Ох
                xr[0, 0] = 1; xr[0, 1] = 0; xr[0, 2] = 0; xr[0, 3] = 0;
                xr[1, 0] = 0; xr[1, 1] = cos(xa); xr[1, 2] = -sin(xa); xr[1, 3] = 0;
                xr[2, 0] = 0; xr[2, 1] = sin(xa); xr[2, 2] = cos(xa); xr[2, 3] = 0;
                xr[3, 0] = 0; xr[3, 1] = 0; xr[3, 2] = 0; xr[3, 3] = 1;

                // Матрица поворота вокруг оси Оy
                yr[0, 0] = cos(ya); yr[0, 1] = 0; yr[0, 2] = sin(ya); yr[0, 3] = 0;
                yr[1, 0] = 0; yr[1, 1] = 1; yr[1, 2] = 0; yr[1, 3] = 0;
                yr[2, 0] = -sin(ya); yr[2, 1] = 0; yr[2, 2] = cos(ya); yr[2, 3] = 0;
                yr[3, 0] = 0; yr[3, 1] = 0; yr[3, 2] = 0; yr[3, 3] = 1;

                // Матрица поворота вокруг оси Оz
                zr[0, 0] = cos(za); zr[0, 1] = -sin(za); zr[0, 2] = 0; zr[0, 3] = 0;
                zr[1, 0] = sin(za); zr[1, 1] = cos(za); zr[1, 2] = 0; zr[1, 3] = 0;
                zr[2, 0] = 0; zr[2, 1] = 0; zr[2, 2] = 1; zr[2, 3] = 0;
                zr[3, 0] = 0; zr[3, 1] = 0; zr[3, 2] = 0; zr[3, 3] = 1;

                return Mult(Mult(xr, yr), zr); // Возвращаем результирующую матрицу
            }

            /// <summary>
            /// Вращение векторов в списке
            /// </summary>
            /// <param name="v">Список векторов, которые будут вращаться</param>
            /// <param name="xa">Вращение вокруг оси Х</param>
            /// <param name="ya">Вращение вокруг оси Y</param>
            /// <param name="za">Вращение вокруг оси Z</param>
            public static void Rotate(List<double[]> v, double xa, double ya, double za)
            {
                double[,] rm = Rotate3D(xa, ya, za); // сгенерировать матрицу вращения
                double[] cm = new double[4] { 0, 0, 0, 1 }; // вектор "столбец"

                for (int i = 0; i < v.Count; i++) // цикл по всем вершинам фигуры
                {
                    // инициализация ветора матрицы "столбца" для умножения на матрицу вращения
                    cm[0] = v[i][0]; // x
                    cm[1] = v[i][1]; // y
                    cm[2] = v[i][2]; // z

                    cm = Mult(rm, cm); // вызов умножения матриц

                    // внесение изменений после вращения
                    v[i][0] = cm[0]; // x
                    v[i][1] = cm[1]; // y
                    v[i][2] = cm[2]; // z
                }
            }

            /// <summary>
            /// Вращение вектора
            /// </summary>
            /// <param name="v">Вектор, который будет вращаться</param>
            /// <param name="xa">Вращение вокруг оси Х</param>
            /// <param name="ya">Вращение вокруг оси Y</param>
            /// <param name="za">Вращение вокруг оси Z</param>
            public static void Rotate(double[] v, double xa, double ya, double za)
            {
                double[,] rm = Rotate3D(xa, ya, za); // сгенерировать матрицу вращения
                double[] cm = new double[] { 0, 0, 0, 1 }; // вектор "столбец"

                // инициализация ветора матрицы "столбца" для умножения на матрицу вращения
                cm[0] = v[0]; // x
                cm[1] = v[1]; // y
                cm[2] = v[2]; // z

                cm = Mult(rm, cm); // вызов умножения матриц

                // внесение изменений после вращения
                v[0] = cm[0]; // x
                v[1] = cm[1]; // y
                v[2] = cm[2]; // z
            }


            public static double[] Perspectiva3D(double[] v, double F)
            {
                /*
                http://stratum.ac.ru/education/textbooks/kgrafic/lection04.html    

                    Матрица масштабирования:
                        1   0   0   0
                        0   1   0   0
                        0   0   1   r
                        0   0   0   1
                 */

                var r = 1 / F;

                var x = v[0];
                var y = v[1];
                var z = v[2];

                x = x / r * z + 1;
                y = y / r * z + 1;
                z = z / r * z + 1;

                return new double[4] { x, y, z, 1 };
            }


            // ПЕРЕГРУЗКА ОПЕРАТОРОВ

            public static Matrix3D operator *(Matrix3D a, Matrix3D b)
            {
                var m11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41;
                var m12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42;
                var m13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43;
                var m14 = a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44;

                var m21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41;
                var m22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42;
                var m23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43;
                var m24 = a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44;

                var m31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41;
                var m32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42;
                var m33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43;
                var m34 = a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44;

                var m41 = a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41;
                var m42 = a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42;
                var m43 = a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43;
                var m44 = a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44;


                return new Matrix3D()
                {
                    M11 = m11,
                    M12 = m12,
                    M13 = m13,
                    M14 = m14,
                    M21 = m21,
                    M22 = m22,
                    M23 = m23,
                    M24 = m24,
                    M31 = m31,
                    M32 = m32,
                    M33 = m33,
                    M34 = m34,
                    M41 = m41,
                    M42 = m42,
                    M43 = m43,
                    M44 = m44
                };
            }

            // Вспомогательные методы
            private static double sin(double x) => Math.Sin(x);
            private static double cos(double x) => Math.Cos(x);
        }
    }
}
