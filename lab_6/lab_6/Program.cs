using System;

class Program
{
    public const double PI = 3.1415926535897931;
    static void Main()
    {
        Console.WriteLine("Решение определенного интеграла");

        double a = 0; //нижний предел интегрирования 
        double b = PI / 3;//верхний предел интегрирования 
        int n = 100;  //количество разбиений
        double epsilon = 0.000001;

        double rectangleResult = RectangleMethodLeft(a, b, n);
        Console.WriteLine(rectangleResult);
        double trapezoidResult = TrapezoidMethod(a, b, n);
        Console.WriteLine(trapezoidResult);
        double SimpsonResult = SimpsonMethod(a, b, n);
        Console.WriteLine(SimpsonResult);

        double resultRectangleLeft = IntegralWithRungeEstimateForRectangleLeft(a, b, epsilon);
        Console.WriteLine($"Результат интегрирования методом прямоугольников(левый): {resultRectangleLeft}");

        double resultRectangleRight = IntegralWithRungeEstimateForRectangleRight(a, b, epsilon);
        Console.WriteLine($"Результат интегрирования методом прямоугольников(правый): {resultRectangleRight}");

        double resultRectangleMiddle = IntegralWithRungeEstimateForRectangleMiddle(a, b, epsilon);
        Console.WriteLine($"Результат интегрирования методом прямоугольников(середины): {resultRectangleMiddle}");

        double result = IntegralWithRungeEstimateTrapezoid(a, b, epsilon);
        Console.WriteLine($"Результат интегрирования методом трапеций: {result}");

        double resultSimpson = IntegralWithRungeEstimateSimpson(a, b, epsilon);
        Console.WriteLine($"Результат интегрирования методом симпсона: {resultSimpson}");

        Console.ReadKey();
    }

    static double Function(double x)
    {
        return 1 / Math.Sqrt(1 - 0.25 * Math.Pow(Math.Sin(x), 2));
    }

    static double RectangleMethodLeft(double a, double b, int n) //прямоугольник
    {
        double h = (b - a) / n;

        double sum = 0.0;
        for (int i = 0; i < n; i++)
        {
            sum += h * Function(a + i * h);
        }

        return sum;
    }

    static double RectangleMethodRight(double a, double b, int n) //прямоугольник
    {
        double h = (b - a) / n;

        double sum = 0.0;
        for (int i = 1; i < n; i++)
        {
            sum += h * Function(a + i * h);
        }

        return sum;
    }

    static double RectangleMethodMiddle(double a, double b, int n) //прямоугольник
    {
        double h = (b - a) / n;

        double sum = (Function(a) + Function(b)) / 2;
        for (int i = 1; i < n; i++)
        {
            sum += Function(a + i * h);
        }

        return h * sum;
    }

    static double TrapezoidMethod(double a, double b, int n) //трапеция
    {
        double h = (b - a) / n;
        double sum = (Function(a) + Function(b))/2;

        for (int i = 1; i < n; i++)
        {
            double x = a + i * h;
            sum += Function(x);
        }

        return sum * h;
    }

    static double SimpsonMethod(double a, double b, int n) //cимпсон
    {
        if (n % 2 == 1)
        {
            n++;
        }
        double h = (b - a) / n;
        double sum = Function(a);
        for (int i = 2; i < n - 1; i += 2)
        {
            double x = a + i * h;
            sum += 2 * Function(x);
        }
        for (int i = 1; i < n; i += 2)
        {
            double x = a + i * h;
            sum += 4 * Function(x);
        }
        return sum * h / 3;
    }

    static double RungeRuleForSimpson(double I1, double I2, double h2)
    {
        return (I2 - I1) / 15;
    }

    static double IntegralWithRungeEstimateSimpson(double a, double b, double epsilon)
    {
        int n = 2;
        double I1 = SimpsonMethod(a, b, n);
        double h2 = (b - a) / n;
        double I2 = SimpsonMethod(a, b, 2 * n);
        double R = RungeRuleForSimpson(I1, I2, h2);
        while (Math.Abs(R) > epsilon)
        {
            n *= 2;
            I1 = I2;
            h2 /= 2;
            I2 = SimpsonMethod(a, b, 2 * n);
            R = RungeRuleForSimpson(I1, I2, h2);
        }
        return I2;
    }

    static double RungeRuleForRectangleAndTrapezoid(double I1, double I2, double h2)
    {
        return (I2 - I1) / 3;
    }

    static double IntegralWithRungeEstimateForRectangleLeft(double a, double b, double epsilon)
    {
        int n = 2;
        double I1 = RectangleMethodLeft(a, b, n);
        double h2 = (b - a) / n;
        double I2 = RectangleMethodLeft(a, b, 2 * n);
        double R = RungeRuleForRectangleAndTrapezoid(I1, I2, h2);
        while (Math.Abs(R) > epsilon)
        {
            n *= 2;
            I1 = I2;
            h2 /= 2;
            I2 = RectangleMethodLeft(a, b, 2 * n);
            R = RungeRuleForRectangleAndTrapezoid(I1, I2, h2);
        }
        return I2;
    }

    static double IntegralWithRungeEstimateForRectangleRight(double a, double b, double epsilon)
    {
        int n = 2;
        double I1 = RectangleMethodRight(a, b, n);
        double h2 = (b - a) / n;
        double I2 = RectangleMethodRight(a, b, 2 * n);
        double R = RungeRuleForRectangleAndTrapezoid(I1, I2, h2);
        while (Math.Abs(R) > epsilon)
        {
            n *= 2;
            I1 = I2;
            h2 /= 2;
            I2 = RectangleMethodRight(a, b, 2 * n);
            R = RungeRuleForRectangleAndTrapezoid(I1, I2, h2);
        }
        return I2;
    }

    static double IntegralWithRungeEstimateForRectangleMiddle(double a, double b, double epsilon)
    {
        int n = 2;
        double I1 = RectangleMethodMiddle(a, b, n);
        double h2 = (b - a) / n;
        double I2 = RectangleMethodMiddle(a, b, 2 * n);
        double R = RungeRuleForRectangleAndTrapezoid(I1, I2, h2);
        while (Math.Abs(R) > epsilon)
        {
            n *= 2;
            I1 = I2;
            h2 /= 2;
            I2 = RectangleMethodMiddle(a, b, 2 * n);
            R = RungeRuleForRectangleAndTrapezoid(I1, I2, h2);
        }
        return I2;
    }

    static double IntegralWithRungeEstimateTrapezoid(double a, double b, double epsilon)
    {
        int n = 2;
        double I1 = TrapezoidMethod(a, b, n);
        double h2 = (b - a) / n;
        double I2 = TrapezoidMethod(a, b, 2 * n);
        double R = RungeRuleForRectangleAndTrapezoid(I1, I2, h2);
        while (Math.Abs(R) > epsilon)
        {
            n *= 2;
            I1 = I2;
            h2 /= 2;
            I2 = TrapezoidMethod(a, b, 2 * n);
            R = RungeRuleForRectangleAndTrapezoid(I1, I2, h2);
        }
        return I2;
    }
}


