using UnityEngine;
using System.Collections;

public class RunningSound : MonoBehaviour {
    
    private UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter m_Character;
    private AudioSource running_audio;
    private AudioSource running_on_obstacles;
    private AudioSource running_on_extents;
    // Use this for initialization
    void Start () {
        running_on_obstacles = GetComponents<AudioSource>()[0];
        running_on_extents = GetComponents<AudioSource>()[1];
        running_audio = running_on_extents;
        m_Character = GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKey (KeyCode.W) && !running_audio.isPlaying)
        {
            running_audio.Play();
        }
        if (Input.GetKey(KeyCode.S) && !running_audio.isPlaying)
        {
            running_audio.Play();
        }
        if (Input.GetKey(KeyCode.A) && !running_audio.isPlaying)
        {
            running_audio.Play();
        }
        if (Input.GetKey(KeyCode.D) && !running_audio.isPlaying)
        {
            running_audio.Play();
        }
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            running_audio.Stop();
        }
        if (!m_Character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            running_audio.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        running_audio.Stop();
        if (collision.transform.name == "GroundExtents")
        {
            running_audio = running_on_extents;
        }
        else if (collision.transform.name == "GroundObstacles")
        {
            running_audio = running_on_obstacles;
        }
    }
}
