
using MKViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Extension;

namespace MKView
{
    public class Container : UnityContainerExtension
    {
        public Container()
        {
        }

        protected override void Initialize()
        {
            this.Container.RegisterType<Provider>();
            this.Container.RegisterType<IMageKnightGenerator, MageKnightGenerator>();
           // this.Container.RegisterType<IArmyBuilder, ArmyBuilder>();
           // this.Container.RegisterType<IMainMenu, MainMenu>();
            this.Container.RegisterType<IBattleGround, BattleGround>();
        }
    }
}
