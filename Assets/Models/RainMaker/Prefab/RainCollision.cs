using UnityEngine;
using System.Collections;

public class RainCollision : MonoBehaviour
{
    private ParticleCollisionEvent[] collisionEvents;
    private Color32 color = new Color32(255, 255, 255, 255);

    public ParticleSystem RainExplosion;
    public ParticleSystem RainParticleSystem;

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    private void Emit(ref Vector3 pos)
    {
        int count = UnityEngine.Random.Range(2, 5);
        while (count != 0)
        {
            float yVelocity = UnityEngine.Random.Range(1.0f, 3.0f);
            float zVelocity = UnityEngine.Random.Range(-2.0f, 2.0f);
            float xVelocity = UnityEngine.Random.Range(-2.0f, 2.0f);
            const float lifetime = 0.75f;// UnityEngine.Random.Range(0.25f, 0.75f);
            float size = UnityEngine.Random.Range(0.05f, 0.1f);
            RainExplosion.Emit(pos, new Vector3(xVelocity, yVelocity, zVelocity), size, lifetime, color);
            count--;
        }
    }

    private void OnParticleCollision(GameObject obj)
    {
        if (RainExplosion != null && RainParticleSystem != null)
        {
            if (collisionEvents == null || collisionEvents.Length != RainParticleSystem.GetSafeCollisionEventSize())
            {
                collisionEvents = new ParticleCollisionEvent[RainParticleSystem.GetSafeCollisionEventSize()];
            }

            int count = RainParticleSystem.GetCollisionEvents(obj, collisionEvents);
            for (int i = 0; i < count; i++)
            {
                ParticleCollisionEvent evt = collisionEvents[i];
                Vector3 pos = evt.intersection;
                Emit(ref pos);
            }
        }
    }
}
