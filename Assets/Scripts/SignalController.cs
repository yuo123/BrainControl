using UnityEngine;
using System.Collections;

public class SignalController : MonoBehaviour
{
    public InputManager inputManager;
    public GameObject signalsCont;

    public GameObject bodyPartsCont;
    public GameObject brainPartsCont;

    public GameObject bodyMarkGO;
    public GameObject brainMarkGO;

    public Vector3[] path;

    public GameObject signalPrefab;

    const float SIGNAL_INTERVAL = 5f;

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
            sigScript.sigClass = SignalMovement.SignalClass.Sensory;
            sigScript.Origin = origin;
            sigScript.Target = GetRandomBrainPart();
            sigScript.StartMove();
        }
    }

    private GameObject GetRandomBodyPart()
    {
        return bodyPartsCont.transform.GetChild(Random.Range(0, bodyPartsCont.transform.childCount - 1)).gameObject;
    }

    private GameObject GetRandomBrainPart()
    {
        return brainPartsCont.transform.GetChild(Random.Range(0, brainPartsCont.transform.childCount - 1)).gameObject;
    }
}
