using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System;
public class ImageManager : MonoBehaviour
{
    //this really is not a great solution
    //but Im behind the schedule so :shrug:
    public List<Image> images;
    public List<GameObject> imageCanvases;
    public List<UnityWebRequest> requests = new List<UnityWebRequest>( new UnityWebRequest[3]);
    public List<bool> enableList;
    public VerticalLayoutGroup verticalLayoutGroup;
    private int dumbshit_counter = 0;
    // Start is called before the first frame update
    private void Start()
    {
    }
    void Update()
    {

        if (dumbshit_counter >= 20)
        {
            //this is really dumb but I do not have the time to figure out how to deal with 
            //unities awful UI components
            verticalLayoutGroup.childScaleWidth = !verticalLayoutGroup.childScaleWidth;
            dumbshit_counter = 0;
        }
        else
        {
            dumbshit_counter++;
        }

        for (int i = 0; i < images.Count; i++)
        {
            if (!enableList[i] )
            {
                if (imageCanvases[i].activeSelf)
                {
                    imageCanvases[i].SetActive(false);
                }
                continue;
            }

            if (!imageCanvases[i].activeSelf)
            {
                imageCanvases[i].SetActive(true);
            }
            continue;
            if (images[i].sprite == null && requests[i] != null)
            {
                if (requests[i].result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(requests[i].error);
                }
                else
                {
                    //move image loading out of courutine and into a normal function
                    Texture loadedTexture = ((DownloadHandlerTexture)requests[i].downloadHandler).texture;
                    Texture2D convertedTexture = (Texture2D)loadedTexture;

                    Sprite imageSprite = Sprite.Create(convertedTexture, new Rect(0, 0, convertedTexture.width, convertedTexture.height), Vector2.one * 0.5f);

                    // Assign the sprite to the UI Image component
                    images[i].sprite = imageSprite;
                }
            }

        }
    }

    public IEnumerator SendImageRequest(int index, string path)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
        requests[index] = www;
        yield return requests[index].SendWebRequest();
    }

    private int CompareImagePaths(string path1, string path2)
    {
        Regex number_finder = new Regex(@".*-(\d+).jpeg");
        var match1 = number_finder.Match(path1);
        var match2 = number_finder.Match(path2);
        if (match1.Success && match2.Success)
        {
            int index1 = int.Parse(match1.Groups[1].Value);
            int index2 = int.Parse(match2.Groups[1].Value);
            
            
            
            return index1.CompareTo(index2);
        }
        return 0;




    }

    public async void LoadImage(string[] paths)
    {

        Regex number_finder = new Regex(@".*-(\d+).jpeg");
        Debug.Log(paths.Length);
        List<string> image_paths = new List<string>(paths);
        image_paths.Sort((x,y) => CompareImagePaths(x,y));
        for (int i = 0; i < images.Count; i++)
        {
            
            if (i >= paths.Length) {

                enableList[i] = false;
                continue;
            }
            Debug.Log(image_paths[i]);
            images[i].sprite = await GetRemoteTexture(image_paths[i]);
            //StartCoroutine(SendImageRequest(i, paths[i]));
            enableList[i] = true;



        }



        
    }
    private Texture2D LoadTexture(string path)
    {
        // Load the image data from the specified path
        byte[] imageData = System.IO.File.ReadAllBytes(path);

        // Create a new Texture2D and load the image data
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);

        return texture;
    }

    public static async Task<Sprite> GetRemoteTexture(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            // begin request:
            var asyncOp = www.SendWebRequest();

            // await until it's done: 
            while (asyncOp.isDone == false)
                await Task.Delay(1000 / 30);//30 hertz

            // read results:
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            // if( www.result!=UnityWebRequest.Result.Success )// for Unity >= 2020.1
            {
                Debug.Log(www.error);

                return null;
            }
            else
            {
                // return valid results:
                Texture2D texture =  DownloadHandlerTexture.GetContent(www);
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
        }
    }
}



// Update is called once per frame


