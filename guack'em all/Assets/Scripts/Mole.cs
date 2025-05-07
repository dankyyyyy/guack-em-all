using System.Collections;
using UnityEngine;

public class Mole : MonoBehaviour {
    [Header("Graphics")]
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite moleHardHat;
    [SerializeField] private Sprite moleHatBroken;
    [SerializeField] private Sprite moleHit;
    [SerializeField] private Sprite moleHatHit;

    [Header("GameManager")]
    [SerializeField] private GameManager gameManager;


    // The offset of the sprite to hide it.
    private Vector2 startPosition = new Vector2(0f, -1.56f);
    private Vector2 endPosition = Vector2.zero;
    // How long it takes to show a mole. 
    private float showDuration = 0.5f;
    private float duration = 1f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private Vector2 boxOffset;
    private Vector2 boxSize;
    private Vector2 boxOffsetHidden;
    private Vector2 boxSizeHidden;

    // Mole Parameters 
    private bool hittable = true; 
    public enum MoleType { Standard, HardHat, Bomb };
    private MoleType moleType;
    private float hardRate = 0.25f; //how often we want hardhat version of mole
    private float bombRate = 0f; // how often bombs appear
    private int lives; // hard hat mole has two lives 
    private int moleIndex = 0;



// Used by the game manager to uniquely identify moles. 
public void SetIndex(int index) {
    moleIndex = index;
}

private IEnumerator ShowHide(Vector2 start,  Vector2 end) {
    transform.localPosition = start;

    // Show the mole.
    float elapsed = 0f;
    while(elapsed < showDuration) {
        transform.localPosition = Vector2.Lerp(start, end, elapsed / showDuration);
        boxCollider2D.offset = Vector2.Lerp(boxOffsetHidden, boxOffset, elapsed / showDuration);
        boxCollider2D.size = Vector2.Lerp(boxSizeHidden, boxSize, elapsed / showDuration);
        //Update at max framerate 
        elapsed += Time.deltaTime;
        yield return null;
    }

// Make sure we're exactly at the end. 
transform.localPosition = end;
boxCollider2D.offset = boxOffset;
boxCollider2D.size = boxSize;


// if we got to the end and its still hittable then we missed it 
if (hittable) {
    hittable = false;
    // we only give time penalty if it isnt a bomb.
    gameManager.Missed(moleIndex, moleType != MoleType.Bomb);
}


// Wait for duration to pass. 
yield return new WaitForSeconds(duration);

// Hide the mole. 
elapsed = 0f;
while (elapsed < showDuration) {
    transform.localPosition = Vector2.Lerp(end, start, elapsed / showDuration);
    boxCollider2D.offset = Vector2.Lerp(boxOffset, boxOffsetHidden, elapsed / showDuration);
    boxCollider2D.size = Vector2.Lerp(boxSize, boxSizeHidden, elapsed / showDuration);
    // Update at max framerate.
    elapsed += Time.deltaTime;
    yield return null;
}
// Make sure we're exactly back at the start position.
transform.localPosition = start;
boxCollider2D.offset = boxOffsetHidden;
boxCollider2D.size = boxSizeHidden;

}

public void Activate(int level) {
    Setlevel(level);
    CreateNext();
    StartCoroutine(ShowHide(startPosition, endPosition));
}



private void CreateNext () {                       // Decides what mole to make
    float random = Random.Range(0f, 1f);
    if (random < bombRate) {
        // make a bomb
        moleType = MoleType.Bomb;
        // the animator handles setting the sprite. 
        animator.enabled = true;
    } else {
    animator.enabled = false;
    random = Random.Range(0f, 1f);
    if (random < hardRate) {
        // create a hard hat mole 
        moleType = MoleType.HardHat;
        spriteRenderer.sprite = moleHardHat;
        lives = 2; // lives or hits of the hard hat mole
    } else {
        // Create a standard one 
        moleType = MoleType.Standard;
        spriteRenderer.sprite = mole;
        lives = 1; // lives of standard mole 
    }
    }
    // Mark as hittable so we can register an onclick event 
    hittable = true;
}

private void Awake(){
    // Get references to the components we'll need.
    spriteRenderer = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
    boxCollider2D = GetComponent<BoxCollider2D>();
    // Work out collider values. 
    boxOffset = boxCollider2D.offset;
    boxSize = boxCollider2D.size;
    boxOffsetHidden = new Vector2(boxOffset.x, (startPosition.y + endPosition.y) / 2f); // position of hiding box collider
    boxSizeHidden = new Vector2(boxSize.x, 0f);  // size of hidden box collider 
}

private void OnMouseDown()    {
    if (hittable) {
        switch (moleType) {
                case MoleType.Standard:
                spriteRenderer.sprite = moleHit;
                gameManager.AddScore(moleIndex);

                // Turn off hittable so that we can't keep tapping the score. 
                hittable = false;

                // Stop the animation
                StopAllCoroutines();
                StartCoroutine(QuickHide());
                 break;
             case MoleType.HardHat:
                // If lives == 2 reduce, and change sprite.
                if (lives == 2) {
                    spriteRenderer.sprite = moleHatBroken;
                    lives--;
                } else {
                     spriteRenderer.sprite = moleHatHit;
                     gameManager.AddScore(moleIndex);
                    // stop the animation 
                    StopAllCoroutines();
                    StartCoroutine(QuickHide());
                    // turn off hittable so that we cant keep tapping for score
                    hittable = false;
                }
                break;
            case MoleType.Bomb:
            // game over, 1 for bomb 
                gameManager.GameOver(1);
                break;
              default:
                break;
        }
    }
}

private IEnumerator QuickHide() {
    yield return new WaitForSeconds(0.25f);
    // Whilst we were waiting we may have spawned again here so just 
    // check that hasn't happened before hiding it. This will stop it 
    // flickering in that case. 
    if (!hittable) {
        Hide();
    }
}


public void Hide() {
    //set the appropriate mole parameters to hide it 
    transform.localPosition = startPosition;
    boxCollider2D.offset = boxOffsetHidden;
    boxCollider2D.size = boxSizeHidden;   
} 

// As the level progresses the game gets harder. 
private void Setlevel(int level) {
    // as level increases increase the bomb rate to 0.25 at level 10 
    bombRate = Mathf.Min(level * 0.025f, 0.25f);

    // Increase the amounts of hardhats until 100% at level 40 
    hardRate = Mathf.Min(level * 0.025f, 1f);

    // duration that moles go up and down bounds get quicker as we progress. No cap on insanity. 
    float durationMin = Mathf.Clamp(1 - level * 0.005f, 0.9f, 1f);
    float durationMax = Mathf.Clamp(2 - level * 0.005f, 1f, 2f);
    duration = Random.Range(durationMin, durationMax);
}


// used to freeze the game on finish
public void StopGame() {
    hittable = false; 
    StopAllCoroutines();
}
}