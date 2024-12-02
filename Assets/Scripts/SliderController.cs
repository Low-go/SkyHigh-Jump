using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Include this if using TextMeshPro

public class SliderController : MonoBehaviour
{
    public Slider healthSlider;     // Drag and drop your slider in the Inspector
    public TextMeshProUGUI healthNumber;  // Use TextMeshProUGUI for better text rendering
    // Or use "public Text healthText;" if you're using the default UI Text

    private void Start()
    {
        // Initialize the text with the slider's current value
        UpdateHealthText();

        // Add a listener to update the text whenever the slider value changes
        healthSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        MainManager.Instance.SetPlayerHealth(value);  // Update MainManager's health
        UpdateHealthText();  // Update the text display
    }

    private void UpdateHealthText()
    {
        // Update the text to show the current slider value
        healthNumber.text = healthSlider.value.ToString("0");
    }
}
