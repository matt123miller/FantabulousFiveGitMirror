using UnityEngine;
using System.Collections;

public class ObjNoise : MonoBehaviour {

    private float weightOfObj;
    private float impactVelocity;
    private Noise noiseScript;
    private float pushNoiseModifier = 100;

    public void Start()
    {
        weightOfObj = GetComponent<Rigidbody>().mass;
        noiseScript = GameObject.FindGameObjectWithTag("NoiseBar").GetComponent<Noise>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        GetImpactVelocity(collision);
        calcNoise(weightOfObj, impactVelocity);
    }

    public void OnCollisionStay(Collision collision)
    {
        GetImpactVelocity(collision);
        calcNoise(weightOfObj, impactVelocity);
    }

    public void GetImpactVelocity(Collision collision)
    {
        impactVelocity = collision.relativeVelocity.magnitude;
        Debug.Log("IMPACT VELOCITY" + impactVelocity);
    }

    public void calcNoise(float weight, float impactVel)
    {
        float noise = weight * impactVel;
        updateNoiseBar(noise);
    }

    public void calcNoise(float weight, float impactVel, float modifier)
    {
        float noise = weight * impactVel / modifier;
        updateNoiseBar(noise);
    }

    public void updateNoiseBar(float noise)
    {
        noiseScript.AddToNoise(noise);
    }
}
