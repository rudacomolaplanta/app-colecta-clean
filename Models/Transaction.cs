using System.ComponentModel.DataAnnotations;
using System;

namespace desafiocoaniquem.Models
{
    public class Transaction
    {
        public int IdTransaction { get; set; }
        public DateTime RequestTimestamp { get; set; }
        public int RequestAmount { get; set; }
        public string? Email { get; set; }
        public string RequestChannel { get; set; }
        public string RequestPaymentReference { get; set; }
        public string RequestToken { get; set; }
        public DateTime ResponseTimestamp { get; set; }
        public string ResponseAuthorizationCode { get; set; }
        public string ResponseResponseCode { get; set; }
        public string RequestDATA { get; set; }
    }

}