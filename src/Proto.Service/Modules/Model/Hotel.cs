using System.Collections.Generic;
using Proto.Service.Modules.Model;
using ProtoBuf;

namespace Proto.Service.Modules.Model
{
    [ProtoContract]
    public class Hotel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string HotelCode { get; set; }
        [ProtoMember(3)]
        public string HotelStatus { get; set; }
        [ProtoMember(4)]
        public string HotelCurrency { get; set; }
        [ProtoMember(5)]
        public int MinutesOffsetUtc { get; set; }
        [ProtoMember(6)]
        public string ProviderName { get; set; }
        [ProtoMember(7)]
        public IList<Room> Rooms { get; set; }
    }
}