using System.Collections;
using UnityEngine;

public class ExplodingEnemy : Enemy
{
    public override IEnumerator Attack(float damage)
    {
        yield break;
    }
}
