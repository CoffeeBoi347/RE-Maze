using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class EffectsManager : MonoBehaviour
{
    public Image imageFlashFX;
    public static EffectsManager instance;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator FlashScreen()
    {
        imageFlashFX.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        imageFlashFX.gameObject.SetActive(false);
    }
}