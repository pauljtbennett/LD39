using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LD39.Config
{
    public class EmailTemplate
    {
        public EmailTaskType taskType;

        public EmailTemplate(JToken token)
        {
            taskType = (EmailTaskType)Enum.Parse(typeof(EmailTaskType), token.Value<string>("taskType"));
        }
    }

    public class EmailTemplates
    {
        private static string[] subjectPeople = new string[]
        {
            "mother",
            "father",
            "brother",
            "sister",
            "mate {0}",
            "cousin",
            "dog",
            "hamster"
        };

        private static string[] subjectPeopleNames = new string[]
        {
            "Dan",
            "Barbara",
            "Steve",
            "Edgar",
            "Elizabeth",
            "Fi"
        };

        private static string[] spamSubjects = new string[]
{
            "Make the most of our late summer deals!",
            "Splash & Save 30% Sitewide",
            "RE - URGENT NOTIFICATION OF RELEASE OF YOUR PAYMENT.$1.5M",
            "10 Grand Challenge Singer Invitation",
            "More SALE, more style! Now everything at 90% off",
            "Someone has your password",
            "(no subject)"
        };

        private static string[] createTextDocSubjects = new string[]
        {
            "New website content needed ASAP",
            "First draft request"
        };

        private static string[] convertTextDocSubjects = new string[] { };

        private static string[] editImageSubjects = new string[]
        {
            "Freelance graphic designers wanted",
            "I need a logo done cheap",
            "Simple image edit required"
        };

        private static string[] convertImageSubjects = new string[] { };

        private static string[] mathsSubjects = new string[]
        {
            "Need help with this maths question",
            "Trying to solve a tricky problem",
            "My calculator fell in the toilet",
            "Need to prove my {0} wrong",
            "My {0} needs help with this homework question",
            "Help calculating my tax return"
        };

        private static string[] mathsContents1Part = new string[]
        {
            "I need you to find the square root of {0}.",
        };

        private static string[] mathsContents2Part = new string[]
        {
            "I need you to work out what {0} {1} {2} is please.",
        };

        private static string[] createDocContents = new string[]
        {
            "This doc had better be {0} characters long for us to approve it!",
            "I need {0} characters on my desk by tomorrow morning.",
            "We loved your last piece of work, this time we need something of {0} characters - are you able to help us out?",
        };

        private static string[] editImageContents = new string[]
        {
            "I need this image rotated by {0} degrees. Oh and it must {1}be inverted!",
            "My daughter took this great image but I think it'd look better if it was rotated {0} degrees. I do {1}want it be inverted.",
            "My pet snake wants this picture rotated {0} degress and to {1}be inverted. Can you help?"
        };

        private static string[] spamContents = new string[]
        {
            "It is required that you reply within the next 24 hours, We will suspend access to your account if we don't recieve your reply within the given time frame, We would appreciate your immediate attention to this matter.",
            "I am contacting you to receive $15.7M left in an investment account by my late client. This is an opportunity that will benefit both of us greatly and it will be legally conducted. If you are interested reply.Regards, Christopher.",
            "EOM",
            "Vacations, swimming pools, camping, cottaging, and long summer days!  You take a lot of amazing photos in the summer...",
            "With late deals and fantastic offers, there's never been a better time to book a last minute summer getaway.",
        };

        public static string[] imagePaths = new string[]
        {
            "MonaLisa",
            "EiffelTower",
            "Pyramids"
        };

        public List<EmailTemplate> templates;

        public EmailTemplates()
        {
            templates = new List<EmailTemplate>();
        }

        public void LoadConfig(string filename)
        {
            string resourcePath = Path.Combine("Config", filename);
            TextAsset t = Resources.Load<TextAsset>(resourcePath);
            var templatesObj = JToken.Parse(t.text);
            foreach (var templateObj in templatesObj["templates"].Children())
            {
                EmailTemplate template = new EmailTemplate(templateObj);
                templates.Add(template);
            }
        }

        public static string GenerateSubject(EmailTaskType taskType)
        {
            string subject = "No Subject";

            switch (taskType)
            {
                case EmailTaskType.DO_MATHS:
                    subject = mathsSubjects[UnityEngine.Random.Range(0, mathsSubjects.Length)];
                    subject = string.Format(subject, subjectPeople[UnityEngine.Random.Range(0, subjectPeople.Length)]);
                    subject = string.Format(subject, subjectPeopleNames[UnityEngine.Random.Range(0, subjectPeopleNames.Length)]);
                    break;
                case EmailTaskType.CREATE_TEXT_DOCUMENT:
                    subject = createTextDocSubjects[UnityEngine.Random.Range(0, createTextDocSubjects.Length)];
                    break;
                case EmailTaskType.EDIT_IMAGE:
                    subject = editImageSubjects[UnityEngine.Random.Range(0, editImageSubjects.Length)];
                    break;
                case EmailTaskType.SPAM:
                    subject = spamSubjects[UnityEngine.Random.Range(0, spamSubjects.Length)];
                    break;
            }

            return subject;
        }

        public static string Generate1PartMathsContent(float partA)
        {
            string content = string.Empty;
            content = mathsContents1Part[UnityEngine.Random.Range(0, mathsContents1Part.Length)];
            content = string.Format(content, partA);

            return content;
        }

        public static string Generate2PartMathsContent(float partA, float partB, string operation)
        {
            string content = string.Empty;
            content = mathsContents2Part[UnityEngine.Random.Range(0, mathsContents2Part.Length)];
            content = string.Format(content, partA, operation, partB);

            return content;
        }

        public static string GenerateCreateDocContent(int contentLength)
        {
            string content = string.Empty;
            content = createDocContents[UnityEngine.Random.Range(0, createDocContents.Length)];
            content = string.Format(content, contentLength);

            return content;
        }

        public static string GenerateEditImageContent(int rotationAmount, bool inverted)
        {
            string content = string.Empty;
            content = editImageContents[UnityEngine.Random.Range(0, editImageContents.Length)];
            content = string.Format(content, rotationAmount, inverted ? "" : "not ");

            return content;
        }

        public static string GenerateSpamContent()
        {
            string content = string.Empty;
            content = spamContents[UnityEngine.Random.Range(0, spamContents.Length)];

            return content;
        }
    }
}