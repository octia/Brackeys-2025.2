using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioSlider : MonoBehaviour
{


    [SerializeField]
    private AudioMixer Mixer;
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private AudioMixMode MixMode;

    [SerializeField]
    private string ExposedParameterName;

    [SerializeField]
    private AudioSource Example_effect;
    private void Start()
    {

    }

    private void OnEnable()
    {
        var value = PlayerPrefs.GetFloat(ExposedParameterName, 1);
        Mixer.SetFloat(ExposedParameterName, Mathf.Log10(value * 20));
        slider.value = value;
        slider.onValueChanged.AddListener(OnChangeSlider);

    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnChangeSlider);
    }

    public void OnChangeSlider(float Value)
    {

        switch (MixMode)
        {

            case AudioMixMode.LinearMixerVolume:
            Mixer.SetFloat(ExposedParameterName, (-80 + Value * 80));
            break;
            case AudioMixMode.LogrithmicMixerVolume:
            Mixer.SetFloat(ExposedParameterName, Mathf.Log10(Value) * 20);
            break;
        }

        float a = Mathf.Log10(Value) * 20;

        PlayerPrefs.SetFloat(ExposedParameterName, Value);
        PlayerPrefs.Save();

        if (Example_effect != null && Example_effect.isPlaying == false)
        {
            Example_effect.Play();
        }
    }


    public enum AudioMixMode
    {
        LinearMixerVolume,
        LogrithmicMixerVolume
    }
}
