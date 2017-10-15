using UnityEngine;

namespace RPG.Core
{

    public class AudioTrigger : MonoBehaviour
    {
        [SerializeField] AudioClip clip;
        [SerializeField] int layerFilter = 0;
        [SerializeField] float playerDistanceThreshhold = 5f;
        [SerializeField] bool isOneTimeOnly = true;
        [SerializeField] float volume = 1f;
        [SerializeField] bool stopCurrentTrack = false;
        //
        bool hasPlayed = false;
        AudioSource audioSource;
        GameObject player;
        private AudioSource[] allAudioSources;

        void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = clip;

            player = GameObject.FindWithTag("Player");
        }

        void StopAllAudio()
        {
            allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            foreach (AudioSource audioS in allAudioSources)
            {
                audioS.Stop();
            }
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= playerDistanceThreshhold)
            {
                RequestPlayAudioClip();
            }
        }

        void RequestPlayAudioClip()
        {

            if (isOneTimeOnly && hasPlayed)
            {
                return;
            }
            else if (audioSource.isPlaying == false)
            {
                if (stopCurrentTrack)
                {
                    StopAllAudio();
                }
                audioSource.volume = volume;
                audioSource.Play();
                hasPlayed = true;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 255f, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, playerDistanceThreshhold);
        }
    }
}