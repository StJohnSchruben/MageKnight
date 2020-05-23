
using MKViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Extension;
using Unity.Lifetime;

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
            this.Container.RegisterType<IMageKnightGenerator, MageKnightGenerator>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IArmyBuilder, ArmyBuilder>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IBattleGround, BattleGround>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IUser, User>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IUserViewModel, UserViewModel>(new ContainerControlledLifetimeManager());
        }
    }
}
