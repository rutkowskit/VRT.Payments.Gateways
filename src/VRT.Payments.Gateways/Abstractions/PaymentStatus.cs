namespace VRT.Payments.Gateways.Abstractions;

public sealed record PaymentStatus
{
    public static readonly PaymentStatus None = new("None", false, true);
    public static readonly PaymentStatus New = new("New", false);
    public static readonly PaymentStatus Pending = new("Pending", false);
    public static readonly PaymentStatus Completed = new("Completed", true);
    public static readonly PaymentStatus Canceled = new("Canceled", true);
    private PaymentStatus(string name, bool isFinal, bool isEmpty = false)
    {
        Name = name;
        IsFinal = isFinal;
        IsEmpty = isEmpty;
    }

    public string Name { get; }
    public bool IsFinal { get; }
    public bool IsEmpty { get; }

    public static implicit operator string(PaymentStatus paymentStatus) => paymentStatus.Name;
}