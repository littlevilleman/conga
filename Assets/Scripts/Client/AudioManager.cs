using Core;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public bool mute;
    }

    [Serializable]
    public class Music
    {
        public EMusicCode code;
        public AudioClip clip;
        public bool mute;
    }

    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<Sound> sounds;
        [SerializeField] private List<Music> music;
        [SerializeField] private AudioSource musicSource;
        private List<AudioSource> audioSources;

        private bool muteSound;

        private IRythm rythm;

        private void Awake()
        {
            audioSources = GetComponents<AudioSource>().ToList();
        }

        private void OnEnable()
        {
            EventBus.Register<EventMuteMusic>(MuteMusic);
            EventBus.Register<EventMuteSound>(MuteSound);
        }

        private void MuteMusic(EventMuteMusic context)
        {
            musicSource.mute = context.mute;
        }

        private void MuteSound(EventMuteSound context)
        {
            muteSound = context.mute;
        }

        public void Setup(IRythm rythmSetup)
        {
            rythm = rythmSetup;
            rythm.OnStep += OnStep;
        }

        private void OnStep()
        {
            //musicSource.pitch = Mathf.Clamp(1 + .4f / (rythm.Cadence) * .15f, 1f, 2f);
        }

        public void PlaySound(ESoundCode code, float pitch = 1f)
        {
            Sound sound = sounds.Find(x => x.code == code);

            if (muteSound || sound == null || sound.mute)
                return;

            AudioSource source = PullAudioSource();
            source.clip = sound.clip;
            source.pitch = pitch;
            source.Play();
        }

        private AudioSource PullAudioSource()
        {
            return audioSources.Find(x => !x.isPlaying);
        }

        public void PlayMusic(EMusicCode code)
        {
            Music music = this.music.Find(x => x.code == code);

            if (music == null)
                return;

            musicSource.clip = music.clip;
            musicSource.Play();
        }

        private void OnDisable()
        {
            EventBus.Unregister<EventMuteMusic>(MuteMusic);
            EventBus.Unregister<EventMuteSound>(MuteSound);
        }
    }
}