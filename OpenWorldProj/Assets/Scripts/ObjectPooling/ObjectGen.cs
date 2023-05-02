using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGen : MonoBehaviour
{
    // Vari�veis p�blicas para os tipos de proj�teis e posi��es de lan�amento.
    public Projectile[] projectiles;
    public Transform[] launchPos;

    // Vari�veis de controle para gera��o de proj�teis.
    public bool canGenProj = true;
    public float genCD = 1.5f;

    // Vari�veis privadas para aleatoriedade.
    private int randomShooter;
    private int randomOrb;
    ObjectPooling objPooler;
    Quaternion randomRot;

    // M�todo chamado quando o objeto � iniciado.
    void Start()
    {
        // Obt�m uma inst�ncia da classe de pooling de objetos.
        objPooler = ObjectPooling.Instance;
    }

    // M�todo chamado a cada frame fixo.
    void FixedUpdate()
    {
        // Se for poss�vel gerar proj�teis...
        if (canGenProj)
        {
            // Marca que n�o � mais poss�vel gerar proj�teis.
            canGenProj = false;

            // Escolhe aleatoriamente um lan�ador e um tipo de proj�til.
            randomShooter = Random.Range(0, launchPos.Length);
            randomOrb = Random.Range(0, projectiles.Length);
            // Escolhe uma rota��o aleat�ria para o proj�til.
            randomRot = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));

            // Inicia a corrotina de queda do proj�til.
            StartCoroutine(OrbFalling());
        }
    }

    // Corrotina de queda do proj�til.
    IEnumerator OrbFalling()
    {
        // Para cada posi��o de lan�amento...
        for (int i = 0; i < launchPos.Length; i++)
        {
            // Se esta � a posi��o escolhida aleatoriamente para lan�ar o proj�til...
            if (i == randomShooter)
            {
                // Escolhe o tipo de proj�til e o lan�a.
                if (randomOrb == 0)
                    objPooler.spawnFromPool("PROJECTILE_S", launchPos[randomShooter].transform.position, randomRot);
                if (randomOrb == 1)
                    objPooler.spawnFromPool("PROJECTILE_M", launchPos[randomShooter].transform.position, randomRot);
                if (randomOrb == 2)
                    objPooler.spawnFromPool("PROJECTILE_L", launchPos[randomShooter].transform.position, randomRot);

                // Espera um tempo antes de continuar a gera��o de proj�teis.
                yield return new WaitForSeconds(genCD);
            }
        }

        // Marca que � poss�vel gerar mais proj�teis.
        canGenProj = true;
    }
}
