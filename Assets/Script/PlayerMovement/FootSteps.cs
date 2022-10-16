using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FootSteps : MonoBehaviour
{
    public float stepRate = 0.5f;
	public float stepCoolDown;
	public AudioClip footStep;
    public AudioSource audioS;
 
 
	// Update is called once per frame
	void Update () {
		stepCoolDown -= Time.deltaTime;
		if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && stepCoolDown < 0f){
			audioS.pitch = 1f + Random.Range (-0.2f, 0.2f);
			audioS.PlayOneShot (footStep, 0.9f);
			stepCoolDown = stepRate;
		}
	}
}
