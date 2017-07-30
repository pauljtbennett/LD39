using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD39.UI.Applications
{
    public class FileManager : MonoBehaviour, IApp
    {
        public float powerUsage { get { return 5f; } set { } }

        public GameObject docPrefab;
        public GameObject docList;

        private Action<Document> clickAction;

        private void Start()
        {
            GameManager.instance.OnDocumentAdded += AppendDocument;
            GameManager.instance.RegisterApplication(this);

            RefreshAll();
        }

        private void OnDestroy()
        {
            GameManager.instance.UnregisterApplication(this);
        }

        public void SetupDocumentClickAction(Action<Document> action)
        {
            clickAction = action;
            RefreshAll();
        }

        private void AppendDocument(Document document)
        {
            GameObject go = docList.AddChild(docPrefab);
            go.GetComponent<DocumentIcon>().Setup(document, this, clickAction);
        }

        private void RefreshAll()
        {
            for (int i = 0; i < docList.transform.childCount; i++)
            {
                Destroy(docList.transform.GetChild(i).gameObject);
            }

            foreach (Document doc in GameManager.instance.docs)
            {
                AppendDocument(doc);
            }
        }
    }
}
