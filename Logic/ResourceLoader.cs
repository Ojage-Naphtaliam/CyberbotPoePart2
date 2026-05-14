// ============================================================================
// ResourceLoader.cs
// Handles loading external resources (ASCII art logo, audio file paths).
// Gracefully handles missing files without crashing.
// ============================================================================

using System.IO;
using System.Media;

namespace CyberChat.WPF.Logic
{
    /// <summary>
    /// Loads and manages application resources such as ASCII art and audio files.
    /// Handles missing files gracefully to prevent application crashes.
    /// </summary>
    public class ResourceLoader
    {
        // ── Automatic Properties ──────────────────────────────────────────

        /// <summary>
        /// The loaded ASCII art logo text.
        /// </summary>
        public string AsciiLogo { get; private set; }

        /// <summary>
        /// The full path to the welcome audio file.
        /// </summary>
        public string WelcomeAudioPath { get; private set; }

        /// <summary>
        /// Whether the welcome audio file exists and can be played.
        /// </summary>
        public bool IsAudioAvailable { get; private set; }

        /// <summary>
        /// Any warning messages generated during resource loading.
        /// </summary>
        public List<string> Warnings { get; private set; }

        // ── Constructor ───────────────────────────────────────────────────

        public ResourceLoader()
        {
            AsciiLogo = GetFallbackLogo();
            WelcomeAudioPath = string.Empty;
            IsAudioAvailable = false;
            Warnings = new List<string>();
        }

        // ── Methods ───────────────────────────────────────────────────────

        /// <summary>
        /// Loads the ASCII art logo from the Resources folder.
        /// Falls back to a built-in logo if the file is missing.
        /// </summary>
        public void LoadAsciiArt()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string logoPath = Path.Combine(basePath, "Resources", "ascii_logo.txt");

                if (File.Exists(logoPath))
                {
                    AsciiLogo = File.ReadAllText(logoPath);
                }
                else
                {
                    Warnings.Add("⚠ ASCII logo file not found. Using built-in logo.");
                    AsciiLogo = GetFallbackLogo();
                }
            }
            catch (Exception ex)
            {
                Warnings.Add($"⚠ Could not load ASCII logo: {ex.Message}");
                AsciiLogo = GetFallbackLogo();
            }
        }

        /// <summary>
        /// Locates and validates the welcome.wav audio file.
        /// </summary>
        public void LoadAudioPath()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string audioPath = Path.Combine(basePath, "Resources", "welcome.wav");

                if (File.Exists(audioPath))
                {
                    WelcomeAudioPath = audioPath;
                    IsAudioAvailable = true;
                }
                else
                {
                    Warnings.Add("⚠ welcome.wav not found in Resources folder. Voice greeting disabled. Please add a welcome.wav file to the Resources folder.");
                    IsAudioAvailable = false;
                }
            }
            catch (Exception ex)
            {
                Warnings.Add($"⚠ Could not locate audio file: {ex.Message}");
                IsAudioAvailable = false;
            }
        }

        /// <summary>
        /// Attempts to play the welcome audio using System.Media.SoundPlayer.
        /// Handles errors gracefully.
        /// </summary>
        public void PlayWelcomeAudio()
        {
            if (!IsAudioAvailable)
                return;

            try
            {
                SoundPlayer player = new SoundPlayer(WelcomeAudioPath);
                player.Play(); // Asynchronous playback
            }
            catch (Exception ex)
            {
                Warnings.Add($"⚠ Could not play welcome audio: {ex.Message}");
            }
        }

        /// <summary>
        /// Returns a fallback ASCII art logo if the file cannot be loaded.
        /// </summary>
        private string GetFallbackLogo()
        {
            return @"
╔═══════════════════════════════════════════════════════╗
║            ██████╗██╗   ██╗██████╗ ███████╗██████╗   ║
║           ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗  ║
║           ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝  ║
║           ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗  ║
║           ╚██████╗   ██║   ██████╔╝███████╗██║  ██║  ║
║            ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝  ║
║                                                       ║
║       ╔══════╗   CYBERSECURITY AWARENESS BOT          ║
║       ║ (\/) ║   Protecting South Africa Online       ║
║       ║ /  \ ║   ─────────────────────────────        ║
║       ╚══════╝   Stay Safe. Stay Informed.            ║
╚═══════════════════════════════════════════════════════╝";
        }
    }
}
