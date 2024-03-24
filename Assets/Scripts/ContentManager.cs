using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContentManager : MonoBehaviour
{
    // Start is called before the first frame update

    public VideoManager VideoManager;
    public ImageManager ImageManager;
    public TextManager TextManager;

    List<Step> steps = new List<Step>();
    int step_counter = 0;

    void Start()
    {
        string recipe_folder_path = "Assets/Resources";

        string[] recipies = System.IO.Directory.GetDirectories(recipe_folder_path);

        string recipe_path = recipies[0];

        string[] steps_paths = System.IO.Directory.GetDirectories(recipe_path);

        
        foreach (string step_path in steps_paths)
        {

            Step step = new Step();
            step.text_paths = System.IO.Directory.GetFiles(step_path, "*.txt");
            step.img_paths = System.IO.Directory.GetFiles(step_path, "*.jpg");
            step.video_paths = System.IO.Directory.GetFiles(step_path, "*.mp4");



            steps.Add(step);

        }


        steps[step_counter].LoadStep(VideoManager, TextManager, ImageManager);

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NextStep()
    {
        step_counter = (step_counter + 1) % (steps.Count);
        steps[step_counter].LoadStep(VideoManager, TextManager, ImageManager);

    }
    public void PreviousStep()
    {
        step_counter = (step_counter - 1) % (steps.Count);
        steps[step_counter].LoadStep(VideoManager, TextManager, ImageManager);

    }


}
public class Step
{
    public string [] text_paths;
    public string [] img_paths;
    public string [] video_paths;


    public void LoadStep(VideoManager videoManager, TextManager textManager, ImageManager imageManager)
    {
        videoManager.LoadVideo(video_paths[0]);
        imageManager.LoadImage(img_paths[0]);
        textManager.LoadText(text_paths[0]);
    }
}
