using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class Spider : MonoBehaviour
{
    [SerializeField]
    LimbSolver2D ts;


    [System.Serializable]
    public struct Leg
    {
        public int slot;

        public Transform IK;
        public Vector3 restPosition;
        public bool move;
    }
    [SerializeField]
    float moveThreshold = 2;

    [SerializeField]
    Leg[] Legs;

    private void Awake()
    {
        target = transform.position;
        for (int i = 0; i < Legs.Length; i++)
        {
            Legs[i].restPosition = Legs[i].IK.position - transform.position;
            Legs[i].move = false;
        }
    }

    int currentSlot = 0;
    int slotCount = 2;
    Vector3 target;

    private void Update()
    {
        if(Vector3.Distance(target,transform.position) > moveThreshold)
        {
            currentSlot = (currentSlot + 1) % slotCount;
            Debug.Log(currentSlot);
            target = transform.position;
            for (int i = 0; i < Legs.Length; i++)
            {
                Leg item = Legs[i];
                if(currentSlot == item.slot)
                {
                    item.IK.position = item.restPosition + transform.position;
                }
            }
        }
    }
}
