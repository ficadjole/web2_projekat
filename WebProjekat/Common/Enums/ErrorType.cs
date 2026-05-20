using System.Runtime.Serialization;

namespace Common.Enums
{
    [DataContract]
    public enum ErrorType
    {
        [EnumMember]
        Failure = 0,
        [EnumMember]
        Validation = 1,
        [EnumMember]
        NotFound = 2,
        [EnumMember]
        Conflict = 3,
        [EnumMember]
        Unauthorized = 4,
        [EnumMember]
        Authentication = 5,
        [EnumMember]
        Unexpected = 6
    }
}
