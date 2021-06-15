using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIndicator : MonoBehaviour
{
    void Start()
    {
        EventManager.instance.OnDataLoaded += Deactivate;
    }

    private void OnDisable()
    {
        EventManager.instance.OnDataLoaded -= Deactivate;
    }

    private void Deactivate() => gameObject.SetActive(false);
}
