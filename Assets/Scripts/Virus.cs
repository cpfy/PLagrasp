using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    public float viability;
    public int wood;
    public ParticleSystem redEffect;
    private List<GameObject> attacker = new List<GameObject>();

    public bool isInvincible=false;
    public bool isHarder=false;

    public void BeAttack(float attackData)
    {
        if(!isInvincible)
        {
            if (viability > attackData) viability -= attackData;
            else if (!isHarder) Die();
        }
        else
        {
            HumanGone();
        }
    }

    public void AddAttacker(GameObject human)
    {
        attacker.Add(human);
    }

    private void HumanGone()
    {
        if (attacker.Count != 0)
        {
            foreach (GameObject item in attacker)
            {
                item.GetComponent<Human>().GoVirus();
            }
            attacker.Clear();
        }
    }

    private void Die()
    {
        GameManager.instance.LogOutVirus(gameObject);
        redEffect.Stop();

        if (attacker.Count != 0)
        {
            foreach (GameObject item in attacker)
            {
                item.GetComponent<Human>().GoVirus();
            }
            attacker[0].GetComponent<Human>().GoHome(wood);
            attacker.Clear();
        }
    }
    private void DestroyParticle()
    {
        Destroy(redEffect.gameObject);
    }



}
