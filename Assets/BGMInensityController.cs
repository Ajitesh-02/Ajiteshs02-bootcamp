using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMInensityController : MonoBehaviour
{
    [Header("Spawner Reference")]
    [SerializeField] private Spawner spawner; // Drag your Spawner here

    [Header("Audio Settings")]
    [SerializeField] private float basePitch = 1f;  // Normal pitch
    [SerializeField] private float maxPitch = 1.5f; // Max pitch at max intensity
    [SerializeField] private float baseVolume = 0.8f;
    [SerializeField] private float maxVolume = 1f;
    [SerializeField] private float smoothSpeed = 2f; // How fast the pitch/volume change

    private AudioSource bgmAudio;

    void Start()
    {
        bgmAudio = GetComponent<AudioSource>();

        // Try auto-find the spawner if not manually assigned
        if (spawner == null)
            spawner = FindObjectOfType<Spawner>();

        if (bgmAudio != null && !bgmAudio.isPlaying)
            bgmAudio.Play();
    }

    void Update()
    {
        if (spawner == null || bgmAudio == null)
            return;

        UpdateBGMIntensity();
    }

    void UpdateBGMIntensity()
    {
        // Calculate how close we are to min spawn delay (0 to 1)
        float intensityT = Mathf.InverseLerp(spawner.initialSpawnDelay, spawner.minSpawnDelay, spawner.CurrentSpawnDelay);

        // Compute target pitch and volume
        float targetPitch = Mathf.Lerp(basePitch, maxPitch, intensityT);
        float targetVolume = Mathf.Lerp(baseVolume, maxVolume, intensityT);

        // Smoothly interpolate for a natural feel
        bgmAudio.pitch = Mathf.Lerp(bgmAudio.pitch, targetPitch, Time.deltaTime * smoothSpeed);
        bgmAudio.volume = Mathf.Lerp(bgmAudio.volume, targetVolume, Time.deltaTime * smoothSpeed);
    }
}
