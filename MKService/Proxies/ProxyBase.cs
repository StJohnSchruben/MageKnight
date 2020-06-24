
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

using Service;
using MKService.ModelUpdaters;
using MKService.Updates;
using ReDefNet;
//using GalaSoft.MvvmLight;

namespace MKService.Proxies
{
    internal abstract class ProxyBase : ObservableObject
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ConcurrentDictionary<Type, List<Action<IMessage>>> messageHandlers = new ConcurrentDictionary<Type, List<Action<IMessage>>>();

        protected ProxyBase(IServiceClient serviceClient, IModelUpdaterResolver modelUpdaterResolver)
        {
            this.ServiceClient = serviceClient;
            this.ModelUpdaterResolver = modelUpdaterResolver;

            this.ServiceClient.MessageReceived += this.OnServiceClientMessageReceived;
        }

        protected IModelUpdaterResolver ModelUpdaterResolver { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected IServiceClient ServiceClient { get; }

        protected void SetUpModelPropertyChangedPropagation<TModel>(TModel model)
            where TModel : IUpdatable, INotifyPropertyChanged
        {
            model.PropertyChanged += this.OnModelPropertyChanged;
        }

        [DebuggerStepThrough]
        protected void SubscribeToMessage<TMessage>(Action<TMessage> messageHandler) where TMessage : IMessage
        {
            if (messageHandler == null)
            {
                throw new ArgumentNullException(nameof(messageHandler));
            }

            var messageType = typeof(TMessage);

            if (this.messageHandlers.ContainsKey(messageType))
            {
                // Warning: not checking for duplicate delegates here, so duplicate message handlers are allowed
                this.messageHandlers[messageType].Add(x => messageHandler((TMessage)x));
            }
            else
            {
                // Wrapping a delegate in a delegate since .NET doesn't allow delegates to be casted
                this.messageHandlers.TryAdd(
                    messageType,
                    new List<Action<IMessage>>
                    {
                        x => messageHandler((TMessage)x)
                    });

                this.ServiceClient.SubscribeAsync<TMessage>();
            }
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(e.PropertyName);
        }

        [DebuggerStepThrough]
        private void OnServiceClientMessageReceived(object sender, PublishedEventArgs e)
        {
            if (e.Message == null)
            {
                return;
            }

            var messageType = e.Message.GetType();

            if (!this.messageHandlers.ContainsKey(messageType))
            {
                return;
            }

            var handlers = this.messageHandlers[messageType];

            foreach (var handler in handlers)
            {
                handler.Invoke(e.Message);
            }
        }
    }
}