﻿$ErrorActionPreference = 'Stop'

#
# Add-Migration
#

Register-TabExpansion Add-Migration @{
    OutputDir = { <# Disabled. Otherwise, paths would be relative to the solution directory. #> }
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Adds a new migration.

.DESCRIPTION
    Adds a new migration.

.PARAMETER Name
    The name of the migration.

.PARAMETER OutputDir
    The directory to put files in. Paths are relative to the project directory. Defaults to "Migrations".

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Namespace
    The namespace to use. Matches the directory by default.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Remove-Migration
    Update-Database
    about_EntityFrameworkCore
#>
function Add-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(Position = 0, Mandatory = $true)]
        [string] $Name,
        [string] $OutputDir,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Namespace,
        [string] $Args)

    WarnIfEF6 'Add-Migration'

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'migrations', 'add', $Name, '--json'

    if ($OutputDir)
    {
        $params += '--output-dir', $OutputDir
    }

    if ($Namespace)
    {
        $params += '--namespace', $Namespace
    }

    $params += GetParams $Context

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    $result = (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json
    Write-Host 'To undo this action, use Remove-Migration.'

    if (!(IsCpsProject $dteProject) -or (GetCpsProperty $dteProject 'EnableDefaultItems') -ne 'true' -or (GetCpsProperty $dteProject 'EnableDefaultCompileItems') -ne 'true')
    {
       $dteProject.ProjectItems.AddFromFile($result.migrationFile) | Out-Null

       $dteProject.ProjectItems.AddFromFile($result.metadataFile) | Out-Null

       $dteProject.ProjectItems.AddFromFile($result.snapshotFile) | Out-Null
    }

    $DTE.ItemOperations.OpenFile($result.migrationFile) | Out-Null
    ShowConsole
}

#
# Bundle-Migration
#

Register-TabExpansion Bundle-Migration @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Creates an executable to update the database.

.DESCRIPTION
    Creates an executable to update the database.

.PARAMETER Output
    The path of executable file to create.

.PARAMETER Force
    Overwrite existing files.

.PARAMETER SelfContained
    Also bundle the .NET runtime so it doesn't need to be installed on the machine.

.PARAMETER TargetRuntime
    The target runtime to bundle for.

.PARAMETER Framework
    The target framework. Defaults to the first one in the project.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Script-Migration
    Update-Database
    about_EntityFrameworkCore
#>
function Bundle-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $Output,
        [switch] $Force,
        [switch] $SelfContained,
        [string] $TargetRuntime,
        [string] $Framework,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    if (!$Framework)
    {
        $Framework = GetProperty $dteStartupProject.Properties 'FriendlyTargetFramework'
    }

    $params = 'migrations', 'bundle', '--framework', $Framework

    if ($Output)
    {
        $params += '--output', $Output
    }

    if ($Force)
    {
        $params += '--force'
    }

    if ($SelfContained)
    {
        $params += '--self-contained'
    }

    if ($TargetRuntime)
    {
        $params += '--target-runtime', $TargetRuntime
    }

    $params += GetParams $Context

    EF $dteProject $dteStartupProject $params $Args
}

#
# Drop-Database
#

Register-TabExpansion Drop-Database @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Drops the database.

.DESCRIPTION
    Drops the database.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Update-Database
    about_EntityFrameworkCore
#>
function Drop-Database
{
    [CmdletBinding(PositionalBinding = $false, SupportsShouldProcess = $true, ConfirmImpact = 'High')]
    param(
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $info = Get-DbContext -Context $Context -Project $Project -StartupProject $StartupProject

    if ($PSCmdlet.ShouldProcess("database '$($info.databaseName)' on server '$($info.dataSource)'"))
    {
        $params = 'database', 'drop', '--force'
        $params += GetParams $Context

        EF $dteProject $dteStartupProject $params $Args -skipBuild
    }
}

#
# Enable-Migrations (Obsolete)
#

function Enable-Migrations
{
    WarnIfEF6 'Enable-Migrations'
    Write-Warning 'Enable-Migrations is obsolete. Use Add-Migration to start using Migrations.'
}

#
# Get-DbContext
#

Register-TabExpansion Get-DbContext @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Lists and gets information about available DbContext types.

.DESCRIPTION
    Lists and gets information about available DbContext types.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    about_EntityFrameworkCore
#>
function Get-DbContext
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    if ($PSBoundParameters.ContainsKey('Context'))
    {
       $params = 'dbcontext', 'info', '--json'
       $params += GetParams $Context

       # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
       return (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json
    }
    else
    {
       $params = 'dbcontext', 'list', '--json'
       # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
       # NB: Write-Output is here to break the JSON array into individual items
       return (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json | Write-Output | Format-Table -Property safeName -HideTableHeaders
    }
}

#
# Get-Migration
#

Register-TabExpansion Get-Migration @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Lists available migrations.

.DESCRIPTION
    Lists available migrations.

.PARAMETER Connection
    The connection string to the database. Defaults to the one specified in AddDbContext or OnConfiguring.

.PARAMETER NoConnect
    Don't connect to the database.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Add-Migration
    Remove-Migration
    Update-Database
    about_EntityFrameworkCore
#>
function Get-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $Connection,
        [switch] $NoConnect,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'migrations', 'list', '--json'

    if ($Connection)
    {
        $params += '--connection', $Connection
    }

    if ($NoConnect)
    {
        $params += '--no-connect'
    }

    $params += GetParams $Context

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    # NB: Write-Output is here to break the JSON array into individual items
    return (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json | Write-Output
}

#
# Remove-Migration
#

Register-TabExpansion Remove-Migration @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Removes the last migration.

.DESCRIPTION
    Removes the last migration.

.PARAMETER Force
    Revert the migration if it has been applied to the database.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Add-Migration
    Get-Migration
    about_EntityFrameworkCore
#>
function Remove-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [switch] $Force,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'migrations', 'remove', '--json'

    if ($Force)
    {
        $params += '--force'
    }

    $params += GetParams $Context

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    $result = (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json

    if (!(IsCpsProject $dteProject) -or (GetCpsProperty $dteProject 'EnableDefaultItems') -ne 'true' -or (GetCpsProperty $dteProject 'EnableDefaultCompileItems') -ne 'true')
    {
        $files = $result.migrationFile, $result.metadataFile, $result.snapshotFile
        $files | ?{ $_ -ne $null } | %{
            $projectItem = GetProjectItem $dteProject $_
            if ($projectItem)
            {
                $projectItem.Remove()
            }
        }
    }
}

#
# Optimize-DbContext
#

Register-TabExpansion Optimize-DbContext @{
    Project = { GetProjects }
    StartupProject = { GetProjects }
    OutputDir = { <# Disabled. Otherwise, paths would be relative to the solution directory. #> }
}

<#
.SYNOPSIS
    Generates a compiled version of the model used by the DbContext.

.DESCRIPTION
    Generates a compiled version of the model used by the DbContext.

.PARAMETER OutputDir
    The directory to put files in. Paths are relative to the project directory.

.PARAMETER Namespace
    The namespace to use. Matches the directory by default.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    about_EntityFrameworkCore
#>
function Optimize-DbContext
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $OutputDir,
        [string] $Namespace,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'dbcontext', 'optimize'

    if ($OutputDir)
    {
        $params += '--output-dir', $OutputDir
    }

    if ($Namespace)
    {
        $params += '--namespace', $Namespace
    }

    if ($Context)
    {
        $params += '--context', $Context
    }

    EF $dteProject $dteStartupProject $params $Args
}

#
# Scaffold-DbContext
#

Register-TabExpansion Scaffold-DbContext @{
    Provider = { param($x) GetProviders $x.Project }
    Project = { GetProjects }
    StartupProject = { GetProjects }
    OutputDir = { <# Disabled. Otherwise, paths would be relative to the solution directory. #> }
    ContextDir = { <# Disabled. Otherwise, paths would be relative to the solution directory. #> }
}

<#
.SYNOPSIS
    Scaffolds a DbContext and entity types for a database.

.DESCRIPTION
    Scaffolds a DbContext and entity types for a database.

.PARAMETER Connection
    The connection string to the database.

.PARAMETER Provider
    The provider to use. (E.g. Microsoft.EntityFrameworkCore.SqlServer)

.PARAMETER OutputDir
    The directory to put files in. Paths are relative to the project directory.

.PARAMETER ContextDir
    The directory to put the DbContext file in. Paths are relative to the project directory.

.PARAMETER Context
    The name of the DbContext. Defaults to the database name.

.PARAMETER Schemas
    The schemas of tables to generate entity types for.

.PARAMETER Tables
    The tables to generate entity types for.

.PARAMETER DataAnnotations
    Use attributes to configure the model (where possible). If omitted, only the fluent API is used.

.PARAMETER UseDatabaseNames
    Use table and column names directly from the database.

.PARAMETER Force
    Overwrite existing files.

.PARAMETER NoOnConfiguring
    Don't generate DbContext.OnConfiguring.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Namespace
    The namespace to use. Matches the directory by default.

.PARAMETER ContextNamespace
    The namespace of the DbContext class. Matches the directory by default.

.PARAMETER NoPluralize
    Don't use the pluralizer.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    about_EntityFrameworkCore
#>
function Scaffold-DbContext
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(Position = 0, Mandatory = $true)]
        [string] $Connection,
        [Parameter(Position = 1, Mandatory = $true)]
        [string] $Provider,
        [string] $OutputDir,
        [string] $ContextDir,
        [string] $Context,
        [string[]] $Schemas = @(),
        [string[]] $Tables = @(),
        [switch] $DataAnnotations,
        [switch] $UseDatabaseNames,
        [switch] $Force,
        [switch] $NoOnConfiguring,
        [string] $Project,
        [string] $StartupProject,
        [string] $Namespace,
        [string] $ContextNamespace,
        [switch] $NoPluralize,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'dbcontext', 'scaffold', $Connection, $Provider, '--json'

    if ($OutputDir)
    {
        $params += '--output-dir', $OutputDir
    }

    if ($ContextDir)
    {
        $params += '--context-dir', $ContextDir
    }

    if ($Context)
    {
        $params += '--context', $Context
    }

    if ($PSBoundParameters.ContainsKey('Namespace'))
    {
        $params += '--namespace', $Namespace
    }

    if ($PSBoundParameters.ContainsKey('ContextNamespace'))
    {
        $params += '--context-namespace', $ContextNamespace
    }

    $params += $Schemas | %{ '--schema', $_ }
    $params += $Tables | %{ '--table', $_ }

    if ($DataAnnotations)
    {
        $params += '--data-annotations'
    }

    if ($UseDatabaseNames)
    {
        $params += '--use-database-names'
    }

    if ($Force)
    {
        $params += '--force'
    }

    if ($NoOnConfiguring)
    {
        $params += '--no-onconfiguring'
    }

    if ($NoPluralize)
    {
        $params += '--no-pluralize'
    }

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    $result = (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json

    if (!(IsCpsProject $dteProject) -or (GetCpsProperty $dteProject 'EnableDefaultItems') -ne 'true' -or (GetCpsProperty $dteProject 'EnableDefaultCompileItems') -ne 'true')
    {
       $files = $result.entityTypeFiles + $result.contextFile
       $files | %{ $dteProject.ProjectItems.AddFromFile($_) | Out-Null }
    }

    $DTE.ItemOperations.OpenFile($result.contextFile) | Out-Null
    ShowConsole
}

#
# Script-DbContext
#

Register-TabExpansion Script-DbContext @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Generates a SQL script from the DbContext. Bypasses any migrations.

.DESCRIPTION
    Generates a SQL script from the DbContext. Bypasses any migrations.

.PARAMETER Output
    The file to write the result to.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    about_EntityFrameworkCore
#>
function Script-DbContext
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $Output,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    if (!$Output)
    {
        $intermediatePath = GetIntermediatePath $dteProject
        if (![IO.Path]::IsPathRooted($intermediatePath))
        {
            $projectDir = GetProperty $dteProject.Properties 'FullPath'
            $intermediatePath = [IO.Path]::GetFullPath((Join-Path $projectDir $intermediatePath))
        }

        $scriptFileName = [IO.Path]::ChangeExtension([IO.Path]::GetRandomFileName(), '.sql')
        $Output = Join-Path $intermediatePath $scriptFileName
    }
    elseif (![IO.Path]::IsPathRooted($Output))
    {
        $Output = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Output)
    }

    $params = 'dbcontext', 'script', '--output', $Output
    $params += GetParams $Context

    EF $dteProject $dteStartupProject $params $Args

    $DTE.ItemOperations.OpenFile($Output) | Out-Null
    ShowConsole
}

#
# Script-Migration
#

Register-TabExpansion Script-Migration @{
    From = { param($x) GetMigrations $x.Context $x.Project $x.StartupProject }
    To = { param($x) GetMigrations $x.Context $x.Project $x.StartupProject }
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Generates a SQL script from migrations.

.DESCRIPTION
    Generates a SQL script from migrations.

.PARAMETER From
    The starting migration. Defaults to '0' (the initial database).

.PARAMETER To
    The target migration. Defaults to the last migration.

.PARAMETER Idempotent
    Generate a script that can be used on a database at any migration.

.PARAMETER NoTransactions
    Don't generate SQL transaction statements.

.PARAMETER Output
    The file to write the result to.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Update-Database
    Get-Migration
    about_EntityFrameworkCore
#>
function Script-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(ParameterSetName = 'WithoutTo', Position = 0)]
        [Parameter(ParameterSetName = 'WithTo', Position = 0, Mandatory = $true)]
        [string] $From,
        [Parameter(ParameterSetName = 'WithTo', Position = 1, Mandatory = $true)]
        [string] $To,
        [switch] $Idempotent,
        [switch] $NoTransactions,
        [string] $Output,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    if (!$Output)
    {
        $intermediatePath = GetIntermediatePath $dteProject
        if (![IO.Path]::IsPathRooted($intermediatePath))
        {
            $projectDir = GetProperty $dteProject.Properties 'FullPath'
            $intermediatePath = [IO.Path]::GetFullPath((Join-Path $projectDir $intermediatePath))
        }

        $scriptFileName = [IO.Path]::ChangeExtension([IO.Path]::GetRandomFileName(), '.sql')
        $Output = Join-Path $intermediatePath $scriptFileName
    }
    elseif (![IO.Path]::IsPathRooted($Output))
    {
        $Output = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Output)
    }

    $params = 'migrations', 'script', '--output', $Output

    if ($From)
    {
        $params += $From
    }

    if ($To)
    {
        $params += $To
    }

    if ($Idempotent)
    {
        $params += '--idempotent'
    }

    if ($NoTransactions)
    {
        $params += '--no-transactions'
    }

    $params += GetParams $Context


    EF $dteProject $dteStartupProject $params $Args

    $DTE.ItemOperations.OpenFile($Output) | Out-Null
    ShowConsole
}

#
# Update-Database
#

Register-TabExpansion Update-Database @{
    Migration = { param($x) GetMigrations $x.Context $x.Project $x.StartupProject }
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Updates the database to a specified migration.

.DESCRIPTION
    Updates the database to a specified migration.

.PARAMETER Migration
    The target migration. If '0', all migrations will be reverted. Defaults to the last migration.

.PARAMETER Connection
    The connection string to the database. Defaults to the one specified in AddDbContext or OnConfiguring.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Script-Migration
    about_EntityFrameworkCore
#>
function Update-Database
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(Position = 0)]
        [string] $Migration,
        [string] $Connection,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    WarnIfEF6 'Update-Database'

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'database', 'update'

    if ($Migration)
    {
        $params += $Migration
    }

    if ($Connection)
    {
        $params += '--connection', $Connection
    }

    $params += GetParams $Context


    EF $dteProject $dteStartupProject $params $Args
}

#
# (Private Helpers)
#

function GetProjects
{
    return Get-Project -All | %{ $_.ProjectName }
}

function GetProviders($projectName)
{
    if (!$projectName)
    {
        $projectName = (Get-Project).ProjectName
    }

    return Get-Package -ProjectName $projectName | %{ $_.Id }
}

function GetContextTypes($projectName, $startupProjectName)
{
    $project = GetProject $projectName
    $startupProject = GetStartupProject $startupProjectName $project

    $params = 'dbcontext', 'list', '--json'

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    # NB: Write-Output is here to break the JSON array into individual items
    $result = (EF $project $startupProject $params $null -skipBuild) -join "`n" | ConvertFrom-Json | Write-Output

    return $result | %{ $_.safeName }
}

function GetMigrations($context, $projectName, $startupProjectName)
{
    $project = GetProject $projectName
    $startupProject = GetStartupProject $startupProjectName $project

    $params = 'migrations', 'list', '--no-connect', '--json'
    $params += GetParams $context

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    # NB: Write-Output is here to break the JSON array into individual items
    $result = (EF $project $startupProject $params $null -skipBuild) -join "`n" | ConvertFrom-Json | Write-Output

    return $result | %{ $_.safeName }
}

function WarnIfEF6($cmdlet)
{
    if (Get-Module 'EntityFramework6')
    {
        Write-Warning "Both Entity Framework Core and Entity Framework 6 are installed. The Entity Framework Core tools are running. Use 'EntityFramework6\$cmdlet' for Entity Framework 6."
    }
    elseif (Get-Module 'EntityFramework')
    {
        Write-Warning "Both Entity Framework Core and Entity Framework 6 are installed. The Entity Framework Core tools are running. Use 'EntityFramework\$cmdlet' for Entity Framework 6."
    }
}

function GetProject($projectName)
{
    if (!$projectName)
    {
        return Get-Project
    }

    return Get-Project $projectName
}

function GetStartupProject($name, $fallbackProject)
{
    if ($name)
    {
        return Get-Project $name
    }

    $startupProjectPaths = $DTE.Solution.SolutionBuild.StartupProjects
    if ($startupProjectPaths)
    {
        if ($startupProjectPaths.Length -eq 1)
        {
            $startupProjectPath = $startupProjectPaths[0]
            if (![IO.Path]::IsPathRooted($startupProjectPath))
            {
                $solutionPath = Split-Path (GetProperty $DTE.Solution.Properties 'Path')
                $startupProjectPath = [IO.Path]::GetFullPath((Join-Path $solutionPath $startupProjectPath))
            }

            $startupProject = GetSolutionProjects | ?{
                try
                {
                    $fullName = $_.FullName
                }
                catch [NotImplementedException]
                {
                    return $false
                }

                if ($fullName -and $fullName.EndsWith('\'))
                {
                    $fullName = $fullName.Substring(0, $fullName.Length - 1)
                }

                return $fullName -eq $startupProjectPath
            }
            if ($startupProject)
            {
                return $startupProject
            }

            Write-Warning "Unable to resolve startup project '$startupProjectPath'."
        }
        else
        {
            Write-Warning 'Multiple startup projects set.'
        }
    }
    else
    {
        Write-Warning 'No startup project set.'
    }

    Write-Warning "Using project '$($fallbackProject.ProjectName)' as the startup project."

    return $fallbackProject
}

function GetSolutionProjects()
{
    $projects = New-Object 'System.Collections.Stack'

    $DTE.Solution.Projects | %{
        $projects.Push($_)
    }

    while ($projects.Count)
    {
        $project = $projects.Pop();

        <# yield return #> $project

        if ($project.ProjectItems)
        {
            $project.ProjectItems | ?{ $_.SubProject } | %{
                $projects.Push($_.SubProject)
            }
        }
    }
}

function GetParams($context)
{
    $params = @()

    if ($context)
    {
        $params += '--context', $context
    }

    return $params
}

function ShowConsole
{
    $componentModel = Get-VSComponentModel
    $powerConsoleWindow = $componentModel.GetService([NuGetConsole.IPowerConsoleWindow])
    $powerConsoleWindow.Show()
}

function WriteErrorLine($message)
{
    try
    {
        # Call the internal API NuGet uses to display errors
        $componentModel = Get-VSComponentModel
        $powerConsoleWindow = $componentModel.GetService([NuGetConsole.IPowerConsoleWindow])
        $bindingFlags = [Reflection.BindingFlags]::Instance -bor [Reflection.BindingFlags]::NonPublic
        $activeHostInfo = $powerConsoleWindow.GetType().GetProperty('ActiveHostInfo', $bindingFlags).GetValue($powerConsoleWindow)
        $internalHost = $activeHostInfo.WpfConsole.Host
        $reportErrorMethod = $internalHost.GetType().GetMethod('ReportError', $bindingFlags, $null, [Exception], $null)
        $exception = New-Object Exception $message
        $reportErrorMethod.Invoke($internalHost, $exception)
    }
    catch
    {
        Write-Host $message -ForegroundColor DarkRed
    }
}

function EF($project, $startupProject, $params, $applicationArgs, [switch] $skipBuild)
{
    if (IsDocker $startupProject)
    {
        throw "Startup project '$($startupProject.ProjectName)' is a Docker project. Select an ASP.NET Core Web " +
            'Application as your startup project and try again.'
    }
    if (IsUWP $startupProject)
    {
        throw "Startup project '$($startupProject.ProjectName)' is a Universal Windows Platform app. This version of " +
            'the Entity Framework Core Package Manager Console Tools doesn''t support this type of project. For more ' +
            'information on using the EF Core Tools with UWP projects, see ' +
            'https://go.microsoft.com/fwlink/?linkid=858496'
    }

    Write-Verbose "Using project '$($project.ProjectName)'."
    Write-Verbose "Using startup project '$($startupProject.ProjectName)'."

    if (!$skipBuild)
    {
        Write-Host 'Build started...'

        # TODO: Only build startup project. Don't use BuildProject, you can't specify platform
        $solutionBuild = $DTE.Solution.SolutionBuild
        $solutionBuild.Build(<# WaitForBuildToFinish: #> $true)
        if ($solutionBuild.LastBuildInfo)
        {
            throw 'Build failed.'
        }

        Write-Host 'Build succeeded.'
    }

    $activeConfiguration = $startupProject.ConfigurationManager.ActiveConfiguration
    if ($activeConfiguration -eq $null)
    {
        throw "Unable to read project configuration settings of project '$($startupProject.ProjectName)' for the " +
            'active solution configuration. Try closing Package Manager Console and restarting Visual Studio. If the ' +
            'problem persists, use Help > Send Feedback > Report a Problem.'
    }

    $startupProjectDir = GetProperty $startupProject.Properties 'FullPath'
    $outputPath = GetProperty $activeConfiguration.Properties 'OutputPath'
    $targetDir = [IO.Path]::GetFullPath([IO.Path]::Combine($startupProjectDir, $outputPath))
    $startupTargetFileName = GetProperty $startupProject.Properties 'OutputFileName'
    $startupTargetPath = Join-Path $targetDir $startupTargetFileName
    $targetFrameworkMoniker = GetProperty $startupProject.Properties 'TargetFrameworkMoniker'
    $frameworkName = New-Object 'System.Runtime.Versioning.FrameworkName' $targetFrameworkMoniker
    $targetFramework = $frameworkName.Identifier

    if ($targetFramework -in '.NETFramework')
    {
        $platformTarget = GetPlatformTarget $startupProject
        if ($platformTarget -eq 'x86')
        {
            $exePath = Join-Path $PSScriptRoot 'net461\win-x86\ef.exe'
        }
        elseif ($platformTarget -eq 'ARM64')
        {
            $exePath = Join-Path $PSScriptRoot 'net461\win-arm64\ef.exe'
        }
        elseif ($platformTarget -in 'AnyCPU', 'x64')
        {
            $exePath = Join-Path $PSScriptRoot 'net461\any\ef.exe'
        }
        else
        {
            throw "Startup project '$($startupProject.ProjectName)' has an active platform of '$platformTarget'. Select " +
                'a different platform and try again.'
        }
    }
    elseif ($targetFramework -eq '.NETCoreApp')
    {
        $targetPlatformIdentifier = GetCpsProperty $startupProject 'TargetPlatformIdentifier'
        if ($targetPlatformIdentifier -and $targetPlatformIdentifier -ne 'Windows')
        {
            throw "Startup project '$($startupProject.ProjectName)' targets platform '$targetPlatformIdentifier'. The Entity Framework " +
                'Core Package Manager Console Tools don''t support this platform. See https://aka.ms/efcore-docs-pmc-tfms for more ' +
                'information.'
        }

        $exePath = (Get-Command 'dotnet').Path

        $startupTargetName = GetProperty $startupProject.Properties 'AssemblyName'
        $depsFile = Join-Path $targetDir ($startupTargetName + '.deps.json')
        $projectAssetsFile = GetCpsProperty $startupProject 'ProjectAssetsFile'
        $runtimeConfig = Join-Path $targetDir ($startupTargetName + '.runtimeconfig.json')
        $runtimeFrameworkVersion = GetCpsProperty $startupProject 'RuntimeFrameworkVersion'
        $efPath = Join-Path $PSScriptRoot 'netcoreapp2.0\any\ef.dll'

        $dotnetParams = 'exec', '--depsfile', $depsFile

        if ($projectAssetsFile)
        {
            # NB: Don't use Get-Content. It doesn't handle UTF-8 without a signature
            # NB: Don't use ReadAllLines. ConvertFrom-Json won't work on PowerShell 3.0
            $projectAssets = [IO.File]::ReadAllText($projectAssetsFile) | ConvertFrom-Json
            $projectAssets.packageFolders.psobject.Properties.Name | %{
                $dotnetParams += '--additionalprobingpath', $_.TrimEnd('\')
            }
        }

        if ([IO.File]::Exists($runtimeConfig))
        {
            $dotnetParams += '--runtimeconfig', $runtimeConfig
        }
        elseif ($runtimeFrameworkVersion)
        {
            $dotnetParams += '--fx-version', $runtimeFrameworkVersion
        }

        $dotnetParams += $efPath

        $params = $dotnetParams + $params
    }
    elseif ($targetFramework -eq '.NETStandard')
    {
        throw "Startup project '$($startupProject.ProjectName)' targets framework '.NETStandard'. There is no " +
            'runtime associated with this framework, and projects targeting it cannot be executed directly. To use ' +
            'the Entity Framework Core Package Manager Console Tools with this project, add an executable project ' +
            'targeting .NET Framework or .NET Core that references this project, and set it as the startup project; ' +
            'or, update this project to cross-target .NET Framework or .NET Core. For more information on using the ' +
            'EF Core Tools with .NET Standard projects, see https://go.microsoft.com/fwlink/?linkid=2034705'
    }
    else
    {
        throw "Startup project '$($startupProject.ProjectName)' targets framework '$targetFramework'. The Entity Framework Core Package " +
            'Manager Console Tools don''t support this framework. See https://aka.ms/efcore-docs-pmc-tfms for more information and examples.'
    }

    $projectDir = GetProperty $project.Properties 'FullPath'
    $targetFileName = GetProperty $project.Properties 'OutputFileName'
    $targetPath = Join-Path $targetDir $targetFileName
    $rootNamespace = GetProperty $project.Properties 'RootNamespace'
    $language = GetLanguage $project
    $nullable = GetNullable $project

    $params += '--verbose',
        '--no-color',
        '--prefix-output',
        '--assembly', $targetPath,
        '--project', $project.FullName,
        '--startup-assembly', $startupTargetPath,
        '--startup-project', $startupProject.FullName,
        '--project-dir', $projectDir,
        '--language', $language,
        '--configuration', $activeConfiguration.ConfigurationName,
        '--working-dir', $PWD.Path

    if (IsWeb $startupProject)
    {
        $params += '--data-dir', (Join-Path $startupProjectDir 'App_Data')
    }

    if ($rootNamespace -ne $null)
    {
        $params += '--root-namespace', $rootNamespace
    }

    if ($nullable -in 'enable', 'annotations')
    {
        $params += '--nullable'
    }

    $arguments = ToArguments $params
    if ($applicationArgs)
    {
        $arguments += ' -- '
        $arguments += $applicationArgs
    }

    $startInfo = New-Object 'System.Diagnostics.ProcessStartInfo' -Property @{
        FileName = $exePath;
        Arguments = $arguments;
        UseShellExecute = $false;
        CreateNoWindow = $true;
        RedirectStandardOutput = $true;
        StandardOutputEncoding = [Text.Encoding]::UTF8;
        RedirectStandardError = $true;
        WorkingDirectory = $startupProjectDir;
    }

    Write-Verbose "$exePath $arguments"

    $process = [Diagnostics.Process]::Start($startInfo)

    while (($line = $process.StandardOutput.ReadLine()) -ne $null)
    {
        $level = $null
        $text = $null

        $parts = $line.Split(':', 2)
        if ($parts.Length -eq 2)
        {
            $level = $parts[0]

            $i = 0
            $count = 8 - $level.Length
            while ($i -lt $count -and $parts[1][$i] -eq ' ')
            {
                $i++
            }

            $text = $parts[1].Substring($i)
        }

        switch ($level)
        {
            'error' { WriteErrorLine $text }
            'warn' { Write-Warning $text }
            'info' { Write-Host $text }
            'data' { Write-Output $text }
            'verbose' { Write-Verbose $text }
            default { Write-Host $line }
        }
    }

    $process.WaitForExit()

    if ($process.ExitCode)
    {
        while (($line = $process.StandardError.ReadLine()) -ne $null)
        {
            WriteErrorLine $line
        }

        exit
    }
}

function IsDocker($project)
{
    return $project.Kind -eq '{E53339B2-1760-4266-BCC7-CA923CBCF16C}'
}

function IsCpsProject($project)
{
    $hierarchy = GetVsHierarchy $project
    $isCapabilityMatch = [Microsoft.VisualStudio.Shell.PackageUtilities].GetMethod(
        'IsCapabilityMatch',
        [type[]]([Microsoft.VisualStudio.Shell.Interop.IVsHierarchy], [string]))

    return $isCapabilityMatch.Invoke($null, ($hierarchy, 'CPS'))
}

function IsWeb($project)
{
    $types = GetProjectTypes $project

    return $types -contains '{349C5851-65DF-11DA-9384-00065B846F21}'
}

function IsUWP($project)
{
    $types = GetProjectTypes $project

    return $types -contains '{A5A43C5B-DE2A-4C0C-9213-0A381AF9435A}'
}

function GetIntermediatePath($project)
{
    $intermediatePath = GetProperty $project.ConfigurationManager.ActiveConfiguration.Properties 'IntermediatePath'
    if ($intermediatePath)
    {
        return $intermediatePath
    }

    return GetMSBuildProperty $project 'IntermediateOutputPath'
}

function GetPlatformTarget($project)
{
    if (IsCpsProject $project)
    {
        $platformTarget = GetCpsProperty $project 'PlatformTarget'
        if ($platformTarget)
        {
            return $platformTarget
        }

        return GetCpsProperty $project 'Platform'
    }

    $platformTarget = GetProperty $project.ConfigurationManager.ActiveConfiguration.Properties 'PlatformTarget'
    if ($platformTarget)
    {
        return $platformTarget
    }

    # NB: For classic F# projects
    $platformTarget = GetMSBuildProperty $project 'PlatformTarget'
    if ($platformTarget)
    {
        return $platformTarget
    }

    return 'AnyCPU'
}

function GetLanguage($project)
{
    if (IsCpsProject $project)
    {
        return GetCpsProperty $project 'Language'
    }

    return GetMSBuildProperty $project 'Language'
}

function GetNullable($project)
{
    if (IsCpsProject $project)
    {
        return GetCpsProperty $project 'Nullable'
    }

    return GetMSBuildProperty $project 'Nullable'
}

function GetVsHierarchy($project)
{
    $solution = Get-VSService 'Microsoft.VisualStudio.Shell.Interop.SVsSolution' 'Microsoft.VisualStudio.Shell.Interop.IVsSolution'
    $hierarchy = $null
    $hr = $solution.GetProjectOfUniqueName($project.UniqueName, [ref] $hierarchy)
    [Runtime.InteropServices.Marshal]::ThrowExceptionForHR($hr)

    return $hierarchy
}

function GetProjectTypes($project)
{
    $hierarchy = GetVsHierarchy $project
    $aggregatableProject = Get-Interface $hierarchy 'Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject'
    if (!$aggregatableProject)
    {
        return $project.Kind
    }

    $projectTypeGuidsString = $null
    $hr = $aggregatableProject.GetAggregateProjectTypeGuids([ref] $projectTypeGuidsString)
    [Runtime.InteropServices.Marshal]::ThrowExceptionForHR($hr)

    return $projectTypeGuidsString.Split(';')
}

function GetProperty($properties, $propertyName)
{
    try
    {
        return $properties.Item($propertyName).Value
    }
    catch
    {
        return $null
    }
}

function GetCpsProperty($project, $propertyName)
{
    $browseObjectContext = Get-Interface $project 'Microsoft.VisualStudio.ProjectSystem.Properties.IVsBrowseObjectContext'
    $unconfiguredProject = $browseObjectContext.UnconfiguredProject
    $configuredProject = $unconfiguredProject.GetSuggestedConfiguredProjectAsync().Result
    $properties = $configuredProject.Services.ProjectPropertiesProvider.GetCommonProperties()

    return $properties.GetEvaluatedPropertyValueAsync($propertyName).Result
}

function GetMSBuildProperty($project, $propertyName)
{
    $msbuildProject = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.LoadedProjects |
        ? FullPath -eq $project.FullName

    return $msbuildProject.GetProperty($propertyName).EvaluatedValue
}

function GetProjectItem($project, $path)
{
    $fullPath = GetProperty $project.Properties 'FullPath'

    if ([IO.Path]::IsPathRooted($path))
    {
        $path = $path.Substring($fullPath.Length)
    }

    $itemDirectory = (Split-Path $path -Parent)

    $projectItems = $project.ProjectItems
    if ($itemDirectory)
    {
        $directories = $itemDirectory.Split('\')
        $directories | %{
            if ($projectItems)
            {
                $projectItems = $projectItems.Item($_).ProjectItems
            }
        }
    }

    if (!$projectItems)
    {
        return $null
    }

    $itemName = Split-Path $path -Leaf

    try
    {
        return $projectItems.Item($itemName)
    }
    catch [Exception]
    {
    }

    return $null
}

function ToArguments($params)
{
    $arguments = ''
    for ($i = 0; $i -lt $params.Length; $i++)
    {
        if ($i)
        {
            $arguments += ' '
        }

        if ($params[$i] -eq '')
        {
            $arguments += '""'

            continue
        }
        if (!$params[$i].Contains(' '))
        {
            $arguments += $params[$i]

            continue
        }

        $arguments += '"'

        $pendingBackslashs = 0
        for ($j = 0; $j -lt $params[$i].Length; $j++)
        {
            switch ($params[$i][$j])
            {
                '"'
                {
                    if ($pendingBackslashs)
                    {
                        $arguments += '\' * $pendingBackslashs * 2
                        $pendingBackslashs = 0
                    }
                    $arguments += '\"'
                }

                '\'
                {
                    $pendingBackslashs++
                }

                default
                {
                    if ($pendingBackslashs)
                    {
                        if ($pendingBackslashs -eq 1)
                        {
                            $arguments += '\'
                        }
                        else
                        {
                            $arguments += '\' * $pendingBackslashs * 2
                        }

                        $pendingBackslashs = 0
                    }

                    $arguments += $params[$i][$j]
                }
            }
        }

        if ($pendingBackslashs)
        {
            $arguments += '\' * $pendingBackslashs * 2
        }

        $arguments += '"'
    }

    return $arguments
}

# SIG # Begin signature block
# MIInqgYJKoZIhvcNAQcCoIInmzCCJ5cCAQExDzANBglghkgBZQMEAgEFADB5Bgor
# BgEEAYI3AgEEoGswaTA0BgorBgEEAYI3AgEeMCYCAwEAAAQQH8w7YFlLCE63JNLG
# KX7zUQIBAAIBAAIBAAIBAAIBADAxMA0GCWCGSAFlAwQCAQUABCBRC55CYtrLxWYG
# sANHaMgRQYHL5Z36WYzCBXy+kn2yuaCCDYEwggX/MIID56ADAgECAhMzAAACzI61
# lqa90clOAAAAAALMMA0GCSqGSIb3DQEBCwUAMH4xCzAJBgNVBAYTAlVTMRMwEQYD
# VQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24xKDAmBgNVBAMTH01pY3Jvc29mdCBDb2RlIFNpZ25p
# bmcgUENBIDIwMTEwHhcNMjIwNTEyMjA0NjAxWhcNMjMwNTExMjA0NjAxWjB0MQsw
# CQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9u
# ZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMR4wHAYDVQQDExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIB
# AQCiTbHs68bADvNud97NzcdP0zh0mRr4VpDv68KobjQFybVAuVgiINf9aG2zQtWK
# No6+2X2Ix65KGcBXuZyEi0oBUAAGnIe5O5q/Y0Ij0WwDyMWaVad2Te4r1Eic3HWH
# UfiiNjF0ETHKg3qa7DCyUqwsR9q5SaXuHlYCwM+m59Nl3jKnYnKLLfzhl13wImV9
# DF8N76ANkRyK6BYoc9I6hHF2MCTQYWbQ4fXgzKhgzj4zeabWgfu+ZJCiFLkogvc0
# RVb0x3DtyxMbl/3e45Eu+sn/x6EVwbJZVvtQYcmdGF1yAYht+JnNmWwAxL8MgHMz
# xEcoY1Q1JtstiY3+u3ulGMvhAgMBAAGjggF+MIIBejAfBgNVHSUEGDAWBgorBgEE
# AYI3TAgBBggrBgEFBQcDAzAdBgNVHQ4EFgQUiLhHjTKWzIqVIp+sM2rOHH11rfQw
# UAYDVR0RBEkwR6RFMEMxKTAnBgNVBAsTIE1pY3Jvc29mdCBPcGVyYXRpb25zIFB1
# ZXJ0byBSaWNvMRYwFAYDVQQFEw0yMzAwMTIrNDcwNTI5MB8GA1UdIwQYMBaAFEhu
# ZOVQBdOCqhc3NyK1bajKdQKVMFQGA1UdHwRNMEswSaBHoEWGQ2h0dHA6Ly93d3cu
# bWljcm9zb2Z0LmNvbS9wa2lvcHMvY3JsL01pY0NvZFNpZ1BDQTIwMTFfMjAxMS0w
# Ny0wOC5jcmwwYQYIKwYBBQUHAQEEVTBTMFEGCCsGAQUFBzAChkVodHRwOi8vd3d3
# Lm1pY3Jvc29mdC5jb20vcGtpb3BzL2NlcnRzL01pY0NvZFNpZ1BDQTIwMTFfMjAx
# MS0wNy0wOC5jcnQwDAYDVR0TAQH/BAIwADANBgkqhkiG9w0BAQsFAAOCAgEAeA8D
# sOAHS53MTIHYu8bbXrO6yQtRD6JfyMWeXaLu3Nc8PDnFc1efYq/F3MGx/aiwNbcs
# J2MU7BKNWTP5JQVBA2GNIeR3mScXqnOsv1XqXPvZeISDVWLaBQzceItdIwgo6B13
# vxlkkSYMvB0Dr3Yw7/W9U4Wk5K/RDOnIGvmKqKi3AwyxlV1mpefy729FKaWT7edB
# d3I4+hldMY8sdfDPjWRtJzjMjXZs41OUOwtHccPazjjC7KndzvZHx/0VWL8n0NT/
# 404vftnXKifMZkS4p2sB3oK+6kCcsyWsgS/3eYGw1Fe4MOnin1RhgrW1rHPODJTG
# AUOmW4wc3Q6KKr2zve7sMDZe9tfylonPwhk971rX8qGw6LkrGFv31IJeJSe/aUbG
# dUDPkbrABbVvPElgoj5eP3REqx5jdfkQw7tOdWkhn0jDUh2uQen9Atj3RkJyHuR0
# GUsJVMWFJdkIO/gFwzoOGlHNsmxvpANV86/1qgb1oZXdrURpzJp53MsDaBY/pxOc
# J0Cvg6uWs3kQWgKk5aBzvsX95BzdItHTpVMtVPW4q41XEvbFmUP1n6oL5rdNdrTM
# j/HXMRk1KCksax1Vxo3qv+13cCsZAaQNaIAvt5LvkshZkDZIP//0Hnq7NnWeYR3z
# 4oFiw9N2n3bb9baQWuWPswG0Dq9YT9kb+Cs4qIIwggd6MIIFYqADAgECAgphDpDS
# AAAAAAADMA0GCSqGSIb3DQEBCwUAMIGIMQswCQYDVQQGEwJVUzETMBEGA1UECBMK
# V2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0
# IENvcnBvcmF0aW9uMTIwMAYDVQQDEylNaWNyb3NvZnQgUm9vdCBDZXJ0aWZpY2F0
# ZSBBdXRob3JpdHkgMjAxMTAeFw0xMTA3MDgyMDU5MDlaFw0yNjA3MDgyMTA5MDla
# MH4xCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdS
# ZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xKDAmBgNVBAMT
# H01pY3Jvc29mdCBDb2RlIFNpZ25pbmcgUENBIDIwMTEwggIiMA0GCSqGSIb3DQEB
# AQUAA4ICDwAwggIKAoICAQCr8PpyEBwurdhuqoIQTTS68rZYIZ9CGypr6VpQqrgG
# OBoESbp/wwwe3TdrxhLYC/A4wpkGsMg51QEUMULTiQ15ZId+lGAkbK+eSZzpaF7S
# 35tTsgosw6/ZqSuuegmv15ZZymAaBelmdugyUiYSL+erCFDPs0S3XdjELgN1q2jz
# y23zOlyhFvRGuuA4ZKxuZDV4pqBjDy3TQJP4494HDdVceaVJKecNvqATd76UPe/7
# 4ytaEB9NViiienLgEjq3SV7Y7e1DkYPZe7J7hhvZPrGMXeiJT4Qa8qEvWeSQOy2u
# M1jFtz7+MtOzAz2xsq+SOH7SnYAs9U5WkSE1JcM5bmR/U7qcD60ZI4TL9LoDho33
# X/DQUr+MlIe8wCF0JV8YKLbMJyg4JZg5SjbPfLGSrhwjp6lm7GEfauEoSZ1fiOIl
# XdMhSz5SxLVXPyQD8NF6Wy/VI+NwXQ9RRnez+ADhvKwCgl/bwBWzvRvUVUvnOaEP
# 6SNJvBi4RHxF5MHDcnrgcuck379GmcXvwhxX24ON7E1JMKerjt/sW5+v/N2wZuLB
# l4F77dbtS+dJKacTKKanfWeA5opieF+yL4TXV5xcv3coKPHtbcMojyyPQDdPweGF
# RInECUzF1KVDL3SV9274eCBYLBNdYJWaPk8zhNqwiBfenk70lrC8RqBsmNLg1oiM
# CwIDAQABo4IB7TCCAekwEAYJKwYBBAGCNxUBBAMCAQAwHQYDVR0OBBYEFEhuZOVQ
# BdOCqhc3NyK1bajKdQKVMBkGCSsGAQQBgjcUAgQMHgoAUwB1AGIAQwBBMAsGA1Ud
# DwQEAwIBhjAPBgNVHRMBAf8EBTADAQH/MB8GA1UdIwQYMBaAFHItOgIxkEO5FAVO
# 4eqnxzHRI4k0MFoGA1UdHwRTMFEwT6BNoEuGSWh0dHA6Ly9jcmwubWljcm9zb2Z0
# LmNvbS9wa2kvY3JsL3Byb2R1Y3RzL01pY1Jvb0NlckF1dDIwMTFfMjAxMV8wM18y
# Mi5jcmwwXgYIKwYBBQUHAQEEUjBQME4GCCsGAQUFBzAChkJodHRwOi8vd3d3Lm1p
# Y3Jvc29mdC5jb20vcGtpL2NlcnRzL01pY1Jvb0NlckF1dDIwMTFfMjAxMV8wM18y
# Mi5jcnQwgZ8GA1UdIASBlzCBlDCBkQYJKwYBBAGCNy4DMIGDMD8GCCsGAQUFBwIB
# FjNodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpb3BzL2RvY3MvcHJpbWFyeWNw
# cy5odG0wQAYIKwYBBQUHAgIwNB4yIB0ATABlAGcAYQBsAF8AcABvAGwAaQBjAHkA
# XwBzAHQAYQB0AGUAbQBlAG4AdAAuIB0wDQYJKoZIhvcNAQELBQADggIBAGfyhqWY
# 4FR5Gi7T2HRnIpsLlhHhY5KZQpZ90nkMkMFlXy4sPvjDctFtg/6+P+gKyju/R6mj
# 82nbY78iNaWXXWWEkH2LRlBV2AySfNIaSxzzPEKLUtCw/WvjPgcuKZvmPRul1LUd
# d5Q54ulkyUQ9eHoj8xN9ppB0g430yyYCRirCihC7pKkFDJvtaPpoLpWgKj8qa1hJ
# Yx8JaW5amJbkg/TAj/NGK978O9C9Ne9uJa7lryft0N3zDq+ZKJeYTQ49C/IIidYf
# wzIY4vDFLc5bnrRJOQrGCsLGra7lstnbFYhRRVg4MnEnGn+x9Cf43iw6IGmYslmJ
# aG5vp7d0w0AFBqYBKig+gj8TTWYLwLNN9eGPfxxvFX1Fp3blQCplo8NdUmKGwx1j
# NpeG39rz+PIWoZon4c2ll9DuXWNB41sHnIc+BncG0QaxdR8UvmFhtfDcxhsEvt9B
# xw4o7t5lL+yX9qFcltgA1qFGvVnzl6UJS0gQmYAf0AApxbGbpT9Fdx41xtKiop96
# eiL6SJUfq/tHI4D1nvi/a7dLl+LrdXga7Oo3mXkYS//WsyNodeav+vyL6wuA6mk7
# r/ww7QRMjt/fdW1jkT3RnVZOT7+AVyKheBEyIXrvQQqxP/uozKRdwaGIm1dxVk5I
# RcBCyZt2WwqASGv9eZ/BvW1taslScxMNelDNMYIZfzCCGXsCAQEwgZUwfjELMAkG
# A1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQx
# HjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEoMCYGA1UEAxMfTWljcm9z
# b2Z0IENvZGUgU2lnbmluZyBQQ0EgMjAxMQITMwAAAsyOtZamvdHJTgAAAAACzDAN
# BglghkgBZQMEAgEFAKCBrjAZBgkqhkiG9w0BCQMxDAYKKwYBBAGCNwIBBDAcBgor
# BgEEAYI3AgELMQ4wDAYKKwYBBAGCNwIBFTAvBgkqhkiG9w0BCQQxIgQgxgtodoXs
# 44sDHQd5jirmK1QP5KyYl08iHbIpmTMCID0wQgYKKwYBBAGCNwIBDDE0MDKgFIAS
# AE0AaQBjAHIAbwBzAG8AZgB0oRqAGGh0dHA6Ly93d3cubWljcm9zb2Z0LmNvbTAN
# BgkqhkiG9w0BAQEFAASCAQAtDogS8AH/D/7MAjJX5yGYd8sZVS8btYqNXtch7SiL
# W81YN+YmRQB6WqasfxN68RgxhgIhPQErD7EuqHSm5Y8SwuQvyt1BmUCuZKP3bkvN
# V0ofGIy8iq2jSLhc4+v37qBkz08rG8487kSq5fcSWyicWT94U/L5G3shEwfwPJya
# NWxEneyv/6AKb8Y/KEVHTkDvZJ9aKmASbVTs+1Ix54303UrLGvIZC3HveqFPKVWH
# dwXI7a8QXa/Y5CIEFEtYO0/aS4KPgjKlQurJ7e/Of+IjzhCHE+hXwKM0dykRM/1a
# hzZQirk+YpCE/uv7uxf/+hkD+iBvkLaeZLZ+t1sZ8aewoYIXCTCCFwUGCisGAQQB
# gjcDAwExghb1MIIW8QYJKoZIhvcNAQcCoIIW4jCCFt4CAQMxDzANBglghkgBZQME
# AgEFADCCAVUGCyqGSIb3DQEJEAEEoIIBRASCAUAwggE8AgEBBgorBgEEAYRZCgMB
# MDEwDQYJYIZIAWUDBAIBBQAEIKsh0RTxH717FgppueBUr8pdTY4XhpHEtbnnmpRd
# HzETAgZjc5fR+vsYEzIwMjIxMTMwMTQwMjAyLjU1NlowBIACAfSggdSkgdEwgc4x
# CzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRt
# b25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xKTAnBgNVBAsTIE1p
# Y3Jvc29mdCBPcGVyYXRpb25zIFB1ZXJ0byBSaWNvMSYwJAYDVQQLEx1UaGFsZXMg
# VFNTIEVTTjpGN0E2LUUyNTEtMTUwQTElMCMGA1UEAxMcTWljcm9zb2Z0IFRpbWUt
# U3RhbXAgU2VydmljZaCCEVwwggcQMIIE+KADAgECAhMzAAABpQDeCMRAB3FOAAEA
# AAGlMA0GCSqGSIb3DQEBCwUAMHwxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNo
# aW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29y
# cG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1lLVN0YW1wIFBDQSAyMDEw
# MB4XDTIyMDMwMjE4NTExOVoXDTIzMDUxMTE4NTExOVowgc4xCzAJBgNVBAYTAlVT
# MRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQK
# ExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xKTAnBgNVBAsTIE1pY3Jvc29mdCBPcGVy
# YXRpb25zIFB1ZXJ0byBSaWNvMSYwJAYDVQQLEx1UaGFsZXMgVFNTIEVTTjpGN0E2
# LUUyNTEtMTUwQTElMCMGA1UEAxMcTWljcm9zb2Z0IFRpbWUtU3RhbXAgU2Vydmlj
# ZTCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBALqxhuggaki8wuoOMMd7
# rsEQnmAhtV8iU1Y0itsHq30TdCXJDmvZjaZ8yvOHYFVhTyD1b5JGJtwZjWz1fglC
# qsx5qBxP1Wx1CZnsQ1tiRsRWQc12VkETmkY8x46MgHsGyAmhPPpsgRnklGai7HqQ
# FB31x/Qjkx7rbAlr6PblB4tOmaR1nKxl4VIgstDwfneKaoEEw4iN/xTdztZjwyGi
# Y5hNp6beetkcizgJFO3/yRHYh0gtk+bREhrmIgbarrrgbz7MsnA7tlKvGcO9iHc6
# +2symrAVy3CzQ4IMNPFcTTx8wTZ+kpv6lFs1eG8xlfsu2NDWKshrMlKH2JpYzWAW
# 1fCOD5irXsE4LOvixZQvbneQE6+iGfIQwabj+fRdouAU2AiE+iaNsIDapdKab8WL
# xz6VPRbEL+M6MFkcsoiuKHHoshCp7JhmZ9iM0yrEx2XebOha/XQ342KsRGs2h02g
# pX6wByyT8eD3MJVIxSRm4MLIilvWcpd9N3rooawbLU6gdk7goKWS69+w2jtouXCE
# Yt6IPfZq8ldi0L/CwYbtv7mbHmIZ9Oc0JEJc6b9gcVDfoPiemMKcz15BLepyx7np
# Q2MiDKIscOqKhXuZI+PZerNOHhi/vsy2/Fj9lB6kJrMYSfV0F2frvBSBXMB7xjv8
# pgqX5QXUe8nTxb4UfJ0cDAvBAgMBAAGjggE2MIIBMjAdBgNVHQ4EFgQUX6aPAwCX
# rq6tcO773FkXS2ipGt8wHwYDVR0jBBgwFoAUn6cVXQBeYl2D9OXSZacbUzUZ6XIw
# XwYDVR0fBFgwVjBUoFKgUIZOaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3BraW9w
# cy9jcmwvTWljcm9zb2Z0JTIwVGltZS1TdGFtcCUyMFBDQSUyMDIwMTAoMSkuY3Js
# MGwGCCsGAQUFBwEBBGAwXjBcBggrBgEFBQcwAoZQaHR0cDovL3d3dy5taWNyb3Nv
# ZnQuY29tL3BraW9wcy9jZXJ0cy9NaWNyb3NvZnQlMjBUaW1lLVN0YW1wJTIwUENB
# JTIwMjAxMCgxKS5jcnQwDAYDVR0TAQH/BAIwADATBgNVHSUEDDAKBggrBgEFBQcD
# CDANBgkqhkiG9w0BAQsFAAOCAgEAlpsLF+UwMKKER2p0WJno4G6GGGnfg3qjDdaH
# c5uvXYtG6KmHrqAf/YqHkmNotSr6ZEEnlGCJYR7W3uJ+5bpvj03wFqGefvQsKIR2
# +q6TrzozvP4NsodWTT5SVp/C6TEDGuLC9mOQKA4tyL40HTW7txb0cAdfgnyHFoI/
# BsZo/FaXezQ8hO4xUjhDpyNNeJ6WYvX5NC+Hv9nmTyzjqyEg/L2cXAOmxEWvfPAQ
# 1lfxvrtUwG75jGeUaewkhwtzanCnP3l6YjwJFKB6n7/TXtrfik1xY1kgev1JwQ5a
# UdPxwSdDmGE4XTN2s6pPOi8IO199Of6AEvh41eDxRz+11VUcpuGn7tJUeSTUSHsv
# zQ8ECOj5w77Mv55/F8hWu07egnG8SrWj5+TFxNPCpx/AFNvzz+odTRTZd4LWuomc
# MHUmLFiUGOAdetF6SofHG5EcFn0DTD1apBZzCP8xsGQcZgwVqo7ov23/uIJlMCLA
# yTYZV9ITCP09ciUJbKBVCQNrGEnQ/XLFO9mysyyDRrvHhU5uGPdXz4Jt2/ZN7JQY
# RuVNSuCpNwoK0Jr1s6ciDvHEeLyiczxoIe9GH3SyfbHx6v/phI+iE3DWo1TCK75E
# L6pt6k5i36/kn2uSVXdTH44ZVkh3/ihV3vEws78uGlvsiMcrKBgpo3HdcjDHiHoW
# sUf4GIwwggdxMIIFWaADAgECAhMzAAAAFcXna54Cm0mZAAAAAAAVMA0GCSqGSIb3
# DQEBCwUAMIGIMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4G
# A1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMTIw
# MAYDVQQDEylNaWNyb3NvZnQgUm9vdCBDZXJ0aWZpY2F0ZSBBdXRob3JpdHkgMjAx
# MDAeFw0yMTA5MzAxODIyMjVaFw0zMDA5MzAxODMyMjVaMHwxCzAJBgNVBAYTAlVT
# MRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQK
# ExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1l
# LVN0YW1wIFBDQSAyMDEwMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEA
# 5OGmTOe0ciELeaLL1yR5vQ7VgtP97pwHB9KpbE51yMo1V/YBf2xK4OK9uT4XYDP/
# XE/HZveVU3Fa4n5KWv64NmeFRiMMtY0Tz3cywBAY6GB9alKDRLemjkZrBxTzxXb1
# hlDcwUTIcVxRMTegCjhuje3XD9gmU3w5YQJ6xKr9cmmvHaus9ja+NSZk2pg7uhp7
# M62AW36MEBydUv626GIl3GoPz130/o5Tz9bshVZN7928jaTjkY+yOSxRnOlwaQ3K
# Ni1wjjHINSi947SHJMPgyY9+tVSP3PoFVZhtaDuaRr3tpK56KTesy+uDRedGbsoy
# 1cCGMFxPLOJiss254o2I5JasAUq7vnGpF1tnYN74kpEeHT39IM9zfUGaRnXNxF80
# 3RKJ1v2lIH1+/NmeRd+2ci/bfV+AutuqfjbsNkz2K26oElHovwUDo9Fzpk03dJQc
# NIIP8BDyt0cY7afomXw/TNuvXsLz1dhzPUNOwTM5TI4CvEJoLhDqhFFG4tG9ahha
# YQFzymeiXtcodgLiMxhy16cg8ML6EgrXY28MyTZki1ugpoMhXV8wdJGUlNi5UPkL
# iWHzNgY1GIRH29wb0f2y1BzFa/ZcUlFdEtsluq9QBXpsxREdcu+N+VLEhReTwDwV
# 2xo3xwgVGD94q0W29R6HXtqPnhZyacaue7e3PmriLq0CAwEAAaOCAd0wggHZMBIG
# CSsGAQQBgjcVAQQFAgMBAAEwIwYJKwYBBAGCNxUCBBYEFCqnUv5kxJq+gpE8RjUp
# zxD/LwTuMB0GA1UdDgQWBBSfpxVdAF5iXYP05dJlpxtTNRnpcjBcBgNVHSAEVTBT
# MFEGDCsGAQQBgjdMg30BATBBMD8GCCsGAQUFBwIBFjNodHRwOi8vd3d3Lm1pY3Jv
# c29mdC5jb20vcGtpb3BzL0RvY3MvUmVwb3NpdG9yeS5odG0wEwYDVR0lBAwwCgYI
# KwYBBQUHAwgwGQYJKwYBBAGCNxQCBAweCgBTAHUAYgBDAEEwCwYDVR0PBAQDAgGG
# MA8GA1UdEwEB/wQFMAMBAf8wHwYDVR0jBBgwFoAU1fZWy4/oolxiaNE9lJBb186a
# GMQwVgYDVR0fBE8wTTBLoEmgR4ZFaHR0cDovL2NybC5taWNyb3NvZnQuY29tL3Br
# aS9jcmwvcHJvZHVjdHMvTWljUm9vQ2VyQXV0XzIwMTAtMDYtMjMuY3JsMFoGCCsG
# AQUFBwEBBE4wTDBKBggrBgEFBQcwAoY+aHR0cDovL3d3dy5taWNyb3NvZnQuY29t
# L3BraS9jZXJ0cy9NaWNSb29DZXJBdXRfMjAxMC0wNi0yMy5jcnQwDQYJKoZIhvcN
# AQELBQADggIBAJ1VffwqreEsH2cBMSRb4Z5yS/ypb+pcFLY+TkdkeLEGk5c9MTO1
# OdfCcTY/2mRsfNB1OW27DzHkwo/7bNGhlBgi7ulmZzpTTd2YurYeeNg2LpypglYA
# A7AFvonoaeC6Ce5732pvvinLbtg/SHUB2RjebYIM9W0jVOR4U3UkV7ndn/OOPcbz
# aN9l9qRWqveVtihVJ9AkvUCgvxm2EhIRXT0n4ECWOKz3+SmJw7wXsFSFQrP8DJ6L
# GYnn8AtqgcKBGUIZUnWKNsIdw2FzLixre24/LAl4FOmRsqlb30mjdAy87JGA0j3m
# Sj5mO0+7hvoyGtmW9I/2kQH2zsZ0/fZMcm8Qq3UwxTSwethQ/gpY3UA8x1RtnWN0
# SCyxTkctwRQEcb9k+SS+c23Kjgm9swFXSVRk2XPXfx5bRAGOWhmRaw2fpCjcZxko
# JLo4S5pu+yFUa2pFEUep8beuyOiJXk+d0tBMdrVXVAmxaQFEfnyhYWxz/gq77EFm
# PWn9y8FBSX5+k77L+DvktxW/tM4+pTFRhLy/AsGConsXHRWJjXD+57XQKBqJC482
# 2rpM+Zv/Cuk0+CQ1ZyvgDbjmjJnW4SLq8CdCPSWU5nR0W2rRnj7tfqAxM328y+l7
# vzhwRNGQ8cirOoo6CGJ/2XBjU02N7oJtpQUQwXEGahC0HVUzWLOhcGbyoYICzzCC
# AjgCAQEwgfyhgdSkgdEwgc4xCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5n
# dG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9y
# YXRpb24xKTAnBgNVBAsTIE1pY3Jvc29mdCBPcGVyYXRpb25zIFB1ZXJ0byBSaWNv
# MSYwJAYDVQQLEx1UaGFsZXMgVFNTIEVTTjpGN0E2LUUyNTEtMTUwQTElMCMGA1UE
# AxMcTWljcm9zb2Z0IFRpbWUtU3RhbXAgU2VydmljZaIjCgEBMAcGBSsOAwIaAxUA
# s8lw20WzmxDKiN1Lhh7mZWXutKiggYMwgYCkfjB8MQswCQYDVQQGEwJVUzETMBEG
# A1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWlj
# cm9zb2Z0IENvcnBvcmF0aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFt
# cCBQQ0EgMjAxMDANBgkqhkiG9w0BAQUFAAIFAOcx3DEwIhgPMjAyMjExMzAxNzQy
# MDlaGA8yMDIyMTIwMTE3NDIwOVowdDA6BgorBgEEAYRZCgQBMSwwKjAKAgUA5zHc
# MQIBADAHAgEAAgIfyzAHAgEAAgISDzAKAgUA5zMtsQIBADA2BgorBgEEAYRZCgQC
# MSgwJjAMBgorBgEEAYRZCgMCoAowCAIBAAIDB6EgoQowCAIBAAIDAYagMA0GCSqG
# SIb3DQEBBQUAA4GBAJ3605xI0aJVviKCL6pxv6JSA9hs0xKzQCcjU1L3cxnEFvDK
# Eqi9iKBM8w6EZk8wWb/o1DjGZqKjCZgcuvrHou5ayLsrXnFu2RbKnbsNRlJLbzAW
# GHkOVTvlXsV3dYbTsFHBw+SBrHTUQteRW4o7DNxpSeYvU5jENaUutRjfP9P6MYIE
# DTCCBAkCAQEwgZMwfDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24x
# EDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlv
# bjEmMCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3RhbXAgUENBIDIwMTACEzMAAAGl
# AN4IxEAHcU4AAQAAAaUwDQYJYIZIAWUDBAIBBQCgggFKMBoGCSqGSIb3DQEJAzEN
# BgsqhkiG9w0BCRABBDAvBgkqhkiG9w0BCQQxIgQgGXSVezh9+9WdJYguJjsBV0rh
# y4odNWU1eNJEvcZdF3gwgfoGCyqGSIb3DQEJEAIvMYHqMIHnMIHkMIG9BCC4Cjhx
# fmYEsaCt2AU83Khh+6JHlyk3B70vfMHMlBLcXDCBmDCBgKR+MHwxCzAJBgNVBAYT
# AlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYD
# VQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBU
# aW1lLVN0YW1wIFBDQSAyMDEwAhMzAAABpQDeCMRAB3FOAAEAAAGlMCIEIKhhPQ2b
# BRlpoaiOQGUDYKOMolYwFys2JDtghrSAwgj0MA0GCSqGSIb3DQEBCwUABIICALP4
# Cyiznv+Wuv1lmytaIRfqTiiDV5E476CUu3Q6EgaOOTQfnhgCGW/PJl3cTeqk4wPv
# cCyNQd6IaWNQbDsL10Pe1vUdyU3wRT8i5bPD16qBqQl+0h5ULaw6NAx5qIWxKNnh
# +0Dqf2w52q44+LCY7EZljozfWFtprNb1PEgdlA9X6JypG+3mPAobTNfPBzRMyBf9
# 2LRG9EI+v3heB2wNCggxXUu6qLNB4e+lRKW3o9b94U2t0ovgM4jEqGy3qr6+Fa3U
# 3rnbY2rEeUJSTUirSWYJOuaT8QIGJW0M1bJaJdx8q2rpeiDlmQgl6Yi1HFCSB48F
# nx687q0D5GJT6tiHvE1EJ3WJKW7KqZcSakPgS5wA6ziYjkpKthiMdEpKhgmpjq4Y
# YTEuZmxxurKD5QjPijRIiJzX4/0sPClcEOZaVZzgcsSJBb3lnl6+HD0Ehqd4Rw5+
# 4JtuAdcIACR0wMx7wN4yLYAFZaGYLX1yVPRSmzLZevnrxhF9luiSyu/Hrf096emk
# D10BKjWkw5mf8jRsUcQWjWsgw+/g1tqYNRa1/Ad1bqU+6U49T1iwgWpAiRySfCzo
# ESr5Iu5oACDoni81x67NF9EHXz22EKev4S1TZ60Wd7/G7FgghE5Tn01sLxIS2ef7
# 2EXcDcIUFDaKS3zu3qfdAXUergp/+qQgEyCDh/8O
# SIG # End signature block
