using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Normal Attacks")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController animOC;
    public float damage;
    public float minTime;
}
