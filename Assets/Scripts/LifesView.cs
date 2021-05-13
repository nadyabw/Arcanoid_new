using System.Collections.Generic;
using UnityEngine;

public class LifesView : MonoBehaviour
{
    #region Variables

    [SerializeField] private Life lifeSample;  
    [SerializeField] private bool isCentered = true; // центр вьюшки это центр всех картинок жизней или левый край ?
    [SerializeField] private int offsetBetweenImagesX = 10;  // отступ между картинками жизней

    private List<Life> lifeImages = new List<Life>();

    #endregion

    public void InitLifes(int lifesLeft, int lifesTotal)  // создаем картинки жизней и переключаем спрайты состояния если нужно
    {
        Life l;

        for (int i = 0; i < lifesTotal; i++)
        {
            Vector2 pos = lifeSample.transform.localPosition;
            float offsetX = GetImagesOffsetX(lifesTotal);

            l = Instantiate(lifeSample, transform);
            pos.x += offsetX + (l.GetWidth() + offsetBetweenImagesX) * i;
            l.transform.localPosition = pos;
            l.SetAvailable(i < lifesLeft);

            lifeImages.Add(l);
        }

        Destroy(lifeSample.gameObject);
    }

    public void UpdateLifes(int lifesLeft) // апдейтим состояния спрайтов в соотв с оставшимся кол-вом жизней
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            Life l = lifeImages[i];
            l.SetAvailable(i < lifesLeft);
        }
    }

    #region Private methods

    private float GetImagesOffsetX(int imagesNumber) // отступ по Х начала отрисовки жизней от центра вьюшки
    {
        if(isCentered)
        {
            float totalWidth = (lifeSample.GetWidth() + offsetBetweenImagesX) * (imagesNumber - 1);

            return -(totalWidth / 2);  // жизни рисуются так, что общий центр картинок будет по центру вьюшки
        }
        else
        {
            return 0; // жизни рисуются от центра вьюшки слева направо
        }
    }

    #endregion
}
