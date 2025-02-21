using MixedReality.Toolkit.UX;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils.Datums;
using UnityEngine;

public class ModeController : MonoBehaviour
{

    public GameObject Video;
    public GameObject Image;
    public GameObject Text;

    public GameObject List;

    public GameObject PlayButton;
    public GameObject StopButton;
    public GameObject PauseButton;

    public ToggleCollection ModeList;
    public int mode_global = 0;
    // Start is called before the first frame update
    void Start()
    {
        setMode(mode_global);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    ///  
    /// </summary>
    /// <param name="mode">
    /// mode: 0-Video, 1-Image, 2-Text 
    /// </param>
    public void setMode(int mode)
    {
        mode_global = mode;

        if (mode < 0 && mode > 3) {
               return;
        }
        switch (mode)
        {
            case 0:
                Video.SetActive(true);
                Image.SetActive(false);
                Text.SetActive(false);

                PlayButton.SetActive(false);
                StopButton.SetActive(true);
                PauseButton.SetActive(true);
                break;
            case 1:    
                Video.SetActive(false);
                Image.SetActive(true);
                Text.SetActive(false);
 
                PauseButton.SetActive(false);
                PlayButton.SetActive(false);
                StopButton.SetActive(false);
                break;
            case 2:
                Video.SetActive(false);
                Image.SetActive(false); 
                Text.SetActive(true);
                
                PauseButton.SetActive(false);
                PlayButton.SetActive(false);
                StopButton.SetActive(false);

                break;
        }

        ModeList.CurrentIndex = mode;
 
    }
}
