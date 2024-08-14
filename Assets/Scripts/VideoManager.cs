using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{


    public VideoPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadVideo(string path)
    {
        player.url = path;
        player.Prepare();
        player.Play();

    }

    public void PlayVideo()
    {
        player.Play();
    }

    public void StopVideo()
    {
        player.Pause();
    }

    public void ReplayVideo()
    {
        player.Stop();
        player.Play();
    }
}
