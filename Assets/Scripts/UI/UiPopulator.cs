using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiPopulator : MonoBehaviour
{
    public TextMeshProUGUI Blue,Red;

    private void Awake()
    {
        GameManager.Instance.Blue = Blue;
        GameManager.Instance.Red = Red;
    }
}
