using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ShopEntry
{
    ShopEntry()
    {
        Instance = this;
    }

    public static ShopEntry Instance;

    [SerializeField] public GameObject Structure;
    [SerializeField] public int Cost;
    [SerializeField] public TextMeshProUGUI PriceTag;
    [SerializeField] public GameObject PressedButton;
    [SerializeField] public GameObject RegularButton;
}
