module Resources

open System
open System.Reflection
open System.Drawing

/// <summary>Key of ChatWork API key.</summary>
[<Literal>]
let API_KEY_NAME = @"CW_API_KEY"

/// <summary>Assembly object.</summary>
let assembly = Assembly.GetExecutingAssembly()

/// <summary>Assembly object.</summary>
let assemblyName = assembly.GetName()

/// <summary>Product name.</summary>
let productName =
    let attrType = typeof<AssemblyProductAttribute>
    match Attribute.GetCustomAttribute(assembly, attrType) with
        | :? AssemblyProductAttribute as atr -> atr.Product
        | _ -> assemblyName.Name

/// <summary>Company.</summary>
let company =
    let attrType = typeof<AssemblyCompanyAttribute>
    match Attribute.GetCustomAttribute(assembly, attrType) with
        | :? AssemblyCompanyAttribute as atr -> atr.Company
        | _ -> @""

/// <summary>Copyright.</summary>
let copyright =
    let attrType = typeof<AssemblyCopyrightAttribute>
    match Attribute.GetCustomAttribute(assembly, attrType) with
        | :? AssemblyCopyrightAttribute as atr -> atr.Copyright
        | _ -> @""

/// <summary>Current version.</summary>
let version =
    assemblyName.Version.ToString()

/// <summary>Instance of icon.</summary>
let icon =
    use stream = @"logo.ico" |> assembly.GetManifestResourceStream
    new Icon(stream)

/// <summary>ChatWork API key.</summary>
let apikey =
    let target = EnvironmentVariableTarget.User
    Environment.GetEnvironmentVariable(API_KEY_NAME, target)

/// <summary>ChatWork API key.</summary>
let aboutMessage =
    sprintf "%s Version %s\n\n%s\n%s" productName version copyright company
