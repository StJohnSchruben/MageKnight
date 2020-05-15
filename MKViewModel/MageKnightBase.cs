using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKViewModel
{
    public abstract class MageKnightBase : IMageKnight
    {
        private string name;
        private Guid id;
        private IReadOnlyList<IMageKnight> army;
        private double ycord;
        private double xcord;
        private IDial dial;
        private int index;

        public MageKnightBase()
        {
            this.Dial = new Dial();
        }

        public string Name { get => name; set => this.name = value; }
        public Guid Id { get => id; set => this.id = value; }
        public double XCord { get => xcord; set => this.xcord = value; }
        public double YCord { get => ycord; set => this.ycord = value; }
        public IDial Dial { get => dial; set => this.dial = value; }
        public int Index { get => index; set => this.index = value; }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            throw new NotImplementedException();
        }
    }
}