using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LD39.UI
{
    public class WindowManager : MonoBehaviour
    {
        public static WindowManager instance;

        public GameObject windowPrefab;

        public GameObject loginBlocker;
        public InputField loginInput;
        public Button loginButton;

        public GameObject gameOverScreen;
        public Text gameOverMessage;

        public GameObject batteryPrefab;

        public List<Window> windows { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            windows = new List<Window>();
            loginBlocker.SetActive(true);
            GameManager.instance.OnGameOver += HandleGameOver;
        }

        private void Start()
        {
            loginButton.interactable = false;

            loginInput.onValueChanged.AddListener(delegate
            {
                loginButton.interactable = loginInput.text.Length > 0;
            });

            loginButton.onClick.AddListener(delegate
            {
                loginBlocker.SetActive(false);
                GameManager.instance.StartGame(loginInput.text);
                AudioManager.instance.PlaySound("login");
                Open("Battery", batteryPrefab, 160, 200);
            });
        }

        private void HandleGameOver(float score)
        {
            CloseAll();

            gameOverScreen.SetActive(true);
            gameOverScreen.GetComponent<RectTransform>().SetAsLastSibling();

            gameOverMessage.text = string.Format("It's now safe to turn off your computer.\n\nYou earnt {0} PowerCoinz.", score);
        }

        public Window Open(string title, GameObject innerContent, int width = 400, int height = 300, bool canClose = true)
        {
            GameObject go = gameObject.AddChild(windowPrefab);
            Window w = go.GetComponent<Window>();
            w.Setup(title, innerContent);
            w.canClose = canClose;
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);
            windows.Add(w);
            return w;
        }

        public void Close(Window window, bool force = false)
        {
            if (force || window.canClose)
            {
                windows.Remove(window);
                Destroy(window.gameObject);
            }
        }

        public void CloseAll()
        {
            foreach (var window in windows)
            {
                Destroy(window.gameObject);
            }

            windows.Clear();
        }
    }
}