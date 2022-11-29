using System;
using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _soundObject;
    [SerializeField] private ParticleSystem _soundObject2;
    [SerializeField] private ParticleSystem _soundObject3;
    [SerializeField] private AudioSource _audioSource;
    
    private float[] _spectrumData;
    private float[] _freqBand;

    private void Awake()
    {
        _spectrumData = new float[512];
        _freqBand = new float[8];
        
    }

    void Start()
    {
        StartCoroutine(StartDelayed());
    }

    private void Update()
    {
        AudioSpectrumControl();
        FreqBands(1);
    }

    private void AudioSpectrumControl()
    {
        _audioSource.GetSpectrumData(_spectrumData, 0, FFTWindow.Blackman);
        
        for (int i = 0; i < _spectrumData.Length ; i++)
        {
            float tmp = _spectrumData[i] * 100;
            if (tmp >= 3F)
            {
            }
        }
    }
    
    /// <summary>
    ///  22050 hertz / 512  43 hertz per spectrum data. 2 sample equals 86 hertz. Fist loop indicates how many samples are
    /// going to be in every frequency band. In second loop put the values of spectrum into frequency band
    /// 
    /// </summary>
    private void FreqBands(int sampleAmount)
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float avg = 0;
            int sampleCount = (int) Mathf.Pow(2, i) * 2;
            if (i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                avg += _spectrumData[count] * (count + 1);
                count++;
            }

            avg /= count;
            _freqBand[i] = avg * 10;
        }
        ChangeParticleEmission( _freqBand[sampleAmount]* 5 );

    }

    private ParticleSystem.EmissionModule _particleEmission;
    private ParticleSystem.MainModule _main;
    private ParticleSystem.NoiseModule _noise;
    
    private void ChangeParticleEmission(float changeRate)
    {
        _particleEmission = _soundObject.emission;
        _main = _soundObject2.main;
        _noise = _soundObject3.noise;

        _main.startSize = changeRate;
        _particleEmission.rateOverTime = changeRate;
         _noise.strength = changeRate;
    }

    private bool _soundState;
    private bool currentState;
    public void ButtonMethod()
    {
        currentState = ToggleSound();
    }
    public bool ToggleSound()
    {
        bool currentState = !_soundState;
        SetSound(currentState);
        return currentState;
    }
    
    private void SetSound(bool state)
    {
        _soundState = state; 
        _audioSource.volume = state ? 0.3f : 0f;
    }



    private IEnumerator StartDelayed()
    {
        yield return new WaitForSeconds(3F);
        _audioSource.Play();
    }
}
