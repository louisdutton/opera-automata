using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public event Action OnDataLoaded;
    public void DataLoaded() => OnDataLoaded?.Invoke();

    public event Action OnGenerate;
    public void Generate() => OnGenerate?.Invoke();

    public event Action<Singer> OnEntityHovered;
    public void EntityHovered(Singer s) => OnEntityHovered?.Invoke(s);

    public event Action<Singer> OnEntitySelected;
    public void EntitySelected(Singer s) => OnEntitySelected?.Invoke(s);

    private void Awake() => instance = this;
}
