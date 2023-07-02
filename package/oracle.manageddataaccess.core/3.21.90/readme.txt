Oracle.ManagedDataAccess.Core NuGet Package 3.21.90 README
==========================================================
Release Notes: Oracle Data Provider for .NET Core

January 2023

This provider supports .NET Core 3.1 and .NET 6.

This README supplements the main ODP.NET 21c documentation.
https://docs.oracle.com/en/database/oracle/oracle-database/21/odpnt/


Bug Fixes since Oracle.ManagedDataAccess.Core NuGet Package 3.21.80
===================================================================
Bug 31456063 ORA-03111 IS ENCOUNTERED WHILE CANCELLING THE CURRENT COMMAND EXECUTION
Bug 31793997 INCORRECT UDTS ARE RETURNED BY DATAREADER AFTER A NULL UDT IS FETCHED 
Bug 34431232 CURRENT DATABASE EDITION NAME IS NOT RETURNED BY ORACLECONNECTION
Bug 34617083 RESOLVE CVE-2023-21893


Known Issues and Limitations
============================
1) BindToDirectory throws NullReferenceException on Linux when LdapConnection AuthType is Anonymous

https://github.com/dotnet/runtime/issues/61683

This issue is observed when using System.DirectoryServices.Protocols, version 6.0.0.
To workaround the issue, use System.DirectoryServices.Protocols, version 5.0.1.

 Copyright (c) 2021, 2023, Oracle and/or its affiliates. 
