using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI startCountdownText;

    private void Start() {
        GameManager.Instace.OnStateChanged += GameManagerOnStateChanged;
        
        Hide();
    }

    private void Update() {
        startCountdownText.text = Mathf.Ceil(GameManager.Instace.GetCountdownToStartTimer()).ToString();
    }

    private void GameManagerOnStateChanged(object sender, EventArgs e) {
        if (GameManager.Instace.IsCountdownToStart()) {
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
