using System.Collections;
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
        
        private int _bgMusicIndex;
        private int _ambientAudioIndex;
        
        private Coroutine _bgMusicCoroutine;
        private Coroutine _ambientAudioCoroutine;
        
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
            if (_bgMusicCoroutine != null)
                StopCoroutine(_bgMusicCoroutine);
            _bgMusicCoroutine = StartCoroutine(BGMusicPlaylist());
        }
        
        public void PlayAmbientAudio()
        {
            if (_ambientAudioCoroutine != null)
                StopCoroutine(_ambientAudioCoroutine);
            _ambientAudioCoroutine = StartCoroutine(AmbientAudioPlaylist());
        }
        
        private IEnumerator BGMusicPlaylist()
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
        
        public void StopBGMusic()
        {
            if (_bgMusicCoroutine != null)
            {
                StopCoroutine(_bgMusicCoroutine);
                _bgMusicCoroutine = null;
            }
            bgAudioSource.Stop();
        }
        
        public void StopAmbientAudio()
        {
            if (_ambientAudioCoroutine != null)
            {
                StopCoroutine(_ambientAudioCoroutine);
                _ambientAudioCoroutine = null;
            }
            ambientAudioSource.Stop();
        }
    }
}
