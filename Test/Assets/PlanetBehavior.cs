using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehavior : MonoBehaviour
{
    public Transform ship;

    private GameObject debrisGrid;
    private GameObject atmosphere;

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

    private void Start()
    {
        debrisGrid = GameObject.Find("Debris Grid");
        atmosphere = GameObject.Find("Atmosphere");
        ship = GameObject.Find("Space Ship").GetComponent<Transform>();

        debrisGrid.SetActive(false);
        atmosphere.SetActive(false);
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
            atmosphere.SetActive(true);
            //start triggering particles and cloud effects. beeping etc
        }
        if (shiptToPlaneDistance < 6)
        {
            debrisGrid.SetActive(true);
            //initial particles and shaking, start to make sounds
        }
    }
}