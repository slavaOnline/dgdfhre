using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerKulon : MonoBehaviour {
    public float q=1;
    public float delay = 0.07f;
    public float k = 10;

    float lastTime;
    
    public Material Electron;
    public Material Proton;
    public Material Neutron;
    Vector3 vel = new Vector3(0, 0, 0);
    Vector3 MajorDeltaPos = new Vector3(0, 0, 0);
    Vector3 MajorVelosity = new Vector3(0, 0, 0);
    void Start()
    {
        lastTime = -1;
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.enabled = true; 
       if (q != 0)
        {
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * Mathf.Abs(q);
            
        }
        if (q > 0){
            
            rend.sharedMaterial = Proton;
        }else if (q < 0)
        {
            rend.sharedMaterial = Electron;
        }
        else
        {
            rend.sharedMaterial = Neutron;
        }
    }

    void FixedUpdate () {
        if (lastTime<0)
        {
            MajorDeltaPos = Vector3.zero;
            MajorVelosity = Vector3.zero;
            foreach (GameObject elem in ListOfElements.Elements)
            {

                if (elem == gameObject)
                {
                    continue;
                }

                Vector3 Major = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Vector3 Minor = new Vector3(elem.transform.position.x, elem.transform.position.y, elem.transform.position.z);
                float R = Mathf.Sqrt(Mathf.Pow((Major.x - Minor.x), 2) + Mathf.Pow((Major.y - Minor.y), 2) + Mathf.Pow((Major.z - Minor.z), 2));
				//print ("R "+R);
                float q1 = q;
                float q2 = elem.GetComponent<PowerKulon>().q;
                float F = Power(q1, q2, R);
				//print ("F "+F);
                Vector3 SF = new Vector3(PowerAxis(Major,Minor,F,R, "x"), PowerAxis(Major,Minor,F,R, "y"), PowerAxis(Major,Minor,F,R, "z"));
                Vector3 deltaPos=new Vector3(vel.x * delay + SF.x * delay * delay/2, vel.y * delay + SF.y * delay * delay / 2, vel.z * delay + SF.z * delay * delay / 2);
                MajorDeltaPos+=deltaPos;
                MajorVelosity+= SF * delay;
                lastTime = delay;
            }
        }
        else {
            lastTime -= Time.deltaTime;
            
        }
        transform.position += MajorDeltaPos;
        vel += MajorVelosity;
	}
	float Power(float q1,float q2,float R)
    {
        if (R < 0.6)
        {
           R=0.6f;
        }
		float f=k * q1 * q2 / Mathf.Pow(R, 2);
		if (Mathf.Abs(f) < 0.0001) {
			return 0;
		}
		return f;

	}
    float PowerAxis(Vector3 Major,Vector3 Minor,float F,float R,string axis)
    {
        if (axis == "x"){
            return F * (Major.x - Minor.x) / R;
        }
        if (axis == "y")
        {
            return F * (Major.y - Minor.y) / R;
        }
        if (axis == "z")
        {
            return F * (Major.z - Minor.z) / R;
        }
        return 0;
    }
}
