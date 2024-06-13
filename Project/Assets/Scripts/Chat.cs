using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [Serializable]
    public class ChatMessage
    {
        public string name;
        public string message;

        public ChatMessage(string name, string message)
        {
            this.name = name;
            this.message = message;
        }
    }

    [SerializeField]
    Text text;

    [SerializeField]
    InputField inputField;

    private SocketIO socket;
    private ConcurrentQueue<ChatMessage> queue = new();

    void Start()
    {
        var uri = new Uri("http://34.123.57.63:3997");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
            {
                {"token", "UNITY" }
            }
            , Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        socket.On("chat", response =>
        {
            string jsonText = response.ToString();
            jsonText = jsonText.Substring(2, jsonText.Length - 4);
            jsonText = jsonText.Replace("\\\"", "\"");

            ChatMessage msg = JsonUtility.FromJson<ChatMessage>(jsonText);
            queue.Enqueue(msg);
        });

        socket.ConnectAsync();
    }

    void Update()
    {
        while (!queue.IsEmpty)
        {
            ChatMessage msg;
            if (queue.TryDequeue(out msg))
                text.text += $"{msg.name} : {msg.message}\n";
        }
    }

    public void Send()
    {
        socket.EmitAsync("chat", JsonUtility.ToJson(new ChatMessage("name", inputField.text)));
        inputField.text = "";
    }
}
