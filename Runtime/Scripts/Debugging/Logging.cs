using UnityEngine;

namespace VirrVarr
{
    public class Logging
    {
        // ------------------------------------------------------------
        // LOG
        // ------------------------------------------------------------
        /// <summary>
        /// Log for general messages about the runtime state.
        /// </summary>
        /// <param name="logObj">The object that logs the message.</param>
        /// <param name="msgFmt">The format string of the message (i.e "Example {0}"</param>
        /// <param name="fmt">OPTIONAL: The format types of the message.</param>
        public static void LogMsg( object logObj, string msgFmt, params object[] fmt )
        {
            Debug.Log(BuildLogMessage(logObj, msgFmt, fmt));
        }
        /// <summary>
        /// Log for general messages about the runtime state.
        /// </summary>
        /// <param name="msgFmt">The format string of the message (i.e "Example {0}"</param>
        /// <param name="fmt">OPTIONAL: The format types of the message.</param>
        public static void LogMsg(string msgFmt, params object[] fmt)
        {
            Debug.Log(BuildLogMessage(null, msgFmt, fmt));
        }
        // ------------------------------------------------------------

        // ------------------------------------------------------------
        // WARNING
        // ------------------------------------------------------------
        /// <summary>
        /// Log for non-critical messages about the runtime state.
        /// </summary>
        /// <param name="logObj">The object that logs the message.</param>
        /// <param name="msgFmt">The format string of the message (i.e "Example {0}"</param>
        /// <param name="fmt">OPTIONAL: The format types of the message.</param>
        public static void LogWarn(object logObj, string msgFmt, params object[] fmt)
        {
            Debug.LogWarning(BuildLogMessage(logObj, msgFmt, fmt));
        }
        /// <summary>
        /// Log for non-critical messages about the runtime state.
        /// </summary>
        /// <param name="msgFmt">The format string of the message (i.e "Example {0}"</param>
        /// <param name="fmt">OPTIONAL: The format types of the message.</param>
        public static void LogWarn(string msgFmt, params object[] fmt)
        {
            Debug.LogWarning(BuildLogMessage(null, msgFmt, fmt));
        }
        // ------------------------------------------------------------

        // ------------------------------------------------------------
        // ERROR
        // ------------------------------------------------------------
        /// <summary>
        /// Log for critical but recoverable messages about the runtime state.
        /// </summary>
        /// <param name="logObj">The object that logs the message.</param>
        /// <param name="msgFmt">The format string of the message (i.e "Example {0}"</param>
        /// <param name="fmt">OPTIONAL: The format types of the message.</param>
        public static void LogErr(object logObj, string msgFmt, params object[] fmt)
        {
            Debug.LogError(BuildLogMessage(logObj, msgFmt, fmt));
        }
        /// <summary>
        /// Log for critical but recoverable messages about the runtime state.
        /// </summary>
        /// <param name="msgFmt">The format string of the message (i.e "Example {0}"</param>
        /// <param name="fmt">OPTIONAL: The format types of the message.</param>
        public static void LogErr(string msgFmt, params object[] fmt)
        {
            Debug.LogError(BuildLogMessage(null, msgFmt, fmt));
        }
        // ------------------------------------------------------------

        // ------------------------------------------------------------
        // ASSERTION
        // ------------------------------------------------------------
        /// <summary>
        /// Log for critical and unrecoverable messages about the runtime state. Will also crash the game.
        /// </summary>
        /// <param name="logObj">The object that logs the message.</param>
        /// <param name="msgFmt">The format string of the message (i.e "Example {0}"</param>
        /// <param name="fmt">OPTIONAL: The format types of the message.</param>
        public static void LogAssert(object logObj, string msgFmt, params object[] fmt)
        {
            Debug.LogAssertion(BuildLogMessage(logObj, msgFmt, fmt));
        }
        /// <summary>
        /// Log for critical and unrecoverable messages about the runtime state. Will also crash the game.
        /// </summary>
        /// <param name="msgFmt">The format string of the message (i.e "Example {0}"</param>
        /// <param name="fmt">OPTIONAL: The format types of the message.</param>
        public static void LogAssert(string msgFmt, params object[] fmt)
        {
            Debug.LogAssertion(BuildLogMessage(null, msgFmt, fmt));
        }
        // ------------------------------------------------------------

        private static string BuildLogMessage( object logObj, string msgFmt, params object[] fmt )
        {
            string formatedMsg = string.Format(msgFmt, fmt);

            if (logObj != null)
            {
                return string.Format("{0}: {1}", logObj.GetType().Name, formatedMsg);
            }
            
            return string.Format("VirrVarr: {0}", formatedMsg);
        }
    }
}