using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class ControleVolume : MonoBehaviour
{
    public AudioMixer audioMixer;
    private string nomeMixer;
    private float volumeAntigo;
    private void Awake()
    {
        if (gameObject.name.StartsWith("Slider")) {
            nomeMixer = gameObject.name.Equals("SliderMus") ? "volumeMusica" : "volumeSFX";
        } else {
            nomeMixer = gameObject.name.Equals("ToggleMus") ? "volumeMusica" : "volumeSFX";
        }
        if (!gameObject.name.StartsWith("Slider"))
            return;
        float valorSlider;
        audioMixer.GetFloat(nomeMixer, out valorSlider);
        valorSlider = Mathf.Pow(10, valorSlider / 20);
        gameObject.GetComponent<Slider>().value = valorSlider;
    }

    public void MudarVolume(float volumeSlider)
    {
        audioMixer.SetFloat(nomeMixer, Mathf.Log10(volumeSlider) * 20);
    }

    public void MutarAudio(bool desmutar)
    {
        if (desmutar)
        {
            audioMixer.SetFloat(nomeMixer, volumeAntigo);
        }
        else
        {
            audioMixer.GetFloat(nomeMixer, out volumeAntigo);
            audioMixer.SetFloat(nomeMixer, Mathf.Log10(0.0001f) * 20);
        }
    }
}
