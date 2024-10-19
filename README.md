# BindingRedirector
Generates or updates binding redirects for .NET assemblies.

This tool aims to simplify the process of creating and managing binding redirects for .NET applications, which can be useful when dealing with assembly version conflicts or when updating dependencies in a project.

# Main Features:
   - Scans a selected folder for DLL files.
   - Analyzes each DLL to extract assembly information (name, version, public key token, etc.).
   - Generates binding redirects based on the found assemblies.
   - Can create a new XML file with binding redirects or update an existing config file.
   - Allows filtering of assemblies based on user-provided keywords.

# File Handling:
   - Creates a backup of the existing output file before modifying it.
   - Checks for write access to the output directory.

# XML Processing:
   - Generates new XML elements for binding redirects.
   - Merges new binding redirects with existing ones in the case of updates.


![binding_redirector](https://github.com/user-attachments/assets/5bc44f4e-1b31-4ab7-81f3-f79af26e5ac4)
