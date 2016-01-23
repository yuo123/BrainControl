using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public GameObject brainMarkGO;
    public GameObject bodyMarkGO;
    public GameObject selectedBrainPart;
    public GameObject selectedBodyPart;
    public Text signalInfoText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
            if (hitInfo.collider != null)
            {
                GameObject mark;
                switch (hitInfo.collider.tag)
                {
                    case "BrainPart":
                        mark = brainMarkGO;
                        selectedBrainPart = hitInfo.collider.gameObject;
                        break;
                    case "BodyPart":
                        mark = bodyMarkGO;
                        selectedBodyPart = hitInfo.collider.gameObject;
                        break;
                    default:
                        return;
                }
                mark.transform.position = hitInfo.collider.transform.position;
            }
        }
    }

    public void SignalClick(GameObject signal)
    {
        signalInfoText.text = new string(signal.GetComponent<SignalMovement>().Info.Reverse().ToArray());
    }
}
