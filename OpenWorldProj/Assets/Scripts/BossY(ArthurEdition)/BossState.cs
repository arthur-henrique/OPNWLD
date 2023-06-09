using System.Collections;
using UnityEngine;
public abstract class BossState
{
    protected BossyAI bossAI;

    public BossState(BossyAI bossAI)
    {
        this.bossAI = bossAI;
    }

    public abstract void UpdateState();
}

