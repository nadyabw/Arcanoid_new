using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    #region Variables

    private const int NORMAL = 0;  // состояния
    private const int EMPTY = 1;   // жизней

    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites; // спрайты состояний жизни

    #endregion

    public void SetAvailable(bool isAvalaible) // переключаем внешний вид жизни
    {
        if (isAvalaible)
        {
            image.sprite = sprites[NORMAL];
        }
        else
        {
            image.sprite = sprites[EMPTY];
        }
    }

    public float GetWidth() // ширина картинки
    {
        return image.sprite.rect.width;
    }
}
