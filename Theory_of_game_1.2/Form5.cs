using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Theory_of_game_1._2
{
    public partial class Form5 : Form
    {
        public Form5(Queue<double> X3, Queue<double> X4 )
        {
            InitializeComponent();
            this.chart1.Legends.Clear();
            this.chart2.Legends.Clear();
            this.chart1.Series["Series1"].Points.DataBindY(X3);
            this.chart2.Series["Series1"].Points.DataBindY(X4);
            this.chart1.Legends.Clear();
            this.chart2.Legends.Clear();
        }
    }
}
