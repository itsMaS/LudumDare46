using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damagable
{
    void DealDamage(float amount, Vector3 direction);
}
