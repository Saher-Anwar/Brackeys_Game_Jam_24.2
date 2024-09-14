using UnityEngine;

public class PanicColorChange : MonoBehaviour
{
    [SerializeField] Color calmColor;
    [SerializeField] Color panicColor;

    PanicBar panicBar;

    private void Start() {
        panicBar = SpawnManager.Instance.panicBar;
    }

    void Update()
    {
        panicBar.panicBarFill.color = Color.Lerp(calmColor, panicColor, panicBar.panicBarFill.fillAmount);
    }
}
