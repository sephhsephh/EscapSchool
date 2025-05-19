using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class VolumeSlider : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer audioMixer;
    private const string VolumeParameter = "MasterVolume";
    private const float DefaultVolume = 0.75f;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeParameter, DefaultVolume);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        // Add listener to the slider
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    /// <summary>
    /// Adjusts the audio volume based on the slider value.
    /// </summary>
    /// <param name="volume">Slider value between 0 and 1</param>
    public void SetVolume(float volume)
    {
        float logVolume = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat(VolumeParameter, logVolume);
    }

    /// <summary>
    /// Save the volume setting when the user releases the slider.
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerPrefs.SetFloat(VolumeParameter, volumeSlider.value);
        PlayerPrefs.Save();
    }
}
