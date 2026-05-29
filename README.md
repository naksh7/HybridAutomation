# HybridAutomation

A comprehensive hybrid test automation framework designed for CCH Central (Tax and Accounting Applications) UK products, supporting both desktop (WinAppDriver) and web (Playwright) application testing with integrated Azure DevOps integration.

## Table of Contents
- [Overview](#overview)
- [Architecture](#architecture)
- [Core Components](#core-components)
- [Log Management](#log-management)
- [Test Data Management](#test-data-management)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Usage Examples](#usage-examples)
- [Contributing](#contributing)
- [Support and Documentation](#support-and-documentation)

## Overview

This framework is specifically built for automated testing of CCH's UK Tax and Accounting applications, primarily targeting the **ClientFrameWork** desktop application while also supporting web-based testing scenarios. The framework implements a hybrid approach combining:

- **Desktop Application Testing** using WinAppDriver for CCH Central ClientFrameWork application
- **Web Application Testing** using Microsoft Playwright for web-based CCH applications  
- **Data-Driven Testing** with Excel-based test data management using EPPlus
- **Azure DevOps Integration** for test case management and attachment handling
- **Advanced Test Step Data Parsing** with direct extraction from Azure DevOps test case steps
- **Sophisticated Multi-Format Logging** with synchronized ExtentReports, file logging, and console output
- **PDF Comparison** capabilities with automated diff-pdf tool setup and environment management
- **Advanced Process Management** and automated environment setup
- **Enterprise-Grade Exception Handling** with comprehensive error tracking and debugging support
- **Comprehensive Test Framework** for complete workflow automation
- **Text-to-Speech Audio Features** for accessibility testing and execution feedback
- **SQL Database Integration** for data validation and database operations
- **Advanced File Management** with JSON and XML configuration support

## Architecture
The framework follows a modular Page Object Model (POM) architecture with centralized utility management:
```
HybridAutomation/
├── HybridAutomation.Helpers/            # Core utilities and helper classes
├── HybridAutomation.POM.OnPrem/         # Page Object Models for on-premise applications
├── HybridAutomation.POM.Cloud/       # Page Object Models for OneClick applications
├── HybridAutomation.SmokeTests/         # Test suite and test infrastructure
└── HybridAutomation.RegressionTests/    # Comprehensive regression test suite
```

## Core Components
### HybridAutomation.Helpers - Core Utilities

| Class | Purpose | Key Features |
|-------|---------|------------|
| `Utilities` | Central access point for all helper classes | Static properties for all utilities |
| `Logger` | Multi-format logging orchestrator | Synchronized HTML, File, Console logging coordination |
| `DriverManager` | **WinAppDriver and WebDriver session management | Unified session creation, screenshot capture, process management |
| `Excel` | Excel data management with EPPlus | Dynamic worksheet loading, structured data access, multi-row support |
| `Azure` | Azure DevOps integration | Test case parameters, attachment management, comprehensive work item API, test step parsing |
| `PDF` | PDF processing and comparison | Automated diff-pdf setup, comparison utilities |
| `ProcessHandler` | Process lifecycle management | WinAppDriver startup, ClientFrameWork control |
| `EnvironmentSetup` | Automated tool installation | ZIP extraction, PATH configuration, cleanup, audio service management |
| `Files` | File operations and verification | Comprehensive file management utilities |
| `Xpath` | Dynamic XPath generation | Element location, page source analysis |
| `FileReader` | JSON and XML configuration management | Strongly-typed configuration loading with JSON and XML support |
| `Input` | Windows input automation | Modern SendInput API, Unicode text input, mouse/keyboard operations |
| `WinApp` | Desktop application automation | Enhanced session initialization with application path support |
| `Playwright` | Web application automation | Microsoft Playwright browser automation methods |
| `Speech` | Text-to-speech functionality | Configurable voice synthesis, audio service management, accessibility support |
| `SQL` | SQL Server database operations | Dynamic query execution, connection management, transaction support |
| `Msaa` | Lightweight MSAA helper for desktop element access | Small XPath-like selector support, Get/Set text, SendKeys, disposal and caching |

### Page Object Models

| Project | Target Application | Purpose |
|---------|-------------------|---------|
| `HybridAutomation.POM.OnPrem` | Desktop ClientFrameWork | Desktop application page objects | 
| `HybridAutomation.POM.Cloud` | Web applications | Web-based application page objects | 

### Test Project Organization
| Project | Purpose | 
|---------|---------|
| `HybridAutomation.SmokeTests` | Primary test project containing workflow tests, smoke tests and test infrastructure |
| `HybridAutomation.RegressionTests` | Comprehensive feature coverage and regression testing | 


## Log Management     
```
Utilities.Logger (Orchestrator)
              ↓
┌─────────────┬─────────────────┬
│             │                 │                 
ExtentLogger  FileLogger       ConsoleLogger
(HTML)        (Text Files)     (Real-time)
│             │                 │
↓             ↓                 ↓
Rich HTML     Trace Files       Console Output
Reports       with timestamps   for CI/CD
```

### 1. **Extent Logger (HTML Reports)**
- **Rich Visual Documentation** with embedded screenshots and dark theme
- **Test execution timeline** and interactive status indicators
- **Base64-encoded screenshot attachment** for Error and Fail log types
- **Thread-safe operations** for parallel test execution
- **Automatic NUnit test context** attachment for CI/CD integration

### 2. **File Logger (Persistent Text Logs)**
- **Timestamped trace files** with structured test class and method boundaries
- **Detailed execution logs** with step-by-step tracking and comprehensive formatting
- **Automatic directory creation** and organized file management
- **Synchronized test method markers** matching HTML and console outputs

### 3. **Console Logger (Real-time Output)**
- **Live test execution feedback** for immediate monitoring and debugging
- **CI/CD pipeline integration** support with structured console formatting
- **Synchronized method boundaries** with other logging formats
- **Real-time error reporting** for development and troubleshooting

### Report Locations and Naming

Reports are automatically generated with timestamped names in organized directory structures:
- **HTML Reports**: `{TestArtefacts}\{TestName}_{timestamp}\ExtentReport_{timestamp}.html`
- **Text Logs**: `{TestArtefacts}\{TestName}_{timestamp}\Trace_{timestamp}.txt`
- **Screenshots**: Embedded in HTML reports as Base64 strings for complete portability

### Log Type Categories
The framework defines comprehensive log types for consistent categorization:

| Log Type | Purpose | Visual Indicator | Screenshot Support |
|----------|---------|------------------|-------------------|
| `Info` | General informational messages | ℹ️ | No |
| `Pass` | Successful test steps or validations | ✅ | No |
| `Warning` | Non-critical issues | ⚠️ | Yes |
| `Skip` | Skipped test steps or scenarios | ⏭️ | No |
| `Error` | Technical failures or exceptions | ❌ | Yes |
| `Fail` | Test failures or assertion errors | ❌ | Yes |

### Centralized Logging Usage

The logging system is automatically managed by the TestBase class, but can also be used directly:// Initialize logging (done automatically in TestBase)
```csharp
// Initialize logging (done automatically in TestBase)
Utilities.Logger.Initialize(traceReport, htmlReport, className);

// Start logging for test method (done automatically in TestBase)
Utilities.Logger.StartLogging(methodName);

// Log different message types with synchronized output
Utilities.Logger.Log(Logger.LogType.Info, "Test step completed successfully");
Utilities.Logger.Log(Logger.LogType.Pass, "Verification passed");
Utilities.Logger.Log(Logger.LogType.Warning, "Non-critical issue detected", isScreenshotNeeded, screenshot);
Utilities.Logger.Log(Logger.LogType.Error, "Error occurred", isScreenshotNeeded, screenshot);
Utilities.Logger.Log(Logger.LogType.Fail, "Test step failed", isScreenshotNeeded, screenshot);

// Flush logs (done automatically in TestBase)
Utilities.Logger.Flush(methodName);
```

## Test Data Management
### Advanced Data Management with Azure DevOps Test Steps

The framework provides sophisticated data parsing capabilities through the `Data` utility class, enabling direct extraction of test data from Azure DevOps test case steps. This approach creates a single source of truth where test steps serve as both documentation and executable test data.

#### Azure DevOps Test Case Structure
The framework expects Azure DevOps test cases with structured steps containing:

#### Test Step Actions (Input Data)
```
Enter value in Screen Name
Navigate to Screen Name 1 > Screen Name 2 > Screen Name 3 > Screen Name 4 > Screen Name 5
Row 1
FieldName1 : Value1
FieldName2: Value2
Row 2  
FieldName1: Value1
FieldName2: Value2
```

#### Expected Results (Output Data)
```
Verify Bad Debts Calculations
Row 1
FieldName1 : Value1
FieldName2: Value2
Row 2
FieldName1 : Value1
FieldName2: Value2
```

#### Data Parsing Operations
```csharp
// Load test case steps from Azure DevOps
string testCaseId = "testCaseID";
Utilities.Azure.LoadTestCaseInfo(testCaseId);  

// Extract navigation path from test steps
string navigation = Utilities.Azure.ParseNavigation("ScreenName");
// Returns: "Corporation Tax > Charities Trading > Trading Income > Trade Name > Bad Debts"

// Parse input data from action steps
var inputData = Utilities.Azure.ParseInputStepsData("ScreenName");
// Returns: List<Dictionary<string, string>> with parsed row data

// Parse output data from expected results
var outputData = Utilities.Azure.ParseOutputData("ScreenName");
// Returns: List<Dictionary<string, string>> with verification data

// Get specific values with row indexing (1-based)
string value1 = Utilities.Azure.GetValue(inputData, "FieldName", 1); 
string value2 = Utilities.Azure.GetValue(outputData, "FieldName", 1);

// Multi-row data processing
var ScreenData = Utilities.Azure.ParseInputStepsData("Screen Name");
var DataList = Enumerable.Range(1, DataList.Count)
    .Select(i => new DataDetails
    {
        FieldName1 = Utilities.Azure.GetValue(disallowableAdjustmentData, "FieldName1", i),
        FieldName2 = Utilities.Azure.GetValue(disallowableAdjustmentData, "FieldName2", i),
    }).ToList(); 
```

#### Data Parsing Methods
| Method | Purpose | Returns |
|--------|---------|---------|
| `LoadTestCaseInfo(testCaseId)` | Initialize Azure class with Azure DevOps test case | void |
| `ParseNavigation(actionText)` | Extract navigation path from test steps | string (navigation path) |
| `ParseInputStepsData(screenName)` | Parse input data from action steps | List<Dictionary<string, string>> |
| `ParseOutputData(screenName)` | Parse output data from expected results | List<Dictionary<string, string>> |
| `GetValue(data, key, index)` | Get specific value from parsed data | string |
| `ParseMultiRowData(rawData)` | Parse static multi-row data (legacy) | List<Dictionary<string, string>> |

#### Benefits of Test Step Data Parsing
1. **Single Source of Truth**: Test steps serve as both documentation and executable data
2. **Automatic Synchronization**: Changes in Azure DevOps automatically reflect in test execution
3. **Living Documentation**: Test cases remain current and accurate
4. **Reduced Maintenance**: No separate Excel files to maintain
5. **Improved Traceability**: Direct link between test documentation and execution
6. **Version Control**: Azure DevOps provides built-in versioning for test data changes

### Advanced Data Management with Excel
#### Excel Structure
```
The framework expects Excel files with this standardized structure:
| Column | Purpose | Example |
|--------|---------|---------|
| Screen Name | Test screen identifier | "Login", "POA", "Reports" |
| Input Navigation | Navigation path to input | "Home > Settings" |
| Input Field | Field identifier | "Username", "End Date" |
| Input Value | Value to input | "testuser", "31/03/2024" |
| Verification Navigation | Navigation for verification | "Reports > Summary" |
| Verification Field | Field to verify | "Total Amount" |
| Verification Value | Expected value | "£1,234.56" |
```

#### Multi-Row Data Structure
```
For handling multiple data entries, use the Row-based naming convention:
| Screen Name | Input Field | Input Value |
|-------------|-------------|-------------|
| Screen Name | Row1_Field1 | "Value 1" |
| Screen Name | Row1_Field2 | "Value 2" |
| Screen Name | Row2_Field1 | "Value 3" |
| Screen Name | Row2_Field2 | "Value 4" |
```

#### Excel Operations
```csharp
// Load entire Excel file
Utilities.Excel.LoadExcelAndMapData(filePath);

// Load specific worksheet  
Utilities.Excel.LoadExcelAndMapData(filePath, "WorksheetName");

// Retrieve input data
string inputValue = Utilities.Excel.GetInputValueByScreenAndField("ScreenName", "FieldName");
string navigation = Utilities.Excel.GetInputNavigationByScreen("ScreenName");

// Retrieve verification data
string verificationValue = Utilities.Excel.GetVerificationValueByScreenAndField("ScreenName", "FieldName");
string verificationNav = Utilities.Excel.GetVerificationNavigationByScreen("ScreenName");

// Get number of entries for multi-row processing
int entriesCount = Utilities.Excel.GetEntriesForInputFields("ScreenName");
int entriesCount = Utilities.Excel.GetEntriesForVerificationFields("ScreenName");

// Multi-row data processing
var dataList = Enumerable.Range(1, Utilities.Excel.GetEntriesForInputFields("Screen Name"))
    .Select(i => new DataDetails
    {
        Field1 = Utilities.Excel.GetInputValueByScreenAndField("Screen Name", $"Row{i}_Field1"),
        Field2 = Utilities.Excel.GetInputValueByScreenAndField("Screen Name", $"Row{i}_Field2")
    }).ToList();

```
#### Data Management Options Comparison

| Approach | Data Source | Best For | Key Benefits |
|----------|-------------|----------|--------------|
| **Excel-Based** | Excel files with structured data | Large datasets, complex data relationships | Easy data maintenance, visual data management |
| **Test Steps Parsing** | Azure DevOps test case steps | Living documentation, step-driven testing | Single source of truth, automatic sync with test cases |
| **Hybrid Approach** | Both Excel and Test Steps | Complex scenarios requiring both approaches | Maximum flexibility and data source options |

## Prerequisites

### Software Requirements
- **.NET 8.0** or higher
- **CCH Central** application (target application)
- **Visual Studio 2022** or **VS Code** with C# extension

### NuGet Dependencies

The framework utilizes these key packages:

#### HybridAutomation.Helpers Project
- `Accessibility` (4.6.0-preview3-27504-2) - Lightweight COM accessibility interop
- `Appium.WebDriver` (4.3.1) - Desktop application automation
- `Microsoft.Playwright` (1.60.0) - Cross-browser web automation
- `EPPlus` (7.0.0) - Excel file processing and data management
- `ExtentReports` (5.0.4) - Rich HTML report generation with screenshots
- `itext` (9.2.0) - PDF processing and comparison capabilities
- `Microsoft.Data.SqlClient` (6.1.0) - SQL Server database connectivity
- `System.Speech` (8.0.0) - Text-to-speech functionality


#### Test Projects (SmokeTests, RegressionTests)
- `Microsoft.NET.Test.Sdk` (17.14.1) - Test SDK and execution framework
- `NUnit` (4.3.2) - Core testing framework with lifecycle management
- `NUnit.Analyzers` (4.9.2) - Static code analysis for test quality
- `NUnit3TestAdapter` (5.0.0) - Test discovery and execution adapter
- `coverlet.collector` (6.0.4) - Code coverage collection and reporting

## Getting Started
### 1. Installation Process

1. **Clone the repository:** 
`git clone https://wkuk-git-vsts@dev.azure.com/wkuk-git-vsts/CCH%20Journey%20to%20Cloud/_git/HybridAutomation`

2. **Install WinAppDriver:**
Run `Utilities.EnvironmentSetup.SetUpWinAppDriver(downloadURL, downloadPath);`

3. **Set diff-pdf tool:** 
Run `Utilities.EnvironmentSetup.SetupDiffPdfToolSystemWide(diffpdfDownloadURL, installationPath)`

4. **Set Developer Mode:** 
Run `Utilities.EnvironmentSetup.EnableDeveloperMode();`

5. **Restore NuGet packages:**
dotnet restore

6. **Build the solution:**
dotnet build


### 2. Configuration Setup
### Configuration Properties

| Setting | Description | Default/Example |
|---------|-------------|-----------------|
| `TestArtefacts` | Base directory for test reports and artifacts | `C:\\Hybrid Automation\\TestArtefacts` |
| `WinAppDriverURI` | WinAppDriver service endpoint | `http://127.0.0.1:4723` |
| `Browser` | Default browser for web testing | `chromium` (firefox, webkit) |
| `WinAppDriverExePath` | Full path to WinAppDriver executable | `C:\\Program Files (x86)\\Windows Application Driver\\WinAppDriver.exe` |
| `ApplicationName` | Target application process name | `ClientFrameWork` |
| `AzureUrl` | Azure DevOps base URL | `https://dev.azure.com` |
| `AzureOrganization` | Azure DevOps organization name | `wkuk-git-vsts` |
| `AzureProject` | Azure DevOps project name | `CCH Software Portfolio` |
| `VSTSPAT` | Personal Access Token for Azure DevOps | (Secure token for authentication) |

Update the `appsettings.json` file in the `HybridAutomation.SmokeTests` project


### 3. Building the Solution
1.  Clean and rebuild the entire solution

        dotnet clean
        dotnet build

2.  Build specific projects

        dotnet build HybridAutomation.Helpers
        dotnet build HybridAutomation.SmokeTests
        dotnet build HybridAutomation.RegressionTests

### 4. Running Tests
1.  Run all tests in the solution

        dotnet test
2.  Run tests from specific projects

        dotnet test HybridAutomation.SmokeTests\HybridAutomation.SmokeTests.csproj
        dotnet test HybridAutomation.RegressionTests\HybridAutomation.RegressionTests.csproj
3. Run specific test projects

        dotnet test HybridAutomation.SmokeTests
        dotnet test HybridAutomation.RegressionTests
5. Run by any class name

        dotnet test --filter "FullyQualifiedName=HybridAutomation.SmokeTests.Golden.CorporationTax.GoldenTest01"

6.  Run by test category

        dotnet test --filter "Category=Smoke"
        dotnet test --filter "Category=Regression"


## Usage Examples

### Logger Usage
```csharp
Utilities.Logger.Log(Logger.LogType.Info, $"Clicked on File");
Utilities.Logger.Log(Logger.LogType.Pass, $"Task performed successfully");
Utilities.Logger.Log(Logger.LogType.Skip, $"Skipped the task");
Utilities.Logger.Log(Logger.LogType.Warning, $"Test has warning.", isScreenshotNeeded, Utilities.DriverManager.GetBase64Screenshot());   
Utilities.Logger.Log(Logger.LogType.Error, $"Test failed with error. \n{ex.Message} \n {ex.StackTrace}", isScreenshotNeeded, Utilities.DriverManager.GetBase64Screenshot()); 
Utilities.Logger.Log(Logger.LogType.Fail, $"Test failed with exception. \n{ex.Message} \n {ex.StackTrace}", isScreenshotNeeded, Utilities.DriverManager.GetBase64Screenshot());                       
```
### MSAA Usage

Provides lightweight wrapper around Microsoft Active Accessibility (MSAA) for locating and interacting with desktop UI elements using a simple XPath-like syntax. This is useful for interacting with legacy desktop controls where WinAppDriver or UIA may be insufficient.

**Key points:**
- Supports XPath-like selectors such as: //window[@Name='Main Window']/editable_text[@Name='Field Name']
- Only the @Name attribute is currently supported and roles map to MSAA roles (window, editable_text, etc.)
- Provides methods: SetTextIn(By selector, string text), GetText(By selector) and SendKeysIn(Keys keys)
- Internally caches window accessibles to reduce expensive window enumeration; implements IDisposable to release COM RCWs
- Thread-safe access for cache and operations

**Usage examples**
```
// Set text using an MSAA XPath-like selector
By description = By.XPath("//window[@Name='Short Life Assets']/editable_text[@Name='Description']");
Utilities.Msaa.SetTextIn(description, "Test123");

// Read text
By DateOfTransfOut = By.XPath("//window[@Name='Short Life Assets']/editable_text[@Name='DateOfTransfOut']");
string value = Utilities.Msaa.GetText(DateOfTransfOut);

// Send keyboard keys to the currently resolved control
Utilities.Msaa.SendKeysIn(Keys.Tab);

// Dispose when you need to release cached COM objects (utilities.ResetAll disposes Msaa automatically)
Utilities.Msaa.Dispose();
```
**Disposal guidance:**

- Msaa caches window accessible objects to improve performance. Call Utilities.Msaa.Dispose() (or Utilities.ResetAll()) when tests complete or when you need to ensure RCWs are released.
- Avoid disposing Msaa in the middle of operations on the same thread unless you recreate the instance.

### WinApp Usage

Provides centralized convenience methods for starting and interacting with desktop application sessions via WinAppDriver.

**Key points:**
- Initialize a WinAppDriver session with Utilities.WinApp.InitializeSession(applicationPath, applicationName, winAppDriverExePath, winAppDriverURI, debug: false)
- Sessions are created via Utilities.DriverManager and exposed as Utilities.WinApp.Driver (WindowsDriver<WindowsElement>) for direct Appium interactions
- Common helper methods (used throughout the framework) include Utilities.WinApp.InitializeSession and Utilities.WinApp.WaitForAppIdle(milliseconds)
- Use debug mode to skip launching the application when you only need a session skeleton for troubleshooting

**Usage examples**
```
// Initialize and attach to the application (launches app unless debug:true)
Utilities.WinApp.InitializeSession(appPath, ApplicationName, WinAppDriverExePath, WinAppDriverURI);

// Wait for the application UI to become idle after operations
Utilities.WinApp.WaitForAppIdle(500);

// Access the underlying driver for standard Appium operations
var element = Utilities.WinApp.Driver.FindElementByAccessibilityId("SomeControlId");
element.Click();

// Use together with Msaa for legacy controls
By description = By.XPath("//window[@Name='Short Life Assets']/editable_text[@Name='Description']");
Utilities.Msaa.SetTextIn(description, "Test123");

// Dispose/cleanup when tests finish (Utilities.ResetAll will dispose Msaa automatically)
Utilities.ResetAll();
```
**Disposal guidance:**

- The WinApp session (Utilities.WinApp.Driver) should be quit or recreated via Utilities.DriverManager when tests end; Utilities.ResetAll helps clear utility singletons.
- Use debug mode (debug:true) to initialize session logic without launching the application when troubleshooting.

### Playwright Usage

Provides centralized helper methods for web automation using Microsoft Playwright with multi-browser support.

**Key points:**
- Initialize a browser session using Utilities.Playwright.InitializeDriver(browserType, headless, downloadPath)
- Supported browser types: `"chromium"`, `"firefox"`, `"webkit"` (default: `"chromium"`)
- Use Utilities.Playwright for common browser interactions (NavigateToURL, SetText, Click, GetText, WaitForSelector, etc.)
- Browser context and page instances are created via Utilities.DriverManager.CreatePlaywrightDriver; the IPage is available via Utilities.DriverManager.PlaywrightDetails.PageInstance for direct access
- CSS selectors, XPath, and Playwright-specific selectors (role, text, etc.) are all supported

**Usage examples**
```
// Start browser (chromium, firefox, webkit) with optional headless mode and download path
Utilities.Playwright.InitializeDriver("chromium", headless: false, downloadPath: downloadFolderPath);

// Navigate to a page
Utilities.Playwright.NavigateToURL("https://example.com");

// Interact with elements using CSS or XPath selectors
Utilities.Playwright.SetText("#username", "testuser");
Utilities.Playwright.Click("button[type='submit']");

// Retrieve text or attribute values
string status = Utilities.Playwright.GetText("#status");
string href = Utilities.Playwright.GetAttribute("a.link", "href");

// Send keyboard input without clearing existing content
Utilities.Playwright.SendKeys("#search", "query text");

// Switch between browser tabs
Utilities.Playwright.SwitchToPage(1);

// Switch to an iframe
var frame = Utilities.Playwright.SwitchToFrame("iframe#content");

// Drag and drop
Utilities.Playwright.DragAndDrop("#source", "#target");

// Scroll element into view
Utilities.Playwright.ScrollToElement("#footer");

// Close browser session
Utilities.Playwright.CloseApp();

// Cleanup singletons (also disposes other utilities via ResetAll)
Utilities.ResetAll();
```
**Disposal guidance:**

- Prefer Utilities.Playwright.CloseApp() to properly close the browser, context, and page. This gracefully disposes all Playwright resources.
- Call Utilities.ResetAll() in test teardown to clear singleton utilities and ensure fresh state between tests.

## Contributing

### Development Guidelines

- Follow established patterns demonstrated in GoldenTest01
- Use data models for complex business objects
- Implement comprehensive logging with appropriate log types
- Add error handling with detailed exception information
- Write maintainable Page Object Models with clear method names
- Document complex business logic with inline comments
- Leverage new features like Speech and SQL integration where appropriate

### Code Standards

- Follow C# naming conventions and coding standards
- Use nullable reference types appropriately (enabled in .NET 8)
- Implement proper disposal patterns for resources
- Add comprehensive XML documentation for public methods
- Utilize new framework features for enhanced testing capabilities

## Support and Documentation

### Key Resources

- **Internal Documentation**: Framework includes comprehensive inline documentation
- **Azure DevOps Integration**: Automated test case management and attachment handling
- **Excel Templates**: Standardized data structure for test inputs and verifications
- **Logging Reports**: Detailed HTML and text-based execution reports
- **Audio Features**: Text-to-speech capabilities for accessibility testing
- **Database Integration**: SQL Server connectivity for data validation

### Troubleshooting

#### Common Issues

1. **WinAppDriver Connection Issues**
   - Ensure WinAppDriver is installed and running if not refer GettingStarted
   - Verify the URI configuration in appsettings.json
   - Check Windows Defender firewall settings

2. **Excel Data Loading Problems**
   - Verify Excel file format and structure
   - Check file permissions and accessibility
   - Ensure EPPlus package is properly installed

3. **Azure DevOps Integration Issues**
   - Verify Personal Access Token permissions and validity
   - Check network connectivity to Azure DevOps
   - Validate AzureUrl, AzureOrganization, and AzureProject parameters in appsettings.json
   - Ensure the organization name matches your Azure DevOps organization exactly
   - Confirm the project name is correct and accessible with the provided PAT

4. **Speech/Audio Issues**
   - Ensure Windows Audio service is running
   - Verify audio drivers are properly installed
   - Check for administrator privileges if needed

5. **Database Connection Issues**
   - Verify SQL Server connectivity and credentials
   - Check network access to database server
   - Validate connection string parameters

#### Debug Mode
```csharp
Use debug mode for development and troubleshooting:// Initialize session in debug mode (skips application launch)
Utilities.WinApp.InitializeSession("", ApplicationName, WinAppDriverExePath , WinAppDriverURI, debug: true);
```
---
*This README provides comprehensive guidance for using the HybridAutomation framework. For specific implementation details, refer to the GoldenTest01 example and the extensive inline documentation throughout the codebase.*
