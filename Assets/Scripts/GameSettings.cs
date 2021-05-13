using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings/New")]
public class GameSettings : ScriptableObject   // настройки игры
{
    public int lifesTotal; // всего жизней при старте
    public bool isAutoplayMode;  // автоплей режим
}
