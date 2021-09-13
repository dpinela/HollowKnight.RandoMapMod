﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace RandoMapMod {
	public static class DebugLog {
		#region Statics
		static DebugLog() {
			try {
				using (var myFile = File.Create(_LogFile)) {
					// opens stream while creating fresh log file, then closes it
				}
			} catch {
				MapModS.Instance.LogWarn("RandoMapLog.log could not be created...");
			}
		}
		public enum Level {
			Log = 0,			//Regular debug stuff
			Warn = 1,			//Refactor or really minor/annoying things
			Error = 2,			//This is a problem, but might not need code changes to fix...
			Critical = 3		//This is crucial! An integral obstruction to the features of the mod.
		}

		private static string _logPath = "";
		private static string _LogPath {
			get {
				if (_logPath == "") {
					string codeBase = Assembly.GetExecutingAssembly().CodeBase;
					UriBuilder uri = new UriBuilder(codeBase);
					_logPath = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
				}
				return _logPath;
			}
		}
		//Mods folder! "hollow_knight_Data\Managed\Mods\[name].log"
		private static string _LogFile => _LogPath + @"/RandoMapMod.log";

		public static void Trace(Level level = Level.Error) {
			string msg = new StackTrace(1, true).ToString();
			_Write(level, msg);
			MapModS.Instance.LogError(msg);
		}

		public static void Critical(string msg, Exception inner) {
			Critical($"{msg}\n-----Inner Exception:-----\n{inner}\n-----");
		}

		public static void Critical(string msg) {
			msg += "\n" + new StackTrace(1, true).ToString();
			_Write(Level.Critical, msg);
			MapModS.Instance.LogError(msg);
		}

		public static void Error(string msg, Exception inner) {
			Error($"{msg}\n-----Inner Exception:-----\n{inner}\n-----");
		}

		public static void Error(string msg) {
			msg += "\n" + new StackTrace(1, true).ToString();
			_Write(Level.Error, msg);
			MapModS.Instance.LogError(msg);
		}

		public static void Log(string v) {
			_Write(Level.Log, v);
			MapModS.Instance.Log(v);
		}

		public static void Warn(string v) {
			_Write(Level.Warn, v);
			MapModS.Instance.LogWarn(v);
		}

		private static void _Write(Level level, string line) {
			if ((int) level > 2) {
				//Only send LOG and WARN if it's a debug build
#if !DEBUG
				return;
#endif
			}

			string levelString = level switch {
				Level.Warn => "WARN",
				Level.Error => "ERROR",
				_ => "LOG",
			};

			string nickName = _DetermineClassNickName();

			string msg = $"{DateTime.Now:HH:mm:ss tt} {levelString,5} {nameof(MapModS),12} - {line}";

			using StreamWriter writer = new StreamWriter(_LogFile, true);
			writer.WriteLine(msg);
		}

		private static string _DetermineClassNickName() {
			StackTrace st = new StackTrace(1);
			int i = 0;

			Type callingClass;
			do {
				callingClass = st.GetFrame(i).GetMethod().ReflectedType;
				i++;
			} while (callingClass == typeof(DebugLog));

			DebugNameAttribute[] attr = (DebugNameAttribute[]) callingClass.GetCustomAttributes(typeof(DebugNameAttribute), false);
			if (attr.Length > 0) {
				return attr[0].ToString();
			}

			return nameof(MapModS);
		}
		#endregion
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class DebugNameAttribute : Attribute {
		private readonly string _nickname;

		public DebugNameAttribute(string nick) {
			_nickname = nick;
		}

		public override string ToString() {
			return _nickname;
		}
	}
}