using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.VFX;

public class ContentManager : MonoBehaviour
{


    // Start is called before the first frame update
    public VideoManager VideoManager;
    public ImageManager ImageManager;
    public TextManager TextManager;
    public ListController ListController;
    public DebbugWindow DebbugWindow;
    public EyeTracking EyeTracking;
    public GameObject StopButton;
    public string url = "192.168.3.160:8000";
    List<Step> steps = new List<Step>();
    List<Recipie> recipes = new List<Recipie>();
    int recipe_counter = 0;

    void Start()
    {


        StartCoroutine(RetrieveData());
  

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateData()
    {
        StartCoroutine(RetrieveData());
    }
    public IEnumerator RetrieveData()
    {
        string address = string.Concat("http://", url, "/getdata");
        Debug.Log(address); 
        UnityWebRequest www = UnityWebRequest.Get(address);
        yield return www.SendWebRequest();
        recipes = new List<Recipie>();
        if (www.result != UnityWebRequest.Result.Success)
        {

            DebbugWindow.textMeshPro.text = www.error;
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text

            string text = www.downloadHandler.text;
            Debug.Log(text);
            Response response = JsonConvert.DeserializeObject<Response>(text);
            Debug.Log(response.recipie_names);
            int name_counter = 0;
            List<string> recipe_names = new List<string>();
            foreach (string recipie_name in response.recipies.Keys)
            {
                Recipie recipie = new Recipie();

                recipie.name = response.recipie_names[name_counter++];

                recipe_names.Add(recipie.name); 
                recipie.steps = new List<Step>();

                int step_number_counter = 0;
                foreach (Dictionary<string, string[]> step_dict in response.recipies[recipie_name])
                {
                    Step step = new Step();

                    step.step_number = step_number_counter++;

                    step.text_paths = step_dict["text"];
                    step.img_paths = step_dict["image"];
                    step.video_paths = step_dict["video"];


                    recipie.steps.Add(step);
                }
                recipie.number_of_steps = recipie.steps.Count();
                recipes.Add(recipie);
            }
            Debug.Log("name length");
            Debug.Log(recipe_names.Count);
            Debug.Log(recipes.Count);

            ListController.GenerateList(recipe_names);

        }




    }

    public void SelectRecipe(int index)
    {
        recipe_counter = index;
        Debug.Log("Select Recipie");
        Debug.Log(index);
        Debug.Log("Length");
        Debug.Log(recipes[recipe_counter].number_of_steps);



        List<string> video_paths = new List<string>();


        foreach (Step step in recipes[recipe_counter].steps)
        {

            string address_part = string.Concat("http://", url, "/");

            string video_path = step.video_paths[0].Replace("./", address_part);
            video_paths.Add(video_path);
        }

        VideoManager.PreLoadData(video_paths);


        recipes[recipe_counter].GetStep().LoadStep(this, VideoManager, TextManager, ImageManager);
    }
    public int CurrentRecipieStepCount()
    {
        return recipes[recipe_counter].number_of_steps;
    }

    public string CurrentRecipieName()
    {
        return recipes[recipe_counter].name;
    }
    public void NextStep()
    {
        if (recipes[recipe_counter].step_counter >= recipes[recipe_counter].number_of_steps-1)
        {
            //EyeTracking.StopTimer();
            StopButton.SetActive(true);
        }else
        {
            recipes[recipe_counter].NextStep();
            recipes[recipe_counter].GetStep().LoadStep(this,   VideoManager, TextManager, ImageManager);
            EyeTracking.AddStepSwitchForwardEvent(recipes[recipe_counter].step_counter);

        }


    }
    public void PreviousStep()
    {
    
        if  (recipes[recipe_counter].step_counter <= 0)
        {
            return;
        }       
        //add a bound
        recipes[recipe_counter].PreviousStep();
        recipes[recipe_counter].GetStep().LoadStep(this, VideoManager, TextManager, ImageManager);
        EyeTracking.AddStepSwitchBackwardEvent(recipes[recipe_counter].step_counter);

    }

    public void ResetSteps()
    {
        recipes[recipe_counter].step_counter = 0;
        VideoManager.SwitchVideos(0);
        recipes[recipe_counter].GetStep().LoadStep(this,VideoManager, TextManager, ImageManager);   
    }


}
public class Step
{

    public int step_number;
    public int substep_number = 0 ;

    public string [] text_paths;
    public string [] img_paths;
    public string [] video_paths;


    public void LoadStep(ContentManager root,VideoManager videoManager, TextManager textManager, ImageManager imageManager)
    {
        string address = string.Concat("http://", root.url,"/");
        //root.StartCoroutine(videoManager.LoadVideo(video_paths[0].Replace("./", address)));


        switch (videoManager.ModeController.mode_global)
        {
            case 0:
                videoManager.SwitchVideos(step_number);
                break;
            case 1:

                var addres_paths = img_paths.Select(x => x.Replace("./", address)).ToArray();
                imageManager.LoadImage(addres_paths);
                break; 
            case 2:
                root.StartCoroutine(textManager.LoadText(text_paths[0].Replace("./", address)));
                break;
            default:
                break;
        }

    }


}
public class Recipie
{

    public string recipe_path;
    public string name;
    public List<Step> steps;
    public int step_counter = 0 ;
    public int number_of_steps = 0;
    

    public Step GetStep() { return this.steps[this.step_counter]; }

    public void NextStep() {


        this.step_counter = (this.step_counter+1)%this.number_of_steps;
    }

    public void PreviousStep()
    {
        this.step_counter = (this.step_counter - 1) % this.number_of_steps;
    }

    public void ReadSteps()
    {
        string[] steps_paths = System.IO.Directory.GetDirectories(this.recipe_path);
        List<Step> steps = new List<Step>();

        foreach (string step_path in steps_paths)
        {
            int step_number;
            Debug.Log(step_path);

            foreach (string part in step_path.Split('_'))
            {
                Debug.Log(part);

            }
            string[] parts = step_path.Split("_");
            if (!Int32.TryParse(parts[parts.Count()-1], out step_number))
            {
                Debug.Log("Failed to read step number from directory name");
                continue;
            }

            Step step = new Step();
            step.step_number = step_number;
            step.text_paths = System.IO.Directory.GetFiles(step_path, "*.txt");
            step.img_paths = System.IO.Directory.GetFiles(step_path, "*.jpeg");
            step.video_paths = System.IO.Directory.GetFiles(step_path, "*.mp4");



            steps.Add(step);

        }

        steps.Sort((q, v) => q.step_number.CompareTo(v.step_number));

        this.steps = steps;
        this.number_of_steps = steps.Count;
        this.step_counter = 0;
    }
}

public class Response
{
    public List<string> recipie_names;
    public Dictionary<string, List<Dictionary<string, string[] >>> recipies;
}