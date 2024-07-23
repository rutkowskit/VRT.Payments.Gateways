namespace Examples.BlazorServer.Extensions;

public static class NavigationManagerExtensions
{
    public static void NavigateToRelativeUrl(this NavigationManager navigation,
        string relativeUrl)
    {
        var unescapedUri = Uri.UnescapeDataString(navigation.Uri);

        var uri = new Uri(unescapedUri)
            .GetComponents(UriComponents.Scheme | UriComponents.Path | UriComponents.HostAndPort, UriFormat.Unescaped);

        var newUrl = new Uri(Path.Combine(uri, relativeUrl));
        navigation.NavigateTo(newUrl.ToString());
    }
}
