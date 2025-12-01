using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MyMultiplayerManager : MonoBehaviour
{
    public string Name;

    public string Room;

    public string Server;

    public GameObject Player;

    public GameObject OtherPlayer;

    public TextMeshProUGUI PlayerText;

    public TextMeshProUGUI OtherPlayerText;
    
    public void AsyncUpdate()
    {
        //run repeating multiplayer code here
    }

    public void OnMessageReceived(string title, string data)
    {
        switch (title)
        {
            case "lobby":
                //assign our player name to UI
                PlayerText.SetText(Name);

                // get message content
                var lobby = Utils.ConvertTo<LobbyContent>(data);

                //assign other player name to UI
                var otherName = "None";
                foreach(string s in lobby.names) {
                    // if it's not us
                    if (s != Name)
                    {
                        otherName = s;
                    }
                }
                OtherPlayerText.SetText(otherName);

                break;

            //handle other message types
        }
    }
}