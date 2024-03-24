using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{

    public Image image;
    // Start is called before the first frame update
    private void Start()
    {
        // Load the image from the specified path

        // Get the UI Image component
        
    }
    void Update()
    {

    }

    public bool LoadImage( string path)
    {
        Texture2D loadedTexture = LoadTexture(path);

        // Create a sprite from the loaded texture
        Sprite imageSprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), Vector2.one * 0.5f);

        // Assign the sprite to the UI Image component
        image.sprite = imageSprite;
        

        return true;
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
}



// Update is called once per frame
