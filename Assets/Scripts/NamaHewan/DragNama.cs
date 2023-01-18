using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DragNama : MonoBehaviour
{
    //===========================================//
    //========== START PROPERTY GAME ============//
    //===========================================//
    public Vector2 SavePosition;
    public bool isDropped;
    public int key;
    public TextMeshProUGUI text;

    [Space]

    public UnityEvent OnDragTrue;

    Transform SetPosition;
    //===========================================//
    //=========== END PROPERTY GAME =============//
    //===========================================//

    void Start()
    {
        // Save default position Name Drag
        SavePosition = transform.position;
    }

    private void OnMouseDown()
    {
        AudioManager.instance.SetSfx(13);
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

                //If True
                DataGameNama.dataScore += 100;
                GameSystemNama.currentData++;
                AudioManager.instance.SetSfx(14);
            }

            else
            {
                transform.position = SavePosition;

                //If False
                DataGameNama.dataHealth --;
                AudioManager.instance.SetSfx(10);
                Handheld.Vibrate();

                if (DataGameNama.dataScore == 0)
                {
                    
                }
                else
                {
                    DataGameNama.dataScore -= 50;   
                }
            }
        }

        else
        {
            transform.position = SavePosition;
        }
    }

    private void OnMouseDrag()
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
