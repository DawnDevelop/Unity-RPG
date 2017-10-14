using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimLight : MonoBehaviour {


    [SerializeField] Light lightToSwitch = null;
	// Use this for initialization
	void Start ()
    {
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        lightToSwitch.range = 0;
        lightToSwitch.intensity = 0;
        lightToSwitch.color = new Color(-255f, -255f, -255f, 1);
        RenderSettings.ambientIntensity = 0f;
        RenderSettings.fogMode = FogMode.Linear;
    }
}
