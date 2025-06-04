using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float growDuration = 0.7f; // slower grow
public float lifetime = 2f; 
    public Vector3 initialScale = Vector3.one;
    public Vector3 targetScale = Vector3.one * 10f;

    public void SetText(string content, Color color)
    {
        text.text = content;
        text.color = color;
        text.fontSize = 72; 
        transform.localScale = initialScale;
        StartCoroutine(AnimateText());
    }

    private System.Collections.IEnumerator AnimateText()
    {
        float elapsed = 0f;

        // Scale up
        while (elapsed < growDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / growDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
// Fade out
elapsed = 0f;
float fadeDuration = 0.5f;
Color originalColor = text.color;

while (elapsed < fadeDuration)
{
    float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
    text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    elapsed += Time.deltaTime;
    yield return null;
}
        // Wait before destroying
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
