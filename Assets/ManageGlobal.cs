using UnityEngine;
using System.Collections;

public class ManageGlobal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void SetFogDensity(float fogDensity)
    {
        RenderSettings.fogDensity = fogDensity;
    }
}
