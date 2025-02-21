using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{

    public ModeController ModeController;
    public VideoPlayer player;
    public List<VideoPlayer> playerList;
    public int activePlayerindex = 0;
    public GameObject plane;
    public GameObject playButton;
    public GameObject pauseButton;

    public GameObject loadingSign;
    public GameObject startButton;
    private bool startedLoading = false;
    private bool stoppedLoading = false;

    private int attempts = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activePlayerindex < playerList.Count)
        {
            if (playerList[activePlayerindex].isPlaying)
            {
                pauseButton.SetActive(true);
                playButton.SetActive(false);
            }
            else
            {
                pauseButton.SetActive(false);
                playButton.SetActive(true);
            }

        }


        if (startedLoading)
        {
            startButton.SetActive(false);
            attempts++;
            if (attempts > 2000)
            {
                stoppedLoading = true;

            }
            if (stoppedLoading)
            {
                Debug.Log("Stoppped loading");
                loadingSign.SetActive(false);
                startButton.SetActive(true);
                startedLoading = false;
                stoppedLoading = false;
                return;
            }
            loadingSign.SetActive(true);

            bool loaded = true;
            foreach (var player in playerList)
            {
                if (!player.isPrepared)
                {
                    //Fuck you unity
                    loaded = false;
                }
            }

            if (loaded)
            {
   
                stoppedLoading = true;
                startButton.SetActive(false );
            }
        }

    }
    public void PreLoadData(List<string> paths)
    {

        if (playerList != null)
        {
            foreach (VideoPlayer player in  playerList)
            {
                if (player != null) {
                    Destroy(player);
                }
            }
        }
        playerList = new List<VideoPlayer>();
        foreach (string path in paths)
        {
            VideoPlayer temp_player = gameObject.AddComponent<VideoPlayer>();
            temp_player.playOnAwake = false;
            //temp_player.waitForFirstFrame = true;
            temp_player.isLooping = true;
            temp_player.renderMode = VideoRenderMode.MaterialOverride;

            temp_player.url = path;
            //temp_player.Prepare();


            playerList.Add(temp_player);



        }



    }

    public void PrepareVideos()
    {

        if (ModeController.mode_global != 0)
        {
            return;
        }
        startedLoading = true;

        Debug.Log("Started Loading");
        startButton.SetActive(false);
        Debug.Log("video Check");
        foreach(VideoPlayer player in playerList)
        {

            player.enabled = true;
            if (!player.isPrepared)
            {
                player.Prepare();

            }

        }
    }

    public void SwitchVideos(int step_number)
    {
        Debug.Log("Switching to video");
        Debug.Log(step_number);
        activePlayerindex = step_number;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (i == step_number)
            {

                playerList[i].targetMaterialRenderer = plane.GetComponent<MeshRenderer>();
                playerList[i].Play();
            }
            else
            {
                playerList[i].targetMaterialRenderer = null;
                if (playerList[i].isPlaying)
                {
                    playerList[i].Stop();
                }
            }
        }
    }
    public IEnumerator LoadVideo(string path)
    {
        player.Stop();
        player.url = path;
        player.Prepare();
        yield return new WaitForSeconds(1);
        while (!player.isPrepared)
        {
            yield return new WaitForSeconds(1);
        }


    }

    public void PlayVideo()
    {
        playerList[activePlayerindex].Play();

        //player.Play();
    }

    public void StopVideo()
    {
        playerList[activePlayerindex].Pause();

        //
        //player.Pause();
    }

    public void ReplayVideo()
    {

        playerList[activePlayerindex].Stop();
        playerList[activePlayerindex].Play();

    }
}
