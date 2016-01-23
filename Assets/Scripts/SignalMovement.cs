using System;
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
    private bool lastWaypoint = false;
    public GameObject infoButton;
    public SignalClass SigClass { get; set; }
    public SignalType SigType { get; set; }
    public GameObject Target { get; set; }
    public GameObject Origin { get; set; }
    public string Info { get; set; }
    /// <summary>
    /// The amount of health this signal will substract if ignored
    /// </summary>
    public int Importance { get; set; }
    private string myname;
    private Vector3 brainPart;

    public string Name
    {
        get { return myname; }
        set
        {
            if (titleText == null)
                titleText = transform.Find("Canvas/Title").GetComponent<Text>();
            if (backImage == null)
                backImage = transform.Find("Canvas/Background").GetComponent<Image>();
            titleText.text = new string(value.Reverse().ToArray());
            var buttonWidth = ((RectTransform)infoButton.transform).rect.width;
            backImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, titleText.preferredWidth + buttonWidth + 120f); // 120f is for the padding
            backImage.transform.Translate(Vector3.Scale(new Vector3(buttonWidth, 0, 0), transform.Find("Canvas").transform.localScale));
            infoButton.transform.localPosition = Vector3.Scale(new Vector3(backImage.rectTransform.rect.xMax, infoButton.transform.localPosition.y, 0), backImage.transform.localScale);
            myname = value;
        }
    }


    public float speed = 1f;

    private int direction;
    private int currentWaypoint = -2;
    private Vector3 GetCurrentWaypoint()
    {

        if (currentWaypoint < -1 || currentWaypoint > signalController.path.Length)
            throw new ArgumentOutOfRangeException();

        if (currentWaypoint < 0)
            return signalController.inputManager.selectedBodyPart.transform.position;

        if (currentWaypoint >= signalController.path.Length)
            return signalController.inputManager.selectedBrainPart.transform.position;
        return signalController.path[currentWaypoint];
        
    }


    // Use this for initialization
    void Start()
    {
    }

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
        if (currentWaypoint != -2)
        {
            if (currentWaypoint <= 2)
            {
            Vector3 cur = GetCurrentWaypoint();
            this.transform.position = Vector3.MoveTowards(this.transform.position, cur, speed * Time.deltaTime);
            if (transform.position == cur)
            {
                    if (currentWaypoint == 2)
                    {
                        brainPart = signalController.inputManager.selectedBrainPart.transform.position;
                    }
                currentWaypoint += direction;
                if (currentWaypoint == -2 || currentWaypoint == signalController.path.Length + 1)
                {
                    currentWaypoint = -2;
                    signalController.SignalReached(this);
                    Destroy(this.gameObject);
                }
                }
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position,brainPart, speed * Time.deltaTime);
            }
           
            
               

        
        }
    }

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
