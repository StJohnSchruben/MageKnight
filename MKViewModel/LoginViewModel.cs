using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MKModel;
using MKService;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MKViewModel
{
    public interface ILoginViewModel : INotifyPropertyChanged
    {
        string UserName { get; set; }
        string Password { get; set; }
        ICommand SignUp { get; }
        ICommand SignIn { get; }
        ICommand LogOut { get; }
        bool IsLoggedIn { get; set; }
    }

    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        private string userName;
        private string password;
        private bool isLoggedIn;
        private IUserCollection model;
        public LoginViewModel(IUserCollection model)
        {
            this.model = model;
            this.UserName = "john";
            this.Password = "password";
            model.Users.CollectionChanged += Users_CollectionChanged;
        }

        private void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var user in e.NewItems.Cast<IUserModel>())
                {
                    if (user.UserName == this.UserName)
                    {
                        user.IsSignedIn = true;
                        ServiceTypeProvider.Instance.LoggedInUserId = user.Id;
                        break;
                    }
                }

                this.IsLoggedIn = true;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return this.isLoggedIn;
            }
            set
            {
                this.Set(() => this.IsLoggedIn, ref this.isLoggedIn, value);

                if (value)
                {
                    // fill data
                }
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.Set(() => this.Password, ref this.password, value);
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.Set(() => this.UserName, ref this.userName, value);
            }
        }

        public ICommand SignUp => new RelayCommand(this.SignUpClicked);

        private void SignUpClicked()
        {
            foreach (var user in this.model.Users)
            {
                if (user.UserName == this.UserName)
                {
                    MessageBox.Show($"User Name: {this.UserName} is in use, please try another name");     
                    return;
                }
            }

            var newuser = this.model.CreateUser(this.UserName, this.Password);
            //ServiceTypeProvider.Instance.LoggedInUser = newuser;
            //this.IsLoggedIn = true;
        }

        public ICommand SignIn => new RelayCommand(this.SignInClicked);


        private void SignInClicked()
        {
            foreach(var user in this.model.Users)
            {
                if (user.UserName == this.UserName && user.Password == this.Password)
                {                   
                    user.PropertyChanged += User_PropertyChanged;
                    user.IsSignedIn = true;
                    break;
                }
            }
        }

        public ICommand LogOut => new RelayCommand(this.LogOutClicked);

        private void LogOutClicked()
        {
            var user = this.model.Users.First(x => x.Id == ServiceTypeProvider.Instance.LoggedInUserId);
            user.IsSignedIn = false;
            this.RaisePropertyChanged(nameof(user));
            this.IsLoggedIn = false;
        }

        private void User_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IUserModel user = sender as IUserModel;

            if (e.PropertyName == nameof(user.IsSignedIn) && user.IsSignedIn)
            {
                ServiceTypeProvider.Instance.LoggedInUserId = user.Id;
                this.IsLoggedIn = true;
            }
        }
    }
}
