using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LD39.UI.Applications
{
    public class EmailLine : MonoBehaviour, IPointerClickHandler
    {
        public Text sender;
        public Text subject;
        public Text status;

        private Email email;
        private EMail emailApplication;

        public void Setup(Email email, EMail emailApplication)
        {
            sender.text = email.sender;
            subject.text = email.subject;

            switch (email.status)
            {
                case EmailStatus.READ:
                    status.text = "Read";
                    break;
                case EmailStatus.REPLIED_CORRECT:
                case EmailStatus.REPLIED_INCORRECT:
                    status.text = "Replied";
                    break;
                case EmailStatus.SPAM:
                    status.text = "Spam";
                    break;
                case EmailStatus.UNREAD:
                    status.text = "New";
                    break;
            }

            this.email = email;
            this.emailApplication = emailApplication;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            emailApplication.SwitchToSingleView(email);
        }
    }
}