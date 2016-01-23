using UnityEngine;
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

    #endregion

    const float SIGNAL_INTERVAL = 5f;

    // Use this for initialization
    void Start()
    {
        path = new Vector3[transform.childCount];
        
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = transform.GetChild(i).transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup % SIGNAL_INTERVAL < Time.deltaTime)
        {
            Array arr = Enum.GetValues(typeof(SignalType));
            SignalType type = (SignalType)arr.GetValue(URandom.Range(0, arr.Length));
            GameObject sig = InstantiateSignal(type);
            SignalMovement sigScript = sig.GetComponent<SignalMovement>();
            sigScript.StartMove();
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

    public virtual GameObject InstantiateSignal(SignalType type)
    {
        GameObject signal = Instantiate(signalPrefab);
        SignalMovement sigObj = signal.GetComponent<SignalMovement>();
        signal.transform.SetParent(signalsCont.transform);
        sigObj.signalController = this;
        sigObj.SigType = type;
        Button infoButton = signal.transform.Find("Canvas/InfoButton").GetComponent<Button>();
        infoButton.onClick.AddListener(() => inputManager.SignalClick(signal));

        switch (type)
        {
            case SignalType.LegPain:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Legs", "Parietal", "כאב ברגל",
                    @"תחושה וכאב מעובדים באונה הקודקודית");
                break;
            case SignalType.HandPain:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Arms", "Parietal", "כאב ביד",
                    @"תחושה וכאב מעובדים באונה הקודקודית");
                break;
            case SignalType.ScaryObjectSight:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Eyes", "Occipital", "עצם מפחיד",
                    @"נתונים חזותיים מהעיניים מעובדים באונה העורפית");
                break;
            case SignalType.FamiliarObjectSight:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Eyes", "Occipital", "עצם מוכר",
                    @"נתונים חזותיים מהעיניים מעובדים באונה העורפית");
                break;
            case SignalType.HotObject:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Arms", "Parietal", "מגע חם מאוד",
                    @"על תגובה לטמפרטורות קיצוניות אחראית האונה הקודקודית");
                break;
            case SignalType.ColdObject:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Arms", "Parietal", "מגע קר מאוד",
                    @"על תגובה לטמפרטורות קיצוניות אחראית האונה הקודקודית");
                break;
            case SignalType.SweetTaste:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Tounge", "Parietal", "טעם מתוק",
                    @"טעם מעובד באונה הקודקודית");
                break;
            case SignalType.SourTaste:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Tounge", "Parietal", "טעם חמוץ",
                    @"טעם מעובד באונה הקודקודית");
                break;
            case SignalType.SpokenTo:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Ears", "Frontal", "שיחה",
                    @"עיבוד ותגובה לדיבור מעובדים באונה המצחית");
                break;
            case SignalType.Falling:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Ears", "Cerebellum", "נפילה",
                    @"שיווי משקל ושליטה מוטורית על השרירים הם תפקידיו של המוח הקטן");
                break;
            case SignalType.Running:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Legs", "Cerebellum", "ריצה",
                    @"שיווי משקל ושליטה מוטורית על השרירים הם תפקידיו של המוח הקטן");
                break;
            case SignalType.HotBody:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus", "חום גוף גבוה",
                    @"בקרה על טמפרטורת הגוף היא אחריותו של ההיפותלמוס");
                break;
            case SignalType.ColdBody:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus", "חום גוף נמוך",
                    @"בקרה על טמפרטורת הגוף היא אחריותו של ההיפותלמוס");
                break;
            case SignalType.HighBloodPressure:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Stem", "לחץ דם גבוה",
                    @"שמירה על לחץ דם יציב נעשית ע""י גזע המוח");
                break;
            case SignalType.LowBloodPressure:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Stem", "לחץ דם נמוך",
                    @"שמירה על לחץ דם יציב נעשית ע""י גזע המוח");
                break;
            case SignalType.LowWater:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus", "ריכוז מים נמוך",
                    @"שמירה על ריכוז נוזלים תקין בדם נעשית ע""י ההיפותלמוס");
                break;
            case SignalType.HighWater:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus", "ריכוז מים גבוה",
                    @"שמירה על ריכוז נוזלים תקין בדם נעשית ע""י ההיפותלמוס");
                break;
            case SignalType.BlockedBreathing:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Trachea", "Stem", "חנק",
                    @"אינסטינקטים כגון השתעלות, שנועדה בין השאר להוציא עצמים זרים מקנה הנשימה, נשלטים ע""י גזע המוח");
                break;
            default:
                throw new ArgumentException("Unknown SignalType: " + type.ToString(), "type");
        }

        signal.name = type.ToString() + " (" + sigObj.Origin.name + " -> " + sigObj.Target.name + ")";
        signal.transform.position = sigObj.Origin.transform.position;
        return signal;
    }


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
