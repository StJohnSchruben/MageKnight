using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MKModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MKViewModel
{
    public class MageKnightGenerator : ViewModelBase, IMageKnightGenerator
    {
        private string name;
        private int index;
        private int pointValue;
        private int range;
        private string set;
        private string faction;
        private int frontArc;
        private int rearArc;
        private int click;
        private IStats stats;
        private Rank rank;
        private MageData mage;
        private List<IMageKnightModel> mageknights;
        private IMageKnightModel selectedMageKnight;
        public MageKnightGenerator( )
        {
            this.mage = new MageData();
            this.selectedMageKnight = new MageKnight(this.mage);
            this.EnterClicked();
        }

        public MageData Mage
        {
            get => this.mage;
            set => this.Set(() => this.Mage , ref this.mage, value); 
        }

        public ICommand Enter => new RelayCommand(this.EnterClicked);

        private void EnterClicked()
        {
           this.MageKnights = MageDB.GetMageKnights();
        }

        private void FillDataBase()
        {
            //    MageDB.ResetDB();
            // this.FillMageKnights();
            //ProcessDirectory("C:\\MageKnightDatabase\\MKData\\Rebellion\\RebellionDialsFormattedData");
        }

        private void FillMageKnights()
        {
            string path = "C:\\MageKnightDatabase\\MKData\\Rebellion\\RebellionText.txt";
            string imagePath = "C:\\MageKnightDatabase\\MKData\\Rebellion\\RebellionImages\\";
            string[] lines = System.IO.File.ReadAllLines(path);

            foreach (string line in lines)
            {
                MageData mage = new MageData();
                mage.Id = Guid.NewGuid();
                mage.Set = "Rebellion";
                if (line.StartsWith("/"))
                {
                    continue;
                }

                var data = line.Split('|');
                int i = 0;
                foreach (var l in data)
                {
                    i++;
                    switch (i)
                    {
                        case 1:
                            mage.Name = l.TrimEnd().TrimStart();
                            break;
                        case 2:
                            mage.PriceValue = l.TrimEnd().TrimStart();
                            break;
                        case 3:
                            mage.Rank = l.TrimEnd().TrimStart();
                            break;
                        case 4:
                            mage.PointValue = Int32.Parse(l.TrimEnd().TrimStart());
                            break;
                        case 5:
                            mage.Index = Int32.Parse(l.TrimEnd().TrimStart());
                            break;
                        case 6:
                            mage.Rarity = Int32.Parse(l.TrimEnd().TrimStart());
                            break;
                        case 7:
                            mage.Faction = l.TrimEnd().TrimStart();
                            break;
                        case 8:
                            mage.Range = Int32.Parse(l.TrimEnd().TrimStart());
                            break;
                        case 9:
                            mage.FrontArc = Int32.Parse(l.TrimEnd().TrimStart());
                            break;
                        case 10:
                            mage.Targets = Int32.Parse(l.TrimEnd().TrimStart());
                            break;
                    }

                 //   mage.ModelImage = imagePath + mage.Name + ".jpg";
                }

                MageDB.GenerateMageKnight(mage);
            }
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            fs.Seek(0, SeekOrigin.Begin);

            StreamReader sw = new StreamReader(fs);
            int number = Int32.Parse(path.Split('\\').Last());
            List<IClick> clicks = new List<IClick>();
            while (sw.Peek() >=0)
            {
                var line = sw.ReadLine();
                var sl = line.Split('\t');
                int i = 0;
                IStat speedStat = new Stat();
                IStat attackStat = new Stat();
                IStat defenseStat = new Stat();
                IStat damageStat = new Stat();
                foreach(var c in sl) 
                {
                    i++;
                    switch (i)
                    {
                        case 1:
                            //click number
                            break;
                        case 2:
                            //speed
                            speedStat.StatType = StatType.Speed;
                            if (c == "----")
                            {
                                ;
                            }
                            speedStat.Value = Int32.Parse(c);
                            break;
                        case 3:
                            //speed special ability
                            if (c != "----")
                                speedStat.Ability = c.TrimEnd();
                            break;
                        case 4:
                            //attack
                            attackStat.StatType = StatType.Attack;
                            if (c == "----")
                            {
                                ;
                            }
                            attackStat.Value = Int32.Parse(c);
                            break;
                        case 5:
                            //attack special ability
                            if (c != "----")
                                attackStat.Ability = c.TrimEnd();
                            break;
                        case 6:
                            //defence
                            defenseStat.StatType = StatType.Defense;
                            if (c == "----")
                            {
                                ;
                            }
                            defenseStat.Value = Int32.Parse(c);
                            break;
                        case 7:
                            //defence special ability
                            if (c != "----")
                                defenseStat.Ability = c.TrimEnd();
                            break;
                        case 8:
                            //damage
                            damageStat.StatType = StatType.Damage;
                            if (c == "----")
                            {
                                ;
                            }
                            damageStat.Value = Int32.Parse(c);
                            break;
                        case 9:
                            //damage special ability
                            if (c != "----")
                                damageStat.Ability = c.TrimEnd();
                            break;
                    }
                }

                IClick click = new Click(speedStat, attackStat, defenseStat, damageStat);
                clicks.Add(click);
            }

            MageDB.CreateDialStats(clicks, number);

            sw.Close();
            fs.Close();
            //string[] lines = System.IO.File.ReadAllLines(path);

            //int i = 0;
            //string newText = string.Empty;
            //foreach (string line in lines)
            //{
            //    i++;

            //    if (i <= 2)
            //    {
            //        newText += line;
            //    }
            //    else if (i == 3)
            //    {
            //        newText += line + '\n';
            //        i = 0;
            //    }
            //}
        }

        public ICommand Edit => new RelayCommand(this.EditClicked);

        private void EditClicked()
        {
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        public int Index
        {
            get
            {
                return this.mage.Index;
            }

            set
            {
                this.mage.Index = value;
            }
        }
        public int PointValue
        {
            get
            {
                return this.mage.PointValue;
            }

            set
            {
                this.mage.PointValue = value;
            }
        }
        public int Range
        {
            get
            {
                return this.mage.Range;
            }

            set
            {
                this.mage.Range = value;
            }
        }
        public string Set
        {
            get
            {
                return this.mage.Set;
            }

            set
            {
                this.mage.Set = value;
            }
        }
        public string Faction
        {
            get
            {
                return this.mage.Faction;
            }

            set
            {
                this.mage.Faction = value;
            }
        }

        public int FrontArc
        {
            get
            {
                return this.mage.FrontArc;
            }

            set
            {
                this.mage.FrontArc = value;
            }
        }
        public int Targets
        {
            get
            {
                return this.mage.Targets;
            }

            set
            {
                this.mage.Targets = value;
            }
        }
        public int Click
        {
            get
            {
                return this.mage.Click;
            }

            set
            {
                this.mage.Click = value;
            }
        }

        public string Rank
        {
            get
            {
                return this.mage.Rank;
            }

            set
            {
                this.mage.Rank = value;
            }
        }

        public List<IMageKnightModel> MageKnights
        { 
            get => this.mageknights; 
            
            set
            {
                this.Set(() => this.MageKnights, ref this.mageknights, value);
            }
        }

        public IMageKnightModel SelectedMageKnight
        {
            get
            {
                return this.selectedMageKnight;
            }

            set
            {
                this.Set(() => this.SelectedMageKnight, ref this.selectedMageKnight, value);
            }
        }
    }
}
