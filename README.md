# proto-service

Noddy app that serves up a bunch of data on json and a protocol buffers endpoints.

This is part of an exercise to trial performance of alternatives to JSON over the wire.  The predominant success criteria is speed.

There are a number of Github repositories:

* [Golang benchmark tests and primary write up of results](https://github.com/gawth/go-proto)
* [Node.js server implementation](https://github.com/gawth/protosrv)
* [.Net server implementation](https://github.com/gawth/proto-service)
* [Protocol Buffer schema](https://github.com/gawth/hotel-proto)
* [Node.js consumer plus benchmarking](https://github.com/gawth/node_proto)

##This Server
This is a self hosted Nancy .Net server implementation with three end points, JSON, Protocol Buffers and Message Pack.

It is __very__ simple.

To get up and running:

1. Build it in Visual Studio
2. Open up a cmd and run the exe

Should then report that its listening on localhost:8888

To test that it is use curl:
    curl -is http://localhost:8888/testjson

##What's Happening

The server has some dummy data methods that just spit out the required stub data for the service.

To do this there is a generated hotel class for protocol buffers (generated from the proto file) and the message pack code is from [MsgPackCli](https://github.com/msgpack/msgpack-cli).


###Message Pack Notes
The original implementation used an IIS ASP.Net hosted server for the tests.  That was cumbersome so I shifted over to using a nancy self host.

Unfortunately that has totally screwed the performance of message pack.  I'm assuming its related to caching/threading.  Check out the results reported in [Golang benchmark tests](https://github.com/gawth/go-proto) for original stats although this currently implementation comes nowhere near those.

In addition to the hosting message pack was also a little be tricky to setup in the first place.  Message pack support serialisation to an array out of the box which you can then override to use serialisation to a map.

To use a map there is a context object that needs to be passed in:

    var context = new MsgPack.Serialization.SerializationContext();
    context.SerializationMethod = SerializationMethod.Map;
    _serializerMsgPack = MessagePackSerializer.Get<HotelDetailsResponseModel>(context);

The construction of this serialiser is slow.  The default implementation is a static so not an issue but as soon as you switch over to using a context the whole thing slows down.  The docs state that maps are slower so your initial assumption is that its the use of maps that have slowed things down.  However, shifting the construction to the constuctor and using a static fixed the perf issue in the first instance.  Maps __are__ slower than arrays but not as bad as the initial benchmarks showed.

