using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float obstacleSpeed;

    // Update is called once per frame
    void Update()
    {
        MoveObstacle();
        DestroyObstacle();
    }

    /// <summary>
    /// 장애물 이동 메서드.
    /// </summary>
    private void MoveObstacle()
    {
        transform.Translate(Vector3.left * obstacleSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 장애물 파괴 메서드.
    /// </summary>
    private void DestroyObstacle()
    {
        if (transform.position.x < -6)
        {
            PoolingManager.ReturnObject(this);
        }
    }
}
