using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;
    public float moveSpeed = 3f;
    public int goldDrop = 10;

    public int pathIndex = 0;

    private int wayPointIndex = 0;

    void Start()
    {
        EnemyManager.Instance.RegisterEnemy(this);
    }
    void OnGotToLastWayPoint()
    {
        Die();
    }

    public void TakeDamage(float amountOfDamage)
    {
        health -= amountOfDamage;

        if (health <= 0)
        {
            DropGold();
            Die();
        }
    }

    void DropGold()
    {
        GameManager.Instance.gold += goldDrop;
    }


    void Die()
    {
        if (gameObject != null)
        {
            EnemyManager.Instance.UnRegister(this);
            //2
            gameObject.AddComponent<AutoScaler>().scaleSpeed = -2;
            //3
            enabled = false;
            //4
            Destroy(gameObject, 0.3f);
        }
    }
    

    void Update()
    {
        
        if (wayPointIndex <
        WayPointManager.Instance.Paths[pathIndex].WayPoints.Count)
        {
            UpdateMovement();
        }
        else
        {
            OnGotToLastWayPoint();
        }
    }
    private void UpdateMovement()
    {
        
        Vector3 targetPosition =
        WayPointManager.Instance.Paths[pathIndex]
        .WayPoints[wayPointIndex].position;
        
        transform.position = Vector3.MoveTowards(
        transform.position, targetPosition,
        moveSpeed * Time.deltaTime);
        
        transform.localRotation = UtilityMethods.SmoothlyLook(transform, targetPosition);
        
        if (Vector3.Distance(transform.position, targetPosition) < .1f)
        {
            wayPointIndex++;
        }
    }
}
