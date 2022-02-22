using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    float fuel;

    //how are you comparing if you're doing well or not? maybe how far your aim is from the landing spot, how long you leave ship fires without fixing etc
    int points;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
