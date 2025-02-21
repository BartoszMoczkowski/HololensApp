using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddressInput : MonoBehaviour
{
    UnityEngine.TouchScreenKeyboard keyboard;
    public ContentManager contentManager;
    public TextMeshProUGUI textMeshPro;
    public  string keyboardText = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (keyboard != null)
        {
        textMeshPro.text = keyboard.text;
        contentManager.url = keyboard.text;

        }

    }

    public void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open(keyboardText);


    }

    
}
