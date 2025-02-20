using BlazorToaster.Observe;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public class ToastModel<T>:IToastModel<T>,IDisposable, IToastArg<T>
    {
        readonly ToastObservable<T> _toastObservable;

        readonly Action _removeAction;

        readonly CancellationTokenSource _cancellationTokenSource = new();

        EventCallback _closeEvent=EventCallback.Empty;

        ToastState _state =ToastState.Stop;

        public Guid Id { get; }=Guid.NewGuid();

        public int ClosedTime { get; } = 3000;

        public T Content { get; set; } = default!;

        public ToastState State => _state;

        public CancellationToken CancelToken => _cancellationTokenSource.Token;

        public IObservable<T> ChangeObservable => _toastObservable;

        public EventCallback CloseEvent 
        {
            get
            {
                if (Equals(_closeEvent, EventCallback.Empty))
                {
                    _closeEvent = EventCallback.Factory.Create(this,Close);
                }
                return _closeEvent;
            }
        }

        public ToastModel(Guid id,Action removeAction,T content,int cloosedTimer)
        {
            Id = id;
            ClosedTime = cloosedTimer;
            Content = content;
            _removeAction = removeAction;
            _toastObservable = new ToastObservable<T>();
        }

        public async Task StartAsync()
        {
            if (0 > ClosedTime)
            {
                return;
            }
            _state = ToastState.Running;
            await Task.Delay(ClosedTime, CancelToken);

            if (CancelToken.IsCancellationRequested)
            {
                return;
            }
            _state = ToastState.Complete;
            _toastObservable.Run(Content);
            await Task.Delay(1000);
            _state = ToastState.Removed;
            Dispose();
        }

        public void Cancel()
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested || _state!=ToastState.Running)
            {
                return;
            }
            _cancellationTokenSource.Cancel();
        }

        public void Close()
        {
            if(_state == ToastState.Complete)
            {
                return;
            }

            if (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }
            _state= ToastState.Complete;
            _toastObservable.Run(Content);
        }

        public void Dispose()
        {
            _removeAction.Invoke();
        }
    }
}
