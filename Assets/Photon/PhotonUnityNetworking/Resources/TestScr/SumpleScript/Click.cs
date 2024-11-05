using UnityEngine;
using UnityEngine.SceneManagement;

public class Click : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("2_CharacterSelect");
    }
}
