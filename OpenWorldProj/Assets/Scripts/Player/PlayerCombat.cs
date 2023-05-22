using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public List<AttackSO> combo;
    Animator anim;
    public float lastClickedTime;
    public float lastComboEnd;
    public int comboCounter;

    //[SerializeField] Weapon weapon;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        ExitAttack();
    }
    public void Attack()
    {
        if(Time.time - lastComboEnd > 0.5f && comboCounter <= combo.Count)
        {
            CancelInvoke("EndCombo");
            if(Time.time - lastClickedTime >= combo[comboCounter].minTime)
            {
                anim.runtimeAnimatorController = combo[comboCounter].animOC;
                anim.Play("Attack", 0, 0);
                //weapon.damag = combo[comboCounter].damage;
                comboCounter++;
                lastClickedTime = Time.time;
                if (comboCounter >= combo.Count)
                {
                    comboCounter = 0;
                }

            }
        }
    }
    public void ExitAttack()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 1);
        }

        if (comboCounter > combo.Count)
        {
            comboCounter = 0;
        }
    }
    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
    }
}
