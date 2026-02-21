using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Main : MonoBehaviour
{
    public static UI_Main instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        TextCheck();
    }

    [SerializeField] private TextMeshProUGUI levelText;

    public void TextCheck()
    {
        levelText.text = "LV." + (Managers.Player.Level + 1).ToString();

    }
}
