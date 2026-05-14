// ============================================================================
// MemoryManager.cs
// Stores and retrieves user information (name, favorite topic) for
// personalized chatbot interactions.
// ============================================================================

namespace CyberChat.WPF.Logic
{
    /// <summary>
    /// Manages user memory including name and favorite cybersecurity topic.
    /// Uses automatic properties and provides methods for storing/retrieving data.
    /// </summary>
    public class MemoryManager
    {
        // ── Automatic Properties ──────────────────────────────────────────

        /// <summary>
        /// The user's name, stored when first provided.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// The user's favorite cybersecurity topic (e.g., "privacy", "passwords").
        /// </summary>
        public string? FavoriteTopic { get; set; }

        /// <summary>
        /// Whether the bot has already asked for the user's name.
        /// </summary>
        public bool HasAskedForName { get; set; }

        /// <summary>
        /// Whether we are currently waiting for the user to provide their name.
        /// </summary>
        public bool IsAwaitingName { get; set; }

        // ── Methods ───────────────────────────────────────────────────────

        /// <summary>
        /// Checks if the user's name has been stored.
        /// </summary>
        public bool HasUserName => !string.IsNullOrWhiteSpace(UserName);

        /// <summary>
        /// Checks if a favorite topic has been stored.
        /// </summary>
        public bool HasFavoriteTopic => !string.IsNullOrWhiteSpace(FavoriteTopic);

        /// <summary>
        /// Returns a display-friendly name or "friend" if unknown.
        /// </summary>
        public string DisplayName => HasUserName ? UserName! : "friend";

        /// <summary>
        /// Attempts to extract and store the user's name from input.
        /// Looks for patterns like "my name is X", "I'm X", "call me X".
        /// </summary>
        /// <param name="input">The user's input text.</param>
        /// <returns>True if a name was successfully extracted.</returns>
        public bool TryExtractName(string input)
        {
            string lower = input.ToLower().Trim();

            // Pattern: "my name is ..."
            string[] namePatterns = new[]
            {
                "my name is ", "i'm ", "i am ", "call me ",
                "they call me ", "you can call me ", "name's "
            };

            foreach (string pattern in namePatterns)
            {
                int index = lower.IndexOf(pattern);
                if (index >= 0)
                {
                    string namePart = input.Substring(index + pattern.Length).Trim();
                    // Take only the first word as the name
                    string[] words = namePart.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length > 0)
                    {
                        // Capitalize first letter
                        string name = char.ToUpper(words[0][0]) + words[0].Substring(1).ToLower();
                        // Remove trailing punctuation
                        name = name.TrimEnd('.', '!', ',', '?');
                        if (name.Length > 0)
                        {
                            UserName = name;
                            IsAwaitingName = false;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to extract a favorite topic from user input.
        /// Looks for patterns like "I'm interested in X", "I like X tips".
        /// </summary>
        /// <param name="input">The user's input text.</param>
        /// <param name="validTopics">List of valid cybersecurity topics.</param>
        /// <returns>True if a topic was successfully extracted.</returns>
        public bool TryExtractFavoriteTopic(string input, List<string> validTopics)
        {
            string lower = input.ToLower();

            string[] topicPatterns = new[]
            {
                "interested in ", "i like ", "i love ", "fascinated by ",
                "want to learn about ", "tell me about ", "curious about ",
                "passionate about "
            };

            foreach (string pattern in topicPatterns)
            {
                if (lower.Contains(pattern))
                {
                    foreach (string topic in validTopics)
                    {
                        if (lower.Contains(topic))
                        {
                            FavoriteTopic = topic;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Resets all stored user data.
        /// </summary>
        public void Reset()
        {
            UserName = null;
            FavoriteTopic = null;
            HasAskedForName = false;
            IsAwaitingName = false;
        }
    }
}
