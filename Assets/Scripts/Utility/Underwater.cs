using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underwater : MonoBehaviour {

	//This script enables underwater effects. Attach to main camera.
	public Color fogColor;
	public float fogDensity = 0.02f;
	private Material noSkybox;

 
	void Start () {
		RenderSettings.fog = true;
		RenderSettings.fogColor = fogColor; 
		RenderSettings.fogDensity = fogDensity;
		RenderSettings.skybox = noSkybox;
	}
}