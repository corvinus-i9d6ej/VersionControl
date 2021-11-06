using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week7_I9D6EJ.Abstractions;

namespace Week7_I9D6EJ.Entities
{
    public class Car : Toy
    {
        protected override void DrawImage(Graphics g)
        {
            Image image = Image.FromFile("Images/car.png");
            g.DrawImage(image, new Rectangle(0, 0, Width, Height));
        }
    }
}
