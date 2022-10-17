using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DevelopersHub.RealtimeNetworking.Client;
using System;

public class Clients : MonoBehaviour
{
    public static Clients instanse;

    public GameObject loadingScreen;
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;

    public static int WorldMenu = 0;
    public static int[,] map;
    public static int width = 0;
    public static int height = 0;
    public static string Wname = "";

    public enum RequestsID
    {
        AUTH = 1,
        Login = 2,
        Join = 3,
        Spawn = 4
    }

    void Awake()
    {
        instanse = this;
    }

    void Start()
    {
        loadingScreen.SetActive(true);
        ConnectToServer();

        RealtimeNetworking.OnStringReceived += RealtimeNetworking_OnRecivedString;
        RealtimeNetworking.OnPacketReceived += RealtimeNetworking_OnPacketReceived;
        //LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RealtimeNetworking_OnPacketReceived(Packet packet)
    {
        int type = packet.ReadInt();
        Debug.Log(type);
        switch (type)
        {
            case 1: //  World data

                Wname = packet.ReadString(); // wname
                width = packet.ReadInt();
                height = packet.ReadInt();
                byte[] a = packet.ReadBytes(100 * 60 * sizeof(int));

                map = new int[100, 60];
                
                FromBytes(map, a);
                WorldMenuChecker.instance.LoadScene(4);

                break;

            case 2: //  Spawn player

                int id = packet.ReadInt();
                string username = packet.ReadString();
                Vector3 position = packet.ReadVector3(true, true);
                Quaternion rotation = packet.ReadQuaternion(true, true);

                Debug.Log("Spawning player: " + id + " " + username);


                WorldGeneration.instance.Spawn(id, username, position, rotation);
                break;

        }
    }

    public void sendSpawn()
    {
        Sender.TCP_Send((int)RequestsID.Spawn, "SpawnRequest");
    }

    private void RealtimeNetworking_OnRecivedString(int id, string data)
    {
        switch(id)
        {
            case (int)RequestsID.AUTH: //Device have an account
                char[] spearator = { '\n' };

                string[] strlist = data.Split(spearator,
                    System.StringSplitOptions.RemoveEmptyEntries);

                //Debug.Log("From players: " + strlist[0] + " " + strlist[1]);

                LoginOrSingUp.user = strlist[0];
                LoginOrSingUp.pass = strlist[1];
                break;

            case (int)RequestsID.Login:
                if (data == "Correct")
                {
                    WorldMenu = 1;
                }
                else if (data == "Failed")
                {

                }
                else
                {

                }
                break;
        }
    }

    private void OnConnectingToServerResult(bool successful)
    {
        if (successful)
        {
            RealtimeNetworking.OnDisconnectedFromServer += DisconnectedFromServer;
            loadingText.text = "Welcome";
            LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            string device = SystemInfo.deviceUniqueIdentifier; //we can use this to ban the device :)

            Sender.TCP_Send((int)RequestsID.AUTH, device);
        }
        else
        {
            //LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            loadingText.text = "Couldn't Connect to the server";
        }
        RealtimeNetworking.OnConnectingToServerResult -= OnConnectingToServerResult;
    }

    private void DisconnectedFromServer()
    {
        RealtimeNetworking.OnDisconnectedFromServer -= DisconnectedFromServer;
        //Debug.Log("Disconnected from the server ):");
        //LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public static void FromBytes(int[,] array, byte[] buffer)
    {
        var len = Math.Min(array.GetLength(0) * array.GetLength(1) * sizeof(int), buffer.Length);
        Buffer.BlockCopy(buffer, 0, array, 0, len);
    }

    public void ConnectToServer()
    {
        loadingText.text = "Connecting to the server";

        /*Bounds bounds = loadingText.textBounds;
        Vector2 newPos = new Vector2(-bounds.center.x, -bounds.center.y);

        RectTransform rt = loadingText.transform.parent.GetComponent<RectTransform>();

        rt.localPosition = newPos;*/

        RealtimeNetworking.OnConnectingToServerResult += OnConnectingToServerResult;
        RealtimeNetworking.Connect();
    }
    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneAsynchronously(index));
    }

    IEnumerator LoadSceneAsynchronously(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        //loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            loadingText.text = operation.progress.ToString();
            yield return null;
        }
    }
}
