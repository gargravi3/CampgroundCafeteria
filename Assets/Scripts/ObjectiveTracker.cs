using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveTracker : MonoBehaviour
{
    public GameObject bearObject;
    private TextMeshProUGUI objText;
    const string display = "{0} / {1}";
    private FoodCollector collector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objText = GetComponent<TextMeshProUGUI>();
        collector = bearObject.GetComponent<FoodCollector>();
    }

    // Update is called once per frame
    void Update()
    {
        objText.text = string.Format(display, collector.foodCollected, '1');
    }
}
