﻿using System;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum SignalType { LegPain, HandPain, ScaryObjectSight, FamiliarObjectSight, HotObject, ColdObject, SweetTaste, SourTaste, SpokenTo, Falling, Running, HotBody, ColdBody, HighBloodPressure, LowBloodPressure, LowWater, HighWater, BlockedBreathing }

public class SignalMovement : MonoBehaviour
{
    public enum SignalClass { Sensory, Motor }

    public SignalController signalController;

    private Text titleText;
    private Image backImage;
    public GameObject infoButton;
    public SignalClass SigClass { get; set; }
    public SignalType SigType { get; set; }
    public GameObject Target { get; set; }
    public GameObject Origin { get; set; }
    /// <summary>
    /// The info displayed for this signal, to help the player select the right part
    /// </summary>
    public string Info { get; set; }
    /// <summary>
    /// The amount of health this signal will substract if ignored
    /// </summary>
    public int Importance { get; set; }
    private string myname;
    public GameObject destPart { get; set; }

    public string Name
    {
        get { return myname; }
        set
        {
            //when the name is set, we need to resize the background and reposition the info button
            if (titleText == null)
                titleText = transform.Find("Canvas/Title").GetComponent<Text>();
            if (backImage == null)
                backImage = transform.Find("Canvas/Background").GetComponent<Image>();

            titleText.text = new string(value.Reverse().ToArray());//reverse the name, because it is RTL

            var buttonWidth = ((RectTransform)infoButton.transform).rect.width;
            backImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, titleText.preferredWidth + buttonWidth + 120f); // resize the background. 120f is for the padding
            backImage.transform.Translate(Vector3.Scale(new Vector3(buttonWidth, 0, 0), transform.Find("Canvas").transform.localScale));//reposition the background, to fit the button
            infoButton.transform.localPosition = Vector3.Scale(new Vector3(backImage.rectTransform.rect.xMax, infoButton.transform.localPosition.y, 0), backImage.transform.localScale);//repostion the button
            myname = value;
        }
    }


    public float speed = 1f;

    private int direction;
    private int currentWaypoint = -2;
    /// <summary>
    /// Computes the current waypoint
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCurrentWaypoint()
    {

        if (currentWaypoint < -1 || currentWaypoint > signalController.path.Length)
            throw new ArgumentOutOfRangeException();

        if (currentWaypoint < 0 || currentWaypoint >= signalController.path.Length)//check if we're at an endpoint of our path
        {
            if (destPart == null)//if this is the first time the above condition was met, assign the approppriate selected path (a bit messy, I know)
                destPart = direction == 1 ? signalController.inputManager.selectedBrainPart : signalController.inputManager.selectedBodyPart;
            return destPart.transform.position;
        }

        return signalController.path[currentWaypoint];//if we're not, we're still on the predefined path
    }


    // Use this for initialization
    void Start()
    {
    }

    /// <summary>
    /// Should be called after all properties are set. starts the signal movement
    /// </summary>
    public void StartMove()
    {
        switch (SigClass)
        {
            case SignalClass.Sensory:
                direction = 1;
                currentWaypoint = 0;
                break;
            case SignalClass.Motor:
                direction = -1;
                currentWaypoint = signalController.path.Length - 1;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypoint != -2) //-2 is the "unset" flag
        {
            Vector3 cur = GetCurrentWaypoint();//find the waypoint we should move towards
            this.transform.position = Vector3.MoveTowards(this.transform.position, cur, speed * Time.deltaTime);
            if (transform.position == cur)//if we've reached the current destination waypoint
            {
                currentWaypoint += direction;//advance the waypoint index. direction is 1 or -1
                if (currentWaypoint == -2 || currentWaypoint == signalController.path.Length + 1) //finished path condition
                {
                    currentWaypoint = -2;
                    signalController.SignalReached(this);//notify the signal controller
                    Destroy(this.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// A helper method for assigning signal properties
    /// </summary>
    public void FillSignalInfo(SignalClass clas, string origin, string target, string name, int importance, string info = "אין מידע")
    {
        this.SigClass = clas;
        this.Name = name;
        this.Info = info;
        this.Importance = importance;
        if (clas == SignalClass.Sensory)
        {
            this.Origin = signalController.GetBodyPart(origin);
            this.Target = signalController.GetBrainPart(target);
        }
        else
        {
            this.Origin = signalController.GetBrainPart(origin);
            this.Target = signalController.GetBodyPart(target);
        }
    }
}
