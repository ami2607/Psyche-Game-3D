using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls when you go to next stage of game
/// </summary>
public class LevelController : MonoBehaviour
{
    private PlanetBehavior planet;

    public void StartPlanetTravel()
    {
        
    }

    public void EnterPlanet()
    {
        //calculate points and set what will happen in the ext screen
        //switch to planet scene
    }
    // Start is called before the first frame update
    void Start()
    {
        planet= FindObjectOfType<PlanetBehavior>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (planet.isMovingToPlanet())
        {
            StartPlanetTravel();
        }
    }
}
