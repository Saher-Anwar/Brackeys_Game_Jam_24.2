using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTable : MonoBehaviour {

    private Transform entryContainer;
    private Transform entryTemplate;

    void Awake() {
        entryContainer = transform.Find("EntryContainer");
        entryTemplate = entryContainer.Find("EntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        float templateHeight = 30f;
        for (int i = 0; i < 5; i++) {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);

            int serial = i + 1;
            string serialString = serial.ToString();
            //entryTransform.Find("Serial").GetComponent<Text>().text = serialString;
            //entryTransform.Find("Name").GetComponent<Text>().text = Random.Range(0, 100).ToString();
            //entryTransform.Find("Score").GetComponent<Text>().text = "AAA";
        }
    }

    void Update() {
        
    }
}
