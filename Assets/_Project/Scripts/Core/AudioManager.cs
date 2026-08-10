using System.Collections;
using UnityEngine;
namespace Core
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioSource sfxAudioSource;
        public AudioSource bgAudioSource;
        public AudioSource ambientAudioSource;
        public AudioClip[] bgMusic;
        public AudioClip[] ambientAudio;
        private int _bgMusicIndex;
        private int _ambientAudioIndex;
        private Coroutine _bgMusicCoroutine;
        private Coroutine _ambientAudioCoroutine;
        
        public void PlaySound(AudioClip clip)
        {
            if (clip == null) return;
            sfxAudioSource.PlayOneShot(clip);
        }
        
        public void PlayBgMusic()
        {
            if (_bgMusicCoroutine != null)
                StopCoroutine(_bgMusicCoroutine);
            _bgMusicCoroutine = StartCoroutine(BgMusicPlaylist());
        }
        
        public void PlayAmbientAudio()
        {
            if (_ambientAudioCoroutine != null)
                StopCoroutine(_ambientAudioCoroutine);
            _ambientAudioCoroutine = StartCoroutine(AmbientAudioPlaylist());
        }
        
        private IEnumerator BgMusicPlaylist()
        {
            while (true)
            {
                bgAudioSource.clip = bgMusic[_bgMusicIndex];
                bgAudioSource.Play();
                yield return new WaitForSeconds(bgAudioSource.clip.length);
                _bgMusicIndex++;
                if (_bgMusicIndex >= bgMusic.Length) _bgMusicIndex = 0;
            }
        }
        
        private IEnumerator AmbientAudioPlaylist()
        {
            while (true)
            {
                ambientAudioSource.clip = ambientAudio[_ambientAudioIndex];
                ambientAudioSource.Play();
                yield return new WaitForSeconds(ambientAudioSource.clip.length);
                _ambientAudioIndex++;
                if (_ambientAudioIndex >= ambientAudio.Length) _ambientAudioIndex = 0;
            }
        }
    }
}
