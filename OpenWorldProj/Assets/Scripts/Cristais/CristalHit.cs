using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalHit : MonoBehaviour
{
    Animator anim;
    [SerializeField] bool cristalAtivado;
    public MovingPlatform movingPlatform;
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerProjectile"))
        {
            if (!cristalAtivado)
            {
                anim.SetBool("Ativado", true);
                anim.SetBool("Desativado", false);
                cristalAtivado = true;
            }
            else
            {
                anim.SetBool("Desativado", true);
                cristalAtivado = false;
            }
            

        }
    }

    public void AtivarPlataforma()
    {
        movingPlatform.enabled = true;
    }
    public void DesativarPlataforma()
    {
        movingPlatform.enabled = false;
    }
}
