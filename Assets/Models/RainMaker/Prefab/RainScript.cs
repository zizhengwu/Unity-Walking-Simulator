using UnityEngine;
using System.Collections;

public class RainScript : MonoBehaviour
{
    /// <summary>
    /// Provides an easy wrapper to looping audio sources with nice transitions for volume when starting and stopping
    /// </summary>
    private class LoopingAudioSource
    {
        public AudioSource AudioSource { get; private set; }
        public float TargetVolume { get; private set; }

        public LoopingAudioSource(MonoBehaviour script, AudioClip clip)
        {
            AudioSource = script.gameObject.AddComponent<AudioSource>();
            AudioSource.loop = true;
            AudioSource.clip = clip;
            AudioSource.Stop();
            TargetVolume = 1.0f;
        }

        public void Play(float targetVolume)
        {
            if (!AudioSource.isPlaying)
            {
                AudioSource.volume = 0.0f;
                AudioSource.Play();
            }
            TargetVolume = targetVolume;
        }

        public void Stop()
        {
            TargetVolume = 0.0f;
        }

        public void Update()
        {
            if (AudioSource.isPlaying && (AudioSource.volume = Mathf.Lerp(AudioSource.volume, TargetVolume, Time.deltaTime)) == 0.0f)
            {
                AudioSource.Stop();
            }
        }
    }

    [Tooltip("Camera the rain should hover over, defaults to main camera")]
    public Camera Camera;

    [Tooltip("Light rain looping clip")]
    public AudioClip RainSoundLight;

    [Tooltip("Medium rain looping clip")]
    public AudioClip RainSoundMedium;

    [Tooltip("Heavy rain looping clip")]
    public AudioClip RainSoundHeavy;

    [Tooltip("Intensity of rain (0-1)")]
    [Range(0.0f, 1.0f)]
    public float RainIntensity;

    [Tooltip("Rain particle system")]
    public ParticleSystem RainFallParticleSystem;

    [Tooltip("Particles system for when rain hits something")]
    public ParticleSystem RainExplosionParticleSystem;

    [Tooltip("Particle system to use for rain mist")]
    public ParticleSystem RainMistParticleSystem;

    [Tooltip("The height above the camera that the rain will start falling from")]
    public float RainHeight = 25.0f;

    [Tooltip("How far the rain particle system is ahead of the player")]
    public float RainForwardOffset = -7.0f;

    [Tooltip("Wind looping clip")]
    public AudioClip WindSound;

    [Tooltip("Wind sound volume modifier, use this to lower your sound if it's too loud.")]
    public float WindSoundVolumeModifier = 0.5f;

    [Tooltip("Wind zone that will affect and follow the rain")]
    public WindZone WindZone;

    [Tooltip("Minimum, maximum and absolute maximum wind speed. Set to 0 if you are managing the wind speed yourself or don't want wind. " +
        "The absolute maximum should always be >= to the current maximum wind speed and should generally never change. The maximum is " +
        "used to determine how much louder your wind sound gets.")]
    public Vector3 WindSpeedRange = new Vector3(50.0f, 500.0f, 500.0f);

    [Tooltip("How often the wind speed and direction changes (minimum and maximum change interval in seconds)")]
    public Vector2 WindChangeInterval = new Vector2(5.0f, 30.0f);

    private LoopingAudioSource audioSourceRainLight;
    private LoopingAudioSource audioSourceRainMedium;
    private LoopingAudioSource audioSourceRainHeavy;
    private LoopingAudioSource audioSourceRainCurrent;
    private LoopingAudioSource audioSourceWind;
    private Material rainMaterial;
    private Material rainExplosionMaterial;
    private Material rainMistMaterial;

    private float nextWindTime;
    private float lastRainValue = -1.0f;

    private void UpdateWind()
    {
        if (WindZone != null && WindSpeedRange.y > 1.0f)
        {
            WindZone.transform.position = Camera.transform.position;
            WindZone.transform.Translate(0.0f, WindZone.radius, 0.0f);
            if (nextWindTime < Time.time)
            {
                WindZone.windMain = UnityEngine.Random.Range(WindSpeedRange.x, WindSpeedRange.y);
                WindZone.windTurbulence = UnityEngine.Random.Range(WindSpeedRange.x, WindSpeedRange.y);
                WindZone.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(-30.0f, 30.0f), UnityEngine.Random.Range(0.0f, 360.0f), 0.0f);
                nextWindTime = Time.time + UnityEngine.Random.Range(WindChangeInterval.x, WindChangeInterval.y);
                audioSourceWind.Play((WindZone.windMain / WindSpeedRange.z) * WindSoundVolumeModifier);
            }
        }
        else
        {
            audioSourceWind.Stop();
        }

        audioSourceWind.Update();
    }

    private void CheckForRainChange()
    {
        if (lastRainValue != RainIntensity)
        {
            lastRainValue = RainIntensity;
            if (RainIntensity <= 0.01f)
            {
                if (audioSourceRainCurrent != null)
                {
                    audioSourceRainCurrent.Stop();
                    audioSourceRainCurrent = null;
                }
                if (RainFallParticleSystem != null)
                {
                    RainFallParticleSystem.enableEmission = false;
                }
                if (RainMistParticleSystem != null)
                {
                    RainMistParticleSystem.enableEmission = false;
                }
            }
            else
            {
                LoopingAudioSource newSource;
                if (RainIntensity >= 0.67f)
                {
                    newSource = audioSourceRainHeavy;
                }
                else if (RainIntensity >= 0.33f)
                {
                    newSource = audioSourceRainMedium;
                }
                else
                {
                    newSource = audioSourceRainLight;
                }
                if (audioSourceRainCurrent != newSource)
                {
                    if (audioSourceRainCurrent != null)
                    {
                        audioSourceRainCurrent.Stop();
                    }
                    audioSourceRainCurrent = newSource;
                    audioSourceRainCurrent.Play(1.0f);
                }
                if (RainFallParticleSystem != null)
                {
                    RainFallParticleSystem.enableEmission = RainFallParticleSystem.GetComponent<Renderer>().enabled = true;
                    RainFallParticleSystem.emissionRate = (RainFallParticleSystem.maxParticles / RainFallParticleSystem.startLifetime) * RainIntensity;
                }
                if (RainMistParticleSystem != null)
                {
                    RainMistParticleSystem.enableEmission = RainMistParticleSystem.GetComponent<Renderer>().enabled = true;
                    float emissionRate;
                    if (RainIntensity < 0.5f)
                    {
                        emissionRate = 0.0f;
                    }
                    else
                    {
                        // must have 0.5 or higher rain intensity to start seeing mist
                        emissionRate = (RainMistParticleSystem.maxParticles / RainMistParticleSystem.startLifetime) * RainIntensity * RainIntensity;
                    }
                    RainMistParticleSystem.emissionRate = emissionRate;
                }
            }
        }
    }

    private void UpdateRain()
    {
        CheckForRainChange();

        // keep rain on top of the player
        if (RainFallParticleSystem != null)
        {
            RainFallParticleSystem.transform.position = Camera.transform.position;
            RainFallParticleSystem.transform.Translate(0.0f, RainHeight, RainForwardOffset);
            RainFallParticleSystem.transform.rotation = Quaternion.Euler(0.0f, Camera.transform.rotation.eulerAngles.y, 0.0f);
        }
        if (RainMistParticleSystem != null)
        {
            Vector3 pos = Camera.transform.position;
            pos.y = 3.0f;
            RainMistParticleSystem.transform.position = pos;
        }

        audioSourceRainLight.Update();
        audioSourceRainMedium.Update();
        audioSourceRainHeavy.Update();
    }

    // Use this for initialization
    void Start()
    {
        if (Camera == null)
        {
            Camera = Camera.main;
        }

        audioSourceRainLight = new LoopingAudioSource(this, RainSoundLight);
        audioSourceRainMedium = new LoopingAudioSource(this, RainSoundMedium);
        audioSourceRainHeavy = new LoopingAudioSource(this, RainSoundHeavy);
        audioSourceWind = new LoopingAudioSource(this, WindSound);

        if (RainFallParticleSystem != null)
        {
            RainFallParticleSystem.enableEmission = false;
            Renderer rainRenderer = RainFallParticleSystem.GetComponent<Renderer>();
            rainRenderer.enabled = false;
            rainMaterial = new Material(rainRenderer.material);
            rainMaterial.EnableKeyword("SOFTPARTICLES_OFF");
            rainRenderer.material = rainMaterial;
        }
        if (RainExplosionParticleSystem != null)
        {
            Renderer rainRenderer = RainExplosionParticleSystem.GetComponent<Renderer>();
            rainExplosionMaterial = new Material(rainRenderer.material);
            rainExplosionMaterial.EnableKeyword("SOFTPARTICLES_OFF");
            rainRenderer.material = rainExplosionMaterial;
        }
        if (RainMistParticleSystem != null)
        {
            RainMistParticleSystem.enableEmission = false;
            Renderer rainRenderer = RainMistParticleSystem.GetComponent<Renderer>();
            rainRenderer.enabled = false;
            rainMistMaterial = new Material(rainRenderer.material);
			rainMistMaterial.EnableKeyword("SOFTPARTICLES_ON");
            rainRenderer.material = rainMistMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWind();
        UpdateRain();
    }
}
