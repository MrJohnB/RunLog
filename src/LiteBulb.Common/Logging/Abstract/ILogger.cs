using System;

namespace LiteBulb.Common.Logging.Abstract
{
	/// <summary>
	/// Interface for simple logger for writing logs to files.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Write an information message to the application log file.
		/// </summary>
		/// <param name="message">Message to write to the log</param>
		void LogInformation(string message);

		/// <summary>
		/// Write an error message to the application log file.
		/// </summary>
		/// <param name="message">Error message to write to the log</param>
		void LogError(string message);

		/// <summary>
		/// Write an exception and error message to the application log file.
		/// Example: logger.LogError(exception, "Error while processing request from {Address}", address)
		/// </summary>
		/// <param name="exception">The exception to log</param>
		/// <param name="message">Format string of the log message in message template format</param>
		/// <param name="args">An object array that contains zero or more objects to format</param>
		void LogError(Exception exception, string message, params object[] args);
	}

	/// <summary>
	/// Interface for simple logger for writing logs to files (generic version).
	/// </summary>
	public interface ILogger<TCategoryName> : ILogger
	{ }
}
