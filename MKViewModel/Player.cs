using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKViewModel
{
    public interface IPlayer
    {
        IArmy Army { get; set; }
        string Name { get; set; }
        Guid Id { get; set; }
    }

    public class Player : IPlayer
    {
        private string name;
        private Guid id;
        private IArmy army;

        public Player()
        {

        }
        public IArmy Army { get => this.army; set => this.army = value; }
        public string Name { get => this.name; set => this.name = value; }
        public Guid Id { get => this.id; set => this.id = value; }
    }
}