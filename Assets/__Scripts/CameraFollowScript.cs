using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InRoomScript))]
public class CameraFollowScript : MonoBehaviour
{   
    static public bool transitioning { get; private set; }

    [Header("Inscribed")]
    public float transTime = .5f;


    private InRoomScript inRm;
    private Vector3 p0, p1;
    private float transStart;

    // Start is called before the first frame update
    void Awake()
    {
        inRm = GetComponent<InRoomScript>();
        transitioning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transitioning)
        {
            float u = (Time.time - transStart) / transTime;
            if (u >= 1)
            {
                u = 1;
                transitioning = false;
            }
            transform.position = (1 - u) * p0 + u * p1;
        }
        else
        {
            if(DrayScript.IFM.roomNum != inRm.roomNum)
            {
                TransitionTo(DrayScript.IFM.roomNum);
            }
        }
    }
    void TransitionTo(Vector2 rm)
    {
        p0 = transform.position;
        inRm.roomNum = rm;
        p1 = transform.position + (Vector3.back * 10);
        transform.position = p0;
        transStart = Time.time;
        transitioning = true;
    }
}
