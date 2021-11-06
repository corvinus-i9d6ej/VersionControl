﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Week7_I9D6EJ.Entities;

namespace Week7_I9D6EJ
{
    public partial class Form1 : Form
    {
        List<Ball> _balls = new List<Ball>();
        private BallFactory _factory;

        public BallFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }


        public Form1()
        {
            InitializeComponent();
            Factory = new BallFactory();
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            var ball = Factory.CreateNew();
            _balls.Add(ball);
            ball.Left = -ball.Width;
            mainPanel.Controls.Add(ball);
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var position = 0;
            foreach (var b in _balls)
            {
                b.MoveToy();
                if (b.Left > position)
                    position = b.Left;
                if (position == 1000)
                {
                    var oldestBall = _balls[0];
                    mainPanel.Controls.Remove(oldestBall);
                    _balls.Remove(oldestBall);
                }
            }
        }
    }
}
