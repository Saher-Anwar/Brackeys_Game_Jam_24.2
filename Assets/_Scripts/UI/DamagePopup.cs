using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour {

    [Header("Popup References")]
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private GameObject prefabDamagePopup;

    [Header("Popup Fade Settings")]
    [SerializeField] private float moveYSpeed = 1f;
    [SerializeField] private float disappearTimer = 1f;
    [SerializeField] private float disappearSpeed = 3f;

    private Color color;

    // Function to create a damage popup at a position
    public static DamagePopup Create(Vector3 position, GameObject prefabDamagePopup, float damageAmount) {

        Transform damagePopupTransform = Instantiate(prefabDamagePopup, position, Quaternion.identity).transform;

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount);
        
        return damagePopup;
    }

    void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    void Setup(float damageAmount) {
        textMesh.SetText(damageAmount.ToString());
        color = textMesh.color;
    }

    // Move popup effect
    void Update() {

        // Move up slowly
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        // Start disappearing
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0) {
            color.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = color;
            // Destroy popup when invisible
            if (color.a < 0) {
                Destroy(gameObject);
            }
        }

    }
}
