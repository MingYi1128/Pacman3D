using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance) return _instance;
            _instance = FindObjectOfType<GameManager>();
            if (_instance) return _instance;
            else throw new ApplicationException("");
        }
    }

    [Header("Parameters")]
    [SerializeField]
    private GameObject _playerSpawn;
    [SerializeField]
    private GameObject _ghostSpawn;

    [Header("Prefabs")] 
    [SerializeField]
    private EntityGhost _ghostPrefab;
    
    
    [Header("Entity")]
    [SerializeField]
    private EntityPlayer _player;
    [SerializeField]
    private List<EntityGhost> _ghosts;
    
    
    private PelletGenerator _pelletGenerator;


    public EntityPlayer GetPlayer()
    {
        return _player;
    }


    public void Restart()
    {
        _pelletGenerator.GeneratePellets();
        _player.Reset();
        _player.PlayerTransform.position = new Vector3(_playerSpawn.transform.position.x, 0, _playerSpawn.transform.position.z);
        foreach (EntityGhost ghost in _ghosts)
        {
            Destroy(ghost.gameObject);
        }
        _ghosts.Clear();

        for (int i = 0; i < 4; i++)
        {
            var ghost = Instantiate(_ghostPrefab);
            ghost.transform.position = _ghostSpawn.transform.position;
            _ghosts.Add(ghost);
        }
        
    }

    private void Awake()
    {
        _pelletGenerator = GetComponent<PelletGenerator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
