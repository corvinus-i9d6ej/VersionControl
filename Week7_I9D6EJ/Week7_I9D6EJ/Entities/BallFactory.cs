﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week7_I9D6EJ.Abstractions;

namespace Week7_I9D6EJ.Entities
{
    public class BallFactory: IToyFactory
    {
        public Toy CreateNew()
        {
            return new Ball();
        }
    }
}
