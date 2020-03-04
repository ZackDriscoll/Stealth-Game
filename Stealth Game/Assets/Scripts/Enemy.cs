using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Limit enemy's sight
    public float fieldOfView = 45f;

    //Keep track of our transform
    private Transform tf;

    //Keep track of our target location
    public Transform target;

    //Track what state the AI is in
    public string AIState = "Idle";

    //Track enemy health
    public float HitPoints;

    //Track attack range
    public float AttackRange;

    //Track health cutoff
    public float HPCutoff;

    //Enemy speed
    public float speed = 5.0f;

    //Track our healing rate per second
    public float restingHealRate = 1.0f;

    //Enemy max HP
    public float maxHP;

    // Start is called before the first frame update
    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CanHear(GameManager.instance.player);
        CanSee(GameManager.instance.player);

        if (AIState == "Idle")
        {
            //Do the state behavior
            Idle();

            //Check for transitions
            if (isInRange())
            {
                ChangeState("Seek");
            }
        }
        else if (AIState == "Rest")
        {
            //Do the state behavior
            Rest();

            //Check for transitions
            if (HitPoints >= HPCutoff)
            {
                ChangeState("Idle");
            }
        }
        else if (AIState == "Seek")
        {
            //Do the state behavior
            Seek();

            //Check for transitions
            if (HitPoints < HPCutoff)
            {
                ChangeState("Rest");
            }

            if (!isInRange())
            {
                ChangeState("Idle");
            }
        }
        else
        {
            Debug.LogError("State does not exist: " + AIState);
        }
    }

    public void Idle()
    {
        //Do nothing
    }

    public void Rest()
    {
        //Stand still
        //Heal
        HitPoints += restingHealRate * Time.deltaTime;

        HitPoints = Mathf.Min(HitPoints, maxHP);
    }

    public void Seek()
    {
        //Move toward player
        Vector3 vectorToTarget = target.position - tf.position;
        tf.position += vectorToTarget.normalized * speed * Time.deltaTime;
    }

    public void ChangeState(string newState)
    {
        AIState = newState;
    }

    public bool isInRange()
    {
        return (Vector3.Distance(tf.position, target.position) <= AttackRange);
    }

    public bool CanHear(GameObject target)
    {
        //Get the noisemaker component from our target
        NoiseMaker noise = target.GetComponent<NoiseMaker>();

        //If there is a noisemaker, we can potentially hear the target
        if (noise != null)
        {
            float adjustedVolumeDistance = noise.volumeDistance - Vector3.Distance(tf.position, target.transform.position);

            //If we're close enough, we heard the noise
            if (adjustedVolumeDistance > 0)
            {
                Debug.Log("I heard the noise.");
                return true;
            }
        }

        return false;
    }

    public bool CanSee(GameObject target)
    {
        // We use the location of our target in a number of calculations - store it in a variable for easy access.
        Transform targetTf = target.GetComponent<Transform>();
        Vector3 targetPosition = targetTf.position;

        // Find the vector from the agent to the target
        // We do this by subtracting "destination minus origin", so that "origin plus vector equals destination."
        Vector3 agentToTargetVector = targetPosition - transform.position;

        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the target.
        float angleToTarget = Vector3.Angle(agentToTargetVector, transform.forward);

        // if that angle is less than our field of view
        if (angleToTarget < fieldOfView)
        {
            // Raycast
            RaycastHit2D hitInfo = Physics2D.Raycast(tf.position, agentToTargetVector);

            // If the first object we hit is our target 
            if (hitInfo.collider.gameObject == target)
            {
                // return true 
                //    -- note that this will exit out of the function, so anything after this functions like an else
                return true;
            }
        }
        //   -- note that because we returned true when we determined we could see the target, 
        //      this will only run if we hit nothing or if we hit something that is not our target.
        return false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(other.gameObject);
    }
}
