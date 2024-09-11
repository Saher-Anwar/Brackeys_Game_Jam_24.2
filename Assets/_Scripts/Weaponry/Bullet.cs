using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] 
    [Range(0,100)]
    float destroyTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);       
    }
}
