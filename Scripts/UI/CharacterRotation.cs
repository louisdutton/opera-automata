using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Lean.Gui;

public class CharacterRotation : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 1f;
    public float deceleration = .1f;

    private float angularVelocity = 0;

    private void Update()
    {
        if (Input.GetMouseButton(0)) angularVelocity = Input.GetAxis("Mouse X") * rotationSpeed;
        else
        {
            angularVelocity *= deceleration;
            if (Mathf.Abs(angularVelocity) < 0.1f) angularVelocity = 0;
        }
        
        target.Rotate(Vector3.up, angularVelocity);
    }
}
