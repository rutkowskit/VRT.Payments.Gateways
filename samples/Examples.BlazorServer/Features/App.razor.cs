namespace Examples.BlazorServer.Features;

public partial class App
{
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        return base.OnInitializedAsync();
    }
}