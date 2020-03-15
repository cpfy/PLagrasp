using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HumanState
{
    GoNone,
    GoHome,
    GoVirus,
    Attack
}

public class Human : MonoBehaviour
{
    private Animator animator;
    private Vector3 moveDirection;
    private Vector3 curenPosition;
    public float speed;

    public Transform target;
    private HumanState humanState = HumanState.GoVirus;

    public float attackpower;
    int sample;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateTarget();
        MoveByTransformTranslate();
    }

    private void Attack()
    {
        target.GetComponent<Virus>().BeAttack(attackpower*GameManager.instance.attackScale);
    }
    public void GoHome(int num)
    {
        sample = num;
        humanState = HumanState.GoHome;
        animator.SetBool("gohome", true);
    }
    public void GoVirus()
    {
        animator.SetBool("attack", false);
        humanState = HumanState.GoVirus;
    }

    public void UpdateTarget()
    {
        switch (humanState)
        {
            case HumanState.GoNone:
                target.position = new Vector3(Random.Range(0, 100), Random.Range(0, 100), 0);
                break;
            case HumanState.GoHome:
                target = GameManager.instance.humanHome.transform;
                break;
            case HumanState.GoVirus:
                target = GameManager.instance.GetHumanTarget(transform);
                break;
            case HumanState.Attack:
                break;
        }
    }

    private void MoveByTransformTranslate()
    {
        //Debug.Log(target.position);
        curenPosition = this.transform.position;
        moveDirection = target.position - curenPosition;
        moveDirection.Normalize();

        if (curenPosition.x > target.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (curenPosition.x < target.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (Mathf.Abs(curenPosition.x - target.position.x) < 1 && Mathf.Abs(curenPosition.y - target.position.y) < 0.5)
        {
            if (target.tag == "Tree" && humanState == HumanState.GoVirus)
            {
                humanState = HumanState.Attack;
                animator.SetBool("attack", true);
                target.GetComponent<Virus>().AddAttacker(gameObject);
            }
            else if (target.tag == "Home" && humanState == HumanState.GoHome)
            {
                GameManager.instance.AddLab(sample);
                sample = 0;
                animator.SetBool("gohome", false);
                humanState = HumanState.GoVirus;
            }
        }
        else
        {
            transform.Translate(moveDirection * speed * GameManager.instance.speedScale * Time.deltaTime);
        }
    }

}
