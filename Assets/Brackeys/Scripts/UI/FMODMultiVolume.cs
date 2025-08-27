using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VCAControl
{
    public string name;
    public string path; // e.g. "vca:/Music"
    public Slider slider;
}

public class FMODMultiVolume : MonoBehaviour
{
    public VCAControl[] vcas;

    void Start()
    {

        foreach (var v in vcas)
        {
            VCA vca = RuntimeManager.GetVCA(v.path);
            float savedVol = PlayerPrefs.GetFloat(v.name, 1f);
            v.slider.value = savedVol;
            vca.setVolume(savedVol);

            v.slider.onValueChanged.RemoveAllListeners();
            v.slider.onValueChanged.AddListener(val =>
            {
                vca.setVolume(val);
                PlayerPrefs.SetFloat(v.name, val);
                PlayerPrefs.Save();
            });
        }
    }
}
