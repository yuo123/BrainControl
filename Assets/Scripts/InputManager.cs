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
    public SignalController signalController;

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
                mark.transform.localScale = hitInfo.collider.transform.localScale;
                mark.transform.rotation = hitInfo.collider.transform.rotation;
            }
        }
    }

    public void SignalClick(GameObject signal)
    {
        signalInfoText.horizontalOverflow = HorizontalWrapMode.Wrap;//set the text to wrap, so unity calculates the wrap for us
        signalInfoText.text = signal.GetComponent<SignalMovement>().Info;//set the raw text, which will display inccorrectly
        Canvas.ForceUpdateCanvases();//make the canvas calculate word wrap
        string nText = "";
        int prev = -1;
        foreach (UILineInfo line in signalInfoText.cachedTextGenerator.lines)
        {
            if (prev != -1)
            {
                nText += new string(signalInfoText.text.Substring(prev, line.startCharIdx - prev).Reverse().ToArray()) + "\n";//reverse each line, and add a line-end character
            }
            prev = line.startCharIdx;
        }
        nText += new string(signalInfoText.text.Substring(prev).Reverse().ToArray()) + "\n";//do the last line
        signalInfoText.text = nText;
        signalInfoText.horizontalOverflow = HorizontalWrapMode.Overflow;//turn off automatic word wrap, so it doesn't mess with ours
        signalController.CurrentInfoSignal = signal.GetComponent<SignalMovement>();//finally, tell signalController what signal is currently displaying info
    }
}
