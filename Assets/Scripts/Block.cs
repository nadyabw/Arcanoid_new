using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Block : MonoBehaviour
{
    #region Variables
   
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int NumberHit;
    [SerializeField] private int scoreForDestroy;
    [SerializeField] private Sprite[] stateSprites; //массив для картинок
    [SerializeField] private bool isUnderstroyable = false; // неубиваемый
    [SerializeField] private bool isInvisible = false; // невидимый
    [SerializeField] private GameObject[] pickUpPrefabs;
    [Range (1, 100)]
    [SerializeField] private int bonusCreateProbability = 30;

    private int currentHits;

    #endregion

    #region Properties

    public int ScoreForDestroy => scoreForDestroy;

    #endregion

    #region Events

    public static event Action<Block> OnDestroy;
    public static event Action<bool> OnCreate;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        UpdateView();

        if(isInvisible)
        {
            SetVisible(false);      // делаем полностью невидимым
        }

        OnCreate?.Invoke(isUnderstroyable); 
    }

    #endregion

    #region Private methods

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(isInvisible)
        {
            SetVisible(true);  // делаем видимым при 1 ударе, но удар не засчитываем и не меняем спрайт
            return;
        }

        if(!isUnderstroyable) 
        {
            Hit();
        }
    }

    private void SetVisible(bool isVisible) // сделать видимым/невидимым
    {
        Color clr = spriteRenderer.color;

        if (isVisible) // делаем видимым
        {
            clr.a = 1.0f;
            isInvisible = false;
        }
        else // // делаем невидимым
        {
            clr.a = 0.0f;
        }

        spriteRenderer.color = clr;
    }

    private void Hit() // проверка удара
    {
        currentHits++;
        if (currentHits == NumberHit) // если удар последний
        {
            DestroyBlock();
        }
        else
        {
            UpdateView();
        }
    }

    private void DestroyBlock()

    {
        if (IsNeedToCreateBonus())
        {
            int randIndex = Random.Range(0, pickUpPrefabs.Length);
            Instantiate(pickUpPrefabs[randIndex], transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        
    }

    
    private void UpdateView() // обновление картинки
    {
        if (currentHits < stateSprites.Length)
        {
            spriteRenderer.sprite = stateSprites[currentHits];
        }
    }
    private bool IsNeedToCreateBonus()
    {
        if ((pickUpPrefabs == null) || (pickUpPrefabs.Length == 0))
            return false;

        int randNum = Random.Range(1, 101);

        return randNum <= bonusCreateProbability; 
    }

    #endregion
}