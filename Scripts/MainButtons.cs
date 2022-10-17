using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtons : MonoBehaviour
{
    /*public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }*/
    public void GoBack()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    /*public void Quit()
    {
        //DialougeManager.Instance.startDialouge();
        string[] lines = { "Hello this is cmd", "Yeah its me"};
        DialougeManager.Instance.SetDialog(lines);
    }*/

}
