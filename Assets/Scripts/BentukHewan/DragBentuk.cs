using UnityEngine;
using UnityEngine.Events;

public class DragBentuk : MonoBehaviour
{
    //===========================================//
    //========== START PROPERTY GAME ============//
    //===========================================//
    public static DragBentuk instance;
    public Vector2 SavePosition;
    public bool isDropped;
    public int key;
    public SpriteRenderer images;
    public AudioClip animVoice;

    [Space]

    public UnityEvent OnDragTrue;

    Transform SetPosition;
    //===========================================//
    //=========== END PROPERTY GAME =============//
    //===========================================//
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Save default position Name Drag
        SavePosition = transform.position;
    }

    private void OnMouseDown()
    {
        if (animVoice)
        {
            // Play Clip in DataGame
            AudioManager.instance.audioSfx.PlayOneShot(animVoice);
        }
        else
        {
            AudioManager.instance.SetSfx(13);
        }
    }

    private void OnMouseUp()
    {
        if (isDropped)
        {
            
            int keyObj = SetPosition.GetComponent<KeyObj>().key;
            if (key == keyObj)
            {
                transform.SetParent(SetPosition);
                transform.localPosition = Vector3.zero;
                transform.localScale = SetPosition.localScale;

                SetPosition.GetComponent<SpriteRenderer>().enabled = false;
                SetPosition.GetComponent<Rigidbody2D>().simulated = false;
                SetPosition.GetComponent<BoxCollider2D>().enabled = false;
                OnDragTrue.Invoke();

                // When answered successfully
                DataGameBentuk.dataScore += 100;
                GameSystemBentuk.currentData++;
                AudioManager.instance.SetSfx(14);
            }

            else
            {
                transform.position = SavePosition;

                // When it fail to answer
                DataGameBentuk.dataHealth --;
                AudioManager.instance.SetSfx(10);
                Handheld.Vibrate();

                if (DataGameBentuk.dataScore == 0)
                {
                    // When the score is 0, the score will not decrease anymore
                }
                else
                {
                    // When the score is more than 0 then the score will decrease by 50
                    DataGameBentuk.dataScore -= 50;   
                }
            }
        }

        else
        {
            // When an object is dropped arbitrarily, it will return to its original position.
            transform.position = SavePosition;
        }
    }

    public void OnMouseDrag()
    {
        Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Pos;
    }

    private void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Drop"))
        {
            isDropped = true;
            SetPosition = trigger.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Drop"))
        {
            isDropped = false;
        }
    }
}
