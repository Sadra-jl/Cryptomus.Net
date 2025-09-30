using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StaticWalletStatus
{
    [EnumMember(Value = "active")] Active,
    [EnumMember(Value = "blocked")] Blocked,
    [EnumMember(Value = "in_active")] Inactive
}
