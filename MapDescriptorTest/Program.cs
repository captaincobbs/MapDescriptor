using System;

namespace MapDescriptorTest
{
    /// <summary>
    /// The entry point to the program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The instance of the program.
        /// </summary>
        private static Game1 program;

        [STAThread]
        static void Main()
        {
            using (program = new Game1())
            {
                program.Exiting += (a, b) => LogUtilities.Flush();
                program.Run();
            }
        }

        /// <summary>
        /// Dumps the current log and exits the program. This should be used only for exceptions that
        /// cannot be handled gracefully during the program.
        /// </summary>
        public static void Exit()
        {
            program.Exit();
        }
    }
}