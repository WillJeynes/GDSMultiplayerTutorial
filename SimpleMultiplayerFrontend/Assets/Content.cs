using UnityEngine;

public class Content
{
    
}

public class ConnectionContent : Content{
    public string name;
    public string roomId;
}
public class StringContent : Content{
    public string data;
}
public class Vector2Content : Content{
    public float x;
    public float y;
}
public class FloatContent : Content{
    public float data;
}
public class LobbyContent : Content{
    public string[] names;
}