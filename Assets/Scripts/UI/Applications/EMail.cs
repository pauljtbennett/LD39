using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LD39.UI.Applications
{
    public class EMail : MonoBehaviour, IApp
    {
        public float powerUsage { get { return 20f; } set { } }

        public GameObject emailPrefab;
        public GameObject emailList;
        public GameObject emailView;
        public Button switchViewButton;

        private void Awake()
        {
            SwitchToListView();
        }

        private void Start()
        {
            GameManager.instance.OnEmailReceived += AppendEmail;
            GameManager.instance.RegisterApplication(this);

            RefreshAll();

            switchViewButton.onClick.AddListener(delegate
            {
                SwitchToListView();
            });
        }

        private void OnDestroy()
        {
            GameManager.instance.OnEmailReceived -= AppendEmail;
            GameManager.instance.UnregisterApplication(this);
        }

        private void RefreshAll()
        {
            for (int i = 0; i < emailList.transform.childCount; i++)
            {
                Destroy(emailList.transform.GetChild(i).gameObject);
            }

            foreach (Email email in GameManager.instance.emails)
            {
                AppendEmail(email);
            }
        }

        private void AppendEmail(Email email)
        {
            GameObject go = emailList.AddChild(emailPrefab);
            go.GetComponent<EmailLine>().Setup(email, this);
            AudioManager.instance.PlaySound("newEmail");
        }

        public void SwitchToSingleView(Email email)
        {
            emailList.SetActive(false);
            emailView.SetActive(true);
            switchViewButton.interactable = true;

            emailView.GetComponent<EmailView>().Setup(email, this);
        }

        public void SwitchToListView()
        {
            RefreshAll();

            emailList.SetActive(true);
            emailView.SetActive(false);
            switchViewButton.interactable = false;
        }
    }
}