using System.Collections.Generic;
using TC_basic;
using TC_data;
using TC_enemy;
using TC_func;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public const string VERSION = "0.0.1";
    public const float UPDATE_FIELD_INTERVAL = 1f;

    [SerializeField] private GameObject executor;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject level;
    [SerializeField] private GameObject settings;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private Canvas canvas;
    [SerializeField] private SpriteAtlas[] sprite_atlases;

    public Camera GameCamera { get => gameCamera; }
    public Canvas Canvas { get => canvas; }

    private void Awake()
    {
        menu.SetActive(true);
        level.SetActive(false);
        settings.SetActive(false);
        executor.SetActive(false);
        gameCamera.enabled = false;

        // Sprite init
        SpriteManager.Initializate(new string[] { "Icons" });

        Control.LoadSettings();
    }

    private void Start()
    {

    }


    private void Update()
    {
        
    }

    public void Play() {
        menu.SetActive(false);
        level.SetActive(false);
        settings.SetActive(false);
        gameCamera.enabled = true;

        LevelWaves waves = new LevelWaves(1);
        GameFieldClass test_field = new GameFieldClass();

        LevelInfo info = new LevelInfo(test_field, waves,"Test", 0);

        executor.GetComponent<GameExecutor>().LoadLevel(info);
        Debug.Log("Load Status: " + executor.GetComponent<GameExecutor>().load_status);
        executor.SetActive(true);
    }

    public void Settings() {

    }

    public void Exit() {
        Application.Quit();
    }


}
