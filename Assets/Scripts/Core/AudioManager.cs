using UnityEngine;

namespace Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public AudioSource sfxAudioSource;
        public AudioSource bgAudioSource;
        public AudioSource ambientAudioSource;
        
        
        public AudioClip[] bgMusic;
        public AudioClip[] ambientAudio;
        
        public int BgMusicIndex;
        public int AmbientAudioIndex;

        //null check and functionality to play a one shot audio clip when authorized.

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

        public void PlayBGMusic()
        {
            bgAudioSource.PlayOneShot(bgMusic[BgMusicIndex]);
            incrementBgMusic();
        }

        public void incrementBgMusic()
        {
            BgMusicIndex++;
            if (BgMusicIndex >= bgMusic.Length) BgMusicIndex = 0;
        }
        
        public void PlayAmbientAudio()
        {
            ambientAudioSource.PlayOneShot(ambientAudio[AmbientAudioIndex]);
            incrementAmbientAudio();
        }

        public void incrementAmbientAudio()
        {
            AmbientAudioIndex++;
            if (AmbientAudioIndex >= ambientAudio.Length) AmbientAudioIndex = 0;
        }
    }
}
