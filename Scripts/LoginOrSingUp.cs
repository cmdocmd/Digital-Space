using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopersHub.RealtimeNetworking.Client;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginOrSingUp : MonoBehaviour
{
    public static string user = "";
    public static string pass = "";

    public TMP_InputField username;
    public TMP_InputField password;


    void Start()
    {
        if (user != "" && pass != "")
        {
            username.text = user;
            password.text = pass;
        }
        
    }

    public void Login()
    {
        //  Sending Login Packet to the server...
        Sender.TCP_Send((int)Clients.RequestsID.Login, username.text + "\n" + password.text + "\n" + SystemInfo.deviceUniqueIdentifier);
    }

    public void SignUP()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Update()
    {
        if (Clients.WorldMenu == 1)
        {
            SceneManager.LoadScene(3);
        }
    }

    //[SerializeField] GameObject DialougeBox;

    /* public TMP_InputField username;
     public TMP_InputField password;

     public void Login()
     {
         //Debug.Log("Hehehehehe");
         string[] lines = { "Hello this is cmd", "Yeah its me" };
         DialougeManager.Instance.startDialouge(lines);
     }*/
    //public TMP_InputField email;

    /*void Start()
    {
        RealtimeNetworking.OnConnectingToServerResult += OnConnectingToServerResult;
        //RealtimeNetworking.Connect();
    }

    private void OnConnectingToServerResult(bool successful)
    {
        if (successful)
        {
            //Sender.TCP_Send()
            Debug.Log("Hi....");
        }
        else
        {
            //DialougeManager.Instance.startDialouge();
            //string[] lines = { "Hello this is cmd", "Yeah its me"};
            //DialougeManager.Instance.SetDialog(lines);
            Debug.Log("Sorry something wrong happened....");
        }
    }

    public void Login()
    {
        //Debug.Log("test");
        //RealtimeNetworking.Connect();
    }

    public void Signup()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreateAccount()
    {
        Debug.Log("test1");
        //RealtimeNetworking.Connect();
        /*Packet packet = new Packet();
        packet.Write(username.text);
        packet.Write(password.text);
        packet.Write(email.text);

        Sender.TCP_Send(packet);*/
    //  }

    // Update is called once per frame
    /*void Update()
    {
        //DialougeBox.SetActive(false);
    }*/

}
