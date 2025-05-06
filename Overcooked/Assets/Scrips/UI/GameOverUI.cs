using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button mainMenuButton;


    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start() {
        GameManager.Instace.OnStateChanged += GameManagerOnStateChanged;
        
        Hide();
    }

    private void GameManagerOnStateChanged(object sender, EventArgs e) {
        if (GameManager.Instace.IsGameOver()) {
            recipesDeliveredText.text = DeliveryManager.Instance.GetRecipesDeliveredCounter().ToString();
            Show();
            mainMenuButton.Select();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}