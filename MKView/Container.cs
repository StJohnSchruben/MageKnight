
using Microsoft.Practices.Unity;
using MKModel;
using MKService;
using MKViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKView
{
    public class Container : UnityContainerExtension
    {
        public Container()
        {
        }

        protected override void Initialize()
        {
            ServiceTypeProvider.Instance.UseService = true;
            ServiceTypeProvider.Instance.EndPoint = MK2DStartupSettings.WstServiceEndPoint;
            this.Container.RegisterType<IArmyBuilder, ArmyBuilder>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IBattleGround, BattleGround>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IGameViewModel, GameViewModel>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IGameModel>(new ContainerControlledLifetimeManager(), new InjectionFactory(x => ServiceTypeProvider.Instance.Game));
            this.Container.RegisterType<IUserModel>(new ContainerControlledLifetimeManager(), new InjectionFactory(x => ServiceTypeProvider.Instance.LoggedInUser));
            this.Container.RegisterType<IUserCollection>(new ContainerControlledLifetimeManager(), new InjectionFactory(x => ServiceTypeProvider.Instance.UserCollection));
            this.Container.RegisterType<IGameModels>(new ContainerControlledLifetimeManager(), new InjectionFactory(x => ServiceTypeProvider.Instance.Games));
            this.Container.RegisterType<IUserViewModel, UserViewModel>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ILoginViewModel, LoginViewModel>(new ContainerControlledLifetimeManager());
        }
    }
}
