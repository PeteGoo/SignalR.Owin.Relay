SignalR.Owin.Relay
==================

An evil thing indeed. 

A web api server that relays incoming requests through SignalR to connected clients using an Owin Context. Useful for web hook relays.

## What for?
I had a need to implement some web hooks for my [chat bot](https://github.com/petegoo/mmbot). For example, GitHhub would call my REST endpoints when someone pushes to a repo or creates an issues etc. 

The trouble is, sometimes I don't want to have to host my bot on a cloud VM and setup a web server on it just to get these messages. 

## Enter the relay
What I can do is host a web site in the cloud e.g. free on Azure Websites, have GitHub call that web api. If that website also runs SignalR, I can have my on-premises client open a persistent connection to it via SignalR and the details of the request can be issued to my client without exposing it on the web.

## How does it work
The requests are read as a standard OWIN context which is serialized to JSON and sent to the connected clients. 

The client then gets the owin context and can spins up an in-memory OWIN host that can process any routes on the client. An example is included that implements a Nancy client that forwards these relayed requests to a the Nancy engine for processing by any Modules that you have defined. All without ever opening an HTTP listener on the client.

## Are you crazy?
Possibly, do not use in a production scenario. AT YOUR OWN RISK.

![](http://www.reactiongifs.com/wp-content/uploads/2013/04/friendly-fire.gif)

## Limitations
* Only UTF8 POST body is supported at the moment. 
* All requests just get a 200 response with no payload (My current scenario is uni-directional web hooks)
