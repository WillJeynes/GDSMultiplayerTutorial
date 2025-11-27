using System;
using Newtonsoft.Json;
using UnityEngine;

public class Utils
{
    // public API
    public static void SendMessage<T>(string title, T cnt) where T : Content
    {
        var msg = new Message<T>();
        msg.title = title;
        msg.id = Guid.NewGuid().ToString();
        msg.content = cnt;

        string content = JsonConvert.SerializeObject(msg);
        MultiplayerManager.instance.websocket.SendText(content);
        Debug.Log("Sent message: " + content);
    }

    // conversions
    public static T ConvertTo<T>(string data) where T : Content
    {
        return JsonConvert.DeserializeObject<Message<T>>(data).content;
    }
}

public class Message<T> where T : Content{
    public string title;
    public string id;

    public T content;
}