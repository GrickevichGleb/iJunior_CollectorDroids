using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterDisplay : MonoBehaviour
{
    [SerializeField] private MineralCounter _mineralCounter;
    [SerializeField] private TMP_Text _valueText;

    private void Start()
    {
        _mineralCounter.CounterUpdated += OnCounterUpdated;
    }

    private void OnCounterUpdated(int newValue)
    {
        _valueText.text = newValue.ToString();
    }
}
