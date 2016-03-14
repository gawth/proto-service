using System.Collections.Generic;
using ProtoBuf;

namespace Proto.Service.Modules.Model
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ByHotelIdRequestModel
    {
        public IEnumerable<int> HotelIds { get; set; }
    }
}