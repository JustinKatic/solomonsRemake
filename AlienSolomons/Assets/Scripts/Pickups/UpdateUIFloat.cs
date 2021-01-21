﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UpdateUIFloat : MonoBehaviour
{
    [SerializeField] Text TextToUpdate;
    [SerializeField] FloatVariable FloatNeedingUpdated;

    private void Start()
    {
        UpdateText(FloatNeedingUpdated);
    }

    public void UpdateText(FloatVariable floatVariable)
    {
        TextToUpdate.text = floatVariable.Value.ToString();
    }
}
