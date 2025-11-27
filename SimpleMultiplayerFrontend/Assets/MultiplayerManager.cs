using System;
using System.Threading.Tasks;
using NativeWebSocket;
using UnityEngine;
using Newtonsoft.Json;
using NUnit.Framework.Internal.Filters;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager instance {get; private set; }

    private MyMultiplayerManager cmm;
    public  WebSocket websocket {get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async Task Start()
    {
        //setup
        cmm = GetComponent<MyMultiplayerManager>();

        //singleton
        instance = this;

        //sanity check
        if (cmm.Name == "CHANGEME" || cmm.Room == "CHANGEME")
        {
            Debug.LogError("Please change the name and room!");
        }
        
        //initialise connection
        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += async () =>
        {
            Debug.Log("Connection open!");
            var connectMessage = new ConnectionContent(){name = cmm.Name, roomId = cmm.Room};

            Utils.SendMessage("init", connectMessage);

            //repeat update. 
            InvokeRepeating(nameof(AsyncUpdate), 0, 0.05f);
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            var content = JsonConvert.DeserializeObject<Message<Content>>(message);
            
            Debug.Log("Message Received: " + content);

            //process it on other script
            cmm.OnMessageReceived(content.title, message);
        };

        await websocket.Connect();
    }

    void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
            websocket.DispatchMessageQueue();
        #endif
    }

    private async void OnDestroy()
    {
        if (websocket != null)
        {
            await websocket.Close();
            Debug.Log("WebSocket closed during OnDestroy");
        }
    }

    private void AsyncUpdate()
    {
        cmm.AsyncUpdate();
    }
}

