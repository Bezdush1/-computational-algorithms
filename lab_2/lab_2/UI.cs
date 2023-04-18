using System;
using System.Collections.Generic;
using System.Text;

namespace lab_2
{
    class UI
    {
        public static void mainMenu()
        {
            Console.WriteLine("Введите: \n 1 - Посчитать экспоненту\n 2 - Посчитать корень\n 3 - Посчитать cos\n 4 - Посчитать sin\n" +
                " 5 - Посчитать ctg\n 6 - Посчитать tg\n 7 - Посчитать ln\n 8 - Завершить работу");
            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    Console.WriteLine("Введите погрешность E");
                    decimal eExp = examinationDecimal(0, 1000000);
                    Console.WriteLine("Введите x");
                    decimal sExp = examinationDecimal(-66, 66);
                    Console.WriteLine($"exp посчитанная нашим кодом {Exp(sExp,eExp)} \n" +
                        $"exp поситанная встроенной функцией {Math.Exp(Convert.ToDouble(sExp))}");
                    mainMenu();
                    break;
                case "2":
                    Console.WriteLine("Введите погрешность E");
                    decimal eSqrt = examinationDecimal(0, 1000000);
                    Console.WriteLine("Введите x");
                    decimal sSqrt = examinationDecimal(0, 1000000);
                    Console.WriteLine($"sqrt посчитанная нашим кодом {Sqrt(sSqrt,eSqrt)} \n" +
                        $"sqrt поситанная встроенной функцией {Math.Sqrt(Convert.ToDouble(sSqrt))}");
                    mainMenu();
                    break;
                case "3":
                    Console.WriteLine("Введите погрешность E");
                    decimal eCos = examinationDecimal(0, 1000000);
                    Console.WriteLine("Введите x");
                    decimal sCos = examinationDecimal(-66,60);
                    Console.WriteLine($"cos посчитанная нашим кодом {Cos(sCos,eCos)} \n" +
                        $"cos поситанная встроенной функцией {Math.Cos(Convert.ToDouble(sCos))}");
                    mainMenu();
                    break;
                case "4":
                    Console.WriteLine("Введите погрешность E");
                    decimal eSin = examinationDecimal(0, 1000000);
                    Console.WriteLine("Введите x");
                    decimal sSin = examinationDecimal(-66, 60);
                    Console.WriteLine($"sin посчитанная нашим кодом {Sin(sSin,eSin)} \n" +
                        $"sin поситанная встроенной функцией {Math.Sin(Convert.ToDouble(sSin))}");
                    mainMenu();
                    break;
                case "5":
                    Console.WriteLine("Введите погрешность E");
                    decimal eCtg = examinationDecimal(0, 1000000);
                    Console.WriteLine("Введите x"); 
                    decimal sCtg = examinationDecimal(-66, 60);
                    Console.WriteLine($"ctg посчитанная нашим кодом {Cos(sCtg,eCtg)/Sin(sCtg,eCtg)} \n" +
                        $"ctg поситанная встроенной функцией {(1.0 / Math.Tan(Convert.ToDouble(sCtg)))}");
                    mainMenu();
                    break;
                case "6":
                    Console.WriteLine("Введите погрешность E");
                    decimal eTg = examinationDecimal(0, 1000000);
                    Console.WriteLine("Введите x");
                    decimal sTg = examinationDecimal(-66, 60);
                    Console.WriteLine($"exp посчитанная нашим кодом {Sin(sTg,eTg)/Cos(sTg,eTg)} \n" +
                        $"exp поситанная встроенной функцией {Math.Tan(Convert.ToDouble(sTg))}");
                    mainMenu();
                    break;
                case "7":
                    Console.WriteLine("Введите погрешность E");
                    decimal eLn = examinationDecimal(0, 1000000);
                    Console.WriteLine("Введите x");
                    decimal sLn = examinationDecimal(1, 1000000);
                    Console.WriteLine($"exp посчитанная нашим кодом {Ln(sLn,eLn)} \n" +
                        $"exp поситанная встроенной функцией {Math.Log(Convert.ToDouble(sLn))}");
                    mainMenu();
                    break;
                case "8":
                    Console.WriteLine("Завершение работ. Повторно нажмите enter");
                    break;
                default:
                    Console.WriteLine("Введено неправильное значение");
                    mainMenu();
                    break;
            }
        }

           public static decimal Sqrt(decimal x ,decimal E)
            {
                decimal result = x;
                while (Math.Abs((decimal)Math.Pow(Convert.ToDouble(result), 2) - x) > E)
                {
                    result = (result + (x / result)) / 2;
                }
                return result;
            }

            public static decimal Cos(decimal x, decimal E)
            {
                decimal v1 = 1;
                decimal result = 0;
                decimal current = v1;
                long k = 1;
                while (Math.Abs(current) > E)
                {
                    result += current;
                    current = (-((decimal)Math.Pow((Convert.ToDouble(x)), 2)) / (decimal)(2 * k * ((2 * k) - 1))) * current;
                    k++;
                }
                return result;
            }

            public static decimal Sin(decimal x, decimal E)
            {
                decimal u1 = x;
                decimal result = 0;
                decimal current = u1;
                long k = 1;
                while (Math.Abs(current) > E)
                {
                    result += current;
                    current = -(decimal)(Math.Pow((Convert.ToDouble(x)), 2)) / (decimal)(2 * k * (2 * k + 1)) * current;
                    k++;
                }
                return result;
            }

           public  static decimal Ln(decimal z, decimal E)
            {
                decimal u0 = (1 - z) / (1 + z);
                decimal result = 0;
                decimal current = u0;
                int k = 1;
                while (Math.Abs(current) > E)
                {
                    result += current;
                    decimal x1 = Convert.ToDecimal(1d / (2 * k + 1));
                    decimal x2 = (decimal)Math.Pow((Convert.ToDouble(u0)), 2 * k + 1);
                    current = x1 * x2;
                    k++;
                }
                return -2 * result;
            }

            public static decimal Exp(decimal x, decimal E)
            {
                decimal result = 0;
                long k = 1;
                decimal current = 1;
                while (Math.Abs(current) > E)
                {
                    result += current;
                    current = (x / k) * current;
                    k++;
                }
                return result;
            }

        public static decimal examinationDecimal(decimal minValue, decimal maxValue)
        {
            bool flag = true;
            decimal newCount = 0;
            while (flag)
            {
                object newCountHuman = Console.ReadLine();
                try
                {
                    newCount = Convert.ToDecimal(newCountHuman);
                    if (newCount < minValue || newCount > maxValue)
                    {
                        Console.WriteLine($"Введите корректные данные (от {minValue} до {maxValue}) т.к. " +
                            $"рассчеты ограничены мощностью компьютера и диапозон используемых значений");
                    }
                    else
                    {
                        flag = false;
                    }
                }
                catch
                {
                    Console.WriteLine("неверно введенное значение");
                }
            }
            return newCount;
        }
    }
    }

