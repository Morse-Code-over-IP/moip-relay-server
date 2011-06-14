using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Morse News")]
[assembly: AssemblyDescription("Morse Code News Reader (RSS and Twitter)")]
#if MONO_BUILD
[assembly: AssemblyConfiguration("Mono 2.4 or later")]
#else
[assembly: AssemblyConfiguration("Windows XP or later")]
#endif
[assembly: AssemblyCompany("Robert B. Denny, Mesa, AZ <rdenny@dc3.com>")]
[assembly: AssemblyProduct("Morse Code Tools")]
[assembly: AssemblyCopyright("Open Source Common Public Attribution License")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("573ac84d-0375-441b-95fc-35d32c27f62e")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.1.3.0")]
[assembly: AssemblyFileVersion("2.1.3.0")]
