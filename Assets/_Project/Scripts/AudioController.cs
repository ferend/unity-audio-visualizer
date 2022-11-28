using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _soundObject;
    [SerializeField] private ParticleSystem _soundObject2;
    [SerializeField] private ParticleSystem _soundObject3;
    [SerializeField] private AudioSource _audioSource;
    
    private float[] _spectrumData;
 
    void Start()
    {
        // Array of all spectrum data will be saved
        _spectrumData = new float[256];
        StartCoroutine(StartDelayed());
    }

    private void Update()
    {
        AudioListener.GetSpectrumData(_spectrumData, 0, FFTWindow.Rectangular);
        
        for (int i = 0; i < _spectrumData.Length ; i++)
        {
            float tmp = _spectrumData[i] * 10;
            if (tmp > 2F)
            {
                ChangeParticleEmission(tmp);
            }
        }
    }

    private ParticleSystem.EmissionModule _particleEmission;
    private ParticleSystem.MainModule _main;
    private void ChangeParticleEmission(float changeRate)
    {
        _particleEmission = _soundObject.emission;
        _particleEmission.rateOverTime = changeRate;

        _main = _soundObject2.main;
        _main.startSize = changeRate;
    }

    private IEnumerator StartDelayed()
    {
        yield return new WaitForSeconds(3F);
        _audioSource.Play();
    }
}
