using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {

    [SerializeField] private Image barImage;
    [SerializeField] private GameObject hasProgressGameObject;
    
    private IHasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

        if (hasProgress == null) {
            Debug.LogError("GameObject " + hasProgressGameObject + " does not have a component IHasProgress!");
        }
        
        hasProgress.OnProgressChanged += HasProgressOnProgressChanged;

        barImage.fillAmount = 0;
        
        Hide();
    }

    private void HasProgressOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0f || e.progressNormalized == 1f) {
            Hide();
        }
        else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    
    private void Hide() {
        gameObject.SetActive(false);
    }
}
