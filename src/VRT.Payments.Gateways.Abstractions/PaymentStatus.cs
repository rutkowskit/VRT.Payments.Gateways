using VRT.Generators.EnumToClass;

namespace VRT.Payments.Gateways;

[EnumToClass<Statuses>]
public sealed partial record PaymentStatus
{
    public enum Statuses
    {
        None = 0,
        New = 1,
        Pending = 2,
        Completed = 3,
        Canceled = 99,
    }

    public bool IsFinal => Value switch
    {
        Statuses.None or Statuses.Completed or Statuses.Canceled => true,
        _ => false,
    };
}