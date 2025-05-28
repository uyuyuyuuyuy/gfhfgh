using UnityEngine;
using UnityEngine.SceneManagement;  // ���������� ���������� �������

public class MenuController : MonoBehaviour
{
    // ����� ��� ������ "������ ����"
    public void LoadGameScene()
    {
        SceneManager.LoadScene("WorkingProject");
    }

    // ����� ��� ������ "����� �� ����" (����������)
    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // ����� ���������� ���� � ���������
#else
        Application.Quit(); // ����� ������� ����
#endif
    }
}
