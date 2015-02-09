// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbCommandExtensions.cs" company="TIMEmSYSTEM ApS">
//   © TIMEmSYSTEM 2015
// </copyright>
// <summary>
//   Class that contains extension methods that apply on <see cref="DbCommandExtensions"/>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Class that contains extension methods that apply on <see cref="DbCommandExtensions"/>.
    /// </summary>
    internal static class DbCommandExtensions
    {
        /// <summary>
        ///     The context info.
        /// </summary>
        private const string ContextInfo = @"-- set context_info
set quoted_identifier on
set arithabort off
set numeric_roundabort off
set ansi_warnings on
set ansi_padding on
set ansi_nulls on
set concat_null_yields_null on
set cursor_close_on_commit off
set implicit_transactions off
set language us_english
set dateformat dmy
set datefirst 1
set transaction isolation level read committed";

        /// <summary>
        ///     The maximum number of bytes in context info.
        /// </summary>
        private const int MaxContextInfo = 128;

        /// <summary>
        /// Sets context info for the current session.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        public static void SetContextInfo(this DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = GetCommandText();
                command.Connection = connection;
                command.ExecuteNonQuery();
                command.Connection = null;
            }
        }

        /// <summary>
        /// Creates context info in binary representation.
        /// </summary>
        /// <param name="claimsPrincipal">
        /// The claims principal.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string CreateBinaryContext(ClaimsPrincipal claimsPrincipal)
        {
            var bytes = new List<byte>(MaxContextInfo);
            var userId = default(int);
            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                int.TryParse(userIdClaim.Value, out userId);
            }

            bytes.AddRange(BitConverter.GetBytes(userId).Reverse());
            var binaryString = BitConverter.ToString(bytes.ToArray()).Replace("-", string.Empty);
            return string.Concat("0x", binaryString);
        }

        /// <summary>
        ///     Gets command to setup context info.
        /// </summary>
        /// <returns>
        ///     The <see cref="DbCommand" />.
        /// </returns>
        private static string GetCommandText()
        {
            var context = new StringBuilder(ContextInfo);
            var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                var contextInfo = CreateBinaryContext(claimsPrincipal);
                context.AppendLine().AppendFormat("set context_info {0}", contextInfo);
            }

            return context.ToString();
        }
    }
}