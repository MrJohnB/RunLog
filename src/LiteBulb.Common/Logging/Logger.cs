using LiteBulb.Common.Logging.Abstract;
using System;
using System.IO;

namespace LiteBulb.Common.Logging
{
	/// <summary>
	/// Simple logger for writing logs to files.
	/// </summary>
	public class Logger : ILogger
	{
		// To make file access in this class thread safe
		private static readonly object SyncLock = new object();

		// Path of log file
		private readonly string _filePath;

		/// <summary>
		/// Instantiates an instance of Logger.
		/// </summary>
		/// <param name="categoryName">Category name for this logger instance</param>
		/// <param name="logFilePath">Path where log file gets written to</param>
		public Logger(string categoryName, string logFilePath)
		{
			// Set file path member (Check whether file path has been set)
			_filePath = string.IsNullOrWhiteSpace(logFilePath) ? Path.Combine(Directory.GetCurrentDirectory(), "Log", $"{categoryName}.log") : logFilePath;

			// Create any needed subdirectories (unless they already exist)
			Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
		}

		/// <summary>
		/// Write an information message to the application log file.
		/// </summary>
		/// <param name="message">Message to write to the log</param>
		public void LogInformation(string message)
		{
			// Prepend date to beginning of log entry
			var time = DateTime.Now;
			var contents = $"[{time}] {message}" + Environment.NewLine;

			lock (SyncLock) // Allow access via only one thread
			{
				// Append line to log file (create file if doesn't already exist)
				File.AppendAllText(_filePath, contents);
			}
		}

		/// <summary>
		/// Write an error message to the application log file.
		/// </summary>
		/// <param name="message">Error message to write to the log</param>
		public void LogError(string message)
		{
			// Call the main logging method
			LogInformation("*** An error occured *** " + message);
		}

		/// <summary>
		/// Write an exception and error message to the application log file.
		/// Example: logger.LogError(exception, "Error while processing request from {Address}", address)
		/// </summary>
		/// <param name="exception">The exception to log</param>
		/// <param name="message">Format string of the log message in message template format</param>
		/// <param name="args">An object array that contains zero or more objects to format</param>
		public void LogError(Exception exception, string message, params object[] args)
		{
			// Build error message
			var errorMessage =
				"*** An error occured *** " + Environment.NewLine +
				message + Environment.NewLine +
				$"Exception Message: {exception.Message}" + Environment.NewLine +
				$"Source: {exception.Source}" +
				// Tack on inner exception messages:
				GetInnerExceptions(exception) + Environment.NewLine +
				$"Stack Trace: {exception.StackTrace}";

			// Call the main logging method
			LogInformation(errorMessage);
		}

		/// <summary>
		/// Recursively build message to capture all inner exceptions.
		/// </summary>
		/// <param name="exception">The exception to check for inner exceptions</param>
		/// <returns>A concatenated string of all nested inner exceptions</returns>
		private string GetInnerExceptions(Exception exception)
		{
			if (exception.InnerException != null)
			{
				return Environment.NewLine +
					   $"Inner Exception: {exception.InnerException.Message}" +
					   GetInnerExceptions(exception.InnerException);
			}

			return string.Empty;
		}
	}

	/// <summary>
	/// Simple logger for writing logs to files (generic version).
	/// </summary>
	public class Logger<TCategoryName> : Logger, ILogger<TCategoryName>
	{
		/// <summary>
		/// Instantiates an instance of Logger (generic version).
		/// </summary>
		/// <param name="logFilePath">Path where log file gets written to</param>
		public Logger(string logFilePath) : base(typeof(TCategoryName).FullName, logFilePath)
		{ }
	}
}
