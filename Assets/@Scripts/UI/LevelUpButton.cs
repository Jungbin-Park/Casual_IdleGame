using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class LevelUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image expSlider;
    [SerializeField] TextMeshProUGUI expText, atkText, goldText, hpText, getExpText;
    bool isPush = false;
    float timer = 0.0f;
    Coroutine coroutine;

    private void Start()
    {
        InitExp();
    }

    private void Update()
    {
        if (isPush)
        {
            timer += Time.deltaTime;
            if (timer >= 0.01f)
            {
                timer = 0.0f;
                EXPUp();
            }
        }
    }

    public void EXPUp()
    {
        Managers.Player.ExpUp();
        InitExp();
        transform.DORewind();
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EXPUp();

        coroutine = StartCoroutine(CoPush());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPush = false;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        timer = 0.0f;
    }

    private void InitExp()
    {
        expSlider.fillAmount = Managers.Player.ExpPercentage();
        expText.text = string.Format("{0:0.00}", Managers.Player.ExpPercentage() * 100.0f) + "%";
        atkText.text = "+" + StringMethod.ToCurrencyString(Managers.Player.NextAtk());
        hpText.text = "+" + StringMethod.ToCurrencyString(Managers.Player.NextHp());
        getExpText.text = "<color=#00FF00>EXP</color> +" + string.Format("{0:0.00}", Managers.Player.NextExp()) + "%";
    }

    IEnumerator CoPush()
    {
        yield return new WaitForSeconds(1.0f);
        isPush = true;
    }
}
