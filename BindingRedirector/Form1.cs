using System.Xml.Linq;
using ICSharpCode.Decompiler.Metadata;

namespace BindingRedirector;

public partial class Form1 : Form
{
    private static readonly XNamespace _asmv1 = "urn:schemas-microsoft-com:asm.v1";
    private const string ErrorCaption = "Error";
    private const string WarningCaption = "Warning";
    private const string SuccessCaption = "Success";

    public Form1()
    {
        InitializeComponent();
    }

    private void btnBrowseFolder_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();
        fbd.Description = "Select the folder containing the DLLs";
        DialogResult result = fbd.ShowDialog();

        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
        {
            tbFolder.Text = fbd.SelectedPath;
        }
    }

    private void btnBrowseFile_Click(object sender, EventArgs e)
    {
        using var sfd = new SaveFileDialog();
        sfd.Filter = "XML files (*.xml)|*.xml|Config files (*.config)|*.config|All files (*.*)|*.*";
        sfd.Title = "Select the config file";
        sfd.FileName = "bindingRedirect.xml";
        sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        DialogResult result = sfd.ShowDialog();

        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(sfd.FileName))
        {
            tbFile.Text = sfd.FileName;
        }
    }

    private async void BtnGenerate_Click(object sender, EventArgs e)
    {
        if (!ValidateInputs())
            return;

        string outputPath = tbFile.Text;
        string outputDirectory = Path.GetDirectoryName(outputPath);

        if (!HasWriteAccessToDirectory(outputDirectory))
        {
            ShowMessage("Administrator privileges may be required to write to the selected location.", WarningCaption, MessageBoxIcon.Warning);
            return;
        }

        SetControlsEnabled(false);
        try
        {
            await Task.Run(() => GenerateBindings(outputPath));
            ShowMessage($"File generated successfully: {outputPath}", SuccessCaption, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            ShowMessage($"An error occurred: {ex.Message}", ErrorCaption, MessageBoxIcon.Error);
            LogError(ex);
        }
        finally
        {
            SetControlsEnabled(true);
        }
    }

    private bool ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(tbFolder.Text))
        {
            ShowMessage("Please select a folder containing the DLLs.", ErrorCaption, MessageBoxIcon.Error);
            return false;
        }

        if (string.IsNullOrWhiteSpace(tbFile.Text))
        {
            ShowMessage("Please select an output file.", ErrorCaption, MessageBoxIcon.Error);
            return false;
        }

        return true;
    }

    private void GenerateBindings(string outputPath)
    {
        string backupPath = outputPath + ".bak";

        // Create a backup of the existing file if it exists
        if (File.Exists(outputPath))
        {
            File.Copy(outputPath, backupPath, true);
            AppendToLogs($"Backup created: {backupPath}");
        }

        var bindings = GetBindings();

        if (rbOutputType1.Checked)
        {
            GenerateNewXml(outputPath, bindings);
        }
        else if (rbOutputType2.Checked)
        {
            UpdateExistingXml(outputPath, bindings);
        }
    }

    private void GenerateNewXml(string outputPath, List<DependentAssembly> bindings)
    {
        var xml = new XElement(_asmv1 + "assemblyBinding",
            bindings.Select(CreateDependentAssemblyElement));

        File.WriteAllText(outputPath, xml.ToString());
    }

    private void UpdateExistingXml(string outputPath, List<DependentAssembly> bindings)
    {
        var xml = XDocument.Load(outputPath);
        var newAssemblyBinding = new XElement(_asmv1 + "assemblyBinding",
            bindings.Select(CreateDependentAssemblyElement));

        var existingAssemblyBinding = xml.Descendants(_asmv1 + "assemblyBinding").FirstOrDefault();

        if (existingAssemblyBinding != null)
        {
            MergeAssemblyBindings(existingAssemblyBinding, newAssemblyBinding);
        }
        else
        {
            AddNewAssemblyBinding(xml, newAssemblyBinding);
        }

        xml.Save(outputPath);
    }
    private XElement CreateDependentAssemblyElement(DependentAssembly b)
    {
        var assemblyIdentity = new XElement(_asmv1 + "assemblyIdentity",
              new XAttribute("name", b.Name),
              new XAttribute("culture", b.Culture));

        if (b.PublicKeyToken != null)
        {
            assemblyIdentity.Add(new XAttribute("publicKeyToken", b.PublicKeyToken));
        }

        return new XElement(_asmv1 + "dependentAssembly",
            assemblyIdentity,
            new XElement(_asmv1 + "bindingRedirect",
                new XAttribute("oldVersion", "0.0.0.0-" + b.NewVersion),
                new XAttribute("newVersion", b.NewVersion)));
    }

    private void MergeAssemblyBindings(XElement existingAssemblyBinding, XElement newAssemblyBinding)
    {
        var mergedAssemblyBinding = new XElement(_asmv1 + "assemblyBinding");
        mergedAssemblyBinding.Add(existingAssemblyBinding.Elements(_asmv1 + "dependentAssembly"));

        foreach (var newDependentAssembly in newAssemblyBinding.Elements(_asmv1 + "dependentAssembly"))
        {
            var assemblyIdentity = newDependentAssembly.Element(_asmv1 + "assemblyIdentity");
            var name = assemblyIdentity?.Attribute("name")?.Value;
            var publicKeyToken = assemblyIdentity?.Attribute("publicKeyToken")?.Value;

            var existingElement = mergedAssemblyBinding.Elements(_asmv1 + "dependentAssembly")
                .FirstOrDefault(e =>
                    e.Element(_asmv1 + "assemblyIdentity")?.Attribute("name")?.Value == name &&
                    e.Element(_asmv1 + "assemblyIdentity")?.Attribute("publicKeyToken")?.Value == publicKeyToken);

            if (existingElement == null)
            {
                mergedAssemblyBinding.Add(newDependentAssembly);
            }
            else
            {
                var newBindingRedirect = newDependentAssembly.Element(_asmv1 + "bindingRedirect");
                existingElement.Element(_asmv1 + "bindingRedirect")?.ReplaceWith(newBindingRedirect);
            }
        }

        existingAssemblyBinding.ReplaceWith(mergedAssemblyBinding);
    }

    private void AddNewAssemblyBinding(XDocument xml, XElement newAssemblyBinding)
    {
        var runtimeElement = xml.Descendants("runtime").FirstOrDefault();
        if (runtimeElement == null)
        {
            runtimeElement = new XElement("runtime");
            xml.Root.Add(runtimeElement);
        }
        runtimeElement.Add(newAssemblyBinding);
    }

    private bool HasWriteAccessToDirectory(string directoryPath)
    {
        try
        {
            string tempFilePath = Path.Combine(directoryPath, Path.GetRandomFileName());
            using (FileStream fs = File.Create(tempFilePath, 1, FileOptions.DeleteOnClose))
            {
                // If we can create and delete the file, we have write access
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    private List<DependentAssembly> GetBindings()
    {
        var assemblies = new List<DependentAssembly>();
        try
        {
            var files = Directory.GetFiles(tbFolder.Text, "*.dll", SearchOption.TopDirectoryOnly);
            var keywords = tbKeywords.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(k => k.Trim().ToLower()).ToList();

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file).ToLower();
                if (keywords.Any() && keywords.Any(fileName.Contains))
                {
                    AppendToLogs($"Skipping {file} because it matches a keyword");
                    continue;
                }

                if (!IsManagedAssembly(file))
                {
                    AppendToLogs($"Skipping {file} because it is not a managed assembly");
                    continue;
                }

                var definition = ReadAssemblyInfo(file);
                assemblies.Add(definition);
            }
        }
        catch (Exception e)
        {
            AppendToLogs($"Error in GetBindings: {e.Message}");
            LogError(e);
        }

        return assemblies;
    }

    private void SetControlsEnabled(bool enabled)
    {
        btnGenerate.Enabled = enabled;
        btnBrowseFolder.Enabled = enabled;
        btnBrowseFile.Enabled = enabled;
    }

    private void ShowMessage(string message, string caption, MessageBoxIcon icon)
    {
        MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
    }

    private void AppendToLogs(string message)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => tbLogs.AppendText(message + Environment.NewLine)));
        }
        else
        {
            tbLogs.AppendText(message + Environment.NewLine);
        }
    }

    private void LogError(Exception ex)
    {
        AppendToLogs($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
    }

    private DependentAssembly ReadAssemblyInfo(string assemblyPath)
    {
        try
        {
            var peFile = new PEFile(assemblyPath);
            var reader = peFile.Metadata;
            var definition = reader.GetAssemblyDefinition();

            var token = reader.GetBlobBytes(definition.PublicKey);
            return new DependentAssembly
            {
                Name = reader.GetString(definition.Name),
                NewVersion = definition.Version.ToString(),
                Culture = definition.Culture.IsNil ? "neutral" : reader.GetString(definition.Culture),
                PublicKeyToken = reader.GetPublicKeyToken(),
            };
        }
        catch (Exception ex)
        {
            LogError(ex);
            return null;
        }
    }

    // https://stackoverflow.com/questions/367761/how-to-determine-whether-a-dll-is-a-managed-assembly-or-native-prevent-loading
    private static bool IsManagedAssembly(string fileName)
    {
        using (Stream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        using (BinaryReader binaryReader = new BinaryReader(fileStream))
        {
            if (fileStream.Length < 64)
            {
                return false;
            }

            // PE Header starts @ 0x3C (60). Its a 4 byte header.
            fileStream.Position = 0x3C;
            uint peHeaderPointer = binaryReader.ReadUInt32();
            if (peHeaderPointer == 0)
            {
                peHeaderPointer = 0x80;
            }

            // Ensure there is at least enough room for the following structures:
            // 24 byte PE Signature & Header
            // 28 byte Standard Fields         (24 bytes for PE32+)
            // 68 byte NT Fields               (88 bytes for PE32+)
            // >= 128 byte Data Dictionary Table
            if (peHeaderPointer > fileStream.Length - 256)
            {
                return false;
            }

            // Check the PE signature.  Should equal 'PE\0\0'.
            fileStream.Position = peHeaderPointer;
            uint peHeaderSignature = binaryReader.ReadUInt32();
            if (peHeaderSignature != 0x00004550)
            {
                return false;
            }

            // skip over the PEHeader fields
            fileStream.Position += 20;

            const ushort PE32 = 0x10b;
            const ushort PE32Plus = 0x20b;

            // Read PE magic number from Standard Fields to determine format.
            var peFormat = binaryReader.ReadUInt16();
            if (peFormat != PE32 && peFormat != PE32Plus)
            {
                return false;
            }

            // Read the 15th Data Dictionary RVA field which contains the CLI header RVA.
            // When this is non-zero then the file contains CLI data otherwise not.
            ushort dataDictionaryStart = (ushort)(peHeaderPointer + (peFormat == PE32 ? 232 : 248));
            fileStream.Position = dataDictionaryStart;

            uint cliHeaderRva = binaryReader.ReadUInt32();
            if (cliHeaderRva == 0)
            {
                return false;
            }

            return true;
        }
    }
}
