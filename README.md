# 🛡️ CyberChat — Cybersecurity Awareness Bot

[![.NET CI Build](../../actions/workflows/dotnet-ci.yml/badge.svg)](../../actions/workflows/dotnet-ci.yml)

A **WPF desktop chatbot application** built in C# (.NET 8) to educate South African citizens about cybersecurity threats including phishing, scams, SIM swap fraud, and online privacy.

Developed as a Portfolio of Evidence (POE) assignment for a cybersecurity awareness campaign.

---

## ✨ Features

| Feature | Description |
|---|---|
| 🎨 **Professional GUI** | Dark cybersecurity-themed WPF interface with gradient colours, rounded message bubbles, and distinct user/bot styling |
| 🔑 **Keyword Recognition** | Detects 7 cybersecurity topics (password, phishing, scam, privacy, virus, safe browsing, links) using `Dictionary<string, List<string>>` |
| 🎲 **Random Responses** | Multiple tips per topic, randomly selected using `Random` for varied interactions |
| 🧠 **Memory & Recall** | Remembers user's name and favourite topic via `MemoryManager` class |
| 💬 **Conversation Flow** | Handles follow-ups like "tell me more" and "give me another tip" naturally |
| 😊 **Sentiment Detection** | Detects worried/scared/excited moods and responds with empathy or encouragement |
| 🔊 **Voice Greeting** | Plays `welcome.wav` on startup using `System.Media.SoundPlayer` |
| 🎭 **ASCII Art Logo** | Cybersecurity-themed ASCII banner displayed in the header |
| 🇿🇦 **SA Context** | Tips reference SARS, SABRIC, POPIA, SIM swaps, EFT fraud, and local brands |

---

## 📁 Project Structure

```
cyberbotpart2/
├── .github/
│   └── workflows/
│       └── dotnet-ci.yml          # GitHub Actions CI pipeline
├── Logic/
│   ├── ChatbotEngine.cs           # Core chatbot logic, keyword matching, delegates
│   ├── MemoryManager.cs           # User name & favourite topic storage
│   ├── ResourceLoader.cs          # ASCII art & audio file loading
│   └── SentimentAnalyzer.cs       # Mood detection (positive/negative/neutral)
├── Models/
│   └── ChatMessage.cs             # Chat message data model
├── Resources/
│   ├── ascii_logo.txt             # ASCII art logo file
│   └── welcome.wav                # Voice greeting audio (user must add)
├── App.xaml                       # Application resources & theme colours
├── App.xaml.cs                    # Application entry point
├── MainWindow.xaml                # WPF UI layout (XAML)
├── MainWindow.xaml.cs             # UI event handlers (code-behind)
├── cyberbotpart2.csproj           # .NET 8 WPF project file
├── cyberbotpart2.slnx             # Solution file
└── README.md                     # This file
```

---

## 🏗️ OOP & Code Architecture

### Classes & Responsibilities
- **`ChatbotEngine`** — Core logic: keyword matching via `Dictionary<string, List<string>>`, random response selection, sentiment-aware responses, memory integration
- **`MemoryManager`** — Stores/retrieves `UserName` and `FavoriteTopic` using automatic properties
- **`SentimentAnalyzer`** — Returns `Sentiment` enum (Positive, Negative, Neutral) from keyword analysis
- **`ResourceLoader`** — Loads ASCII art from file, validates audio path, plays WAV greeting
- **`ChatMessage`** — Data model with automatic properties for UI binding
- **`MainWindow`** — UI-only: subscribes to engine events, handles button/key events

### Key C# Features Used
| Feature | Usage |
|---|---|
| **Delegates & Events** | `NewMessageHandler` delegate; `ChatbotEngine.OnNewMessage` event |
| **Generic Collections** | `Dictionary<string, List<string>>`, `List<string>`, `ObservableCollection<ChatMessage>` |
| **Automatic Properties** | All model and manager classes |
| **Enums** | `Sentiment` enum in `SentimentAnalyzer` |
| **Pattern Matching** | Switch expressions in empathy responses |
| **OOP Composition** | Engine composes `MemoryManager` and `SentimentAnalyzer` |

---

## 🚀 Setup & Run

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Windows 10/11 (WPF requires Windows)

### Steps
1. **Clone the repository:**
   ```bash
   git clone https://github.com/YOUR-USERNAME/cyberbotpart2.git
   cd cyberbotpart2
   ```

2. **Add welcome audio (optional):**
   - Place a `welcome.wav` file in the `Resources/` folder
   - The app works without it (graceful fallback with warning message)

3. **Build and run:**
   ```bash
   dotnet build
   dotnet run
   ```

---

## 📸 Screenshots

<!-- Add screenshots of the running application here -->
*Screenshot of the CyberChat bot interface will be added after first run.*

---

## 🔄 CI/CD

This project uses **GitHub Actions** for continuous integration. The workflow:
1. Checks out the code
2. Sets up .NET 8
3. Restores NuGet packages
4. Builds the project in Release mode
5. Runs any tests

<!-- ![CI Badge](../../actions/workflows/dotnet-ci.yml/badge.svg) -->

---

## 📝 Git Commit History Guide

| # | Commit Message | Changes |
|---|---|---|
| 1 | `Initial commit: Set up WPF project structure and MainWindow` | Project file, App.xaml, empty MainWindow, folder structure |
| 2 | `Added ChatbotEngine core logic with keyword recognition` | ChatbotEngine.cs with keyword dictionary and random responses |
| 3 | `Implemented MemoryManager for name and topic recall` | MemoryManager.cs with name extraction and topic storage |
| 4 | `Added SentimentAnalyzer with empathetic response logic` | SentimentAnalyzer.cs with mood keyword detection |
| 5 | `Integrated voice greeting and ASCII art header in GUI` | ResourceLoader.cs, MainWindow UI, welcome.wav handling, ASCII art |
| 6 | `Set up GitHub Actions CI workflow and final code optimisation` | dotnet-ci.yml, README.md, final polish |

---

## 📄 Licence

This project is developed for educational purposes as part of a cybersecurity awareness POE assignment.

---

> 🛡️ **Stay Safe. Stay Informed.** — Protecting South Africa Online
