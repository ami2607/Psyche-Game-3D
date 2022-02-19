using UnityEngine;

public class VattalusThrusterController : MonoBehaviour
{
    //This script handles the visual effects of the directional thrusters
    //It has variables to determine which movement type this thruster contributes to when maneuvering the ship. The ship controller will then selectively enable thrusters based on the desired movement.


    [Header("Check the movement types for which you want this thruster to be enabled")]
    //list of movements the thruster contributes to
    public bool usedForAcceleration = false;
    public bool usedForDeceleration = false;
    public bool usedForStrafeLeft = false;
    public bool usedForStrafeRight = false;
    public bool usedForMoveUp = false;
    public bool usedForMoveDown = false;
    public bool usedForRollLeft = false;
    public bool usedForRollRight = false;
    public bool usedForPitchUp = false;
    public bool usedForPitchDown = false;
    public bool usedForYawLeft = false;
    public bool usedForYawRight = false;

    [Space]

    private float currentThrust = 0f;
    private float thrustValue = 0f;
    [Header("Thrust effect buildup and decay speeds")]
    public float thrustIncreaseSpeed = 2f;
    public float thrustDecaySpeed = 1f;

    [Header("Effect references")]
    public Light lightComponent = null;
    public ParticleSystem particles = null;
    public Renderer engineGlowMesh = null;
    public float maxGlowIntensity = 1.35f;


    void Start()
    {
        currentThrust = 0f;
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        //we want the thruster effect to to increase and decrease with different speeds
        float lerpSpeed = thrustValue > currentThrust ? thrustIncreaseSpeed : thrustDecaySpeed;

        //smoothly adjust the current thrust value towards the desired value. This variable will then drive the various effects
        currentThrust = Mathf.Lerp(currentThrust, thrustValue, lerpSpeed * Time.deltaTime);

        //update effect

        //particles
        if (particles != null)
        {
            ParticleSystem.EmissionModule particleEmission = particles.emission;
            ParticleSystem.MainModule particleMain = particles.main;

            if (currentThrust > 0.1f)
            {
                particleEmission.enabled = true;
                particleMain.startColor = new Color(particleMain.startColor.color.r, particleMain.startColor.color.g, particleMain.startColor.color.b, currentThrust);
            }
            else
            {
                //disable particle system if thrust level is low
                particleEmission.enabled = false;
            }
        }

        //glow mesh
        if (engineGlowMesh != null)
        {
            engineGlowMesh.material.SetFloat("_Glow", currentThrust * maxGlowIntensity);
        }
    }

    public void SetThrust(float thrustVal)
    {
        thrustValue = thrustVal;
    }
}
