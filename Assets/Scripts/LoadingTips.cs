using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTips : MonoBehaviour
{
    public Texture[] Background;
    public GameObject tips;
    public GameObject canvasTransition;
    public GameObject menyocokanNamaHewan;
    public GameObject menyocokanBentukHewan;

    public void MenyocokanNamaHewanLoad()
    {
        StartCoroutine(MenyocokanNamaHewan());
    }

    public void MenyocokanBentukHewanLoad()
    {
        StartCoroutine(MenyocokanBentukHewan());
    }

    IEnumerator MenyocokanNamaHewan()
    {
        tips.SetActive(true);
        menyocokanNamaHewan.SetActive(true);
        tips.GetComponent<RawImage>().texture = Background[0];
        yield return new WaitForSeconds(3.50f);
        AudioManager.instance.SetBgm(1);
        canvasTransition.GetComponent<UiControl>().BtnScene("Game0");
    }

    IEnumerator MenyocokanBentukHewan()
    {
        tips.SetActive(true);
        menyocokanBentukHewan.SetActive(true);
        tips.GetComponent<RawImage>().texture = Background[1];
        yield return new WaitForSeconds(3.50f);
        AudioManager.instance.SetBgm(12);
        canvasTransition.GetComponent<UiControl>().BtnScene("GameBentuk0");
    }
}
