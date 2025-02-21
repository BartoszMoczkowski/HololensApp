using MixedReality.Toolkit.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySizeUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform rectTransformVideo;
    public RectTransform rectTransformImage;
    public RectTransform rectTransformText;
    public Slider widthSlider;
    public Slider heightSlider;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateScaleWidth ()
    {
        float scaleWidth = widthSlider.Value;
        Vector3 oldScale = rectTransformVideo.localScale;
        rectTransformVideo.localScale = new Vector3(scaleWidth, oldScale.y,oldScale.z);
        rectTransformImage.localScale = new Vector3(scaleWidth, oldScale.y, oldScale.z);
        rectTransformText.localScale = new Vector3(scaleWidth, oldScale.y, oldScale.z);

    }
    public void updateScaleHeight()
    {
        float scaleHeight = heightSlider.Value;
        Vector3 oldScale = rectTransformVideo.localScale;
        Vector3 oldPos = rectTransformVideo.localPosition;

        rectTransformVideo.localScale = new Vector3(oldScale.x, scaleHeight, oldScale.z);
        rectTransformImage.localScale = new Vector3(oldScale.x, scaleHeight, oldScale.z);
        rectTransformText.localScale = new Vector3(oldScale.x, scaleHeight, oldScale.z);

        //rectTransformVideo.localPosition = new Vector3(oldPos.x, (20f + ((scaleHeight-1)/2f*100f)), oldPos.z);

    }

}
