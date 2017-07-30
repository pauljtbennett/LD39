using LD39.Config;
using LD39.UI.Applications;
using LD39.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD39
{
    public enum EmailStatus
    {
        UNREAD,
        READ,
        DELETED,
        SPAM,
        REPLIED_CORRECT,
        REPLIED_INCORRECT
    }

    public enum EmailTaskType
    {
        CREATE_TEXT_DOCUMENT,
        CONVERT_TEXT_DOCUMENT,
        EDIT_IMAGE,
        CONVERT_IMAGE,
        DO_MATHS,
        LOOKUP_PERSON_DETAILS,
        SPAM,
    }

    public class Email
    {
        public string sender { get; private set; }
        public string subject { get; private set; }
        public string content { get; private set; }
        public EmailStatus status { get; private set; }

        public List<Document> attachments { get; private set; }
        public List<Email> replies { get; private set; }

        private EmailTemplate template;

        // Task attributes
        private float reward;

        // Create Doc
        private int contentLength;

        // Convert Doc

        // Maths
        private float partA;
        private float partB;
        private CalculatorOperation calcOp;
        private float result;

        // Images
        private int rotationAmount;
        private bool inverted;

        public Email(Email parent, string content, string sender)
        {
            this.content = content;
            this.sender = sender;

            subject = string.Format("RE: {0}", parent.subject);
        }

        public Email(EmailTemplate template)
        {
            this.template = template;

            attachments = new List<Document>();
            replies = new List<Email>();

            sender = NameGenerator.Generate();
            sender = NameGenerator.GenerateEmailAddress(sender);

            subject = EmailTemplates.GenerateSubject(template.taskType);

            switch (template.taskType)
            {
                case EmailTaskType.DO_MATHS:
                    GenerateRandomMathsQuestion();
                    GenerateMathsContent();
                    break;
                case EmailTaskType.CREATE_TEXT_DOCUMENT:
                    contentLength = UnityEngine.Random.Range(0, 1000);
                    reward = contentLength * UnityEngine.Random.Range(0.01f, 0.1f);
                    GenerateDocContent();
                    break;
                case EmailTaskType.EDIT_IMAGE:
                    GenerateRandomImageEdit();
                    GenerateImageContent();
                    reward = UnityEngine.Random.Range(15f, 30f);
                    break;
                case EmailTaskType.SPAM:
                    GenerateRandomSpamAttachment();
                    GenerateSpamContent();
                    break;
            }

            if (subject == string.Empty)
            {
                subject = "No Subject";
            }

            if (reward > 0f)
            {
                content += string.Format("\n\nThe reward is {0}", reward);
            }
        }

        public void View()
        {
            if (status == EmailStatus.UNREAD)
            {
                status = EmailStatus.READ;
            }
        }

        public void Reply(bool playerReply, string message, List<Document> attachments)
        {
            Email reply = new Email(this, message, playerReply ? NameGenerator.GenerateEmailAddress(GameManager.instance.playerName) : this.sender);
            replies.Add(reply);
            if (attachments != null)
            {
                foreach (Document doc in attachments)
                {
                    if (!this.attachments.Contains(doc))
                    {
                        this.attachments.Add(doc);
                    }
                }
            }

            if (playerReply)
            {
                bool goodReply = false;

                switch (template.taskType)
                {
                    case EmailTaskType.DO_MATHS:
                        goodReply = message.Contains(result.ToString());
                        break;
                    case EmailTaskType.CREATE_TEXT_DOCUMENT:
                        if (attachments != null)
                        {
                            goodReply = attachments.Count(x => x.contentLength == contentLength) > 0;
                        }
                        break;
                    case EmailTaskType.EDIT_IMAGE:
                        if (attachments != null)
                        {
                            goodReply = attachments.Count(x => x.rotation == rotationAmount && x.inverted == inverted) > 0;
                        }
                        break;
                }

                if (goodReply)
                {
                    if (status == EmailStatus.REPLIED_CORRECT)
                    {
                        Reply(false, "Thanks but I already gave you a reward for this.", null);
                    }
                    else
                    {
                        Reply(false, "Thanks, sending your reward over now!", null);
                        GameManager.instance.UpdateFunds(reward);
                    }
                    status = EmailStatus.REPLIED_CORRECT;
                }
                else
                {
                    Reply(false, "Hmm, that doesn't look right to me...", null);

                    // Prevent switching between incorrect and correct to gain infinite rewards!
                    if (status != EmailStatus.REPLIED_CORRECT)
                    {
                        status = EmailStatus.REPLIED_INCORRECT;
                    }
                }
            }
        }

        private void GenerateRandomMathsQuestion()
        {
            partA = UnityEngine.Random.Range(1, 1001);
            partB = UnityEngine.Random.Range(1, 1001);

            var v = Enum.GetValues(typeof(CalculatorOperation));
            calcOp = (CalculatorOperation)v.GetValue(UnityEngine.Random.Range(1, v.Length));

            switch (calcOp)
            {
                case CalculatorOperation.ADD:
                    result = partA + partB;
                    reward = UnityEngine.Random.Range(2f, 10f);
                    break;
                case CalculatorOperation.SUB:
                    result = partA - partB;
                    reward = UnityEngine.Random.Range(2f, 10f);
                    break;
                case CalculatorOperation.MUL:
                    result = partA * partB;
                    reward = UnityEngine.Random.Range(5f, 15f);
                    break;
                case CalculatorOperation.DIV:
                    result = partA / partB;
                    reward = UnityEngine.Random.Range(5f, 15f);
                    break;
                case CalculatorOperation.SQRT:
                    result = Mathf.Sqrt(partA);
                    reward = UnityEngine.Random.Range(15f, 25f);
                    break;
                case CalculatorOperation.POW:
                    result = Mathf.Pow(partA, partB);
                    reward = UnityEngine.Random.Range(15f, 25f);
                    break;
            }

            result = (float)Math.Round(result, 3);
        }

        private void GenerateMathsContent()
        {
            content = string.Empty;

            if (template.taskType == EmailTaskType.DO_MATHS)
            {
                if (calcOp == CalculatorOperation.SQRT)
                {
                    content = EmailTemplates.Generate1PartMathsContent(partA);
                }
                else
                {
                    content = EmailTemplates.Generate2PartMathsContent(partA, partB, CalculatorOperationToSymbol(calcOp));
                }
            }
        }

        private void GenerateDocContent()
        {
            content = string.Empty;

            if (template.taskType == EmailTaskType.CREATE_TEXT_DOCUMENT)
            {
                content = EmailTemplates.GenerateCreateDocContent(contentLength);
            }
        }

        private void GenerateRandomImageEdit()
        {
            string path = EmailTemplates.imagePaths[UnityEngine.Random.Range(0, EmailTemplates.imagePaths.Length)];
            Document d = new Document(path, DocumentType.BMP);
            d.imagePath = path;
            attachments.Add(d);

            int rot = UnityEngine.Random.Range(0, 4);
            rotationAmount = rot * 90;

            int inv = UnityEngine.Random.Range(0, 2);
            inverted = inv == 1 ? true : false;
        }

        private void GenerateImageContent()
        {
            content = string.Empty;

            if (template.taskType == EmailTaskType.EDIT_IMAGE)
            {
                content = EmailTemplates.GenerateEditImageContent(rotationAmount, inverted);
            }
        }

        private void GenerateRandomSpamAttachment()
        {
            if (UnityEngine.Random.Range(0f, 1f) > 0.25f)
            {
                Document d = new Document("Click Me", DocumentType.EXE);
                attachments.Add(d);
            }
        }

        private void GenerateSpamContent()
        {
            content = string.Empty;

            if (template.taskType == EmailTaskType.SPAM)
            {
                content = EmailTemplates.GenerateSpamContent();
            }
        }

        private string CalculatorOperationToSymbol(CalculatorOperation op)
        {
            if (op == CalculatorOperation.ADD) return "+";
            if (op == CalculatorOperation.SUB) return "-";
            if (op == CalculatorOperation.MUL) return "*";
            if (op == CalculatorOperation.DIV) return "/";
            if (op == CalculatorOperation.SQRT) return "root";
            if (op == CalculatorOperation.POW) return "^";
            return string.Empty;
        }
    }
}