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
            var response = (Response)(JsonConvert.SerializeObject(DataStub.DummyData(DataStub.testHotelCount,DataStub.testRoomCount)));
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
                Contents = stream => DataStub.DummyDataPb(DataStub.testHotelCount, DataStub.testRoomCount).WriteTo(stream)
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
                Contents = stream => serializer.Pack(stream, DataStub.DummyData(DataStub.testHotelCount, DataStub.testRoomCount))
            };
        }

    }

}