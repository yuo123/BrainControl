﻿using System;

using UnityEngine;
using System.Collections;

public enum SignalType { LegPain, HandPain, ScaryObjectSight, FamiliarObjectSight, HotObject, ColdObject, SweetTaste, SourTaste, SpokenTo, Insulted, Greeted, Falling, Running, HotBody, ColdBody, HighBloodPressure, LowBloodPressure, LowWater, HighWater, BlockedBreathing }

public class SignalMovement : MonoBehaviour
{
    public enum SignalClass { Sensory, Motor }

    public SignalController signalController;

    public SignalClass SigClass { get; set; }
    public SignalType SigType { get; set; }
    public GameObject Target { get; set; }
    public GameObject Origin { get; set; }

    public float speed = 1f;

    private int direction;
    private int currentWaypoint = -2;
    private Vector3 GetCurrentWaypoint()
    {

        if (currentWaypoint < -1 || currentWaypoint > signalController.path.Length)
            throw new ArgumentOutOfRangeException();

        if (currentWaypoint < 0)
            return signalController.inputManager.bodyMarkGO.transform.position;

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
            Vector3 cur = GetCurrentWaypoint();
            this.transform.position = Vector3.MoveTowards(this.transform.position, cur, speed * Time.deltaTime);
            if (transform.position == cur)
            {
                currentWaypoint += direction;
                if (currentWaypoint == -2 || currentWaypoint == signalController.path.Length + 1)
                {
                    currentWaypoint = -2;
                    //Finish path
                }
            }
        }
    }
    
    public void FillSignalInfo(SignalClass clas, string origin, string target)
    {
        this.SigClass = clas;
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
