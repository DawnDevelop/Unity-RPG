using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightenLight : MonoBehaviour {

    [SerializeField] Light lightToSwitch = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        lightToSwitch.intensity = 1;
        lightToSwitch.range = 1;
        lightToSwitch.color = new Color(0.28f,0.17f,0.76f,1);
        RenderSettings.ambientIntensity = 1.5f;
    }
}
