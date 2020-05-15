using MKModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKViewModel
{
    public class Provider
    {
        MageKnightProvider provider;
        public Provider()
        {
            this.provider = new MageKnightProvider();
        }

        MageKnightProvider MageKnights => this.provider;
    }
}
