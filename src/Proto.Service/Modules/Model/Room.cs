using MsgPack.Serialization;
using ProtoBuf;
namespace Proto.Service.Modules.Model
{
    [ProtoContract]
    public class Room
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string InvCode { get; set; }
        [ProtoMember(3)]
        public string RackRate { get; set; }
        [ProtoMember(4)]
        public bool HideRackRate { get; set; }
    }
}