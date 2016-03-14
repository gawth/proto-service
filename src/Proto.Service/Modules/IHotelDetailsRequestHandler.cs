using Proto.Service.Modules.Model;

namespace Proto.Service.Modules
{
    public interface IHotelDetailsRequestHandler
    {
        HotelDetailsResponseModel Handle(ByHotelIdRequestModel byHotelIdRequestModel);
    }
}