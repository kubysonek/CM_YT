using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour {
    
    public static GamePauseUI Instance { get; private set; }

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;

    private void Awake() {
        Instance = this;
        
        resumeButton.onClick.AddListener(() => {
            GameManager.Instace.TogglePauseGame();
        });
        
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        
        optionsButton.onClick.AddListener(() => {
            OptionsUI.Instance.Show();
            Hide();
        });
    }

    private void Start() {
        GameManager.Instace.OnGamePaused += GameManagerOnGamePaused;
        GameManager.Instace.OnGameUnpaused += GameManagerOnGameUnpaused;
        
        Hide();
    }

    private void GameManagerOnGameUnpaused(object sender, EventArgs e) {
        Hide();
    }

    private void GameManagerOnGamePaused(object sender, EventArgs e) {
        Show();
    }

    public void Show() {
        gameObject.SetActive(true);
        
        resumeButton.Select();
    }
    
    public void Hide() {
        gameObject.SetActive(false);
    }
}
