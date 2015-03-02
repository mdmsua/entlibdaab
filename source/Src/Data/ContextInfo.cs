// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextInfo.cs" company="TIMEmSYSTEM ApS">
//   © TIMEmSYSTEM 2015
// </copyright>
// <summary>
//   The context info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    using System.Data;
    using System.Data.Common;
    using System.Security.Claims;
    using System.Threading;

    /// <summary>
    ///     The context info.
    /// </summary>
    public static class ContextInfo
    {
        /// <summary>
        ///     The stored procedure name.
        /// </summary>
        private const string CommandText = "log.SaveContext";

        /// <summary>
        /// Sets context info and executes stored procedure on the connection.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        public static void Set(DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                AssignParameters(command);
                command.CommandText = CommandText;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.ExecuteNonQuery();
                command.Connection = null;
            }
        }

        /// <summary>
        /// Assigns parameters to a command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        private static void AssignParameters(DbCommand command)
        {
            var claimsIdentity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return;
            }

            var user = FindFirstValue(claimsIdentity, claimsIdentity.NameClaimType);
            var role = FindFirstValue(claimsIdentity, claimsIdentity.RoleClaimType);
            var system = FindFirstValue(claimsIdentity, ClaimTypes.System);

            var userParameter = command.CreateParameter();
            userParameter.DbType = DbType.String;
            userParameter.Direction = ParameterDirection.Input;
            userParameter.IsNullable = true;
            userParameter.ParameterName = "@User";
            userParameter.Value = user;
            command.Parameters.Add(userParameter);

            var roleParameter = command.CreateParameter();
            roleParameter.DbType = DbType.String;
            roleParameter.Direction = ParameterDirection.Input;
            roleParameter.IsNullable = true;
            roleParameter.ParameterName = "@Role";
            roleParameter.Value = role;
            command.Parameters.Add(roleParameter);

            var customerParameter = command.CreateParameter();
            customerParameter.DbType = DbType.String;
            customerParameter.Direction = ParameterDirection.Input;
            customerParameter.IsNullable = true;
            customerParameter.ParameterName = "@Customer";
            customerParameter.Value = system;
            command.Parameters.Add(customerParameter);
        }

        /// <summary>
        /// Finds first value.
        /// </summary>
        /// <param name="claimsIdentity">
        /// The claims identity.
        /// </param>
        /// <param name="claimType">
        /// The claim type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string FindFirstValue(ClaimsIdentity claimsIdentity, string claimType)
        {
            var claim = claimsIdentity.FindFirst(claimType);
            return claim == null ? null : claim.Value;
        }
    }
}