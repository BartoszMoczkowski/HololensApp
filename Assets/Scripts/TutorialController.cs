using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TutorialController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<VideoClip> clips;
    public VideoPlayer player;
    public TextMeshProUGUI text;
    
    private int clipCounter = 0;

    private DateTime start_time = DateTime.Now;
    private int timer = 0;
    private bool timerRunning = false;
    void Start()
    {
        
    }

    void Update()
    {
        
        text.text = $"" +
            $"Pressing Start starts the timer [{timer}s]\n" +
            $"Pressing Stops stops it and saves information about the attempt\n" +
            $"\n" +
            $"During the experiment the stop button will appear after reaching the last step" +
            $"press it after you finish the dish";
        if (timerRunning)
        {
            timer = (DateTime.Now- start_time).Seconds;
        };
    }

    public void NextVideo()
    {

        if (clipCounter < 2)
        {
        clipCounter++;

        }
        player.clip = clips[clipCounter];
        player.Play();

    }


    public void StartTimer()
    {
        start_time = DateTime.Now;
        timerRunning = true;
    }
    public void StopTimer() { 
        timerRunning = false; 
    }
    public void PreviousVideo()
    {
        if (clipCounter > 0)
        {
        clipCounter--;

        }
        player.clip = clips[clipCounter];
        player.Play();
    }

    public void PlayVideo()
    {
        player.Play();
    }

    public void PauseVideo()
    {
        player.Pause();
    }

    public void StopVideo()
    {
        player.Stop();
    }


    // Update is called once per frame

}
