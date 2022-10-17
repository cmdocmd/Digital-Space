using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DevelopersHub.RealtimeNetworking.Client;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldMenuChecker : MonoBehaviour
{
    public static WorldMenuChecker instance;

    [SerializeField] TMP_InputField worldName;
    public TextMeshProUGUI ButtonText;
    public static string wName;
    [SerializeField] GameObject loadingScreen;
    public Slider loadingBar;

    void Awake()
    {
        instance = this;
    }

    public void JoinWorld()
    {
        if (worldName.text.Length >= 1)
        {
            loadingScreen.SetActive(true);
            // Send packet to server with world name
            Sender.TCP_Send((int)Clients.RequestsID.Join, "join\n" + worldName.text);
        }
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
            yield return null;
        }
    }
}
