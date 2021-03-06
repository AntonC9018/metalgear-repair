﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public abstract class Step
{
}

public class GoToStep : Step
{
    public Vector3 dest;
}

public class TurnToStep : Step
{
    public Vector3 dest;
}

public class WaitStep : Step
{
    public float interval;
}

public class Patroling : MonoBehaviour
{
    public Vector3 headOffset;
    public Vector3 targetOffset;
    public Vector3 defaultOrientation;

    private Step currentStep;
    public float turnSpeed;
    int currentStepIndex = 0;
    public List<Step> steps;
    public GameObject[] points;

    MoveState currentState;
    NavMeshAgent agent;

    enum MoveState
    {
        Turn,
        SetDestination,
        Wait,
        CheckDone,
        Done
    }

    public void Start()
    {
        steps = new List<Step>();
        TurnPointsIntoSteps();

        agent = GetComponent<NavMeshAgent>();
        currentStep = steps[0];
        SetState(currentStep);
    }

    void TurnPointsIntoSteps()
    {
        for (int i = 0; i < points.Length; i+=2)
        {
            GoToStep step1 = new GoToStep();
            step1.dest = points[i].transform.position;
            steps.Add(step1);

            TurnToStep step2 = new TurnToStep();
            step2.dest = points[i + 1].transform.position;
            steps.Add(step2);

            WaitStep step3 = new WaitStep();
            step3.interval = 2;
            steps.Add(step3);
        }
    }


    void GoTo(Vector3 dest, float dt)
    {
        if (currentState == MoveState.Turn)
        {
            TurnTo(dest, dt);
            if (currentState == MoveState.Done)
            {
                currentState = MoveState.SetDestination;
            }
        }
        else if (currentState == MoveState.SetDestination)
        {
            agent.destination = dest;
            currentState = MoveState.CheckDone;
        }
        else if (currentState == MoveState.CheckDone)
        {
            if ((dest - transform.position).sqrMagnitude < 3)
            {
                currentState = MoveState.Done;
            }
        }
        else
        {
            currentState = MoveState.Done;
        }

    }

    void TurnTo(Vector3 dest, float dt) { 
        
        if (currentState == MoveState.Turn)
        {
            var headPosition = transform.position + headOffset;
            var destHeadLevel = dest + targetOffset;

            var toTarget = destHeadLevel - headPosition;
            var targetRotation = Quaternion.LookRotation(toTarget, Vector3.up);

            var lookingRotation = transform.rotation.eulerAngles + defaultOrientation;
            var lookingAt = transform.forward;

            var forAngleTarget = new Vector3(toTarget.x, 0, toTarget.z);
            var forAngleStart = new Vector3(lookingAt.x, 0, lookingAt.z);

            float angle = Vector3.Angle(forAngleStart, forAngleTarget);

            var axis = GetNormal(lookingAt, toTarget, Vector3.zero);
            float dtheta = dt * turnSpeed;

            if (Mathf.Abs(angle) > Mathf.Abs(dtheta))
            {
                transform.rotation *= Quaternion.Euler(axis * dtheta);            
            }
            else
            {
                transform.rotation = targetRotation;
                currentState = MoveState.Done;
            }
        }
        else
        {
            currentState = MoveState.Done;
        }
    }


    private float timePassed = 0;
    void Wait(float interval, float dt)
    {
        if (currentState == MoveState.Wait)
        {
            timePassed += dt;
            if (timePassed > interval)
            {
                currentState = MoveState.Done;
            }
        }
        else
        {
            currentState = MoveState.Done;
        }
    }


    // Get the normal to a triangle from the three corner points, a, b and c.
    Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        // Find vectors corresponding to two of the sides of the triangle.
        Vector3 side1 = b - a;
        Vector3 side2 = c - a;

        // Cross the vectors to get a perpendicular vector, then normalize it.
        return Vector3.Cross(side1, side2).normalized;
    }



    public void Patrol()
    {
        float dt = Time.deltaTime;

        if (currentStep is GoToStep)
        {
            GoTo(((GoToStep)currentStep).dest, dt);
        } 
        else if (currentStep is TurnToStep)
        {
            TurnTo(((TurnToStep)currentStep).dest, dt);
        }
        else if (currentStep is WaitStep)
        {
            Wait(((WaitStep)currentStep).interval, dt);
        }

        if (currentState == MoveState.Done)
        {
            currentStep = NextStep();
        }
    }

    Step NextStep()
    {
        var nextStep = steps[++currentStepIndex % steps.Count];
        SetState(nextStep);
        return nextStep;
    }

    void SetState(Step step)
    {
        if (step is GoToStep || step is TurnToStep)
        {
            currentState = MoveState.Turn;
        }
        else
        {
            currentState = MoveState.Wait;
        }
    }

    public void Halt() {
        agent.destination = transform.position;
    }

    public void Continue() {
        for (int i = 0; i < steps.Count; i++) {
            var step = steps[(i + currentStepIndex) % steps.Count];
            if (step is GoToStep) {
                agent.destination = ((GoToStep)step).dest;
                currentStepIndex = (i + currentStepIndex) % steps.Count;
                currentStep = step;
                currentState = MoveState.CheckDone;
                return;
            }
        }
    }
}
