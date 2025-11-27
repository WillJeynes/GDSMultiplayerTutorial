# Simple Multiplayer Tutorial
Written For [Sheffield Game DevSoc](https://bit.ly/ShefDev)

## Student Guide

TODO: Explain helper methods, setup etc


## Session Runner Guide

**Backend**
The backend is a simple node.js app, with a websocket server and simple http server

The websocket is open by default on ws://localhost:8080

The plaintext report is available on http://localhost:3000

The flow is simple
- A client connects directly to the websocket
- They send an "init" message, with name and roomId
- From this point onwards, any message they send will be rebroadcast to all other clients in the room, and vice versa

This can be hosted anything that supports Node, the session runners laptop will suffice for a temporary setup.

**Frontend**
This is a very simple 2D platformer game, to provide people with something they can expand on

Currently setup to have two players, but this is an artificial limitation for simplicity not a fundamental backend one

Most setup code is available in MultiplayerManager.cs, with anything custom living in MyMultiplayerManager.cs