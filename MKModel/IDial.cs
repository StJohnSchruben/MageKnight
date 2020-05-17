using MKModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKModel
{
    public interface IDial
    {
        System.Collections.Generic.List<IClick> Clicks { get; set; }
        int ClickIndex { get; set; }

        IClick Click { get; }
    }
}