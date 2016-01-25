﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using URandom = UnityEngine.Random;

public class SignalController : MonoBehaviour
{
    #region editorReferences

    public InputManager inputManager;
    public GameObject signalsCont;

    public GameObject bodyPartsCont;
    public GameObject brainPartsCont;

    public GameObject bodyMarkGO;
    public GameObject brainMarkGO;

    public Vector3[] path;

    public GameObject signalPrefab;

    public Text healthText;
    public Canvas lostCanvas;

    #endregion

    private int health = 100;

    public float signalInterval = 5f;
    //the time left for the next signal
    private float intervalTime;
    public float signalSpeed = 1f;

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
            healthText.text = health.ToString();
            if (health <= 0)
            {
                ShowLostPopup();
            }
        }
    }

    private void ShowLostPopup()
    {
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Menu"); //show the "Menu" layer. the unusual symbols are bitwise operators, because cullingMask is a flags field.
        lostCanvas.GetComponent<Image>().raycastTarget = false; //supposed to block clicks behind the menu, but not realy working right now. also not realy important...
    }

    // Use this for initialization
    void Start()
    {
        path = new Vector3[transform.childCount];

        for (int i = 0; i < path.Length; i++)//put the positions of the "waypoint" GameObjects into an array
        {
            path[i] = transform.GetChild(i).transform.position;
        }

        intervalTime = signalInterval;
    }

    // Update is called once per frame
    void Update()
    {
        intervalTime -= Time.deltaTime;//see comment above intervalTime
        if (intervalTime <= 0)
        {
            Array arr = Enum.GetValues(typeof(SignalType));
            SignalType type = (SignalType)arr.GetValue(URandom.Range(0, arr.Length));//select a random SignalType
            GameObject sig = InstantiateSignal(type);//see more info in method
            SignalMovement sigScript = sig.GetComponent<SignalMovement>();
            sigScript.StartMove();
            intervalTime = signalInterval;
        }
    }

    private GameObject GetRandomBodyPart()
    {
        return bodyPartsCont.transform.GetChild(URandom.Range(0, bodyPartsCont.transform.childCount - 1)).gameObject;
    }

    private GameObject GetRandomBrainPart()
    {
        return brainPartsCont.transform.GetChild(URandom.Range(0, brainPartsCont.transform.childCount - 1)).gameObject;
    }

    /// <summary>
    /// Creates a full signal GameObject for a given SignalType
    /// </summary>
    /// <param name="type">The type of the signal</param>
    public virtual GameObject InstantiateSignal(SignalType type)
    {
        GameObject signal = Instantiate(signalPrefab);
        SignalMovement sigObj = signal.GetComponent<SignalMovement>();
        signal.transform.SetParent(signalsCont.transform);//put the signals in a container, for a cleaner hierarchy
        sigObj.signalController = this;
        sigObj.SigType = type;
        sigObj.speed = this.signalSpeed;
        Button infoButton = signal.transform.Find("Canvas/InfoButton").GetComponent<Button>();
        infoButton.onClick.AddListener(() => inputManager.SignalClick(signal));//set the click event

        switch (type) //fill the appropppriate values for each SignalType
        {
            case SignalType.LegPain:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Legs", "Parietal", "כאב ברגל", 5,
                    @"תחושה וכאב מעובדים באונה הקודקודית");
                break;
            case SignalType.HandPain:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Arms", "Parietal", "כאב ביד", 5,
                    @"תחושה וכאב מעובדים באונה הקודקודית");
                break;
            case SignalType.ScaryObjectSight:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Eyes", "Occipital", "עצם מפחיד", 20,
                    @"נתונים חזותיים מהעיניים מעובדים באונה העורפית");
                break;
            case SignalType.FamiliarObjectSight:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Eyes", "Occipital", "עצם מוכר", 20,
                    @"נתונים חזותיים מהעיניים מעובדים באונה העורפית");
                break;
            case SignalType.HotObject:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Arms", "Parietal", "מגע חם מאוד", 30,
                    @"על תגובה לטמפרטורות קיצוניות אחראית האונה הקודקודית");
                break;
            case SignalType.ColdObject:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Arms", "Parietal", "מגע קר מאוד", 25,
                    @"על תגובה לטמפרטורות קיצוניות אחראית האונה הקודקודית");
                break;
            case SignalType.SweetTaste:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Tounge", "Parietal", "טעם מתוק", 5,
                    @"טעם מעובד באונה הקודקודית");
                break;
            case SignalType.SourTaste:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Tounge", "Parietal", "טעם חמוץ", 5,
                    @"טעם מעובד באונה הקודקודית");
                break;
            case SignalType.SpokenTo:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Ears", "Frontal", "שיחה", 10,
                    @"עיבוד ותגובה לדיבור מעובדים באונה המצחית");
                break;
            case SignalType.Falling:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Ears", "Cerebellum", "נפילה", 30,
                    @"שיווי משקל ושליטה מוטורית על השרירים הם תפקידיו של המוח הקטן");
                break;
            case SignalType.Running:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Legs", "Cerebellum", "ריצה", 20,
                    @"שיווי משקל ושליטה מוטורית על השרירים הם תפקידיו של המוח הקטן");
                break;
            case SignalType.HotBody:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus", "חום גוף גבוה", 20,
                    @"בקרה על טמפרטורת הגוף היא אחריותו של ההיפותלמוס");
                break;
            case SignalType.ColdBody:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus", "חום גוף נמוך", 20,
                    @"בקרה על טמפרטורת הגוף היא אחריותו של ההיפותלמוס");
                break;
            case SignalType.HighBloodPressure:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Stem", "לחץ דם גבוה", 20,
                    @"שמירה על לחץ דם יציב נעשית ע""י גזע המוח");
                break;
            case SignalType.LowBloodPressure:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Stem", "לחץ דם נמוך", 20,
                    @"שמירה על לחץ דם יציב נעשית ע""י גזע המוח");
                break;
            case SignalType.LowWater:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus", "ריכוז מים נמוך", 25,
                    @"שמירה על ריכוז נוזלים תקין בדם נעשית ע""י ההיפותלמוס");
                break;
            case SignalType.HighWater:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus", "ריכוז מים גבוה", 25,
                    @"שמירה על ריכוז נוזלים תקין בדם נעשית ע""י ההיפותלמוס");
                break;
            case SignalType.BlockedBreathing:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Trachea", "Stem", "חנק", 40,
                    @"אינסטינקטים כגון השתעלות, שנועדה בין השאר להוציא עצמים זרים מקנה הנשימה, נשלטים ע""י גזע המוח");
                break;
            default:
                throw new ArgumentException("Unknown SignalType: " + type.ToString(), "type");
        }

        signal.name = type.ToString() + " (" + sigObj.Origin.name + " -> " + sigObj.Target.name + ")"; //set a name, for debugging
        signal.transform.position = sigObj.Origin.transform.position;
        return signal;
    }

    /// <summary>
    /// Called when a signal reached a body/brain part
    /// </summary>
    internal void SignalReached(SignalMovement signal)
    {
        if (signal.destPart != signal.Target)
        {
            Health -= signal.Importance;
            StartCoroutine(BlinkHealth());
        }
    }

    /// <summary>
    /// Blinks the health text for a few seconds. Should be used with StartCoroutine
    /// </summary>
    public IEnumerator BlinkHealth()
    {
        for (int blinks = 5; blinks > 0; blinks--)
        {
            healthText.enabled = !healthText.enabled;
            yield return new WaitForSeconds(0.5f);
        }
        healthText.enabled = true;
    }

    /// <summary>
    /// Gets a body part from its name
    /// </summary>
    public GameObject GetBodyPart(string name)
    {
        try
        {
            return bodyPartsCont.transform.FindChild(name).gameObject;
        }
        catch (NullReferenceException)
        {
            Debug.LogError("NullReferenceException: name is " + name);
            return null;
        }
    }

    /// <summary>
    /// Gets a brain part from its name
    /// </summary>
    public GameObject GetBrainPart(string name)
    {
        try
        {
            return brainPartsCont.transform.FindChild(name).gameObject;
        }
        catch (NullReferenceException)
        {
            Debug.LogError("NullReferenceException: name is " + name);
            return null;
        }
    }
}
