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
using System.IO;

namespace Theory_of_game_1._2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            button3.Visible = false;
            button4.Visible = false;
        }

        Formula formula = new Formula();
        MathParser parser = new MathParser();
        double[,] a1; int n, nmax, c1 = 0, c2 = 0, c3 = 0, c4 = 0;
        double eps; string function;
        //double[] price_of_game;
        Queue<double> price_of_game = new Queue<double>();
        //справка по генерации
        private void button1_Click(object sender, EventArgs e)
        {
            string help = "Справка по генерации и вычислению матрицы:" + Environment.NewLine;
            help += "Генерируется квадратная матрица. Размерность матрицы определяется в графе 'Размерность матрицы'" + Environment.NewLine;
            help += "Расчет матрицы проводится с помощью поочередного розыгрыша игр, согласно сгенерируемой матрице." + Environment.NewLine;
            help += "Количество розыгрышей определяется в графе 'Количество вычислений'" + Environment.NewLine;
            help += "В графе 'Отклонение' указывается точность вычисления." + Environment.NewLine;
            help += "Генерация матрицы проводится с помощью функции распределения, указанной в соответсвующей графе." + Environment.NewLine;
            help += "Функция генерации может задаваться следующими функциями:" + Environment.NewLine;
            help += "Сложение|вычитание '+'|'-'; Умножение|деление '*'|'/'" + Environment.NewLine;
            help += "Возведение в степень '^'; Корень квадратный 'sqr';" + Environment.NewLine;
            help += "Синус|Косинус 'sin'|'cos'; Модуль 'abs'" + Environment.NewLine;
            //здесь можно указать еще функции, какие я забыл
            help += "И1 мин.(С1) И1 макс.(С2) И2 мин.(С3) И2 макс.(С4) пределяют платудля игроков" + Environment.NewLine;
            help += "Расчет происходит по формуле:А[i, j] =(Х,У), где" + Environment.NewLine;
            help += "Х=С1 + (С2 - С1) * (i-1), Y=С3 + (С4 - С3) * (j-1), i,j номер строки/столбца" + Environment.NewLine;
            help += "Для вычисления игры следует нажать кнопку 'Генерация и вычисление'." + Environment.NewLine;
            help += "Для демонстрации матрицы следует нажать кнопку 'Показать матрицу'." + Environment.NewLine;
            help += "Кнопка 'Пошаговые результаты' выводит массив результатов розыгрышей. При большом числе розыгрышей (1500+) при выводе возможно зависание программы." + Environment.NewLine;
            MessageBox.Show(help);
        }
        //генерация и вычисление
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime time1 = DateTime.Now;
            bool repit = false;
            bool print = false;
            if (readConst() && readFormula())
            {
                repit = true;
                print = true;
            } //читаем константы
            // читаем формулы

            //Объявление основных используемых переменных
            double[] x = new double[n];
            double[] y = new double[n];
            double[] pc = new double[n];
            double[] fi = new double[n];
            double price_game = 0;
            // price_of_game = new double[nmax];
            double omax = 1000000; //~ max V(n))/n 
            double omin = -1000000; //~(min U(n))/n 
            int ai = 1; // такт вычисления
            int J = 0;  // случайно выбраный столбец (+ его инициализация)
            int I = 0; // инициализация константы со строкой.

            a1 = new double[n, n];
            if(repit){
                price_of_game.Clear();
                for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a1[i, j] = funk(c1 + (c2 - c1) * (i), c3 + (c4 - c3) * (j));
                } 
                price_of_game.Clear();
                    button4.Text = "Пошаговые результаты";
            }
            }
            J = fing_string();
            int num_f = 0;
            //baka! Не юзай метки
            price_of_game.Clear();
            for (; repit;)
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
                //цена игры
                price_game += a1[I, J];
                price_of_game.Enqueue(a1[I, J]);
                //массив цен игры
               // if (num_f == 0) { price_of_game[num_f] = a1[I, J]; }
                //так легче модернизировать под цену игры на каждом шаге
              //  if (num_f != 0) { price_of_game[num_f] = a1[I, J]; }
                num_f++;

                if (ai > nmax) { repit = false; ai--; }
                if (Math.Abs(omax - omin) < eps) { if (ai > 100) { repit = false; ai--; } }
            }
            Queue<double> X1 = new Queue<double>();
            Queue<double> X2 = new Queue<double>();
            for (int i = 0; i < n; i++)
            {
                X1.Enqueue(x[i] / ai);
                X2.Enqueue(y[i] / ai);
            }
            if (print)
            {
                Print(ai, omax, omin, X1, X2);
            
            textBox9.Text += "Практическая цена игры: " + price_game + Environment.NewLine;
            textBox9.Text += "Практическая цена игры (приведенная): " + price_game / ai + Environment.NewLine;
            textBox9.Text += Environment.NewLine;
            DateTime time2 = DateTime.Now;
            textBox9.Text += "Время работы (миллисекунд) " + (time2 - time1).Milliseconds + Environment.NewLine;
            button3.Visible = true;
            button4.Visible = true;
            }
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
        }// печать результатов
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
        private void button4_Click(object sender, EventArgs e)
        {
            textBox9.Text += Environment.NewLine;
            if (button4.Text != "Следующие 100 шагов")
            {
                textBox9.Text += "Результаты для каждого розыгрыша:" + Environment.NewLine;
                button4.Text = "Следующие 100 шагов";
            }

            for (int i = 0; price_of_game.Count != 0 && i <= 100; i++)
            {
                textBox9.Text += Math.Round(price_of_game.Dequeue(), 3) + " ";
            }
        }//вывод пошагового решения
        private void button5_Click(object sender, EventArgs e)
        {
            string file_name = "";

            file_name = textBox10.Text;
            if (file_name != "")
            {
                try
                {
                    file_name += ".txt";
                    string adress = Directory.GetCurrentDirectory();
                    string path = adress + "\\" + file_name;
                    bool create_file = true;
                    string[] lines_in_file = new string[n];
                    try
                    {
                        for (int i = 0; i < n; i++)
                        {
                            for (int j = 0; j < n; j++)
                            {
                                lines_in_file[i] += a1[i, j] + " ";
                            }
                        }
                    }
                    catch
                    {
                        string error_text_4 = "Ошибка. Матрица не переводится в строки.";
                        MessageBox.Show(error_text_4);
                    }
                    if (File.Exists(path))
                    {
                        string error_text_3 = "Ошибка. Файл с таким названием уже существет.";
                        MessageBox.Show(error_text_3);
                        create_file = false;
                    }
                    if (create_file)
                    {
                        using (FileStream fs = File.Create(path))
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes("");
                            fs.Write(info, 0, info.Length);
                        }
                        File.WriteAllLines(path, lines_in_file, Encoding.UTF8);
                    }
                }
                catch
                {
                    string error_text_2 = "Ошибка при создании файла.";
                    MessageBox.Show(error_text_2);
                }
            }
            else { MessageBox.Show("Ошибка при вводе названия файла."); }
        }//сохранение в внешний файл
        bool readConst()
        {
            bool b = true;
            try { n = int.Parse(textBox1.Text);}
            catch{MessageBox.Show(("Ошибка введения размера матрицы."+Environment.NewLine+"Используйте целые числа.")); b = false; }
            try { nmax = int.Parse(textBox2.Text); }
            catch { MessageBox.Show(("Ошибка введения количества вычислений" + Environment.NewLine + "Используйте целые числа.")); b = false; }
            try { eps = double.Parse(textBox3.Text); }
            catch { MessageBox.Show(("Ошибка введения размера матрицы." + Environment.NewLine + "Проверьте знак разделения целой и дробной части числа.")); b = false; }
            try { c1 = int.Parse(textBox4.Text); }
            catch { MessageBox.Show(("Ошибка введения расчетного параметра С1" + Environment.NewLine + "Проверьте знак разделения целой и дробной части числа.")); b = false; }
            try { c2 = int.Parse(textBox5.Text); }
            catch { MessageBox.Show(("Ошибка введения расчетного параметра С2" + Environment.NewLine + "Проверьте знак разделения целой и дробной части числа.")); b = false; }
            try { c3 = int.Parse(textBox6.Text); }
            catch { MessageBox.Show(("Ошибка введения расчетного параметра С3" + Environment.NewLine + "Проверьте знак разделения целой и дробной части числа.")); b = false; }
            try { c4 = int.Parse(textBox7.Text); }
            catch { MessageBox.Show(("Ошибка введения расчетного параметра С4" + Environment.NewLine + "Проверьте знак разделения целой и дробной части числа.")); b = false; }
            return (true);
        } // чтение констант
        bool readFormula()
        {
            function = textBox8.Text;
            parser.Expression = function;
            try { double a = parser.ValueAsDouble;  }
            catch { MessageBox.Show(("Ошибка введения формулы рассчета"));return (false); }
            return (true);
            // Reader reader = new Reader();
            // reader.input_string(function);
            // formula = reader.Reading();
        } // чтение формулы
        double funk(double wx, double wy)
        {
           // MathParser parser = new MathParser();
            parser.SetVariable("e", Math.E, null);
            parser.Expression = function;
            parser.X = wx;
            parser.Y = wy;
            double a= parser.ValueAsDouble;
            a = Math.Round(a, 5);
            return (a);
            //  double value = parser.GetValueAsDouble();
            // Calkuer calkuer = new Calkuer();
            //  formula.constants[1] = wx;
            //  formula.constants[2] = wy;
            //return (calkuer.Calculation(formula));
        } //расчет значений функции
        private int fing_string()
        {
            int number_string = 0;
            double maxmin = 0, local_min_j = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j == 0) local_min_j = a1[i, j];
                    if (local_min_j > a1[i, j]) local_min_j = a1[i, j];
                }
                if (i == 0) { maxmin = local_min_j; number_string = i; }
                if (maxmin < local_min_j) { maxmin = local_min_j; number_string = i; }
            }
            return number_string;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form forma = new Form4(a1);
            forma.ShowDialog();
        } //показ матрицы.

    }
}
