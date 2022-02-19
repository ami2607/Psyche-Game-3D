using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VattalusHologramController : MonoBehaviour
{
    public float rotateSpeed = 2f;

    public Light hologramLight;
    public Material hologramBeamMaterial;
    public Material hologramMaterial;
    private Color hologramColor;

    public float cycleDuration = 2f; // How long before looping back to the beginning of the curve
    public float startVal = 1f; // The value around which the intensity will be randomized
    public float flickrIntensity = 1f; // The curve will be multiplied by that value and added to startVal	
    // You could use only the curve, but to change the light's behaviour you would need to
    // Resample the keys. It involves Random calls, and can be expensive.
    // By using intensity and startVal, you can modulate it at any time for no cost.


    public AnimationCurve curve;


    void OnEnable()
    {
        hologramColor = hologramBeamMaterial.color;
        StartCoroutine(StartFlicker());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartFlicker()
    {
        while (Application.isPlaying)
        {
            float t = 0f;
            float cycleInv = 1f / cycleDuration;
            while (t < 1f)
            {
                float evalValue = curve.Evaluate(t) * flickrIntensity;
                hologramLight.intensity = startVal + evalValue;

                hologramBeamMaterial.color = new Color(hologramColor.r, hologramColor.g, hologramColor.b, 1f + evalValue);
                hologramMaterial.color = hologramBeamMaterial.color;

                yield return null;

                t += Time.deltaTime * cycleInv;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
