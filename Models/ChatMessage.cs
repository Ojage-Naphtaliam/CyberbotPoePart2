// ============================================================================
// ChatMessage.cs
// Model class representing a single chat message in the conversation.
// Used with ObservableCollection for UI data binding.
// ============================================================================

namespace CyberChat.WPF.Models
{
    /// <summary>
    /// Represents a single message in the chat conversation.
    /// Uses automatic properties for clean data binding.
    /// </summary>
    public class ChatMessage
    {
        /// <summary>The message text content.</summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>Whether this message was sent by the user (true) or bot (false).</summary>
        public bool IsUser { get; set; }

        /// <summary>Timestamp when the message was created.</summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>Display-friendly timestamp string.</summary>
        public string TimeDisplay => Timestamp.ToString("HH:mm");

        /// <summary>Sender label for display.</summary>
        public string Sender => IsUser ? "You" : "CyberBot";
    }
}
