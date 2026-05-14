// ============================================================================
// SentimentAnalyzer.cs
// Detects user mood from input text using keyword matching.
// Returns a Sentiment enum value (Positive, Negative, Neutral).
// ============================================================================

namespace CyberChat.WPF.Logic
{
    /// <summary>
    /// Enum representing detected user sentiment.
    /// </summary>
    public enum Sentiment
    {
        Neutral,
        Positive,
        Negative
    }

    /// <summary>
    /// Analyzes user input to detect emotional sentiment using keyword lists.
    /// </summary>
    public class SentimentAnalyzer
    {
        // Generic collection: List<string> for sentiment keyword storage
        private readonly List<string> _negativeKeywords;
        private readonly List<string> _positiveKeywords;

        /// <summary>
        /// Initializes the analyzer with predefined sentiment keyword lists.
        /// </summary>
        public SentimentAnalyzer()
        {
            _negativeKeywords = new List<string>
            {
                "worried", "scared", "frustrated", "confused",
                "afraid", "stressed", "anxious", "upset",
                "nervous", "helpless", "angry", "annoyed",
                "overwhelmed", "terrified", "concerned"
            };

            _positiveKeywords = new List<string>
            {
                "happy", "curious", "excited", "interested",
                "great", "good", "wonderful", "fantastic",
                "awesome", "confident", "eager", "glad",
                "pleased", "thankful", "motivated"
            };
        }

        /// <summary>
        /// Analyzes the input string and returns the detected sentiment.
        /// Checks negative keywords first (to prioritize empathy).
        /// </summary>
        /// <param name="input">The user's input text (lowercased).</param>
        /// <returns>A Sentiment enum value.</returns>
        public Sentiment Analyze(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Sentiment.Neutral;

            string lower = input.ToLower();

            // Check negative sentiment first to prioritize empathetic response
            foreach (string keyword in _negativeKeywords)
            {
                if (lower.Contains(keyword))
                    return Sentiment.Negative;
            }

            // Check positive sentiment
            foreach (string keyword in _positiveKeywords)
            {
                if (lower.Contains(keyword))
                    return Sentiment.Positive;
            }

            return Sentiment.Neutral;
        }

        /// <summary>
        /// Returns the specific negative keyword found in input for tailored responses.
        /// </summary>
        public string? GetDetectedNegativeKeyword(string input)
        {
            string lower = input.ToLower();
            foreach (string keyword in _negativeKeywords)
            {
                if (lower.Contains(keyword))
                    return keyword;
            }
            return null;
        }

        /// <summary>
        /// Returns the specific positive keyword found in input for tailored responses.
        /// </summary>
        public string? GetDetectedPositiveKeyword(string input)
        {
            string lower = input.ToLower();
            foreach (string keyword in _positiveKeywords)
            {
                if (lower.Contains(keyword))
                    return keyword;
            }
            return null;
        }
    }
}
