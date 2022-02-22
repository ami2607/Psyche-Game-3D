using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehavior : MonoBehaviour
{
    public Transform ship;

    bool movingToPlanet;
    float distanceScalePower;

    public void StartMovingToPlanet()
    {
        movingToPlanet = true;
    }

    public bool isMovingToPlanet()
    {
        return movingToPlanet;
    }

    //this shoud trigger the transition of fading into one coloer and loading the planet scene
    void StartEnteringAtmosphere()
    {
        //probably a transition coroutine
    }

    

    private void Update()
    {
        float shipSpeed = 0f; //tie this to your ships speed
        float shiptToPlaneDistance = Vector3.Distance(transform.position, ship.position);
        if (movingToPlanet)
        {
            transform.position = Vector3.MoveTowards(transform.position, ship.position, shipSpeed);
            transform.localScale *= (1/ shiptToPlaneDistance) * distanceScalePower;
        }


        if(shiptToPlaneDistance < 1)
        {
            StartEnteringAtmosphere();
        }
        if (shiptToPlaneDistance < 3)
        {
            //start triggering particles and cloud effects. beeping etc
        }
        if (shiptToPlaneDistance < 6)
        {
            //initial particles and shaking, start to make sounds
        }
    }
}