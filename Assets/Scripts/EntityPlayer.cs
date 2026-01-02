using System;
using Oculus.Interaction.Locomotion;
using UnityEngine;
using CharacterController = Oculus.Interaction.Locomotion.CharacterController;

public class EntityPlayer : MonoBehaviour
{

    [Header("Parameters")]
    [SerializeField]
    private int defaultLifeCount = 3;
    
    public delegate void EventPlayerDamaged();
    public event EventPlayerDamaged OnPlayerDamaged;
    public delegate void EventPlayerDeath();
    public event EventPlayerDeath OnPlayerKilled;

    public delegate void EventPlayerEatPellet(EntityPellet pellet);
    public event EventPlayerEatPellet OnPlayerEatPellet;

    private CharacterController _characterController;
    private FirstPersonLocomotor _locomotor;
    
    private int _lifeRemaining;

    public int DefaultLifeCount => defaultLifeCount;
    public int LifeRemaining => _lifeRemaining
    ;
    private void Awake()
    {
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pellet"))
        {
            OnPlayerEatPellet.Invoke(other.GetComponent<EntityPellet>());
        }

        if (other.CompareTag("Enemy"))
        {
            lifeCount--;
            OnPlayerDamaged.Invoke();
            if (lifeCount == 0)
            {
                OnPlayerKilled.Invoke();
            }
        }
    }
}