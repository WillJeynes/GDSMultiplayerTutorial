# Simple Multiplayer Tutorial
Written For [Sheffield Game DevSoc](https://bit.ly/ShefDev)

## Student Guide

For this tutorial, all code we need to edit is in Assets/MyMultiplayerManager.cs

### Content Types
There are a few content types that the message can be
- StringContent 
- Vector2Content 
- FloatContent

These can be send as a message, then parsed on the other side

### Sending a message
You can make a content, then set it's values

From AsyncUpdate, we can use Utils.SendMessage with the type, and the content
```cs
Vector2Content content = new Vector2Content();
content.x = 3.0f;
content.y = 6.0f;

Utils.SendMessage("type", content);
```

### Receiving a message
We can expand the existing switch statement to handle a new type
```cs
case "type":
    Vector2Content content = Utils.ConvertTo<Vector2Content>(data);
    Debug.Log(content.x);
    Debug.Log(content.y);

```



## Session Runner Guide

### Backend

The backend is a simple node.js app, with a websocket server and simple http server

The websocket is open by default on ws://localhost:7777

The plaintext report is available on http://localhost:7778

The flow is simple
- A client connects directly to the websocket
- They send an "init" message, with name and roomId
- From this point onwards, any message they send will be rebroadcast to all other clients in the room, and vice versa

This can be hosted anything that supports Node, the session runners laptop will suffice for a temporary setup.

### Frontend

This is a very simple 2D platformer game, to provide people with something they can expand on

Currently setup to have two players, but this is an artificial limitation for simplicity not a fundamental backend one

Most setup code is available in MultiplayerManager.cs, with anything custom living in MyMultiplayerManager.cs