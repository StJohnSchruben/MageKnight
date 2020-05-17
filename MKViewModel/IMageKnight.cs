using MKModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKViewModel
{
    public interface IMageKnight
    {
        string Name { get; set; }
        int Index { get; set; }
        double XCord { get; set; }
        double YCord { get; set; }
        IDial Dial { get; set; }

        void Move();
        void Attack();
    }
}