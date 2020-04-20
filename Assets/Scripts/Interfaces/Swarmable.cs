using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Swarmable
{
    void Swarm(Transform swarmer, float strength = 1);
}
