using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSleep : MonoBehaviour
{
    [SerializeField] BossyAI boss;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(AwakeBoss());
        }
    }

    IEnumerator AwakeBoss()
    {
        yield return new WaitForSeconds(2f);
        boss.enabled = true;
        this.enabled = false;
    }
}
