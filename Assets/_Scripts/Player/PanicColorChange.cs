using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PanicColorChange : MonoBehaviour
{
    [SerializeField] Light2D light2D;
    [SerializeField] Color calmColor;
    [SerializeField] Color panicColor;
    [SerializeField] float maxIntensity;
    [SerializeField] float maxOuterRadius;
    [SerializeField] float maxInnerRadius;

    PanicBar panicBar;

    private void Awake() {
        light2D = GetComponent<Light2D>();
    }

    private void Start() {
        panicBar = SpawnManager.Instance.panicBar;
    }

    void Update()
    {
        float currFillAmt = panicBar.panicBarFill.fillAmount;
        panicBar.panicBarFill.color = Color.Lerp(calmColor, panicColor, currFillAmt);
        light2D.color = Color.Lerp(calmColor, panicColor, currFillAmt);
        light2D.intensity = Mathf.Lerp(maxIntensity, 0, currFillAmt);
        light2D.pointLightOuterRadius = Mathf.Lerp(maxOuterRadius, 0, currFillAmt);
        light2D.pointLightInnerRadius = Mathf.Lerp(maxInnerRadius, 0, currFillAmt);
    }
}
