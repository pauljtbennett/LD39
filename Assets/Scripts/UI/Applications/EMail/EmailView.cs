using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LD39.UI.Applications
{
    public class EmailView : MonoBehaviour
    {
        public Text content;
        public InputField replyInput;
        public Button replyButton;
        public Button attachButton;
        public Button downloadButton;
        public GameObject fileManager;
        public GameObject imageEditor;
        public GameObject writePad;
        public GameObject virus;

        private Email email;
        private EMail emailApplication;

        private Window attachWindow;
        private Window imageWindow;
        private Window writePadWindow;
        private List<Document> tempAttachments;

        private void Start()
        {
            tempAttachments = new List<Document>();
            replyButton.interactable = false;

            replyInput.onValueChanged.AddListener(delegate
            {
                replyButton.interactable = replyInput.text.Length > 0;
            });

            replyButton.onClick.AddListener(delegate
            {
                email.Reply(true, replyInput.text, tempAttachments);
                replyInput.text = string.Empty;
                RefreshContent();
                AudioManager.instance.PlaySound("replyEmail");
            });

            attachButton.onClick.AddListener(delegate
            {
                attachWindow = WindowManager.instance.Open("Attach File", fileManager, 480, 480);
                attachWindow.GetComponentInChildren<FileManager>().SetupDocumentClickAction(delegate (Document doc)
                {
                    if (!tempAttachments.Contains(doc))
                    {
                        tempAttachments.Add(doc);
                    }
                    WindowManager.instance.Close(attachWindow);
                });
                AudioManager.instance.PlaySound("attachDoc");
            });

            downloadButton.onClick.AddListener(delegate
            {
                switch (email.attachments[0].type)
                {
                    case DocumentType.EXE:
                        Window w = WindowManager.instance.Open(email.attachments[0].name, virus, 320, 180, false);
                        w.GetComponentInChildren<Progress>().SetSpeed(10f);
                        break;
                    case DocumentType.BMP:
                    case DocumentType.PNG:
                    case DocumentType.JPG:
                        imageWindow = WindowManager.instance.Open("Scribbler", imageEditor, 264, 312);
                        imageWindow.GetComponentInChildren<Scribbler>().LoadDocument(email.attachments[0]);
                        break;
                    case DocumentType.DOC:
                    case DocumentType.TXT:
                        writePadWindow = WindowManager.instance.Open("Write Pad", imageEditor, 640, 480);
                        writePadWindow.GetComponentInChildren<WritePad>().LoadDocument(email.attachments[0]);
                        break;
                }
            });
        }

        private void OnEnable()
        {
            tempAttachments = new List<Document>();
            replyInput.text = string.Empty;
        }

        private void OnDisable()
        {
            if (attachWindow != null)
            {
                WindowManager.instance.Close(attachWindow);
            }

            if (imageWindow != null)
            {
                WindowManager.instance.Close(imageWindow);
            }

            if (writePadWindow != null)
            {
                WindowManager.instance.Close(writePadWindow);
            }
        }

        public void Setup(Email email, EMail emailApplication)
        {
            this.email = email;
            this.emailApplication = emailApplication;

            downloadButton.interactable = email.attachments.Count > 0;

            email.View();
            RefreshContent();
        }

        private void RefreshContent()
        {
            content.text = string.Format("From: {0}\nSubject: {1}\nMessage: {2}", email.sender, email.subject, email.content);

            foreach (Email reply in email.replies)
            {
                content.text += "\n\n---------------------------------\n\n";
                content.text += string.Format("From: {0}\nSubject: {1}\nMessage: {2}", reply.sender, reply.subject, reply.content);
            }

            if (email.attachments != null && email.attachments.Count > 0)
            {
                content.text += "\n\n---------------------------------\n\n";
                content.text += "Attachments: ";

                foreach (Document doc in email.attachments)
                {
                    content.text += doc.name;
                    content.text += "\n             ";
                }
            }
        }
    }
}