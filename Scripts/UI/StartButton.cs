using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Button button;
    public TMPro.TextMeshProUGUI text;

    private void Awake() => button.interactable = false;

    private void Start() => EventManager.instance.OnDataLoaded += Activate;

    private void OnDisable() => EventManager.instance.OnDataLoaded -= Activate;

    private void Activate()
    {
        button.interactable = true;
        text.text = "START";
    }
}
