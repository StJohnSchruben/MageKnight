using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKViewModel
{
    public interface IDial
    {
        System.Collections.Generic.List<IClick> Clicks { get; set; }
        int ClickIndex { get; set; }
    }
}