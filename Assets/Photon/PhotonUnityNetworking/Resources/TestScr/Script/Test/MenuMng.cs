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
        //�w�肳�ꂽ���j���[���j���[���J������ȊO�����
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
        //�w�肳�ꂽ���j���[���J���O�Ɍ��݊J���Ă��郁�j���[�����
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
        //���j���[�����
        menu.Close();
    }
}
