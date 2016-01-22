using UnityEngine;
using System.Collections;
using System;
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

    private void InitSignalFlow()
    {

    }

    // Use this for initialization
    void Start()
    {

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
        return bodyPartsCont.transform.GetChild(UnityEngine.Random.Range(0, bodyPartsCont.transform.childCount - 1)).gameObject;
    }

    private GameObject GetRandomBrainPart()
    {
        return brainPartsCont.transform.GetChild(UnityEngine.Random.Range(0, brainPartsCont.transform.childCount - 1)).gameObject;
    }

    public virtual GameObject InstantiateSignal(SignalType type)
    {
        GameObject signal = Instantiate(signalPrefab);
        SignalMovement sigObj = signal.GetComponent<SignalMovement>();
        signal.transform.SetParent(signalsCont.transform);
        sigObj.signalController = this;

        switch (type)
        {
            case SignalType.LegPain:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Legs", "Parietal", "כאב ברגל");
                break;
            case SignalType.HandPain:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Arms", "Parietal", "כאב ביד");
                break;
            case SignalType.ScaryObjectSight:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Eyes", "Occipital", "עצם מפחיד");
                break;
            case SignalType.FamiliarObjectSight:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Eyes", "Occipital", "עצם מוכר");
                break;
            case SignalType.HotObject:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Arms", "Parietal", "מגע חם");
                break;
            case SignalType.ColdObject:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Arms", "Parietal", "מגע קר");
                break;
            case SignalType.SweetTaste:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Tounge", "Parietal", "טעם מתוק");
                break;
            case SignalType.SourTaste:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Tounge", "Parietal", "טעם חמוץ");
                break;
            case SignalType.SpokenTo:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Ears", "Frontal", "שיחה");
                break;
            case SignalType.Falling:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Ears", "Cerebellum", "נפילה");
                break;
            case SignalType.Running:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Legs", "Cerebellum", "ריצה");
                break;
            case SignalType.HotBody:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus", "חום גוף גבוה");
                break;
            case SignalType.ColdBody:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus", "חום גוף נמוך");
                break;
            case SignalType.HighBloodPressure:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Stem", "לחץ דם גבוה");
                break;
            case SignalType.LowBloodPressure:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Stem", "לחץ דם נמוך");
                break;
            case SignalType.LowWater:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus", "ריכוז מים נמוך");
                break;
            case SignalType.HighWater:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus", "ריכוז מים גבוה");
                break;
            case SignalType.BlockedBreathing:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Trachea", "Stem", "נשימה חסומה");
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
