using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKViewModel
{
    public class Dial : IDial
    {
        private List<IClick> clicks;
        private int clickIndex;

        public Dial()
        {
        }

        public List<IClick> Clicks { get => this.clicks; set => this.clicks = value; }
        public int ClickIndex { get => this.clickIndex; set => this.clickIndex = value; }
    }
}