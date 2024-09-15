using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(0,10)]
    [SerializeField] float destroyTime = 5f;

    [SerializeField] float bulletDamage = 10f; 

    void Start()
    {
        Destroy(gameObject, destroyTime);       
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // if bullet hit the instantiating object, don't do anything
        if(other.gameObject.layer == this.gameObject.layer) return;
        if(other.gameObject.layer < 6 || other.gameObject.layer > 7) return;
        
        // 6 is layer mask for enemy. 
        if(other.gameObject.layer == 6){
            other.GetComponent<Enemy>()?.TakeDamage(bulletDamage);
            Destroy(gameObject);
        } else if(other.gameObject.layer == 7){
            other.GetComponent<Player>()?.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
