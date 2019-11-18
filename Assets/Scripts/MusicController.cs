using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    public enum Variation
    {
        A_NoLowpass, A_Lowpass, B_NoLowpass, B_Lowpass, C_NoLowpass, C_Lowpass, D_NoLowpass, D_Lowpass
    }
    
    public AudioMixer mixer;
    public AudioSource track1;
    public AudioSource track2;
    public AudioSource track3;
    public AudioSource track4;
    public float transitionTime = 1.0f;
    public Variation variation = Variation.A_NoLowpass;

    private AudioMixerSnapshot _aNoLowpass;
    private AudioMixerSnapshot _aLowpass;
    private AudioMixerSnapshot _bNoLowpass;
    private AudioMixerSnapshot _bLowpass;
    private AudioMixerSnapshot _cNoLowpass;
    private AudioMixerSnapshot _cLowpass;
    private AudioMixerSnapshot _dNoLowpass;
    private AudioMixerSnapshot _dLowpass;

    private Variation _currentVariation = Variation.A_NoLowpass;

    private void Start()
    {
        _aNoLowpass = mixer.FindSnapshot("A_NoLowpass");
        _aLowpass = mixer.FindSnapshot("A_Lowpass");
        _bNoLowpass = mixer.FindSnapshot("B_NoLowpass");
        _bLowpass = mixer.FindSnapshot("B_Lowpass");
        _cNoLowpass = mixer.FindSnapshot("C_NoLowpass");
        _cLowpass = mixer.FindSnapshot("C_Lowpass");
        _dNoLowpass = mixer.FindSnapshot("D_NoLowpass");
        _dLowpass = mixer.FindSnapshot("D_Lowpass");
        
        track1.Play();
        track2.Play();
        track3.Play();
        track4.Play();
    }

    public void TransitionTo(Variation variation)
    {
        switch (variation)
        {
            case Variation.A_NoLowpass:
                _aNoLowpass.TransitionTo(transitionTime);
                break;
            case Variation.A_Lowpass:
                _aLowpass.TransitionTo(transitionTime);
                break;
            case Variation.B_NoLowpass:
                _bNoLowpass.TransitionTo(transitionTime);
                break;
            case Variation.B_Lowpass:
                _bLowpass.TransitionTo(transitionTime);
                break;
            case Variation.C_NoLowpass:
                _cNoLowpass.TransitionTo(transitionTime);
                break;
            case Variation.C_Lowpass:
                _cLowpass.TransitionTo(transitionTime);
                break;
            case Variation.D_NoLowpass:
                _dNoLowpass.TransitionTo(transitionTime);
                break;
            case Variation.D_Lowpass:
                _dLowpass.TransitionTo(transitionTime);
                break;
        }
    }

    private void Update()
    {
        if (!variation.Equals(_currentVariation))
        {
            TransitionTo(variation);
            _currentVariation = variation;
        }
    }
}
