namespace VRT.Payments.Gateways.Extensions;
public static class DecimalExtensions
{
    public static int ToCents(this decimal value) => (int)(value * 100);
}
