using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKViewModel
{
    public class Click : IClick
    {
        public Click()
        {
        }

        public List<IStat> Stats { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}