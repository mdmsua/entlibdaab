// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Configuration;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly: AssemblyTitle("Enterprise Library Data Access Application Block")]
[assembly: AssemblyDescription("Enterprise Library Data Access Application Block")]
[assembly: AssemblyVersion("1.3.0.0")]
[assembly: AssemblyFileVersion("1.3.0.0")]
[assembly: AssemblyInformationalVersion("1.2.0-release")]

[assembly: SecurityTransparent]

[assembly: ComVisible(false)]

[assembly: HandlesSection(DataAccessDesignTime.ConnectionStringSettingsSectionName)]
[assembly: HandlesSection(DatabaseSettings.SectionName, ClearOnly = true)]

[assembly: AddApplicationBlockCommand(
            DataAccessDesignTime.ConnectionStringSettingsSectionName,
            typeof(ConnectionStringsSection),
            TitleResourceName = "AddDataSettings",
            TitleResourceType = typeof(DesignResources),
            CommandModelTypeName = DataAccessDesignTime.CommandTypeNames.AddDataAccessBlockCommand)]
