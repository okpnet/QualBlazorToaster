using BlazorToaster.Observe;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Core
{
    public class ToastModel<T> : IToastModel<T>, IDisposable, IToastArg<T>
    {
        readonly ToastObservable<T> _toastObservable;

        readonly Action _removeAction;

        readonly CancellationTokenSource _cancellationTokenSource = new();

        EventCallback _closeEvent = EventCallback.Empty;

        ToastState _state = ToastState.Stop;

        public Guid Id { get; } = Guid.NewGuid();

        public int ClosedTime { get; }

        public T Content { get; }

        public ToastState State => _state;

        public CancellationToken CancelToken => _cancellationTokenSource.Token;

        public IObservable<T> ChangeObservable => _toastObservable;

        public PresentationMode Presentation { get; set; }

        public EventCallback CloseEvent
        {
            get
            {
                if (Equals(_closeEvent, EventCallback.Empty))
                {
                    _closeEvent = EventCallback.Factory.Create(this, CloseAsync);
                }
                return _closeEvent;
            }
        }

        public ToastModel(Guid id, Action removeAction, T content, int cloosedTimer)
        {
            Id = id;
            ClosedTime = cloosedTimer;
            Content = content;
            _removeAction = removeAction;
            _toastObservable = new ToastObservable<T>();
            Presentation = PresentationMode.Auto;
        }

        public async Task StartAsync()
        {
            _state = ToastState.Running;
            _toastObservable.Run(Content);
            await Task.Delay(ClosedTime, CancelToken);

            if (CancelToken.IsCancellationRequested || Presentation==PresentationMode.Event)
            {
                return;
            }

            await HiddenAsync();
        }

        public void Cancel()
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested || _state != ToastState.Running)
            {
                return;
            }
            _cancellationTokenSource.Cancel();
        }

        public async Task CloseAsync()
        {
            if (_state == ToastState.Complete || _state==ToastState.Removed)
            {
                return;
            }

            if (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }

            await HiddenAsync();
        }

        public void Dispose()
        {
            _removeAction.Invoke();
        }

        private async Task HiddenAsync()
        {
            _state = ToastState.Complete;
            _toastObservable.Run(Content);
            await Task.Delay(10);
            _state = ToastState.Removed;
            _toastObservable.Run(Content);
            await Task.Delay(500);
            Dispose();
        }
    }
}
