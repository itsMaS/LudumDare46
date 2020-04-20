using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject beePrefab;
    public bool spawning = true;
    public float interval = 1;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while(spawning)
        {
            Instantiate(beePrefab,transform.position+new Vector3(Random.Range(-20, 20),0),Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }
}
