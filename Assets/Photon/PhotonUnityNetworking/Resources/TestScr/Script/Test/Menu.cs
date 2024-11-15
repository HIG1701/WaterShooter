using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;
    private void Start()
    {
        this.gameObject.SetActive(open);
    }
    public void Open()
    {
        open = true;
        this.gameObject.SetActive(true);
    }
    public void Close()
    {
        open = false;
        this.gameObject.SetActive(false);
    }
}
