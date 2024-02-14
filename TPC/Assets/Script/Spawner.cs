using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] int cnt;
    [SerializeField] GameObject prefab;

    [Header("Walls")]
    [SerializeField] Transform front;
    [SerializeField] Transform back;
    [SerializeField] Transform left;
    [SerializeField] Transform right;


    private void Start()
    {
        Spawn();
    }


    private void Spawn()
    {
        for (int i = 0; i < cnt; i++)
        {
            GameObject gem = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            gem.transform.SetParent(transform);
            gem.transform.position = new Vector3(Random.Range(left.position.x, right.position.x), 20, Random.Range(back.position.z, front.position.z));
        }
    }
}