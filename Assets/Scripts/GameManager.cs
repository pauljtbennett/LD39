using LD39.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD39
{
    public delegate void GameStartHandler();
    public delegate void GameOverHandler(float score);
    public delegate void PowerUpdatedHandler(float power, float percentage);
    public delegate void FundsUpdatedHandler(float funds);
    public delegate void EmailReceivedHandler(Email email);
    public delegate void DocumentAddedHandler(Document document);
    public delegate void DocumentRemovedHandler(Document document);

    public class GameManager : MonoBehaviour
    {
        private const float STARTING_POWER = 10000f;

        public static GameManager instance;

        public event GameStartHandler OnGameStart;
        public event GameOverHandler OnGameOver;
        public event PowerUpdatedHandler OnPowerUpdated;
        public event FundsUpdatedHandler OnFundsUpdated;
        public event EmailReceivedHandler OnEmailReceived;
        public event DocumentAddedHandler OnDocumentAdded;
        public event DocumentRemovedHandler OnDocumentRemoved;

        public EmailTemplates templateManager { get; private set; }
        public WritingTexts textsManager { get; private set; }

        public List<Document> docs { get; private set; }
        public List<Email> emails { get; private set; }
        public List<IApp> runningApps { get; private set; }

        public string playerName { get; private set; }

        private float funds;
        private float power;
        private bool gameOver;

        private readonly float timeBetweenEmails = 5f;
        private float currentTimeBetweenEmails;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            gameOver = true;

            docs = new List<Document>();
            emails = new List<Email>();
            runningApps = new List<IApp>();

            power = STARTING_POWER;

            // Load config
            templateManager = new EmailTemplates();
            templateManager.LoadConfig("EmailTemplates");

            textsManager = new WritingTexts();
            textsManager.LoadConfig("WritePadTexts");
        }

        private void Update()
        {
            if (gameOver)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Quit();
                }
                return;
            }

            // Use a small amount of power just idling
            power -= 1f * Time.deltaTime;

            // DEBUG send some emails
            currentTimeBetweenEmails += Time.deltaTime;
            if (currentTimeBetweenEmails > timeBetweenEmails)
            {
                if (Random.Range(0f, 1f) > 0.1f && emails.Count < 200)
                {
                    Email email = new Email(templateManager.templates[UnityEngine.Random.Range(0, templateManager.templates.Count)]);
                    emails.Add(email);
                    if (OnEmailReceived != null) OnEmailReceived(email);
                }
                currentTimeBetweenEmails = 0f;
            }

            // Handle apps using power
            foreach (IApp app in runningApps)
            {
                power -= app.powerUsage * Time.deltaTime;
            }

            // Check for game over state
            if (power <= 0)
            {
                gameOver = true;
                if (OnGameOver != null) OnGameOver(funds);
                return;
            }

            float percentage = power / STARTING_POWER;
            if (OnPowerUpdated != null) OnPowerUpdated(power, percentage);
        }

        public void StartGame(string playerName)
        {
            this.playerName = playerName;
            gameOver = false;
            if (OnGameStart != null) OnGameStart();
        }

        public void EndGame()
        {
            power = 0;
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void RegisterApplication(IApp app)
        {
            runningApps.Add(app);
        }

        public void UnregisterApplication(IApp app)
        {
            runningApps.Remove(app);
        }

        public void UpdateFunds(float delta)
        {
            if (delta > 0)
            {
                funds += delta;
                if (OnFundsUpdated != null) OnFundsUpdated(funds);
            }
        }

        public void AddDocument(Document doc)
        {
            docs.Add(doc);
            if (OnDocumentAdded != null) OnDocumentAdded(doc);
        }

        public void RemoveDocument(Document doc)
        {
            docs.Remove(doc);
            if (OnDocumentRemoved != null) OnDocumentRemoved(doc);
        }
    }
}