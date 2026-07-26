using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 5f;

    private float timer;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        Vector3 previousPosition = transform.position;
        Vector3 movement = transform.forward * speed * Time.deltaTime;
        Vector3 nextPosition = previousPosition + movement;


        Vector3 rayStart = previousPosition + transform.forward * 0.1f;

        if (Physics.Raycast(rayStart, movement.normalized, out RaycastHit hit, movement.magnitude))
        {
            Debug.Log($"[Arrow] Hit: {hit.collider.gameObject.name}, has IDamageable: {hit.collider.TryGetComponent<IDamageable>(out _)}");

            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(damage);
            }

            ReturnToPool();
            return;
        }

        transform.position = nextPosition;

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        ArrowPool.Instance.ReturnArrow(this);
    }
}