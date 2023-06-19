using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void Damage(float damage, WorldItem _Item, int tier, PlayerSpecifications specs);
}
