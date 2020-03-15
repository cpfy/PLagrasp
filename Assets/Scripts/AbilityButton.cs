using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public AbilityType abilityType;

    public Image CDImage;
    public float CDTime;
    private bool isCDOver=true;

    public Image DuringImage;
    public bool hasDuringTime;
    public float duringTime;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ThisButtonDown);
    }

    public void PointerEnter()
    {
        UITextManager.instance.ShowTextGrid(abilityType);
    }
    public void PointerExit()
    {
        UITextManager.instance.HideTextGrid(abilityType);
    }

    public void StartCD()
    {
        if(hasDuringTime)
        {
            StartCoroutine(During());
        }
        else
        {
            StartCoroutine(CD());
        }
    }
    IEnumerator CD()
    {
        isCDOver = false;
        CDImage.fillAmount = 1;
        while (CDImage.fillAmount > 0)
        {
            yield return new WaitForSeconds(0.1f);
            CDImage.fillAmount -= 1 / (CDTime / 0.1f);
        }
        isCDOver = true;
    }

    private void ThisButtonDown()
    {
        if (isCDOver)
        {
            GameManager.instance.SetAbility(abilityType,duringTime);
        }
    }

    IEnumerator During()
    {
        isCDOver = false;
        while (DuringImage.fillAmount < 1)
        {
            yield return new WaitForSeconds(0.1f);
            DuringImage.fillAmount += 1 / (duringTime / 0.1f);
        }
        DuringImage.fillAmount = 0;
        StartCoroutine(CD());
    }

}