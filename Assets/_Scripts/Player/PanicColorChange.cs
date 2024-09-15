using System.Collections;
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
    [SerializeField] float flickerDuration = 0.1f;
    [SerializeField] float flickerInterval = 1f;
    [SerializeField, Range(0, 1)] float flickerIntensityFactor = 0.5f; // Percentage reduction during flicker

    PanicBar panicBar;
    private bool isFlickering = false;
    private float baseIntensity;

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

        // Gradually decrease intensity based on panic bar fill amount
        baseIntensity = Mathf.Lerp(maxIntensity, 0, currFillAmt);

        if (!isFlickering) // Only apply gradual changes if not flickering
        {
            light2D.intensity = baseIntensity;
        }

        // Adjust the outer and inner radius based on the panic bar fill amount
        light2D.pointLightOuterRadius = Mathf.Lerp(maxOuterRadius, 0, currFillAmt);
        light2D.pointLightInnerRadius = Mathf.Lerp(maxInnerRadius, 0, currFillAmt);
    }

    public void InitiateFlickering(){
        if (!isFlickering) // Prevent multiple flicker coroutines from starting
        {
            isFlickering = true;
            StartCoroutine(Flicker());
        }
    }

    public void increaseFlickerInterval(float increaseAmount){
        flickerInterval /= increaseAmount;
    }

    public void decreaseFlickerInterval(float decreaseAmount){
        flickerInterval *= decreaseAmount;
    }

    public void StopFlickering(){
        if (isFlickering) // Only stop if flickering is active
        {
            isFlickering = false;
            StopCoroutine(Flicker());
            // Reset light intensity to the base intensity based on the panic bar
            light2D.intensity = baseIntensity;
        }
    }

    IEnumerator Flicker(){
        while(isFlickering)
        {
            // Flicker by reducing the current intensity by a factor
            float flickerIntensity = baseIntensity * flickerIntensityFactor;
            light2D.intensity = flickerIntensity;

            yield return new WaitForSeconds(flickerDuration);

            // Restore to the base intensity after the flicker
            light2D.intensity = baseIntensity;

            yield return new WaitForSeconds(flickerInterval);
        }
    }
}
