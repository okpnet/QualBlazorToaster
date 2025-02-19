using BlazorToaster.Observe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public class ToastModel<T>:IToastModel<T>,IDisposable
    {
        readonly Action _removeAction;

        readonly CancellationTokenSource _cancellationTokenSource = new();

        ToastState _state =ToastState.Stop;

        public Guid Id { get; }=Guid.NewGuid();

        public int ClosedTime { get; } = 3000;

        public T Content { get; set; } = default!;

        public ToastState State => _state;

        public CancellationToken CancelToken => _cancellationTokenSource.Token;

        public ToastObservable<T> StateChangeObservable { get; } = new();

        public ToastModel(Guid id,Action removeAction,T content,int cloosedTimer)
        {
            Id = id;
            Content = content;
            ClosedTime = cloosedTimer;
            _removeAction = removeAction;
        }

        public async Task StartAsync()
        {
            if (0 > ClosedTime)
            {
                return;
            }
            _state = ToastState.Running;
            await Task.Delay(10);
            StateChangeObservable.Run(Content);
            await Task.Delay(ClosedTime,CancelToken).
                ContinueWith(task => 
                {
                    if (CancelToken.IsCancellationRequested)
                    {
                        _state= ToastState.Stop;
                        StateChangeObservable.Run(Content);
                        return;
                    }
                    _state = ToastState.Complete;
                    StateChangeObservable.Run(Content);
                    //Dispose();
                }).
                ContinueWith(async _=>await Task.Delay(100)).
                ContinueWith(_=>Dispose());//待ってから消去
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
            StateChangeObservable.Run(Content);
        }

        public void Dispose()
        {
            _removeAction.Invoke();
        }
    }
}
