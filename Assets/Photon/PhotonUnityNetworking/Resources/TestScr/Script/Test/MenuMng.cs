using UnityEngine;

public class MenuMng : MonoBehaviour
{
    public static MenuMng Instance;
    [SerializeField] Menu[] menus;

    private void Awake()
    {
        Instance = this;
    }
    public void OpenMenu(string menuName)
    {
        //指定されたメニューメニューを開きそれ以外を閉じる
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                menus[i].Close();
            }
        }
    }
    public void OpenMenu(Menu menu)
    {
        //指定されたメニューを開く前に現在開いているメニューを閉じる
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        //メニューを閉じる
        menu.Close();
    }
}
