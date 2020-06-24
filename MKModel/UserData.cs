using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKModel
{
    public class UserData
    {
        private string userName = string.Empty;
        private string password = string.Empty;
        private Guid id;
        private int rebellionBoosterPacks;
        private int whirlWindBoosterPacks;
        private int lancersBoosterPacks;
        private int unlimitedBoosterPacks;
        private int sinisterBoosterPacks;
        private int minionsBoosterPacks;
        private int uprisingBoosterPacks;

        public UserData()
        {
            id = Guid.NewGuid();
        }

        public UserData(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
            id = Guid.NewGuid();
        }

        public string UserName { get => this.userName; set => this.userName = value; }
        public string Password { get => this.password; set => this.password = value; }
        public int RebellionBoosterPacks { get => this.rebellionBoosterPacks; set => this.rebellionBoosterPacks = value; }
        public int WhirlWindBoosterPacks { get => this.whirlWindBoosterPacks; set => this.whirlWindBoosterPacks = value; }
        public int LancersBoosterPacks { get => this.lancersBoosterPacks; set => this.lancersBoosterPacks = value; }
        public int UnlimitedBoosterPacks { get => this.unlimitedBoosterPacks; set => this.unlimitedBoosterPacks = value; }
        public int SinisterBoosterPacks { get => this.sinisterBoosterPacks; set => this.sinisterBoosterPacks = value; }
        public int MinionsBoosterPacks { get => this.minionsBoosterPacks; set => this.minionsBoosterPacks = value; }
        public int UprisingBoosterPacks { get => this.uprisingBoosterPacks; set => this.uprisingBoosterPacks = value; }

        public Guid Id { get => this.id; set => this.id = value; }
    }
}
