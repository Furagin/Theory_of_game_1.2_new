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
    public partial class Form3 : Form
    {
        
        public Form3()
        {
            InitializeComponent();
            button4.Visible = false;
            button5.Visible = false;
        }

        double[,] a1, a2; int n = 0; int m = 0;
        //double[] price_of_game;
        Queue<double> price_of_game = new Queue<double>();
        int nmax;
        bool flad_load_matrix = false; string adress;
        //справка
        private void button1_Click(object sender, EventArgs e)
        {
            string help = "Справка по загрузке и вычислению матрицы:" + Environment.NewLine;
            help += "Загрузка матрицы из внешнего источника возможна из файла с расширением '.txt'." + Environment.NewLine;
            help += "Расчет матрицы проводится с помощью поочередного розыгрыша игр, согласно загруженной матрице." + Environment.NewLine;
            help += "Количество розыгрышей определяется в графе 'Количество вычислений'" + Environment.NewLine;
            help += "В графе 'Отклонение' указывается точность вычисления." + Environment.NewLine;
            help += "Для вычисления игры следует нажать кнопку 'Генерация и вычисление'." + Environment.NewLine;
            help += "Для демонстрации матрицы следует нажать кнопку 'Показать матрицу'." + Environment.NewLine;
            help += "Кнопка 'Пошаговые результаты' выводит массив результатов розыгрышей. При большом числе розыгрышей (1500+) при выводе возможно зависание программы." + Environment.NewLine;
            MessageBox.Show(help);
        }
        //вычисление
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime time1 = DateTime.Now;            
            nmax = 0;
            double eps = 0;
            bool repit = true;
            bool bool_print = true;
            if(!flad_load_matrix)
            {
                string message = "Матрица не загружена.";
                MessageBox.Show(message);
                repit = false;
                bool_print = false;
            }
            //Получение значений nps,eps            
            try
            {
                nmax = int.Parse(textBox3.Text);
            }
            catch
            {
                string message = "Ошибка при введении количества итерация расчетов.";
                MessageBox.Show(message);
                repit = false;
                bool_print = false;
            }
            try
            {
                eps = double.Parse(textBox4.Text);
            }
            catch
            {
                string message = "Ошибка при введении отклонения (точности)." + Environment.NewLine + "Проверьте знак разделения целой и дробоной части.";
                MessageBox.Show(message);
                repit = false;
                bool_print = false;
            }
                        
            double[] x = new double[n];
            double[] y = new double[n];
            double[] pc = new double[n];
            double[] fi = new double[n];
            double[] mass_ocen = new double[nmax];
            double price_game = 0;
            //price_of_game = new double [nmax];
            double omax1;
            double omin1;
            int ai = 1;

            // метка 57 (Собираем массив А)
            double omax = 1000000; //~ max V(n))/n 
            double omin = -1000000; //~(min U(n))/n 
            int J = fing_string();//(n + 1) / 2 - 1;  случайно выбраный столбец (+ его инициализация)
            int I = 0; // инициализация константы со строкой.
                       //baka! Не юзай метки
            int flag = 0; int num_f = 0;
            if (repit)
            {
                price_of_game.Clear();
                button5.Text = "Пошаговые результаты";
            }
            for (;  repit;)
            {
                I = 0; // Выбрали первый элемент столбца, вернее первыый столбец

                for (int i = 0; i < n; i++) // перебираем столбец и ищем минимальный
                {
                    pc[i] = pc[i] + a1[J, i]; // Вектор = вектор + элементы J строки 
                    if (pc[i] <= pc[I]) { I = i; }// если I (элемент столбца) не минимальный, то I=i   pc - вектор первого игрока
                }
                x[I] = x[I] + 1; // x имеет размерность количества строк и показывает сколько раз была выбранна I-я строка 
                J = 0; //запомнили первый столбец
                //J = find_row(x, y);
                for (int j = 0; j < n; j++) //перебираем строку и ищем минимальное
                {
                    fi[j] = fi[j] + a1[j, I]; // Вектор = вектор + элементы I стотбца 
                    if (fi[j] >= fi[J]) { J = j; } // если J (элемент строки) не мax, то J=j   Fi - вектор второго игрока
                }
                y[J] = y[J] + 1; // x имеет размерность количества столбцов и показывает сколько раз был выбранн J-й столбец 

                omax1 = pc[I] / ai; //Уточнили условия в начале min U(n))/n  
                omin1 = fi[J] / ai; //Уточнили условия в начале max V(n))/n
                //изменение итоговой цены игры
                price_game += a1[I, J];
                //массив цен игры
                price_of_game.Enqueue(a1[I, J]);
              //  if (num_f == 0) { price_of_game[num_f] = a1[I, J];}
                //так легче модернизировать под цену игры на каждом шаге
               // if (num_f != 0) { price_of_game[num_f] = a1[I, J];}
              //  num_f++;

                if (Math.Abs(omax - omax1) >= 0) { omax = omax1; }// уточнили начальное условие
                if (Math.Abs(omin - omin1) >= 0) { omin = omin1; }// уточнили начальное условие
                ai++;

                if (ai > nmax) { repit = false; ai--; }
                mass_ocen[flag] = Math.Abs(omax - omin);
                flag++;
                if (Math.Abs(omax - omin) < eps) { if (ai > 100) { repit = false; ai--; } }
            }

            if (bool_print)
            {
                //печать i3,ai,ai2,ai3,ai4
                double F = (omax + omin) / 2;//печать f, omax, omin
                                             //вывод констант 
                textBox5.Multiline = true;
                textBox5.Text = "Вычисленные значения:" + Environment.NewLine;
                textBox5.Text += "Тактов вычисления выполнено: " + ai + "/" + nmax + Environment.NewLine;
                textBox5.Text += "Среднее = " + F + "; omax = " + omax + "; omin = " + omin + Environment.NewLine;// ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                                                                                                                  //Вывод массива
                textBox5.Text += "Результаты:" + Environment.NewLine;

                Queue<double> X1 = new Queue<double>();
                Queue<double> X2 = new Queue<double>();
                for (int i = 0; i < n; i++)// ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                {
                    X1.Enqueue(x[i] / ai);  // пока приравниваю к количеству выборов, должна быть вероятность (x1[i])
                    X2.Enqueue(y[i] / ai);// пока приравниваю к количеству выборов, должна быть вероятность (y1[i])
                }// ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                //добавление в SeriesCollection строк и столбцов

                //отрисовка гистограмм
                this.chart1.Series["Series1"].Points.DataBindY(X1);
                this.chart2.Series["Series1"].Points.DataBindY(X2);
                textBox5.Text += "Игрок 1 (Выборы столбцов): ";
                for (; X1.Count != 0;)
                {
                    textBox5.Text += Math.Round(X1.Dequeue(), 3) + " "; // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                }
                textBox5.Text += Environment.NewLine;
                textBox5.Text += "Игрок 2 (Выборы строк): ";
                for (; X2.Count != 0;)
                {
                    textBox5.Text += Math.Round(X2.Dequeue(), 3) + " "; // ДОЛЖНЫ БЫТЬ ДРОБНЫЕ
                }
                textBox5.Text += Environment.NewLine;

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
                textBox5.Text += Environment.NewLine;
                textBox5.Text += "MinMax = " + minmax + ";  MaxMin = " + maxmin;
                textBox5.Text += Environment.NewLine;
                DateTime time2 = DateTime.Now;
                textBox5.Text += "Время работы (миллисекунд) " + Math.Round((time2 - time1).TotalMilliseconds, 0) + Environment.NewLine;
                textBox5.Text += Environment.NewLine;
                //массив цены игры
                textBox5.Text += "Практическая цена игры (суммарная): " + price_game + Environment.NewLine;
                textBox5.Text += Environment.NewLine;
                textBox5.Text += "Практическая цена игры (приведенная): " + price_game/ai + Environment.NewLine;
                textBox5.Text += Environment.NewLine;
                //массив оценок

                /*textBox5.Text += "Массив интервалов" + Environment.NewLine;
                foreach (double element in mass_ocen)
                {
                    textBox5.Text += Math.Round(element, 3) + "; ";
                }*/
                button5.Visible = true;
            }
        }

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
        //не используется
        private int find_row(double[] x, double[] y)
        {
            int number_row = 0;
            double minmax = 0, local_max_i = 0;
            copy_matrix();
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (i == 0) { local_max_i = a2[i, j]; a2[i, j] += a2[i, j] * x[i] * y[j]; }
                    if (local_max_i < a2[i, j]) { local_max_i = a2[i, j]; a2[i, j] += a2[i, j] * x[i] * y[j]; }
                }
                if (j == 0) { minmax = local_max_i; number_row = j; }
                if (minmax > local_max_i) { minmax = local_max_i; number_row = j; }
            }
            return number_row; 
        }

        private void copy_matrix()
        {
            a2 = new double[n, m];
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    a2[i, j] = a1[i, j];
                }
            }
        }
        //выбор файла
        private void button3_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = adress;
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            n = 0; m = 0;
                            flad_load_matrix = true;
                            System.IO.StreamReader file = new System.IO.StreamReader(myStream);
                            textBox1.Text = openFileDialog1.FileName;
                            string line;
                            string full_line = "";
                            //массив для разбивания строки
                            string[] drop_line; int flag = 0;
                            //получение размерности
                            //размерность по столбцам                            
                            for (int i = 0; (line = file.ReadLine()) != null; i++)
                            {
                                //чтени построчно           
                                if (flag == 0) { full_line = line; }
                                if (flag != 0) { full_line += " " + line; }                     
                                drop_line = line.Split(' ');
                                if (flag == 0)
                                {
                                    foreach (string element in drop_line) { n++; };                                    
                                    flag++;
                                }                                
                                m++;
                            }
                            a1 = new double[n, m];                            
                            //чтение                  
                            string[] drop_full_line = full_line.Split(' ');
                            int r = 0; int k = 0; 
                            foreach (string element in drop_full_line)
                            {
                                a1[r, k] = int.Parse(element);
                                k++;
                                if(k == m) { r++; k = 0; }
                            }   
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }                
            }
            if (m > n) { n = m; }
            button4.Visible = true;            
        }
        //показ матрицы
        private void button4_Click(object sender, EventArgs e)
        {
            Form forma = new Form4(a1);
            forma.ShowDialog();
            /*потом удалить
            Form forma2 = new Form4(a2);
            forma2.ShowDialog();*/
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
                textBox5.Text += Environment.NewLine;
                if (button5.Text != "Следующие 100 шагов")
                {
                    textBox5.Text += "Результаты для каждого розыгрыша:" + Environment.NewLine;
                    button5.Text = "Следующие 100 шагов";
                }

                for (int i = 0; price_of_game.Count != 0 && i <= 100; i++)
                {
                    textBox5.Text += Math.Round(price_of_game.Dequeue(), 3) + " ";
                }
            
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        //получение адреса
        private string get_adress()
        {
            adress = Directory.GetCurrentDirectory();
            return adress;
        }
    }
}
