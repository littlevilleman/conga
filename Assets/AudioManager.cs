using Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public enum EMusicCode
    {
        MAIN_THEME
    }
    public enum ESoundCode
    {
        MENU_SWITCH, MENU_CONFIRM, MENU_BACK, MATCH_START, MATCH_DEFEAT, PARTICIPANT_JOIN_CONGA
    }

    [Serializable]
    public class Sound
    {
        public ESoundCode code;
        public AudioClip clip;
    }
    public class Music
    {
        public EMusicCode code;
        public AudioClip clip;
    }

    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<Sound> sounds;
        [SerializeField] private List<Music> music;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource musicSource;

        private IRythm rythm;

        public void Setup(IRythm rythmSetup)
        {
            rythm = rythmSetup;
            rythm.OnStep += OnStep;
        }

        private void OnStep()
        {
            musicSource.pitch = Mathf.Clamp(2 * (1 - rythm.Cadence), 1f, 2f);
            //Debug.Log();
        }

        public void PlaySound(ESoundCode code)
        {
            Sound sound = sounds.Find(x => x.code == code);

            if (sound == null)
                return;

            audioSource.clip = sound.clip;
            audioSource.Play();
        }

        public void PlayMusic(EMusicCode code)
        {
            Music music = this.music.Find(x => x.code == code);

            if (music == null)
                return;

            musicSource.clip = music.clip;
            musicSource.Play();
        }
    }
}