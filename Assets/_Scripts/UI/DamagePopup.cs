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

    [Header("Popup Scale Settings")]
    [SerializeField] private bool scaleEffect = true;
    [SerializeField] private float increaseScaleAmount = 0.5f;
    [SerializeField] private float decreaseScaleAmount = 0.5f;

    private const float disappearTimerMax = 1f;
    private static int sortingOrder;
    private Color color;
    private Vector3 initialPosition;

    // Function to create a damage popup at a position
    public static DamagePopup Create(Vector3 position, GameObject prefabDamagePopup, float damageAmount, Color textColor) {
        Transform damagePopupTransform = Instantiate(prefabDamagePopup, position, Quaternion.identity).transform;
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, textColor);
        return damagePopup;
    }

    void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    void Setup(float damageAmount, Color textColor) {
        // Set the text and reset timer
        textMesh.SetText(damageAmount.ToString());
        // Apply the color to the text mesh
        color = textColor;
        textMesh.color = textColor;
        // Reset the disappear timer
        disappearTimer = disappearTimerMax;
        // Set unique sorting order to ensure the popups render correctly
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        // Store the initial position to maintain the popup in place during scaling
        initialPosition = transform.position;
    }

    // Move popup effect
    void Update() {
        // Move up slowly
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        // Scale effect
        if (scaleEffect) {
            if (disappearTimer > disappearTimerMax * 0.5f) {
                // First half of popup lifetime
                transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
            } else {
                // Second half of popup lifetime
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }
        }
        
        // Fade effect
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
