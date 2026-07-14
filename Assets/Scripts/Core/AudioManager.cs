using System.Collections;
using UnityEngine;

namespace Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        private float currentBGtrackLength;
        private float  currentAmbientAudioLength;
        
        public AudioSource sfxAudioSource;
        public AudioSource bgAudioSource;
        public AudioSource ambientAudioSource;
        /*public AudioSource playerAudioSource;*/
        
        public AudioClip[] bgMusic;
        public AudioClip[] ambientAudio;
        
        public int BgMusicIndex;
        public int AmbientAudioIndex;
        
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            Instance = this;
        }

        public void PlaySound(AudioClip clip)
        {
            if (clip == null) return;
            sfxAudioSource.PlayOneShot(clip);
        }

        /*public void PlayplayerSound(AudioClip clip)
        {
            if (clip == null) return;
            playerAudioSource.PlayOneShot(clip);
        }*/

        public void PlayBGMusic()
        {
            bgAudioSource.clip = bgMusic[BgMusicIndex];
            bgAudioSource.Play();
            
            currentBGtrackLength = bgAudioSource.clip.length;
            incrementBgMusic();
            StartCoroutine(NextBGMusicClip());
        }

        public void incrementBgMusic()
        {
            BgMusicIndex++;
            if (BgMusicIndex >= bgMusic.Length) BgMusicIndex = 0;
        }
        
        public void PlayAmbientAudio()
        {
            ambientAudioSource.clip = ambientAudio[AmbientAudioIndex];
            ambientAudioSource.Play();
            
            currentAmbientAudioLength = ambientAudioSource.clip.length;
            incrementAmbientAudio();
            StartCoroutine(NextAmbientAudioClip());
        }

        public void incrementAmbientAudio()
        {
            AmbientAudioIndex++;
            if (AmbientAudioIndex >= ambientAudio.Length) AmbientAudioIndex = 0;
        }

        private IEnumerator NextAmbientAudioClip()
        {
            yield return new WaitForSeconds(currentAmbientAudioLength);
            PlayAmbientAudio();
        }

        private IEnumerator NextBGMusicClip()
        {
            yield return new WaitForSeconds(currentBGtrackLength);
            PlayBGMusic();
        }
        
    }
}
