using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_4
{
    internal class Program
    {
        static double[] array = new double[2];

        static double Function(double x)
        {
            return (x * Math.Pow(2, x)) + x - 3.1; // функция, которую мы решаем
        }

        static double Derivative(double x)
        {
            return Math.Pow(2, x) * (x * Math.Log(2) + 1) + 1; // производная функции
        }
        static double TwoDerivative(double x)
        {
            return (Math.Pow(Math.Log(2), 2) * x + 2 * Math.Log(2)) * Math.Pow(2, x); // производная функции
        }

        static void FirstProcessRoots(double a, double b)
        {
            int n = 30;
            double h = (b - a) / 30; //вычисляем наш шаг
            double[] x = new double[n];
            double[] y = new double[n];

            Console.WriteLine("Отрезок [{0}, {1}]", a, b);
            Console.WriteLine("Шаг: {0}", h);

            // вычисление точек х
            for (int i = 0; i < n; i++)
            {
                x[i] = a + h * i;
            }

            // вычисление точек у
            for (int i = 0; i < n; i++)
            {
                y[i] = Function(x[i]);
            }

            for (int i = 1; i < n; i++)
            {
                if (y[i - 1] * y[i] < 0)//проверяем условие которое удовлетворяет найденному корню ур-ия
                {
                    a = x[i - 1];
                    b = x[i];
                    array[0] = a;
                    array[1] = b;
                    break;//нашли интервал, где находится наш единственный корень
                }
            }

            Console.WriteLine("\nТаблица значений функции:");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("x[{0,2}] = {1,20}    y[{2,2}] = {3,20}", i + 1, x[i], i + 1, y[i]);
            }
            Console.WriteLine("\nОтрезок, содержащий корни [{0}, {1}]", a, b);

        }

        static void HalfDivision(double []array,double eps)
        {
            double x1 = array[0], x2 = array[1], x, fx;
            int iteration = 0;
            Console.WriteLine("\n{0,20} | {1,20} ", "Итерация",
                "Половинное деление");
            x = (x1 + x2) / 2;//начальное приближение (середина отрезка)
            while (Math.Abs(x2 - x1) > eps)
            {
                fx = Function(x);//значение функции в точке х
                if (fx == 0)
                {
                    break;
                }
                if (Function(x1) * fx < 0)//смотрим в какой границе мы оказались
                {
                    x2 = x;
                }
                else
                {
                    x1 = x;
                }
                x = (x2 + x1) / 2;
                Console.WriteLine("{0,20} | {1,20:f6}", iteration++, x);
            }
        }

        static void MethodNewton(double[] array, double eps)
        {
            double x, fx;
            double x1 = array[0];
            double x2 = array[1];
            int iteration = 0;
            Console.WriteLine("\n{0,20} | {1,20} ", "Итерация",
                "Метод Ньютона");
            if (Derivative(x1) * TwoDerivative(x1) > 0)
            {
                x = x1;
            }
            else
                x = x2;
            while (Math.Abs(x2 - x) > eps)//сокращаем интервал пока разница между значениями > точности
            {
                x = x2;
                fx = Function(x);
                x2 = x - (fx / Derivative(x)); // Метод Ньютона

                Console.WriteLine("{0,20} | {1,20:f6}", iteration++, x2);

            }
        }

        static void MethodSecant(double[] array, double eps)
        {
            double x, fx;
            int iteration = 0;
            double x1 = array[0];
            Console.WriteLine("\n{0,20} | {1,20} ", "Итерация",
                "Метод секущих");
            double x0 = array[0];
            x = x0 + eps;


            while (Math.Abs(x - x0) >= eps)
            {
                fx = Function(x);
                double x_secant = x - ((fx * (x - x0)) / (fx - Function(x0))); // Метод секущих
                x0 = x;
                x = x_secant;

                Console.WriteLine("{0,20} | {1,20:f6} ", iteration++, x_secant);
            }
        }

        static void Main(string[] args)
        {
            const double eps = 0.0000001; // точность
            double a = 0, b = 10; // начальные значения отрезков

            // 1. Отделяем корни уравнения алгебраическим способом

            Console.WriteLine("\n\tАлгебраический способ\n");
            FirstProcessRoots(a, b);

            // 2. Уточняем корни уравнения методом половинного деления с методом Ньютона (нечетный вариант)

            Console.WriteLine("\n\tНечетный вариант\n");
            HalfDivision(array, eps);
            MethodNewton(array, eps);

            // 3. Уточняем корни уравнения половинного деления и методом секущих (четный вариант)

            Console.WriteLine("\n\tЧетный вариант\n");
            HalfDivision(array, eps);
            MethodSecant(array, eps);

            Console.ReadKey();
        }
    }
}