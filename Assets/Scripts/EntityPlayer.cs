using System;
using System.Collections;
using Oculus.Interaction.Locomotion;
using UnityEngine;
using CharacterController = Oculus.Interaction.Locomotion.CharacterController;

public class EntityPlayer : MonoBehaviour
{

    [Header("Parameters")]
    [SerializeField]
    private int defaultLifeCount = 3;
    
    [Header("Audio")]
    [SerializeField]
    private AudioClip _deathAudioClip;
    [SerializeField]
    private AudioClip _beginningMusicClip;
    [SerializeField]
    private AudioClip _movingAudioClip;
    
    public delegate void EventPlayerDamaged();
    public event EventPlayerDamaged OnPlayerDamaged;
    public delegate void EventPlayerDeath();
    public event EventPlayerDeath OnPlayerKilled;
    public delegate void EventPlayerEatPellet(EntityPellet pellet);
    public event EventPlayerEatPellet OnPlayerEatPellet;

    public delegate void EventPlayerReady();
    public event EventPlayerReady OnPlayerReady;

    private AudioSource _audioSource;
    private CharacterController _characterController;
    private FirstPersonLocomotor _locomotor;
    
    private int _lifeRemaining;
    private Vector3 _lastPosition;
    
    public int DefaultLifeCount => defaultLifeCount;
    public int LifeRemaining => _lifeRemaining
    ;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _movingAudioClip;
        _audioSource.playOnAwake = false;
        _audioSource.loop = true;
        _audioSource.Stop();
        _locomotor = GetComponent<FirstPersonLocomotor>();
        _characterController = GetComponent<CharacterController>();
    }

    public void SetPosition(Vector3 pos)
    {
        _characterController.SetPosition(pos);
    }

    public void OnGameReset()
    {
        _locomotor.ResetPlayerToCharacter();
        _lifeRemaining = defaultLifeCount;
        StartCoroutine(BeginningCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pellet"))
        {
            OnPlayerEatPellet?.Invoke(other.GetComponent<EntityPellet>());
        }

        if (other.CompareTag("Enemy"))
        {
            _lifeRemaining--;
            OnPlayerDamaged?.Invoke();
            StartCoroutine(DeathCoroutine());
        }
    }

    private IEnumerator BeginningCoroutine()
    {
        _characterController.enabled = false;
        _audioSource.PlayOneShot(_beginningMusicClip);
        yield return new WaitForSeconds(2.0f);
        _characterController.enabled = true;
        OnPlayerReady?.Invoke();
    }

    private IEnumerator DeathCoroutine()
    {
        _characterController.enabled = false;
        _audioSource.PlayOneShot(_deathAudioClip);
        yield return new WaitForSeconds(2.0f); 
        _characterController.enabled = true;
        if (_lifeRemaining == 0)
        {
            OnPlayerKilled?.Invoke();
        }
        else
        {
            OnPlayerReady?.Invoke();
        }
    }

    void Update()
    {
        /*
        float currentSpeed = 0f;
        
        Vector3 displacement = _characterController.transform.position - _lastPosition;
        displacement.y = 0;
        currentSpeed = displacement.magnitude / Time.deltaTime;
        _lastPosition = transform.position;
        
        if (currentSpeed > 0.1)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.Stop();
        }
        */
    }
}