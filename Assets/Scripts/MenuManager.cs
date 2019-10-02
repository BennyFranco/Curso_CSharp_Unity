using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Canvas menuCanvas;

    public static MenuManager sharedInstance;
    // Start is called before the first frame update
    void Start()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    public void ShowMainMenu()
    {
        menuCanvas.enabled = true;
    }

    public void HideMainMenu()
    {
        menuCanvas.enabled = false;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
