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
        double[,] a1; int n;

        public double funk(double wx, double wy)
        {
            Calkuer calkuer = new Calkuer();
            formula.constants[1] = wx;
            formula.constants[2] = wy;
            return (calkuer.Calculation(formula));
        }
        //справка по генерации
        private void button1_Click(object sender, EventArgs e)
        {

        }
        //генерация и вычисление
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime time1 = DateTime.Now;

            n = 0; int nmax = 0, c1 = 0, c2 = 0, c3 = 0, c4 = 0;
            double eps = 0;
            string function = textBox8.Text;
            Reader reader = new Reader();
            reader.input_string(function);
            formula = reader.Reading();
            //Получение значений n,nps,eps,c1,c2,c3,c4 из текстбоксов
            n = int.Parse(textBox1.Text);
            nmax = int.Parse(textBox2.Text);
            eps = double.Parse(textBox3.Text);
            c1 = int.Parse(textBox4.Text);
            c2 = int.Parse(textBox5.Text);
            c3 = int.Parse(textBox6.Text);
            c4 = int.Parse(textBox7.Text);
            
            //Объявление основных используемых переменных
            double[] x = new double[n];
            double[] x1 = new double[n];
            double[] y = new double[n];
            double[] y1 = new double[n];
            double[] pc = new double[n];
            double[] fi = new double[n];
            double[] x2 = new double[n];
            double[] y2 = new double[n];
            double[] d = new double[n];
            double[,] z1 = new double[n, 9];

            double omax1;
            double omin1;           
            
            int ai = 1, ai1 = 0, ai2 = 0, ai3 = 0, ai4 = 0;
            double dx = (c2 - c1) / (n - 1), dy = (c4 - c3) / (n - 1);
            int i3 = -1, i4;
            a1 = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a1[i, j] = funk(c1 + (c2 - c1) * (i - 1), c3 + (c4 - c3) * (j - 1));
                } //функция выбирается через checkbox
            }
            // метка 57 (Собираем массив А)
            double omax = 1000000; //~ max V(n))/n 
            double omin = -1000000; //~(min U(n))/n 

            int J = (n + 1) / 2 - 1; // случайно выбраный столбец (+ его инициализация)
            int I = 0; // инициализация константы со строкой.
            double C = 0;
            int K = 0; // надо сейвить до цикла
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


                // Тут делим элементы вектора на номер раунда
                // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ omax1 omin1


                omax1 = pc[I] / ai; //Уточнили условия в начале min U(n))/n  
                omin1 = fi[J] / ai; //Уточнили условия в начале max V(n))/n
                                
                if (Math.Abs(omax - omax1) >= 0) //  зачем то сравниваем начальные условия  
                {
                    omax = omax1; // уточнили начальное условие  
                    ai1 = ai; //Запомнили ПОСЛЕДНИЙ такт на котором было изменение 
                    ai3++; // общее количество изменений (омах)
                    C = 0; K = 0; //поправка на счет с нуля 
                    for (int i = 0; i < n; i++) //(i2) поправка на счет с нуля 
                    {
                        x1[i] = x[i] / ai;   // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                        d[i] = Math.Abs(x1[i] - x2[i]); // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                        C += d[i]; // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                        x2[i] = x1[i]; // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                        if (d[i] > d[K]) { K = i; }  // выбрали минимальный К // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                    }
                    //метка 7
                    //Запомнили минимумы

                }             
                   
                if (Math.Abs(omin - omin1) >= 0)
                {
                    omin = omin1;
                    ai2 = ai; ai4++;
                    C = 0; K = 0;
                    for (int i = 0; i < n; i++)
                    {
                        y1[i] = y[i] / ai; // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                        d[i] = Math.Abs(y1[i] - y2[i]); // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                        C += d[i]; y2[i] = y1[i]; // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                        if (d[i] > d[K]) K = i; // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                    } //метка 10

                    
                }
                ai++;

                if (ai > nmax) { repit = false; ai--; }
                if (Math.Abs(omax - omin) < eps) { repit = false; }
            }
            //печать i3,ai,ai2,ai3,ai4
            double F = (omax + omin) / 2;
            //печать f, omax, omin
            Queue<double> X1 = new Queue<double>();
            Queue<double> Y1 = new Queue<double>();
            Queue<double> X2 = new Queue<double>();
            Queue<double> Y2 = new Queue<double>();
            for (int i = 0; i < n; i++)// ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
            {
                X1.Enqueue(x[i]);  // пока приравниваю к количеству выборов, должна быть вероятность (x1[i])
                Y1.Enqueue(c4 + (i - 1) * dy);
                X2.Enqueue(y[i]);// пока приравниваю к количеству выборов, должна быть вероятность (y1[i])
                Y2.Enqueue(c2 + (i - 1) * dx);
            }// ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
            //вывод констант 
            textBox9.Multiline = true;
            textBox9.Text = "Вычисленные значения:" + Environment.NewLine;
            textBox9.Text += "Тактов вычисления выполнено: " + ai + "/" + nmax + Environment.NewLine;
            textBox9.Text += "Среднее = " + F + "; omax = " + omax + "; omin = " + omin + Environment.NewLine;// ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
            //Вывод массива
            textBox9.Text += "Результаты:" + Environment.NewLine;            
            textBox9.Text += "Игрок 1 (Выборы столбцов): ";
            for (; X1.Count != 0;)
            {
                textBox9.Text += Math.Round(X1.Dequeue() / ai, 3) + " "; // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
            }
            textBox9.Text += Environment.NewLine;
            textBox9.Text += "Игрок 2 (Выборы строк): ";
            for (; X2.Count != 0;)
            {
                textBox9.Text += Math.Round(X2.Dequeue() / ai, 3) + " "; // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
            }
            textBox9.Text += Environment.NewLine;                       
            //переменные для нахождения максимина и минимакса нужно искать по матрице A1!!
            double maxmin = 0, minmax = 0, local_min_j = 0, local_max_i = 0;
            //поиск наибольшего минимума
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
            //поиск наименьшего максимума
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
            //вывод максимин и минимакс
            textBox9.Text += Environment.NewLine;
            textBox9.Text += "MinMax = " + minmax + ";  MaxMin = " + maxmin;
            textBox9.Text += Environment.NewLine;

            DateTime time2 = DateTime.Now;
            textBox5.Text += "Время работы (миллисекунд)" + (time2 - time1).Milliseconds + Environment.NewLine;
            button3.Visible = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form forma = new Form4(a1);
            forma.ShowDialog();
        }
    }
}
