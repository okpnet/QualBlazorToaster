using BlazorToaster.Observe;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
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

        ToastState _state = ToastState.Stanby;

        public Guid Id { get; } = Guid.NewGuid();

        public IToastConfigure Configure { get; }

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

        public ToastModel(Guid id, Action removeAction, T content, IToastConfigure configure)
        {
            _removeAction = removeAction;
            _toastObservable = new ToastObservable<T>();
            _state = ToastState.Stanby;
            Id = id;
            Configure = configure;
            Content = content;
            Presentation = PresentationMode.Auto;
        }

        public async Task StartAsync()
        {
            try
            {
                if (_state == ToastState.Stanby)
                {
                    _state = ToastState.Start;
                    _toastObservable.Run(Content);
                    await Task.Delay(ToasterDefine.DEFAULT_DELAY, CancelToken);
                }
                if (_state == ToastState.Start)
                {
                    _state = ToastState.Run;
                    _toastObservable.Run(Content);
                    await Task.Delay(Configure.Duration, CancelToken);
                }
                if (_state == ToastState.Run)
                {
                    _state = ToastState.Stop;
                    _toastObservable.Run(Content);
                    await Task.Delay(ToasterDefine.DEFAULT_DELAY, CancelToken);
                }
                _state = ToastState.Delete;
                _toastObservable.Run(Content);
            }
            catch (TaskCanceledException taskEx)
            {
                _state = ToastState.Stanby;
                _toastObservable.Run(Content);
                return;
            }
            catch (Exception ex)
            {
                throw;
            }

            if (CancelToken.IsCancellationRequested || Presentation==PresentationMode.Event)
            {
                return;
            }

            await HiddenAsync();
        }

        public void Cancel()
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested || _state != ToastState.Run)
            {
                return;
            }
            _cancellationTokenSource.Cancel();
        }

        public async Task CloseAsync()
        {
            if (_state == ToastState.Stop || _state==ToastState.Delete)
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
            if(_state!=ToastState.Stop && _state != ToastState.Delete)
            {
                _state = ToastState.Stop;
                _toastObservable.Run(Content);
                await Task.Delay(ToasterDefine.DEFAULT_DELAY, CancelToken);
                _state = ToastState.Delete;
                _toastObservable.Run(Content);
            }
            Dispose();
        }
    }
}
