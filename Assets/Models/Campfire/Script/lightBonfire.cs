using UnityEngine;
using System.Collections;

public class lightBonfire : MonoBehaviour {
    private bool bonfire_status = false;
    private AudioSource light_bonfire_audio;
    GameObject campfire_light, campfire_spark_particles, campfire_flame_particles, text_light_bonfire;

    // Use this for initialization
    void Start () {
        light_bonfire_audio = GetComponent<AudioSource>();
        campfire_light = GameObject.Find("Campfire/Light");
        campfire_spark_particles = GameObject.Find("Campfire/Spark Particles");
        campfire_flame_particles = GameObject.Find("Campfire/Flame Particles");
        text_light_bonfire = GameObject.Find("Canvas/Text Light Bonfire");
        campfire_light.SetActive(false);
        campfire_flame_particles.SetActive(false);
        campfire_spark_particles.SetActive(false);
        text_light_bonfire.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionStay(Collision collision)
    {
        if (!bonfire_status && collision.transform.name == "Boy")
        {
            text_light_bonfire.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                light_bonfire_audio.Play();
                bonfire_status = true;
                campfire_light.SetActive(true);
                campfire_flame_particles.SetActive(true);
                campfire_spark_particles.SetActive(true);
                text_light_bonfire.SetActive(false);
            }
        }
        
    }
    void OnCollisionExit()
    {
        text_light_bonfire.SetActive(false);
    }
}
