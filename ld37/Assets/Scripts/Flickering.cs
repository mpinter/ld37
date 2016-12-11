using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Flickering : MonoBehaviour {

	public float MinIntensity = 0.25f;
	public float MaxIntensity = 0.5f;
	public float MinRange = 20f;
	public float MaxRange = 30f;
	public float MinTime = 0.25f;
	public float MaxTime = 0.5f;
	public bool Intensity = true;

	private float timeLeft;

	float random;

	void Start()
	{
	   random = Random.Range(0.0f, 65535.0f);
	}

	void Update()
	{
		timeLeft -= Time.deltaTime;
		float noise = Mathf.PerlinNoise(random, Time.time);
		if (Intensity) {
			GetComponent<Light>().intensity = Mathf.Lerp(MinIntensity, MaxIntensity, noise);
			if (timeLeft < 0){
				timeLeft = Random.Range(MinTime, MaxTime);
		  	GetComponent<Light>().intensity = Random.Range(MinIntensity, MaxIntensity);
		  }
		  } else {
		  	GetComponent<Light>().range = Mathf.Lerp(MinRange, MaxRange, noise);
				if (timeLeft < 0){
					timeLeft = Random.Range(MinTime, MaxTime);
			  	GetComponent<Light>().range = Random.Range(MinRange, MaxRange);
			  }
		  }
	}
}
