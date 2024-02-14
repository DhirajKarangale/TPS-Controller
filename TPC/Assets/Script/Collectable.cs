using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] float speed;

    private Transform player;
    private Vector3 playerPos;
    private bool isMove;


    private void Start()
    {
        player = GameManager.instance.player.transform;
    }

    private void Update()
    {
        if (isMove) Move();
        else if (!rigidBody.useGravity) rigidBody.useGravity = true;
    }


    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isMove = true;
            if (Mathf.Abs(Vector3.Distance(transform.position, playerPos)) < 1f) Collect();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player")) isMove = false;
    }


    private void Move()
    {
        playerPos = player.position;
        playerPos.y += 1;
        transform.position = Vector3.Lerp(transform.position, playerPos, speed * Time.deltaTime);
        if (rigidBody.useGravity) rigidBody.useGravity = false;
    }

    private void Collect()
    {
        isMove = false;
        GameManager.instance.effects.Collect(transform.position);
        GameManager.instance.collectableData.UpdateGems(1);
        gameObject.SetActive(false);
    }
}