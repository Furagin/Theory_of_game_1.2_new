﻿using System;
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
    public partial class Form4 : Form
    {
        public Form4(double[,] A)
        {
            InitializeComponent();
            for (int i = 0, j = 0; j < A.GetLength(1);)
            {
                textBox1.Text += A[j, i] + " ";
                i++;
                if (i == A.GetLength(0)) { i = 0; j++; textBox1.Text += Environment.NewLine; }
            }
        }
    }
}
