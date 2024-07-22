namespace VRT.Payments.Gateways.Extensions;
internal static class DecimalExtensions
{
    public static int ToCents(this decimal value) => (int)(value * 100);
}
