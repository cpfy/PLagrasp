using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextManager : MonoBehaviour
{
    public static UITextManager instance;
    AbilityType abilityType;

    public GameObject walk;
    public GameObject train;
    public GameObject plane;
    public GameObject deal;
    public GameObject dinner;
    public GameObject shut;
    public GameObject mask;
    public GameObject bat;
    public GameObject fight;
    public GameObject rebel;
    public GameObject tired;

    private void Awake()
    {
        instance = this;
    }

    public void ShowTextGrid(AbilityType abilityType)
    {
        this.abilityType = abilityType;
        switch(abilityType)
        {
            case AbilityType.Walk:
                walk.SetActive(true);
                break;
            case AbilityType.Train:
                train.SetActive(true);
                break;
            case AbilityType.Plane:
                plane.SetActive(true);
                break;
            case AbilityType.Deal:
                deal.SetActive(true);
                break;
            case AbilityType.Dinner:
                dinner.SetActive(true);
                break;
            case AbilityType.Shut:
                shut.SetActive(true);
                break;
            case AbilityType.Mask:
                mask.SetActive(true);
                break;
            case AbilityType.Bat:
                bat.SetActive(true);
                break;
            case AbilityType.Fight:
                fight.SetActive(true);
                break;
            case AbilityType.Rebel:
                rebel.SetActive(true);
                break;
            case AbilityType.Tired:
                tired.SetActive(true);
                break;
        }
    }

    public void HideTextGrid(AbilityType abilityType)
    {
        this.abilityType = abilityType;
        switch (abilityType)
        {
            case AbilityType.Walk:
                walk.SetActive(false);
                break;
            case AbilityType.Train:
                train.SetActive(false);
                break;
            case AbilityType.Plane:
                plane.SetActive(false);
                break;
            case AbilityType.Deal:
                deal.SetActive(false);
                break;
            case AbilityType.Dinner:
                dinner.SetActive(false);
                break;
            case AbilityType.Shut:
                shut.SetActive(false);
                break;
            case AbilityType.Mask:
                mask.SetActive(false);
                break;
            case AbilityType.Bat:
                bat.SetActive(false);
                break;
            case AbilityType.Fight:
                fight.SetActive(false);
                break;
            case AbilityType.Rebel:
                rebel.SetActive(false);
                break;
            case AbilityType.Tired:
                tired.SetActive(false);
                break;
        }
    }

}
