using System.Collections;
using UnityEngine;

public class Cado : MonoBehaviour {
    // The offset of the sprite to hide it.
    private Vector2 startPosition = new Vector2(0f, -2.56f);
    private Vector2 endPosition = Vector2.zero;
    // How long it takes to show a mole. 
    private float showDuration = 0.5f;
    private float duration = 1f;



private IEnumerator ShowHide(Vector2 start,  Vector2 end) {
    transform.localPosition = start;

    // Show the mole.
    float elapsed = 0f;
    while(elapsed < showDuration) {
        transform.localPosition = Vector2.Lerp(end, start, elapsed / showDuration);
        //Update at max framerate 
        elapsed += Time.deltaTime;
        yield return null;
    }

// Make sure we're exactly at the end. 
transform.localPosition = end;

// Wait for duration to pass. 
yield return new WaitForSeconds(duration);

// Hide the mole. 
elapsed = 0f;
while (elapsed < showDuration) {
    transform.localPosition = Vector2.Lerp(end, start, elapsed / showDuration);
    // Update at max framerate.
    elapsed += Time.deltaTime;
    yield return null;
}
// Make sure we're exactly back at the start position.
transform.localPosition = start;
}

private void Start() {
    StartCoroutine(ShowHide(startPosition, endPosition));
}

}