using System;
using System.IO;
using System.Collections.Generic;
using Nancy;
using Nancy.Json;
using Nancy.ModelBinding;
using Proto.Service.Modules.Model;
using ProtoBuf;
using MsgPack.Serialization;
using Google.Protobuf;
using Newtonsoft.Json;


namespace Proto.Service.Modules.V3
{
    public class HotelDetailsModule : NancyModule
    {
        private readonly int testHotelCount = 50;
        private readonly int testRoomCount = 10;
        private static MessagePackSerializer<HotelDetailsResponseModel> _serializerMsgPack ;

        public HotelDetailsModule()
        {
            JsonSettings.MaxJsonLength = Int32.MaxValue;
            Get["/testjson"] = parameters => TestJson();
            Get["/testprotobuf"] = parameters => TestProtoBuf();
            Get["/testmsgpack"] = parameters => TestMsgPack(true);
            Get["/testmsgpack2"] = parameters => TestMsgPack(false);


            if (_serializerMsgPack == null)
            {
                Console.WriteLine("HotelDetailsModule Setup Serializer");
                // Initialise the serializer we're going to use for Message Pack
                var context = new MsgPack.Serialization.SerializationContext();
                context.SerializationMethod = SerializationMethod.Map;
                _serializerMsgPack = MessagePackSerializer.Get<HotelDetailsResponseModel>(context);
            }
        }

        public Response TestJson()
        {
            return HandleJson();
        }

        public Response TestProtoBuf()
        {
            return HandleProtoBuf();
        }
        public Response TestMsgPack(bool useMap)
        {
            return HandleMsgPack(useMap);
        }

        public Response HandleJson()
        {
            var response = (Response)(JsonConvert.SerializeObject(DummyData(testHotelCount,testRoomCount)));
            response.ContentType = "application/json";
            response.StatusCode = HttpStatusCode.OK;
            response.Headers.Add("Cache-Control", string.Format("public, max-age={0}", (15 * 60)));
            return response;
        }
        public Response HandleProtoBuf()
        {
            return new Response
            {
                ContentType = "application/x-protobuf",
                StatusCode = HttpStatusCode.OK,
                Contents = stream => DummyDataPb(testHotelCount, testRoomCount).WriteTo(stream)
            };
        }
        public Response HandleMsgPack(bool useMap)
        {

            MessagePackSerializer<HotelDetailsResponseModel> serializer ;
            if (useMap)
            {
                serializer = _serializerMsgPack;
            }
            else
            {
                serializer = MessagePackSerializer.Get<HotelDetailsResponseModel>();
            }

            return new Response
            {
                ContentType = "application/x-msgpack",
                StatusCode = HttpStatusCode.OK,
                Contents = stream => serializer.Pack(stream, DummyData(testHotelCount, testRoomCount))
            };
        }

        private static HotelDetailsResponseModel DummyData(int hotelCount, int roomCount)
        {
            IList<Room> rooms = new List<Room>();
            for (int i = 0; i < roomCount; i++)
            {
                var tmp = new Room
                {
                    Id = i,
                    InvCode = "inv",
                    HideRackRate = false,
                    RackRate = "1.0m"
                };
                rooms.Add(tmp);
            }

            IList<Model.Hotel> hotels = new List<Model.Hotel>();
            for (int j = 0; j < hotelCount; j++)
            {
                var tmp = new Model.Hotel
                {
                    HotelCode = "Code",
                    Id = j,
                    ProviderName = "Provider",
                    HotelCurrency = "GBP",
                    HotelStatus = "1",
                    Rooms = rooms
                };
                hotels.Add(tmp);
            }

            var mod = new HotelDetailsResponseModel
            {
                Hotels = hotels
            };

            return mod;

        }
        private static Proto.Hotel.HotelDetailsResponseModel DummyDataPb(int hotelCount, int roomCount)
        {
            var mod = new Proto.Hotel.HotelDetailsResponseModel();
            for (int j = 0; j < hotelCount; j++)
            {
                var tmp = new Proto.Hotel.Hotel
                {
                    HotelCode = "Code",
                    Id = j.ToString(),
                    ProviderName = "Provider",
                    HotelCurrency = "GBP",
                    HotelStatus = "1"
                };

                for (int i = 0; i < roomCount; i++)
                {
                    var tmpR = new Proto.Hotel.Room
                    {
                        Id = i,
                        InvCode = "inv" + i,
                        HideRackRate = false,
                        RackRate = "1.0"
                    };
                    tmp.Rooms.Add(tmpR);
                }
                mod.Add(tmp);
            }


            return mod;

        }
    }

}