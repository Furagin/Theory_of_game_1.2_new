using Bestcode.MathParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Theory_of_game_1._2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            button3.Visible = false;
        }

        Formula formula = new Formula();
        double[,] a1; int n, nmax, c1 = 0, c2 = 0, c3 = 0, c4 = 0;
        double eps; string function;

        //справка по генерации
        private void button1_Click(object sender, EventArgs e)
        {

        }
        //генерация и вычисление
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime time1 = DateTime.Now;

            readConst(); //читаем константы
            readFormula();// читаем формулы

            //Объявление основных используемых переменных
            double[] x = new double[n];
            double[] y = new double[n];
            double[] pc = new double[n];
            double[] fi = new double[n];
            double omax = 1000000; //~ max V(n))/n 
            double omin = -1000000; //~(min U(n))/n 
            int ai = 1; // такт вычисления
            int J = (n + 1) / 2 - 1; // случайно выбраный столбец (+ его инициализация)
            int I = 0; // инициализация константы со строкой.

            a1 = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a1[i, j] = funk(c1 + (c2 - c1) * (i), c3 + (c4 - c3) * (j));
                } 
            }

            //baka! Не юзай метки
            for (bool repit = true; repit;)
            {
                I = 0; // Выбрали первый элемент столбца         
                for (int i = 0; i < n; i++) // перебираем столбец и ищем минимальный
                {
                    pc[i] = pc[i] + a1[J, i]; // Вектор = вектор + элементы J строки 
                    if (pc[i] <= pc[I]) { I = i; }// если I (элемент столбца) не минимальный, то I=i   pc - вектор первого игрока
                }

                x[I] = x[I] + 1; // x имеет размерность количества строк и показывает сколько раз была выбранна I-я строка 
                J = 0; //запомнили первый столбец

                for (int j = 0; j < n; j++) //перебираем строку и ищем минимальное
                {
                    fi[j] = fi[j] + a1[j, I]; // Вектор = вектор + элементы I стотбца 
                    if (fi[j] >= fi[J]) { J = j; } // если J (элемент строки) не мax, то J=j   Fi - вектор второго игрока
                }

                y[J] = y[J] + 1; // x имеет размерность количества столбцов и показывает сколько раз был выбранн J-й столбец 

                if (Math.Abs(omax - pc[I] / ai) >= 0) {omax = pc[I] / ai; }
                if (Math.Abs(omin - fi[J] / ai) >= 0){omin = fi[J] / ai; }
                ai++;

                if (ai > nmax) { repit = false; ai--; }
                if (Math.Abs(omax - omin) < eps) { repit = false; }
            }
            Queue<double> X1 = new Queue<double>();
            Queue<double> X2 = new Queue<double>();
            for (int i = 0; i < n; i++)
            {
                X1.Enqueue(x[i] / ai);
                X2.Enqueue(y[i] / ai);
            }

            Print(ai,omax,omin,X1,X2);

            DateTime time2 = DateTime.Now;
            textBox9.Text += "Время работы (миллисекунд)" + (time2 - time1).Milliseconds + Environment.NewLine;
            button3.Visible = true;

        }

        void Print(int ai, double omax, double omin, Queue<double> X1, Queue<double> X2)
        {
            //вывод констант 
            textBox9.Multiline = true;
            textBox9.Text = "Вычисленные значения:" + Environment.NewLine;
            textBox9.Text += "Тактов вычисления выполнено: " + ai + "/" + nmax + Environment.NewLine;
            textBox9.Text += "Среднее = " + ((omax + omin) / 2) + "; omax = " + omax + "; omin = " + omin + Environment.NewLine;// ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
            //Вывод массива
            textBox9.Text += "Результаты:" + Environment.NewLine;
            textBox9.Text += "Игрок 1 (Выборы столбцов): ";
            for (; X1.Count != 0;)
            {
                textBox9.Text += Math.Round(X1.Dequeue(), 3) + " ";
            }
            textBox9.Text += Environment.NewLine;
            textBox9.Text += "Игрок 2 (Выборы строк): ";
            for (; X2.Count != 0;)
            {
                textBox9.Text += Math.Round(X2.Dequeue(), 3) + " ";
            }
            textBox9.Text += Environment.NewLine;

            textBox9.Text += Environment.NewLine;
            textBox9.Text += "MinMax = " + minmax() + ";  MaxMin = " + maxmin();
            textBox9.Text += Environment.NewLine;
        }

        double maxmin()
        {
            double maxmin = 0,local_min_j=0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j == 0) local_min_j = a1[i, j];
                    if (local_min_j > a1[i, j]) local_min_j = a1[i, j];
                }
                if (i == 0) { maxmin = local_min_j; }
                if (maxmin < local_min_j) { maxmin = local_min_j; }
            }
            return (maxmin);
        }//поиск наибольшего минимума

        double minmax()
        {
            double minmax = 0, local_max_i = 0;
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (i == 0) local_max_i = a1[i, j];
                    if (local_max_i < a1[i, j]) local_max_i = a1[i, j];
                }
                if (j == 0) { minmax = local_max_i; }
                if (minmax > local_max_i) { minmax = local_max_i; }
            }
            return (minmax);
        }//поиск наименьшего максимума

        void readConst()
        {
            n = int.Parse(textBox1.Text);
            nmax = int.Parse(textBox2.Text);
            eps = double.Parse(textBox3.Text);
            c1 = int.Parse(textBox4.Text);
            c2 = int.Parse(textBox5.Text);
            c3 = int.Parse(textBox6.Text);
            c4 = int.Parse(textBox7.Text);
        } // чтение констант

        void readFormula()
        {
            function = textBox8.Text;
           // Reader reader = new Reader();
           // reader.input_string(function);
           // formula = reader.Reading();
        } // чтение формулы

        double funk(double wx, double wy)
        {
            MathParser parser = new MathParser();
            parser.SetVariable("e", Math.E, null);
            parser.X = wx;
            parser.Y = wy;
            parser.Expression = function;
            double a= parser.ValueAsDouble;
            a = Math.Round(a, 5);
            return (a);
            //  double value = parser.GetValueAsDouble();
            // Calkuer calkuer = new Calkuer();
            //  formula.constants[1] = wx;
            //  formula.constants[2] = wy;
            //return (calkuer.Calculation(formula));
        } //расчет значений функции


        private void button3_Click(object sender, EventArgs e)
        {
            Form forma = new Form4(a1);
            forma.ShowDialog();
        } //показ матрицы.

    }
}
