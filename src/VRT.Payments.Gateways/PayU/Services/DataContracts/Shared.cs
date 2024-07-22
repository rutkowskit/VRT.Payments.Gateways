﻿// <auto-generated />

using System.Text.Json.Serialization;
using VRT.Payments.Gateways.Abstractions;

namespace VRT.Payments.Gateways.PayU.Services.DataContracts;
internal static class Shared
{
    public static class PayUOrderStatuses
    {
        public const string New = "NEW";
        public const string Pending = "PENDING";
        public const string Canceled = "CANCELED";
        public const string Rejected = "REJECTED";
        public const string Completed = "COMPLETED";
        public const string WaitingForConfirmation = "WAITING_FOR_CONFIRMATION";
    }
    public class Status
    {
        public string statusCode { get; set; }
        public string severity { get; set; }
        public string code { get; set; }
        public string codeLiteral { get; set; }
        public string statusDesc { get; set; }     
    }

    public class Order
    {
        public string orderId { get; set; }
        public string orderCreateDate { get; set; }
        public string notifyUrl { get; set; }
        public string customerIp { get; set; }
        public string merchantPosId { get; set; }
        public string description { get; set; }
        public string currencyCode { get; set; }
        public string totalAmount { get; set; }
        public string status { get; set; }
        public Product[] products { get; set; }

        public PaymentStatus GetPaymentStatus()
        {
            return status?.ToUpper() switch
            {
                PayUOrderStatuses.Pending => PaymentStatus.Pending,
                PayUOrderStatuses.New => PaymentStatus.New,
                PayUOrderStatuses.Completed => PaymentStatus.Completed,
                PayUOrderStatuses.Canceled => PaymentStatus.Canceled,
                PayUOrderStatuses.WaitingForConfirmation => PaymentStatus.Pending,
                _ => PaymentStatus.None
            };
        }
    }

    public class Product
    {
        public string name { get; set; }
        public string unitPrice { get; set; }
        public string quantity { get; set; }
        [JsonPropertyName("virtual")]
        public bool _virtual { get; set; }
    }
    public class Buyer
    {
        public string extCustomerId { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string language { get; set; }
    }
}
