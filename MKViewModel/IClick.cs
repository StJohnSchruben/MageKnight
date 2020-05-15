using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKViewModel
{
    public interface IClick
    {
        System.Collections.Generic.List<IStat> Stats { get; set; }
    }
}