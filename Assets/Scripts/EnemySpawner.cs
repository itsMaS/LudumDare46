using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    AnimationCurve intervalPerLevel;
    [SerializeField]
    AnimationCurve countPerLevel;

    [SerializeField]
    GameObject beePrefab;
    public bool spawning = true;
    public float radius = 20;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while(spawning)
        {
            for (int i = 0; i < countPerLevel.Evaluate(GameManager.level); i++)
            {
                float angle = Mathf.PI * 2f * Random.value;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
                Instantiate(beePrefab, newPos, Quaternion.identity);
            }
            yield return new WaitForSeconds(intervalPerLevel.Evaluate(GameManager.level));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
