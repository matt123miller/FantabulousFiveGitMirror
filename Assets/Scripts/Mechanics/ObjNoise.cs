using UnityEngine;
using System.Collections;

public class ObjNoise : MonoBehaviour {

    private float _weightOfObj;
    private float _impactVelocity;
    private Noise _noiseScript;
    private float _pushNoiseModifier = 100;
    private float _pushTimer = 0;

    public void Start()
    {
        _weightOfObj = GetComponent<Rigidbody>().mass;
        _noiseScript = GameObject.FindGameObjectWithTag("NoiseBar").GetComponent<Noise>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        GetImpactVelocity(collision);
        calcNoise(_weightOfObj, _impactVelocity);
    }

    public void OnCollisionStay(Collision collision)
    {
        _pushTimer += Time.deltaTime;

        if(_pushTimer >= 1)
        {
            GetImpactVelocity(collision);
            calcNoise(_weightOfObj, _impactVelocity);
            _pushTimer = 0;
        }
    }

    public void OnCollisionExit()
    {
        _pushTimer = 0;
    }

    public void GetImpactVelocity(Collision collision)
    {
        _impactVelocity = collision.relativeVelocity.magnitude;
        Debug.Log("IMPACT VELOCITY" + _impactVelocity + " " + gameObject.name + " " +  collision.collider.name);
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
        _noiseScript.AddToNoise(noise);
    }
}
