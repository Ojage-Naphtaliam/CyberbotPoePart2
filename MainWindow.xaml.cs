// ============================================================================
// MainWindow.xaml.cs
// UI code-behind: handles Window events, subscribes to ChatbotEngine events,
// and updates the chat UI. All logic is delegated to engine classes.
// ============================================================================

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CyberChat.WPF.Logic;
using CyberChat.WPF.Models;

namespace CyberChat.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// Only handles UI events and delegates all logic to ChatbotEngine.
    /// Subscribes to ChatbotEngine.OnNewMessage delegate event.
    /// </summary>
    public partial class MainWindow : Window
    {
        // ── Private Fields ───────────────────────────────────────────────
        private readonly ChatbotEngine _chatEngine;
        private readonly ResourceLoader _resourceLoader;

        // Generic collection: ObservableCollection<ChatMessage> for chat history
        private readonly ObservableCollection<ChatMessage> _chatHistory;

        // ── Constructor ──────────────────────────────────────────────────
        public MainWindow()
        {
            InitializeComponent();

            // Initialize ObservableCollection and bind to ItemsControl
            _chatHistory = new ObservableCollection<ChatMessage>();
            ChatItemsControl.ItemsSource = _chatHistory;

            // Initialize logic classes
            _resourceLoader = new ResourceLoader();
            _chatEngine = new ChatbotEngine();

            // Subscribe to the ChatbotEngine delegate event
            _chatEngine.OnNewMessage += ChatEngine_OnNewMessage;
        }

        // ── Window Loaded Event ──────────────────────────────────────────

        /// <summary>
        /// Handles the Window Loaded event: loads resources, plays audio, sends greeting.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load ASCII art and display in header
            _resourceLoader.LoadAsciiArt();
            AsciiArtHeader.Text = _resourceLoader.AsciiLogo;

            // Load and play welcome audio
            _resourceLoader.LoadAudioPath();
            _resourceLoader.PlayWelcomeAudio();

            // Display any resource warnings in chat
            foreach (string warning in _resourceLoader.Warnings)
            {
                AddMessageToChat(warning, false);
            }

            // Send initial greeting from chatbot
            _chatEngine.SendGreeting();

            // Focus the input textbox
            UserInputTextBox.Focus();
        }

        // ── UI Event Handlers ────────────────────────────────────────────

        /// <summary>
        /// Handles Send button click — sends user input to the engine.
        /// </summary>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendUserMessage();
        }

        /// <summary>
        /// Handles Enter key press in the input TextBox.
        /// </summary>
        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendUserMessage();
                e.Handled = true;
            }
        }

        // ── Delegate Event Handler ───────────────────────────────────────

        /// <summary>
        /// Handles messages from ChatbotEngine via the NewMessageHandler delegate.
        /// Uses Dispatcher to ensure UI updates happen on the main thread.
        /// </summary>
        private void ChatEngine_OnNewMessage(string message, bool isUser)
        {
            // Use Dispatcher for thread-safe UI updates
            Dispatcher.Invoke(() =>
            {
                AddMessageToChat(message, isUser);
            });
        }

        // ── Private Helper Methods ───────────────────────────────────────

        /// <summary>
        /// Sends the current user input to the ChatbotEngine for processing.
        /// Validates input is not empty or whitespace.
        /// </summary>
        private void SendUserMessage()
        {
            string input = UserInputTextBox.Text;

            // Handle empty/whitespace input
            if (string.IsNullOrWhiteSpace(input))
                return;

            // Clear the input box
            UserInputTextBox.Clear();
            UserInputTextBox.Focus();

            // Process input through the engine (engine raises events for messages)
            _chatEngine.ProcessInput(input);
        }

        /// <summary>
        /// Adds a ChatMessage to the ObservableCollection and scrolls to bottom.
        /// </summary>
        private void AddMessageToChat(string text, bool isUser)
        {
            var message = new ChatMessage
            {
                Text = text,
                IsUser = isUser,
                Timestamp = DateTime.Now
            };

            _chatHistory.Add(message);

            // Auto-scroll to the latest message
            ChatScrollViewer.ScrollToEnd();
        }
    }
}