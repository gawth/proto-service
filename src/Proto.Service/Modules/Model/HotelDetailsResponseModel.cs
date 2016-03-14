using System.Collections.Generic;
using ProtoBuf;

namespace Proto.Service.Modules.Model
{
    [ProtoContract]
    public class HotelDetailsResponseModel
    {
        [ProtoMember(1)]
        public IEnumerable<Hotel> Hotels { get; set; }
    }
}