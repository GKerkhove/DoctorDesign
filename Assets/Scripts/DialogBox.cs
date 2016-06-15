

using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    public Text Title;
    public Text SubTitle;
    public Text Info;
    public Image Picture;
    public Button Btn;
    public GameObject Input;

    void Start()
    {
        Game.Get().MainDialog = this;
        gameObject.SetActive(false);
        print("test");
        Btn.onClick.AddListener(Click);
    }

    void Click()
    {
        gameObject.SetActive(false);
    }

    public void Show(string title, string subtitle, string info, Sprite img)
    {
        gameObject.SetActive(true);
        Title.text = title;
        SubTitle.text = subtitle;
        Info.text = info;
        Picture.sprite = img;
        GetComponent<RectTransform>().rect.Set(GetComponent<RectTransform>().rect.x, GetComponent<RectTransform>().rect.y,450,410);
        Input.SetActive(false);
        Info.gameObject.SetActive(true);
    }

    public void ShowLarge(string title, string subtitle, string info, Sprite img)
    {
        gameObject.SetActive(true);
        Title.text = title;
        SubTitle.text = subtitle;
        Info.text = info;
        Picture.sprite = img;
        GetComponent<RectTransform>().rect.Set(GetComponent<RectTransform>().rect.x, GetComponent<RectTransform>().rect.y, 900, 510);
        Input.SetActive(false);
        Info.gameObject.SetActive(true);
    }

    public void ShowLarge(string title, string subtitle, string info, Sprite img, bool input)
    {
        gameObject.SetActive(true);
        Title.text = title;
        SubTitle.text = subtitle;
        Info.text = info;
        Picture.sprite = img;
        if (input)
        {
            Input.SetActive(true);
            Info.gameObject.SetActive(false);
            Input.transform.Find("Input").Find("Text").GetComponent<Text>().text = info;
        }
        GetComponent<RectTransform>().rect.Set(GetComponent<RectTransform>().rect.x, GetComponent<RectTransform>().rect.y, 900, 510);

    }
}