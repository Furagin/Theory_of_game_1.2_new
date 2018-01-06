﻿using System;
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
        }

        double[,] a1; int n = 0; int m = 0;
        
        //справка
        private void button1_Click(object sender, EventArgs e)
        {

        }
        //вычисление
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime time1 = DateTime.Now;

            int nmax = 0;
            double eps = 0;                        
            //Получение значений nps,eps            
            nmax = int.Parse(textBox3.Text);
            eps = double.Parse(textBox4.Text);
                        
            double[] x = new double[n];
            double[] y = new double[n];
            double[] pc = new double[n];
            double[] fi = new double[n];
            double[] mass_ocen = new double[nmax];
            double price_game = 0;
            double[] price_of_game = new double [nmax];
            double omax1;
            double omin1;
            int ai = 1;

            // метка 57 (Собираем массив А)
            double omax = 1000000; //~ max V(n))/n 
            double omin = -1000000; //~(min U(n))/n 
            int J = (n + 1) / 2 - 1; // случайно выбраный столбец (+ его инициализация)
            int I = 0; // инициализация константы со строкой.
                       //baka! Не юзай метки
            int flag = 0; int num_f = 0;
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

                omax1 = pc[I] / ai; //Уточнили условия в начале min U(n))/n  
                omin1 = fi[J] / ai; //Уточнили условия в начале max V(n))/n
                //изменение итоговой цены игры
                price_game += a1[I, J];
                //массив цен игры
                if (num_f == 0) { price_of_game[num_f] = a1[I, J];}
                //так легче модернизировать под цену игры на каждом шаге
                if (num_f != 0) { price_of_game[num_f] = a1[I, J];}
                num_f++;

                if (Math.Abs(omax - omax1) >= 0) { omax = omax1; }// уточнили начальное условие
                if (Math.Abs(omin - omin1) >= 0) { omin = omin1; }// уточнили начальное условие
                ai++;

                if (ai > nmax) { repit = false; ai--; }
                mass_ocen[flag] = Math.Abs(omax - omin);
                flag++;
                if (Math.Abs(omax - omin) < eps) { repit = false; }
            }


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
            textBox5.Text += "Время работы (миллисекунд)" + (time2 - time1).Milliseconds + Environment.NewLine;
            textBox5.Text += Environment.NewLine;
            //массив цены игры
            textBox5.Text += "Итоговая цена игры: " + price_game + Environment.NewLine;
            textBox5.Text += Environment.NewLine;
            //массив оценок
            textBox5.Text += "Массив цен для каждой игры: " + Environment.NewLine;
            foreach (double element in price_of_game) { textBox5.Text += element + "; "; }
            textBox5.Text += Environment.NewLine;
            textBox5.Text += "Массив интервалов" + Environment.NewLine;
            foreach (double element in mass_ocen)
            {
                textBox5.Text += Math.Round(element, 3) + "; ";
            }
        }
        //выбор файла
        private void button3_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
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
        }
    }
}