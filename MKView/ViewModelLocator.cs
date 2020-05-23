using MKModel;
using MKViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace MKView
{
    public class ViewModelLocator
    {
        private IUnityContainer container;

        public ViewModelLocator()
        {
            this.container = new UnityContainer();
            this.container.AddExtension(new Container());

        }

        public object ActivePage => this.container.Resolve<IMainMenu>();

        public IBattleGround BattleGround => this.container.Resolve<IBattleGround>();
        public IClick Click => this.container.Resolve<IClick>();
        public IDial Dial => this.container.Resolve<IDial>();
        public IMageKnight MageKnight => this.container.Resolve<IMageKnight>();
        public IStat Stat => this.container.Resolve<IStat>();
        public IMageKnightGenerator MageKnightGenerator => this.container.Resolve<IMageKnightGenerator>();
        public IMageKnightGenerator MainMenu => this.container.Resolve<IMageKnightGenerator>();
        public IArmyBuilder ArmyBuilder => this.container.Resolve<IArmyBuilder>();
        public IUser User => this.container.Resolve<IUser>();
        public IUserViewModel GameUser => this.container.Resolve<IUserViewModel>();

    }
}
