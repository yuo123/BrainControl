using UnityEngine;
using System.Collections;
using System;

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
            GameObject sig = Instantiate(signalPrefab);
            sig.transform.SetParent(signalsCont.transform);
            GameObject origin = GetRandomBodyPart();
            sig.transform.position = origin.transform.position;
            SignalMovement sigScript = sig.GetComponent<SignalMovement>();
            sigScript.signalController = this;
            sigScript.SigClass = SignalMovement.SignalClass.Sensory;
            sigScript.Origin = origin;
            sigScript.Target = GetRandomBrainPart();
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
        

        switch (type)
        {
            case SignalType.LegPain:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Legs", "Parietal");
                break;
            case SignalType.HandPain:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Arms", "Parietal");
                break;
            case SignalType.ScaryObjectSight:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Eyes", "Occipital");
                break;
            case SignalType.FamiliarObjectSight:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Eyes", "Occipital");
                break;
            case SignalType.HotObject:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Hands", "Parietal");
                break;
            case SignalType.ColdObject:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Hands", "Parietal");
                break;
            case SignalType.SweetTaste:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Tounge", "Parietal");
                break;
            case SignalType.SourTaste:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Tounge", "Parietal");
                break;
            case SignalType.SpokenTo:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Ears", "Frontal");
                break;
            case SignalType.Falling:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Ears", "Crebellum");
                break;
            case SignalType.Running:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Legs", "Crebellum");
                break;
            case SignalType.HotBody:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus");
                break;
            case SignalType.ColdBody:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus");
                break;
            case SignalType.HighBloodPressure:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Stem");
                break;
            case SignalType.LowBloodPressure:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Stem");
                break;
            case SignalType.LowWater:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus");
                break;
            case SignalType.HighWater:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Blood", "Thalamus");
                break;
            case SignalType.BlockedBreathing:
                sigObj.FillSignalInfo(SignalMovement.SignalClass.Sensory, "Trachea", "Stem");
                break;
            default:
                throw new ArgumentException("Unknown SignalType", "type");
        }

        return signal;
    }


    public GameObject GetBodyPart(string name)
    {
        return bodyPartsCont.transform.FindChild(name).gameObject;
    }

    public GameObject GetBrainPart(string name)
    {
        return brainPartsCont.transform.FindChild(name).gameObject;
    }
}
