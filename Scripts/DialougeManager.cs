using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialougeManager : MonoBehaviour
{
    public static DialougeManager Instance { get; private set; }

    [SerializeField] GameObject DialougeBox;
    public TextMeshProUGUI ButtonText;
    public TextMeshProUGUI textCom;
    private string[] lines = null;
    private float Speed = 0.01f;
    private int index;

    bool running = false;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (running)
        {
            if (lines.Length - 1 != index)
            {
                ButtonText.text = "Next";
            }
            else
            {
                ButtonText.text = "Ok";
            }
        }
    }

    public void GoToNextOrOK()
    {
        if (running & lines.Length - 1 != index)
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textCom.text = string.Empty;
            running = false;
            DialougeBox.SetActive(false);
        }
    }
    void Start()
    {
       // GameObject[] objs = GameObject.FindGameObjectsWithTag("DialougeManager");
      //  if (DialougeBox != null)
      //  {
          //  if (objs.Length > 1)
         //   {
           //     Destroy(this);
           // }
            DontDestroyOnLoad(DialougeBox);
            textCom.text = string.Empty;
       // }
    }

    public void startDialouge(string[] lines)
    {
        textCom.text = string.Empty;
        running = true;
        this.lines = lines;
        DialougeBox.SetActive(true);
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textCom.text = "";
        foreach(char c in lines[index].ToCharArray())
        {
            textCom.text += c;
            //yield return null;
            yield return new WaitForSeconds(Speed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textCom.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            textCom.text = string.Empty;
        }
    }
}
