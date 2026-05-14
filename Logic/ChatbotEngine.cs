// ChatbotEngine.cs
// Core chatbot logic: keyword matching, random responses, sentiment handling,
// memory integration, and conversation flow management.

namespace CyberChat.WPF.Logic
{
    // ── Custom Delegate for UI communication 
    public delegate void NewMessageHandler(string message, bool isUser);

    /// <summary>
    /// Core engine for the Cybersecurity Awareness Chatbot.
    /// Uses Dictionary for keyword mapping, List for random responses,
    /// delegates/events for UI communication, and OOP composition.
    /// </summary>
    public class ChatbotEngine
    {
        // ── Events (Delegate usage) 
        public event NewMessageHandler? OnNewMessage;

        // ── Private Fields
        private readonly Dictionary<string, List<string>> _keywordResponses;
        private readonly SentimentAnalyzer _sentimentAnalyzer;
        private readonly MemoryManager _memoryManager;
        private readonly Random _random;
        private string? _currentTopic;
        private readonly List<string> _validTopics;

        // ── Automatic Properties ─────────────────────────────────────────
        public string? CurrentTopic { get => _currentTopic; private set => _currentTopic = value; }
        public MemoryManager Memory => _memoryManager;

        // ── Constructor ──────────────────────────────────────────────────
        public ChatbotEngine()
        {
            _sentimentAnalyzer = new SentimentAnalyzer();
            _memoryManager = new MemoryManager();
            _random = new Random();

            _keywordResponses = new Dictionary<string, List<string>>
            {
                ["password"] = new List<string>
                {
                    "Use strong, unique passwords for each account. Avoid using personal details like your SA ID number or date of birth.",
                    "Consider using a password manager to generate and store complex passwords securely.",
                    "Enable two-factor authentication (2FA) wherever possible — especially on your banking and email accounts.",
                    "Never share your passwords via WhatsApp, SMS, or email. No bank in South Africa will ever ask for your password.",
                    "Change your passwords regularly, especially if you suspect a data breach. Use at least 12 characters with a mix of letters, numbers, and symbols.",
                    "Avoid using the same password for FNB, Capitec, or any other banking app as you do for social media."
                },
                ["phishing"] = new List<string>
                {
                    "Phishing emails often impersonate trusted SA brands like Takealot, SARS, or your bank. Always check the sender's actual email address.",
                    "Never click on links in unexpected emails or SMSes claiming to be from SARS during tax season — go directly to the official SARS eFiling website.",
                    "Look for red flags: urgent language, spelling errors, and generic greetings like 'Dear Customer' instead of your actual name.",
                    "If you receive a suspicious email from what looks like Vodacom or MTN, contact them directly through their official app or website.",
                    "Report phishing attempts to your email provider and to the South African Banking Risk Information Centre (SABRIC).",
                    "Be cautious of WhatsApp messages offering free data or airtime — these are common phishing scams in South Africa."
                },
                ["scam"] = new List<string>
                {
                    "SIM swap fraud is a major threat in South Africa. If your phone suddenly loses signal, contact your network provider immediately.",
                    "EFT fraud is common — always verify banking details telephonically before making large payments, especially for property transactions.",
                    "Be wary of 'too good to be true' offers on Facebook Marketplace or Gumtree. Always meet in public and verify items before paying.",
                    "Never share your OTP (One-Time PIN) with anyone, even if they claim to be from your bank. Banks will never ask for this.",
                    "Romance scams are on the rise. Be cautious of online relationships where the person quickly asks for money or financial help.",
                    "Advance fee fraud (419 scams) promising lottery winnings or inheritances are still active. If you didn't enter, you didn't win."
                },
                ["privacy"] = new List<string>
                {
                    "Review your social media privacy settings regularly. On Facebook, limit who can see your posts and personal information.",
                    "South Africa's POPIA (Protection of Personal Information Act) gives you the right to know what data companies collect about you.",
                    "Be careful what personal information you share on public Wi-Fi networks at Vida e Caffè, McDonald's, or shopping centres.",
                    "Use a VPN when connecting to public Wi-Fi to encrypt your internet traffic and protect your personal data.",
                    "Regularly check what apps have access to your phone's camera, microphone, and location — remove permissions you don't need.",
                    "Under POPIA, you can request companies to delete your personal data. Exercise your right to digital privacy."
                },
                ["virus"] = new List<string>
                {
                    "Keep your antivirus software up to date. Windows Defender is a solid free option built into Windows 10 and 11.",
                    "Be careful downloading software from unofficial sources. Stick to the Microsoft Store, Google Play Store, or Apple App Store.",
                    "Malware can hide in email attachments — never open .exe or .zip files from unknown senders.",
                    "Ransomware attacks are increasing in South Africa. Back up important files to an external drive or cloud service regularly.",
                    "Keep your operating system and apps updated — patches fix security vulnerabilities that malware exploits.",
                    "If your computer is running slowly or showing pop-ups, it may be infected. Run a full antivirus scan immediately."
                },
                ["safe"] = new List<string>
                {
                    "Practice safe browsing: look for the padlock icon (HTTPS) in your browser's address bar before entering personal information.",
                    "Only download apps from official stores. Third-party APK files can contain malware targeting your banking apps.",
                    "Keep your devices updated with the latest security patches to protect against known vulnerabilities.",
                    "Use separate email accounts for banking and social media to limit exposure if one account is compromised.",
                    "Be cautious when using public computers at libraries or internet cafés — always log out and clear your browsing data.",
                    "Teach your family members about online safety. Cybercriminals often target elderly South Africans who may be less tech-savvy."
                },
                ["link"] = new List<string>
                {
                    "Before clicking any link, hover over it to see the actual URL. Scammers often use URLs that look similar to legitimate SA websites.",
                    "Shortened links (bit.ly, tinyurl) can hide malicious destinations. Use a URL expander tool to check where they lead.",
                    "Never click links in SMS messages claiming your Capitec, FNB, or Standard Bank account has been compromised.",
                    "If a link asks you to 'verify your account' or 'confirm your details', it's almost certainly a phishing attempt.",
                    "Bookmark your banking websites and access them directly rather than following links from emails or messages.",
                    "Be especially careful with links shared in WhatsApp groups — they may lead to malware or phishing sites."
                }
            };

            _validTopics = new List<string>(_keywordResponses.Keys);
        }

        // ── Public Methods ───────────────────────────────────────────────

        /// <summary>
        /// Sends the initial greeting when the chatbot starts.
        /// </summary>
        public void SendGreeting()
        {
            string greeting = "🛡️ Welcome to the Cybersecurity Awareness Bot!\n\n" +
                "I'm here to help you stay safe online in South Africa.\n" +
                "You can ask me about:\n" +
                "  • Password safety\n" +
                "  • Phishing attacks\n" +
                "  • Online scams & SIM swap fraud\n" +
                "  • Privacy & POPIA\n" +
                "  • Viruses & malware\n" +
                "  • Safe browsing\n" +
                "  • Suspicious links\n\n" +
                "To get started, what's your name?";

            _memoryManager.IsAwaitingName = true;
            _memoryManager.HasAskedForName = true;

            RaiseBotMessage(greeting);
        }

        /// <summary>
        /// Processes user input and generates appropriate responses.
        /// </summary>
        public void ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return;

            // Raise the user's message to UI
            OnNewMessage?.Invoke(input, true);

            string trimmed = input.Trim();
            string lower = trimmed.ToLower();

            // 1. Check if we're awaiting the user's name
            if (_memoryManager.IsAwaitingName)
            {
                HandleNameInput(trimmed);
                return;
            }

            // 2. Check for name introduction at any point
            if (_memoryManager.TryExtractName(trimmed))
            {
                RaiseBotMessage($"Nice to meet you, {_memoryManager.UserName}! How can I help you stay safe online today?");
                return;
            }

            // 3. Check for favorite topic extraction
            if (_memoryManager.TryExtractFavoriteTopic(lower, _validTopics))
            {
                string topicResponse = GetRandomResponse(_memoryManager.FavoriteTopic!);
                RaiseBotMessage($"Great choice, {_memoryManager.DisplayName}! I'll remember that you're interested in {_memoryManager.FavoriteTopic}. Here's a tip:\n\n{topicResponse}");
                _currentTopic = _memoryManager.FavoriteTopic;
                return;
            }

            // 4. Check for follow-up requests
            if (IsFollowUpRequest(lower))
            {
                HandleFollowUp();
                return;
            }

            // 5. Detect sentiment
            Sentiment sentiment = _sentimentAnalyzer.Analyze(lower);

            // 6. Match keywords
            string? matchedTopic = FindMatchingTopic(lower);

            // 7. Generate response based on sentiment + topic
            if (matchedTopic != null)
            {
                _currentTopic = matchedTopic;
                string tip = GetRandomResponse(matchedTopic);

                if (sentiment == Sentiment.Negative)
                {
                    string empathy = GetEmpathyResponse(_sentimentAnalyzer.GetDetectedNegativeKeyword(lower));
                    RaiseBotMessage($"{empathy}\n\nHere's a helpful tip about {matchedTopic}: {tip}");
                }
                else if (sentiment == Sentiment.Positive)
                {
                    RaiseBotMessage($"Love your enthusiasm, {_memoryManager.DisplayName}! 🌟 Here's what you should know about {matchedTopic}:\n\n{tip}");
                }
                else
                {
                    RaiseBotMessage(tip);
                }
            }
            else if (sentiment == Sentiment.Negative)
            {
                string empathy = GetEmpathyResponse(_sentimentAnalyzer.GetDetectedNegativeKeyword(lower));
                if (_currentTopic != null)
                {
                    string tip = GetRandomResponse(_currentTopic);
                    RaiseBotMessage($"{empathy}\n\nLet me share another tip about {_currentTopic}: {tip}");
                }
                else
                {
                    RaiseBotMessage($"{empathy}\n\nI'm here to help! You can ask me about passwords, phishing, scams, privacy, viruses, safe browsing, or suspicious links.");
                }
            }
            else if (sentiment == Sentiment.Positive)
            {
                RaiseBotMessage($"That's wonderful to hear, {_memoryManager.DisplayName}! 😊 Is there a cybersecurity topic you'd like to explore? I can help with passwords, phishing, scams, privacy, and more.");
            }
            else
            {
                // No keyword matched, no sentiment — default response
                HandleUnknownInput();
            }
        }

        // ── Private Helper Methods ───────────────────────────────────────

        private void HandleNameInput(string input)
        {
            string name = input.Trim().TrimEnd('.', '!', ',', '?');
            string[] words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length > 0)
            {
                string firstName = char.ToUpper(words[0][0]) + words[0].Substring(1).ToLower();
                _memoryManager.UserName = firstName;
                _memoryManager.IsAwaitingName = false;

                RaiseBotMessage($"Great to meet you, {firstName}! 🇿🇦\n\nI'm your cybersecurity assistant. What topic would you like to learn about? You can ask me about passwords, phishing, scams, privacy, viruses, safe browsing, or suspicious links.");
            }
            else
            {
                RaiseBotMessage("I didn't catch that. Could you please tell me your name?");
            }
        }

        private bool IsFollowUpRequest(string lower)
        {
            string[] followUpPhrases = {
                "give me another tip", "another tip", "tell me more",
                "explain more", "more tips", "more info", "go on",
                "continue", "what else", "anything else", "another one",
                "keep going", "more please", "more about"
            };

            foreach (string phrase in followUpPhrases)
            {
                if (lower.Contains(phrase))
                    return true;
            }
            return false;
        }

        private void HandleFollowUp()
        {
            if (_currentTopic != null)
            {
                string tip = GetRandomResponse(_currentTopic);
                string prefix = _memoryManager.HasFavoriteTopic && _memoryManager.FavoriteTopic == _currentTopic
                    ? $"Since you're interested in {_currentTopic}, {_memoryManager.DisplayName}, here's another tip:\n\n"
                    : $"Here's another tip about {_currentTopic}:\n\n";
                RaiseBotMessage(prefix + tip);
            }
            else
            {
                RaiseBotMessage($"I'd love to share more, {_memoryManager.DisplayName}! What topic are you interested in? Try asking about passwords, phishing, scams, privacy, viruses, safe browsing, or links.");
            }
        }

        private string? FindMatchingTopic(string lower)
        {
            foreach (var kvp in _keywordResponses)
            {
                if (lower.Contains(kvp.Key))
                    return kvp.Key;
            }
            return null;
        }

        private string GetRandomResponse(string topic)
        {
            if (_keywordResponses.TryGetValue(topic, out List<string>? responses) && responses.Count > 0)
            {
                int index = _random.Next(responses.Count);
                return responses[index];
            }
            return "I have information on that topic. Could you be more specific?";
        }

        private string GetEmpathyResponse(string? keyword)
        {
            string name = _memoryManager.DisplayName;
            return keyword switch
            {
                "worried" => $"It's completely understandable to feel worried, {name}. Cyber threats can seem overwhelming, but knowledge is your best defence.",
                "scared" => $"There's no need to be scared, {name}. I'm here to help you understand and protect yourself.",
                "frustrated" => $"I understand your frustration, {name}. Cybersecurity can be confusing, but we'll work through it together.",
                "confused" => $"No worries about feeling confused, {name}. These topics can be complex — let me break it down for you.",
                "afraid" => $"Don't be afraid, {name}. With the right knowledge, you can protect yourself effectively.",
                "stressed" => $"I can see this is stressful, {name}. Let's take it one step at a time.",
                "anxious" => $"It's natural to feel anxious, {name}. Let me help put your mind at ease with some practical advice.",
                _ => $"I hear you, {name}. Let me help with some practical advice."
            };
        }

        private void HandleUnknownInput()
        {
            RaiseBotMessage($"I'm not sure I understand, {_memoryManager.DisplayName}. Could you try rephrasing? I can help with passwords, phishing, privacy, scams, viruses, safe browsing, and suspicious links.");
        }

        private void RaiseBotMessage(string message)
        {
            OnNewMessage?.Invoke(message, false);
        }
    }
}
