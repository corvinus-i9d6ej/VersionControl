using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Week7_I9D6EJ.Abstractions;

namespace Week7_I9D6EJ.Entities
{
    public class Ball: Toy
    {
        protected override void DrawImage(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Blue), 0, 0, Width, Height);
        }
    }
}
