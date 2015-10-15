using UnityEngine;
using System.Collections;

public class BoxCollide : MonoBehaviour {
    AudioSource boxCollideAudio;

    // Use this for initialization
    void Start()
    {
        boxCollideAudio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.relativeVelocity.magnitude > 2)
            boxCollideAudio.Play();

    }
}
