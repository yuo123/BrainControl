using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InputManager : MonoBehaviour
{
    public Vector2[] pos;
    public Dictionary<string, Vector2> poses;
    public GameObject brainMarkGO;
    public GameObject bodyMarkGO;

    // Use this for initialization
    void Start()
    {
        poses = new Dictionary<string, Vector2>(pos.Length);
        //brain parts
        poses.Add("Frontal", pos[0]);
        poses.Add("Hypophysis", pos[1]);
        poses.Add("Thalamus", pos[2]);
        poses.Add("Limbic", pos[3]);
        poses.Add("Cerebellum", pos[4]);
        poses.Add("Stem", pos[5]);
        poses.Add("Occipital", pos[6]);
        poses.Add("Parietal", pos[7]);
        //body parts
        poses.Add("Lungs", pos[8]);
        poses.Add("Heart", pos[9]);
        poses.Add("Liver", pos[10]);
        poses.Add("Legs", pos[11]);
        poses.Add("Arms", pos[12]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 camPos = Camera.main.transform.position;
            RaycastHit2D hitInfo  = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
            bool res = hitInfo.collider != null;
            if (res && poses.ContainsKey(hitInfo.collider.name))
            {
                GameObject mark = hitInfo.collider.tag == "BrainPart" ? brainMarkGO : bodyMarkGO;
                mark.transform.position = poses[hitInfo.collider.name];
            }
        }
    }
}
