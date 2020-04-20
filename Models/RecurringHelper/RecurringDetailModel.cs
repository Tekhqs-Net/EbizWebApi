using PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.RecurringHelper
{
    public class RecurringDetailModel
    {
        public SecurityToken securityToken { get; set; }
        public string customerId { get; set; }
        public string customerInternalId { get; set; }
        public string paymentMethodProfileId { get; set; }
        public string scheduledPaymentInternalId { get; set; }
        public string paymentInternalId { get; set; }
        public int statusId { get; set; }
        public string invoiceNumber { get; set; }
        public int start { get; set; }
        public int limit { get; set; }
        public string sort { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public DateTime nextDate { get; set; }
        public DateTime fromDateTime { get; set; }
        public DateTime toDateTime { get; set; }
        public RecurringBilling recurringBilling { get; set; }

    }
    public struct DateOnly
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public static class DateOnlyExtensions
    {
        public static DateOnly GetDateOnly(this DateTime dt)
        {
            return new DateOnly
            {
                Day = dt.Day,
                Month = dt.Month,
                Year = dt.Year
            };
        }
    }
}
