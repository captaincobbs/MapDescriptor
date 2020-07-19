using System;
using System.Collections.Generic;

namespace MapDescriptorTest
{
    /// <summary>
    /// Provides a logging mechanism for exceptions across the repository.
    /// </summary>
    public static class LogUtilities
    {
        #region Members
        /// <summary>
        /// A string containing all logs, separated with newlines.
        /// </summary>
        private static readonly List<string> log;
        #endregion

        #region Constructors
        static LogUtilities()
        {
            log = new List<string>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Includes a date-prefixed message in the log.
        /// </summary>
        public static void Log(string message)
        {
            log.Add(PrepareMessage(message));

            if (log.Count == 100)
            {
                Flush();
            }
        }

        /// <summary>
        /// Generates a date-prefixed message with the exception details in the log, then rethrows.
        /// </summary>
        /// <param name="ex">The exception object to generate logging for.</param>
        public static void Log(Exception ex, string startMessage = null)
        {
            string message = $"{startMessage} " ?? "" +
                $"Log caught exception: {ex.Message}," +
                $"with inner exception: {ex.InnerException?.Message ?? "null"}," +
                $"with stack trace: {ex.StackTrace}";

            Log(message);
        }

        /// <summary>
        /// If an exception occurs while executing the action, generates a date-prefixed message
        /// with the exception details in the log, then rethrows.
        /// </summary>
        /// <param name="action">The action to execute with logging.</param>
        public static void Log(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                Log(ex);
                throw;
            }
        }

        /// <summary>
        /// Saves all logging to a file and resets the running log.
        /// </summary>
        public static void Flush()
        {
            // Nothing to flush
            if (log?.Count == 0)
            {
                return;
            }

            // Append all strings and flush.
            string logAsString = "";
            log.ForEach((str) => logAsString += str);

            try
            {
                FileUtilities.SaveLog(logAsString);
            }
            catch (Exception)
            {
                // The log has failed to save, it isn't important enough to alert the user
            }

            log.Clear();
        }

        /// <summary>
        /// Returns a date-prefixed message (local timezone).
        /// </summary>
        /// <param name="message">The string to be logged.</param>
        private static string PrepareMessage(string message)
        {
            return $"date: {DateTime.Now.ToString()}, message: {message}" + Environment.NewLine + Environment.NewLine;
        }
        #endregion
    }
}
