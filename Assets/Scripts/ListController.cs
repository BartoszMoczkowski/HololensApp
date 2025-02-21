using MixedReality.Toolkit.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Microsoft.MixedReality.GraphicsTools.ClippingPrimitive;
public class ListController : MonoBehaviour

{
    public ContentManager contentManager;
    public VideoManager videoManager;
    public GameObject listOption;
    public GameObject list;
    public ToggleCollection toggleCollection;

    public ModeController modeController;

    public GameObject recipieList;
    public GameObject startButton;

    public GameObject displayRoot;

    public int recipieIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GenerateList(List<string> recipe_names)
    {
        if (toggleCollection.Toggles.Count != 0)
        {
            foreach (var toggle in toggleCollection.Toggles)
            {
                Destroy(toggle.gameObject);
            }
            toggleCollection.Toggles.Clear();
        }

        for (int i = 0; i < recipe_names.Count; i++)
        {
            string recipe = recipe_names[i];
            GameObject option = Instantiate(listOption);
            option.transform.SetParent(list.transform, false);
            option.transform.localPosition -= new Vector3 (0, 0f+ (0.032f * i), 0);
            option.name = recipe;
            PressableButton button = option.GetComponent<PressableButton>();
            TextMeshPro textMeshPro = button.GetComponentInChildren<TextMeshPro>();
            if (textMeshPro != null) 
            {
                textMeshPro.text = recipe_names[i];
            }
            //button.OnClicked.AddListener(() => setRecipie(i));
            Debug.Log(i);
            button.OnClicked.AddListener(() => setRecipieIndex(i));
            toggleCollection.Toggles.Add(button);
            toggleCollection.enabled = true;

        }



    }

    public void setRecipieIndex(int index)
    {
        recipieIndex = index;
    }
    public void setRecipie()
    {


        contentManager.SelectRecipe(toggleCollection.CurrentIndex);

        //toggleCollection.CurrentIndex = recipieIndex;

    }
}
