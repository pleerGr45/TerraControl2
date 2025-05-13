using UnityEngine;
using TC_basic;
using TC_data;
using Unity.VisualScripting;
using System;
using TC_enemy;

public class GameExecutor : MonoBehaviour
{
    private GameFieldClass field;
    private Wave current_wave;
    private float calm_time = 30f;
    private float time;
    private bool status;
    private LevelInfo level_info;
    public bool load_status;

    [SerializeField] private GameObject[] cell_prefabs;
    [SerializeField] private GameObject[] building_prefabs;
    public static GameObject[][] cells_model;

    private void Awake()
    {
        load_status = false;
    }

    private void OnEnable() {
        Debug.Log(load_status);
        /* Primary stage */
        if(load_status) {
            enabled = false;
            load_status = false;
            Debug.LogError("Level not loaded");
            return;
        }
    }

    private void Start()
    {
        field = new GameFieldClass(transform, cell_prefabs, building_prefabs);
        /* Launch stage */
        cells_model = field.InitCells();
        
        InvokeRepeating("UpdateField", 0f, MainScript.UPDATE_FIELD_INTERVAL);
        InvokeRepeating("Update", 0f, 0.1f);
    }

    private void UpdateField() => field.UpdateCells();

    private void Update()  {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (status)
            if (current_wave.WaveTime <= time) {
                status = false;
                current_wave = null;
            }
        else
            if (calm_time <= time) {
                status = true;
                current_wave = CreateWave();
            }

        if (field.UpdateRules()) {
            CancelInvoke("Update");
            CancelInvoke("UpdateField");
            GameLose();
        }

        time += 0.1f;
    }

    private void GameLose() {
        
       
    }

    private Wave CreateWave() {
        return null;
    }


    public void LoadLevel(LevelInfo level_info) {
        this.level_info = level_info;
        this.field = level_info.field;
        this.load_status = true;
        Debug.Log("Level loaded");
    }


}