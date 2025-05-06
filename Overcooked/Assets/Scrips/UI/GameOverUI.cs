using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;

    private void Start() {
        GameManager.Instace.OnStateChanged += GameManagerOnStateChanged;
        
        Hide();
    }

    private void GameManagerOnStateChanged(object sender, EventArgs e) {
        if (GameManager.Instace.IsGameOver()) {
            recipesDeliveredText.text = DeliveryManager.Instance.GetRecipesDeliveredCounter().ToString();
            Show();
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