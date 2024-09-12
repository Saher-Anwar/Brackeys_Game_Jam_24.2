using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] 
    [Range(0,10)]
    float destroyTime = 5f;

    void Start()
    {
        Destroy(gameObject, destroyTime);       
    }
}
