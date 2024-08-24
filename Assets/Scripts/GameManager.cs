using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using DungeonCrawler.Assets.Scripts.BossBattle;
using DungeonCrawler.Assets.Scripts;
using System;



public class GameManager : MonoBehaviour
{


    // Singletoon
    public static GameManager Instance { get; private set; }

    [HideInInspector] public bool isGameOver;

    // Interaction
    public List<Interaction> interactionList;

    // Rendering
    [Header("Rendering")]
    public Camera worldUiCamera;

    // Physics
    [Header("Physics")]
    [SerializeField] public LayerMask groundLayer;

    // Character
    [Header("Character")]
    public GameObject player;

    // Inventory
    [Header("Inventory")]
    public int keys;
    public bool hasBossKey;


    // Music
    [Header("Music")]
    public AudioSource gameplayMusic;
    public AudioSource bossMusic;
    public AudioSource ambienceMusic;


    // Boss
    [Header("Boss")]
    public GameObject boss;
    public GameObject bossBattleParts;
    public BossBattleHandler bossBattleHandler;
    public GameObject bossDeathSequence;

    // UI
    [Header("UI")]
    public GameplayUI gameplayUI;


    
    
    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    private void Start() {

        // Boss Battle handler
        bossBattleHandler = new BossBattleHandler();

        // Play music
        var musicTargetVolume = gameplayMusic.volume;
        gameplayMusic.volume = 0;
        gameplayMusic.Play();
        StartCoroutine(FadeAudioSource.StartFade(gameplayMusic, musicTargetVolume, 1f));

        // Play ambience
        var ambienceTargetVolume = ambienceMusic.volume;
        ambienceMusic.volume = 0;
        ambienceMusic.Play();
        StartCoroutine(FadeAudioSource.StartFade(ambienceMusic, ambienceTargetVolume, 1f));

        // Listen to OnGameOver
        GlobalEvent.Instance.OnGameOver += (sender, args) => isGameOver = true;
      
    }


    void Update() {
        bossBattleHandler.Update();

    }

    

}
