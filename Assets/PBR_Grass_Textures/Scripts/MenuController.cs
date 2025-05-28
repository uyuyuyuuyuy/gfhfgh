using UnityEngine;
using UnityEngine.SceneManagement;  // подключаем управление сценами

public class MenuController : MonoBehaviour
{
    // Метод для кнопки "Начать игру"
    public void LoadGameScene()
    {
        SceneManager.LoadScene("WorkingProject");
    }

    // Метод для кнопки "Выйти из игры" (остановить)
    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // чтобы остановить игру в редакторе
#else
        Application.Quit(); // чтобы закрыть билд
#endif
    }
}
