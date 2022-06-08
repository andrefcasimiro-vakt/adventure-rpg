using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    [RequireComponent(typeof(AudioSource))]
    public class BGMManager : MonoBehaviour, ISaveable
    {
        AudioSource audioSource => GetComponent<AudioSource>();

        public AudioClip currentMusic;
        public AudioClip previousMusic;

        public static BGMManager instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }

            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            SaveSystem.instance.OnGameLoad += OnGameLoaded;

            if (this.currentMusic != null)
            {
                PlayMusic(this.currentMusic);
            }
        }

        private void Update()
        {

        }

        public void PlayMusic(AudioClip musicToPlay)
        {
            this.previousMusic = this.currentMusic;
            this.currentMusic = musicToPlay;

            this.audioSource.clip = this.currentMusic;
            this.audioSource.Play();
        }

        public void PlayPreviousMusic()
        {
            AudioClip musicToPlay = this.previousMusic;

            this.PlayMusic(musicToPlay);
        }

        public void OnGameLoaded(GameData gameData)
        {
        }


    }
}