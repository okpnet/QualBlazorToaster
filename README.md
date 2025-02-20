# Blazor Toast Service

## What is this?
This is a toast notification library for Blazor. Currently, it only supports sliding in from the bottom right of the screen and fading out.  
This library was created because there were no existing toast libraries that allowed free customization of tags and CSS.

## How to Use
Include the library in your project or build and install it as a NuGet package.

### 
Adding a model through the `IToastModelCollection` interface will generate the content.  
The maximum number of toasts that can be displayed and the display duration are configured using a `ToastConfigure` class object, which is passed to the `Create` method of the Factory class.  
The number of toasts can be set between 1 and 30, and the display duration can be set between 0.5 and 10 seconds.

```csharp
IToastModelCollection<Model.ToastMessage> modelCollection;

// Use default settings
modelCollection = ToastExtension.Factory.CreateCollection<Model.ToastMessage>();

// Use custom configuration
modelCollection = ToastExtension.Factory.CreateCollection<Model.ToastMessage>(
    new ToastConfigure() { Duration = 5000, MaxToast = 10 }
);
```

### Namespace
Use the following `@using` directives in the Razor page where you want to use the library:

```razor
@using BlazorToaster
@using BlazorToaster.Core
```

### Usage
Embed the toast content into the `ToastTemplate` using a `RenderFragment`.  
The context passed to the content is an instance of `IToastArg<T>`, which includes a T-type object, a close event handler for when the toast is clicked, and a presentation type that determines whether the toast closes via event or after a set duration.  
For toasts that close after a set duration, assigning a `CloseEvent` within the display time allows for manual closure before the timer expires.

```razor
<ToastGroup Collection="modelCollection">
    <ToastTemplate Context="model">
       @{
            model.Presentation = PresentationMode.Event;
        } 
        <div class="notification is-info">
            <button class="delete" @onclick="model.CloseEvent"></button>
            ID: @model.Content.Id.ToString()
        </div>
    </ToastTemplate>
</ToastGroup>
```

To add a toast, use the `Enqueue` method of the `IToastModelCollection` interface.

```razor
@code{

    IToastModelCollection<Model.ToastMessage> modelCollection;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        modelCollection = ToastExtension.Factory.CreateCollection<Model.ToastMessage>();
    }

    void OnClick()
    {
        modelCollection.Enqueue(new());  
    }
}
```

## License
This project is licensed under the MIT License.
See the LICENSE file for details.
