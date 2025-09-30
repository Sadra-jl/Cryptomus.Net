using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentStatus
{
    [EnumMember(Value = "new")] New,
    [EnumMember(Value = "pending")] Pending,
    [EnumMember(Value = "process")] Process,
    [EnumMember(Value = "check")] Check,
    [EnumMember(Value = "paid")] Paid,
    [EnumMember(Value = "paid_over")] PaidOver,
    [EnumMember(Value = "confirm_check")] ConfirmCheck,
    [EnumMember(Value = "wrong_amount")] WrongAmount,
    [EnumMember(Value = "wrong_amount_waiting")] WrongAmountWaiting,
    [EnumMember(Value = "wrong_currency")] WrongCurrency,
    [EnumMember(Value = "expired")] Expired,
    [EnumMember(Value = "fail")] Fail,
    [EnumMember(Value = "refund_process")] RefundProcess,
    [EnumMember(Value = "refund_paid")] RefundPaid,
    [EnumMember(Value = "refund_fail")] RefundFail,
    [EnumMember(Value = "cancel")] Cancel,
    [EnumMember(Value = "system_fail")] SystemFail,
    [EnumMember(Value = "locked")] Locked
}