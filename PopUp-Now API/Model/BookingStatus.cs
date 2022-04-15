using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PopUp_Now_API.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BookingStatus
    {
        [EnumMember(Value = "pending")]
        Pending = 1,
        [EnumMember(Value = "confirmed")]
        Confirmed = 2,
        [EnumMember(Value = "declined")]
        Declined = 3
    }
}