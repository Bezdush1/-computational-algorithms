using System.Diagnostics;
class Program
{
    static void Main(string[] args)
    {
        int rows; //количество строк
        int cols; //количество столбцов

        for (int step = 0; step < 11; step++)
        {
            // Задаем размер матрицы
            rows = 100 + step * 10; //количество строк
            cols = 100 + step * 10; //количество столбцов
            double epsilion = 0.00000000000000001; //допустимая погрешность

            Console.WriteLine($"Размер матрицы: {rows}x{cols}");

            // Создаем матрицы
            double[,] matrixBasic = new double[rows, cols]; //матрица для заполнения значениями
            double[,] matrix = new double[rows, cols]; //матрица для выполнения алгоритмов
                                                       // Создаем вектора
            double[] vectorBasic = new double[rows];
            double[] vector = new double[rows];

            RandomMatrix(rows, cols, matrixBasic);
            CopyMatrix(matrixBasic, matrix);

            RandomVector(vectorBasic, rows);
            CopyVector(vectorBasic, vector);

            Stopwatch stopwatchGauss = new Stopwatch();//замерз производительности Гауссом
            stopwatchGauss.Start();
            double[] solutionGauss = Gauss(matrix, vector, rows);
            stopwatchGauss.Stop();
            Console.WriteLine("Время выполнения методом Гаусса: " + stopwatchGauss.Elapsed);
            Console.WriteLine("Накопленная ошибка методом Гаусса: " + CheckGauss(matrix, solutionGauss, vectorBasic, matrixBasic));

            CopyMatrix(matrixBasic, matrix);
            CopyVector(vectorBasic, vector);

            Stopwatch stopwatchJacobi = new Stopwatch();//замер производительности Якоби
            stopwatchJacobi.Start();
            double[] solutionJacobi = Jacobi(matrix, vector, epsilion);
            stopwatchJacobi.Stop();
            Console.WriteLine("Время выполнения методом Якоби: " + stopwatchJacobi.Elapsed);
            Console.WriteLine("Накопленная ошибка методом Якоби: " + CheckJacobi(matrixBasic, vectorBasic, solutionJacobi));

            Console.WriteLine();
        }

        // Заполняем матрицу рандомными числами
        double[,] RandomMatrix(int rows, int cols, double[,] matrixBasic)
        {
            Random rnd = new Random();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == j)
                        matrixBasic[i, j] = 4000;
                    else
                    {
                        matrixBasic[i, j] = rnd.Next(1, 10); // рандомное число от 1 до 10
                    }
                }
            }
            return matrixBasic;
        }

        //заполнение вектора рандомными значениями
        double[] RandomVector(double[] vectorBasic, int rows)
        {
            Random rnd = new Random();
            for (int i = 0; i < rows; i++)
            {
                vectorBasic[i] = rnd.Next(1, 2);
            }
            return vectorBasic;
        }

        // Выводим матрицу на консоль
        void PrintMatrix(double[,] matrix)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write("{0,10}", matrix[i, j]); // форматированный вывод
                }
                Console.WriteLine(); // переход на следующую строку
            }
        }

        //выводим вектор на консоль
        void PrintVector(double[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                Console.Write("{0,10}", vector[i]); // форматированный вывод
                Console.WriteLine(); // переход на следующую строку
            }
            Console.WriteLine(); // переход на следующую строку
        }

        //копируем матрицу
        double[,] CopyMatrix(double[,] matrixBasic, double[,] matrix)
        {
            Array.Clear(matrix);
            Array.Copy(matrixBasic, matrix, matrixBasic.Length);
            return matrix;
        }

        //копируем вектор
        double[] CopyVector(double[] vectorBasic, double[] vector)
        {
            Array.Clear(vector);
            Array.Copy(vectorBasic, vector, vectorBasic.Length);
            return vector;
        }

        double[] Gauss(double[,] matrix, double[] vector, int rows)
        {
            // Приводим матрицу A к ступенчатому виду
            for (int k = 0; k < rows; k++)
            {
                for (int i = k + 1; i < rows; i++)
                {
                    double coef = matrix[i, k] / matrix[k, k];
                    for (int j = k; j < rows; j++)
                    {
                        matrix[i, j] -= coef * matrix[k, j];
                    }
                    vector[i] -= coef * vector[k];
                }
            }

            // Обратный ход матрицы А
            for (int k = rows - 1; k >= 0; k--)
            {
                for (int i = k - 1; i >= 0; i--)
                {
                    double coef = matrix[i, k] / matrix[k, k];
                    for (int j = k; j >= 0; j--)
                    {
                        matrix[i, j] -= coef * matrix[k, j];
                    }
                    vector[i] -= coef * vector[k];
                }
            }

            // Матрица А становится единичной
            for (int i = 0; i < rows; i++)
            {
                double coef = 1 / matrix[i, i];
                for (int j = 0; j < rows; j++)
                {
                    matrix[i, j] *= coef;
                }
                vector[i] *= coef;
            }
            //PrintMatrix(matrix);
            return vector;
        }

        double CheckGauss(double[,] matrix, double[] solutionGauss, double[] vectorBasic, double[,] matrixBasic)
        {
            double[] Ax = new double[solutionGauss.Length];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double sum = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sum += matrixBasic[i, j] * solutionGauss[j];//умножаем каждую строчку матрицы на вектор
                                                                //значений
                }
                Ax[i] = sum;//полученное значение записываем в новый вектор
            }
            double error = 0;
            for (int i = 0; i < solutionGauss.Length; i++)
            {
                error += Math.Abs(Ax[i] - vectorBasic[i]);
            }
            return error;
        }

        double[] Jacobi(double[,] matrix, double[] vector, double epsilion)
        {
            double[] solutionJacobi = new double[vector.Length];
            // Разложение матрицы на диагональную и остаточную
            double[,] D = new double[vector.Length, vector.Length];
            double[,] R = new double[vector.Length, vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                for (int j = 0; j < vector.Length; j++)
                {
                    if (i == j)
                    {
                        D[i, j] = matrix[i, j];
                    }
                    else
                    {
                        R[i, j] = matrix[i, j];
                    }
                }
            }
            // Итерационный процесс
            double[] NewVector = new double[vector.Length];
            double norm;
            do
            {
                for (int i = 0; i < vector.Length; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < vector.Length; j++)
                    {
                        if (i != j)
                        {
                            sum += R[i, j] * solutionJacobi[j];
                        }
                    }
                    NewVector[i] = (vector[i] - sum) / D[i, i];//вычисление нового вектора неизвестных
                                                               //на каждой итерации
                }
                // Проверка критерия останова(т.е.оценка нормы разности текущего и предыдущего вектора
                // неизвестных)
                norm = 0;
                for (int i = 0; i < vector.Length; i++)
                {
                    norm += Math.Abs(NewVector[i] - solutionJacobi[i]);
                }
                solutionJacobi = (double[])NewVector.Clone();
            }
            while (norm > epsilion);//итерации продолжаются до тех пор,
                                    //пока эта норма больше заданного значения epsilon.
            return solutionJacobi;
        }

        double CheckJacobi(double[,] matrixBasic, double[] vectorBasic, double[] solutionJacobi)
        {
            double[] NewVector = new double[vectorBasic.Length];
            for (int i = 0; i < vectorBasic.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < vectorBasic.Length; j++)
                {
                    sum += matrixBasic[i, j] * solutionJacobi[j];//умножаем каждую строчку матрицы на вектор
                                                                 //значений
                }
                NewVector[i] = sum;//полученное значение записываем в новый вектор
            }
            double error = 0;
            for (int i = 0; i < vectorBasic.Length; i++)
            {
                error += Math.Abs(NewVector[i] - vectorBasic[i]);//из нового вектора вычитаем базовый 
                                                                 //тем самым мы находим накопленную ошибку в
                                                                 //решении
            }
            return error;
        }

        Console.ReadKey();
    }
}