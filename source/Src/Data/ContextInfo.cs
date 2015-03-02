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
    using System;
    using System.Data;
    using System.Data.Common;

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
        /// The user.
        /// </summary>
        [ThreadStatic]
        private static string user;

        /// <summary>
        /// The customer.
        /// </summary>
        [ThreadStatic]
        private static string customer;

        /// <summary>
        /// The role.
        /// </summary>
        [ThreadStatic]
        private static string role;

        /// <summary>
        /// Sets context info.
        /// </summary>
        /// <param name="userName">
        /// The user.
        /// </param>
        /// <param name="customerAbbreviation">
        /// The customer.
        /// </param>
        /// <param name="roleKey">
        /// The role.
        /// </param>
        public static void Set(string userName = null, string customerAbbreviation = null, string roleKey = null)
        {
            user = userName;
            customer = customerAbbreviation;
            role = roleKey;
        }

        /// <summary>
        /// Sets context info and executes stored procedure on the connection.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        internal static void Set(DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                SetParameters(command);
                command.CommandText = CommandText;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                command.ExecuteNonQuery();
                command.Connection = null;
            }
        }

        /// <summary>
        /// The set parameters.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        private static void SetParameters(DbCommand command)
        {
            var userParameter = command.CreateParameter();
            userParameter.DbType = DbType.String;
            userParameter.Direction = ParameterDirection.Input;
            userParameter.IsNullable = false;
            userParameter.ParameterName = "@User";
            userParameter.Value = user;
            command.Parameters.Add(userParameter);

            var customerParameter = command.CreateParameter();
            customerParameter.DbType = DbType.String;
            customerParameter.Direction = ParameterDirection.Input;
            customerParameter.IsNullable = false;
            customerParameter.ParameterName = "@Customer";
            customerParameter.Value = customer;
            command.Parameters.Add(customerParameter);

            var roleParameter = command.CreateParameter();
            roleParameter.DbType = DbType.String;
            roleParameter.Direction = ParameterDirection.Input;
            roleParameter.IsNullable = false;
            roleParameter.ParameterName = "@Role";
            roleParameter.Value = role;
            command.Parameters.Add(roleParameter);
        }
    }
}