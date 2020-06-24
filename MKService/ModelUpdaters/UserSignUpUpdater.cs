using MKModel;
using MKService.Messages;
using MKService.ModelFactories;
using MKService.Updates;
using System;
using MKService.DefaultModel;

namespace MKService.ModelUpdaters
{
    internal class UserSignUpUpdater : ModelUpdaterBase<IUpdatableUserCollection, UserSignUp>
    {
        public UserSignUpUpdater(
            IModelFactoryResolver modelFactoryResolver, IDefaultModel defaultMode)
            : base(modelFactoryResolver, defaultMode)
        {
        }

        protected override bool UpdateInternal(IUpdatableUserCollection model, UserSignUp message)
        {
            UserModel newUserData;
            try
            {
                UserData data = new UserData();
                data.UserName = message.UserName;
                data.Password = message.Password;
                data.Id = message.Id;
                newUserData = UserDataDBService.SignUp(data);
                var newUser = this.ModelFactoryResolver.GetFactory<IUpdatableUser>().Create();
                newUser.UserName = newUserData.UserName;
                newUser.Id = newUserData.Id;
                model.Users.Add(newUser);
              
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
