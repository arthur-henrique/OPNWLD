using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGen : MonoBehaviour
{
    // Variáveis públicas para os tipos de projéteis e posições de lançamento.
    public Projectile[] projectiles;
    public Transform[] launchPos;

    // Variáveis de controle para geração de projéteis.
    public bool canGenProj = true;
    public float genCD = 1.5f;

    // Variáveis privadas para aleatoriedade.
    private int randomShooter;
    private int randomOrb;
    ObjectPooling objPooler;
    Quaternion randomRot;

    // Método chamado quando o objeto é iniciado.
    void Start()
    {
        // Obtém uma instância da classe de pooling de objetos.
        objPooler = ObjectPooling.Instance;
    }

    // Método chamado a cada frame fixo.
    void FixedUpdate()
    {
        // Se for possível gerar projéteis...
        if (canGenProj)
        {
            // Marca que não é mais possível gerar projéteis.
            canGenProj = false;

            // Escolhe aleatoriamente um lançador e um tipo de projétil.
            randomShooter = Random.Range(0, launchPos.Length);
            randomOrb = Random.Range(0, projectiles.Length);
            // Escolhe uma rotação aleatória para o projétil.
            randomRot = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));

            // Inicia a corrotina de queda do projétil.
            StartCoroutine(OrbFalling());
        }
    }

    // Corrotina de queda do projétil.
    IEnumerator OrbFalling()
    {
        // Para cada posição de lançamento...
        for (int i = 0; i < launchPos.Length; i++)
        {
            // Se esta é a posição escolhida aleatoriamente para lançar o projétil...
            if (i == randomShooter)
            {
                // Escolhe o tipo de projétil e o lança.
                if (randomOrb == 0)
                    objPooler.spawnFromPool("PROJECTILE_S", launchPos[randomShooter].transform.position, randomRot);
                if (randomOrb == 1)
                    objPooler.spawnFromPool("PROJECTILE_M", launchPos[randomShooter].transform.position, randomRot);
                if (randomOrb == 2)
                    objPooler.spawnFromPool("PROJECTILE_L", launchPos[randomShooter].transform.position, randomRot);

                // Espera um tempo antes de continuar a geração de projéteis.
                yield return new WaitForSeconds(genCD);
            }
        }

        // Marca que é possível gerar mais projéteis.
        canGenProj = true;
    }
}
