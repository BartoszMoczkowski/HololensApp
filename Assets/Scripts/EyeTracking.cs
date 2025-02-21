using MixedReality.Toolkit.Input;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class EyeTracking : MonoBehaviour
{

    public FuzzyGazeInteractor gazeInteractor;

    private DateTime start_time = DateTime.Now;
    private bool timer_started = false;
    private DataContainer data;
    public ContentManager contentManager;
    // Start is called before the first frame update
    void Start()
    {
        data = new DataContainer();
        data.eye_tracking_data = new Dictionary<string, List<EyeTrackingEvent>> { };
        data.step_switches = new List<StepSwitchEvent> { };
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void StartTimer()
    {
        Debug.Log("TimerStarted");
        data.recipie_name = contentManager.CurrentRecipieName();
        start_time = DateTime.Now;
        AddStartEvent();
        timer_started = true;
    }
    public void StopTimer() 
    {

        SendEyetrackingData();
        timer_started = false;
    }

    //TODO : Rename this 
    public void addStartEvent(string object_name)
    {
        if (!timer_started) 
        {
            return;
        }
        EyeTrackingEvent eye_tracking_event = new EyeTrackingEvent();
        eye_tracking_event.event_id = data.AssignEventId();
        eye_tracking_event.miliseconds_since_start = (DateTime.Now - start_time).TotalMilliseconds;
        eye_tracking_event.has_started_looking = true;
        if (!data.eye_tracking_data.ContainsKey(object_name))
        {

            List<EyeTrackingEvent> list = new List<EyeTrackingEvent>();
            list.Add(eye_tracking_event);
            data.eye_tracking_data.Add(object_name, list);

        }
        else
        {
            data.eye_tracking_data[object_name].Add(eye_tracking_event);
        }
    }
    public void addStopEvent(string object_name)
    {
        if (!timer_started)
        {
            return;
        }
        EyeTrackingEvent eye_tracking_event = new EyeTrackingEvent();
        eye_tracking_event.event_id = data.AssignEventId();
        eye_tracking_event.miliseconds_since_start = (DateTime.Now - start_time).TotalMilliseconds;
        eye_tracking_event.has_started_looking = false;
        if (!data.eye_tracking_data.ContainsKey(object_name))
        {

            List<EyeTrackingEvent> list = new List<EyeTrackingEvent>();
            list.Add(eye_tracking_event);
            data.eye_tracking_data.Add(object_name, list);

        }
        else
        {
            data.eye_tracking_data[object_name].Add(eye_tracking_event);
        }
    }


    public void AddStepSwitchForwardEvent(int step_index)
    {
        if (!timer_started)
        {
            return;
        }
        StepSwitchEvent stepSwitchEvent = new StepSwitchEvent();
        stepSwitchEvent.event_id = data.AssignEventId();
        stepSwitchEvent.step_number = step_index;
        stepSwitchEvent.miliseconds_since_start = (DateTime.Now - start_time).TotalMilliseconds;
        stepSwitchEvent.switched_forward = true;

        data.step_switches.Add(stepSwitchEvent);
    }
    public void AddStepSwitchBackwardEvent(int step_index)
    {
        if (!timer_started)
        {
            return;
        }
        StepSwitchEvent stepSwitchEvent = new StepSwitchEvent();
        stepSwitchEvent.event_id = data.AssignEventId();
        stepSwitchEvent.step_number = step_index;
        stepSwitchEvent.miliseconds_since_start = (DateTime.Now - start_time).TotalMilliseconds;
        stepSwitchEvent.switched_forward = false;

        data.step_switches.Add(stepSwitchEvent);
    }
    public void AddStopEvent()
    {
        //Special case of the step switch event 
        StepSwitchEvent stepSwitchEvent = new StepSwitchEvent();
        stepSwitchEvent.event_id = data.AssignEventId();
        stepSwitchEvent.step_number = contentManager.CurrentRecipieStepCount();
        stepSwitchEvent.miliseconds_since_start = (DateTime.Now - start_time).TotalMilliseconds;
        stepSwitchEvent.switched_forward = true;

        data.step_switches.Add(stepSwitchEvent);

    }
    public void AddStartEvent()
    {
        //Special case of the step switch event 
        StepSwitchEvent stepSwitchEvent = new StepSwitchEvent();
        stepSwitchEvent.event_id = data.AssignEventId();
        stepSwitchEvent.step_number = 0;
        stepSwitchEvent.miliseconds_since_start = (DateTime.Now - start_time).TotalMilliseconds;
        stepSwitchEvent.switched_forward = true;

        data.step_switches.Add(stepSwitchEvent);

    }
    public void SendEyetrackingData()
    {
        AddStopEvent();
        StartCoroutine(SendEyetrackingDataCoroutine());
    }

    public IEnumerator SendEyetrackingDataCoroutine()
    {

        string post_data = JsonConvert.SerializeObject(data);


        string address = string.Concat("http://", contentManager.url, "/eyetracking");

        using (UnityWebRequest www = UnityWebRequest.Post(address, post_data, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }


        data = new DataContainer();
        data.eye_tracking_data = new Dictionary<string, List<EyeTrackingEvent>> { };
        data.step_switches = new List<StepSwitchEvent> { };
    }

    //maybe add an event id to resolve conflicts when to event fire on the same frame/timestamp
    class DataContainer
    {
        int current_event_id = 0;
        public string recipie_name = "";
        public Dictionary<string, List<EyeTrackingEvent>> eye_tracking_data;
        public List<StepSwitchEvent> step_switches;

        public int AssignEventId()
        {
            return current_event_id++;
        }
    }
    class EyeTrackingEvent
    {
        public int event_id;
        public double miliseconds_since_start;
        public bool has_started_looking; // true if user started looking at the object, false otherwise

    }

    class StepSwitchEvent
    {
        public int event_id;
        public double miliseconds_since_start;
        public bool switched_forward;
        public int step_number;

    }
}
