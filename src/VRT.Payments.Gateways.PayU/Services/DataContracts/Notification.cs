﻿//<auto-generated />

namespace VRT.Payments.Gateways.PayU.Services.DataContracts;

internal sealed class Notification
{
    public string localReceiptDateTime { get; set; }
    public Shared.Order order { get; set; }    
    public Property[] properties { get; set; }    
    public class Property
    {
        public string name { get; set; }
        public string value { get; set; }
    }    
}

