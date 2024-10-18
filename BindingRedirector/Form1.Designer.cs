namespace BindingRedirector;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        label1 = new Label();
        tbFolder = new TextBox();
        btnBrowseFolder = new Button();
        rbOutputType1 = new RadioButton();
        rbOutputType2 = new RadioButton();
        label2 = new Label();
        tbFile = new TextBox();
        btnBrowseFile = new Button();
        btnGenerate = new Button();
        label3 = new Label();
        tbKeywords = new TextBox();
        tbLogs = new RichTextBox();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(27, 23);
        label1.Name = "label1";
        label1.Size = new Size(60, 20);
        label1.TabIndex = 0;
        label1.Text = "Folder:";
        // 
        // tbFolder
        // 
        tbFolder.Location = new Point(93, 20);
        tbFolder.MaxLength = 255;
        tbFolder.Name = "tbFolder";
        tbFolder.Size = new Size(406, 27);
        tbFolder.TabIndex = 1;
        // 
        // btnBrowseFolder
        // 
        btnBrowseFolder.Location = new Point(505, 19);
        btnBrowseFolder.Name = "btnBrowseFolder";
        btnBrowseFolder.Size = new Size(94, 29);
        btnBrowseFolder.TabIndex = 2;
        btnBrowseFolder.Text = "Browse";
        btnBrowseFolder.UseVisualStyleBackColor = true;
        btnBrowseFolder.Click += btnBrowseFolder_Click;
        // 
        // rbOutputType1
        // 
        rbOutputType1.AutoSize = true;
        rbOutputType1.Checked = true;
        rbOutputType1.Location = new Point(51, 68);
        rbOutputType1.Name = "rbOutputType1";
        rbOutputType1.Size = new Size(173, 24);
        rbOutputType1.TabIndex = 3;
        rbOutputType1.TabStop = true;
        rbOutputType1.Text = "Generate to xml file";
        rbOutputType1.UseVisualStyleBackColor = true;
        // 
        // rbOutputType2
        // 
        rbOutputType2.AutoSize = true;
        rbOutputType2.Location = new Point(251, 68);
        rbOutputType2.Name = "rbOutputType2";
        rbOutputType2.Size = new Size(193, 24);
        rbOutputType2.TabIndex = 4;
        rbOutputType2.Text = "Generate to config file";
        rbOutputType2.UseVisualStyleBackColor = true;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(27, 122);
        label2.Name = "label2";
        label2.Size = new Size(65, 20);
        label2.TabIndex = 5;
        label2.Text = "Output:";
        // 
        // tbFile
        // 
        tbFile.Location = new Point(93, 118);
        tbFile.MaxLength = 255;
        tbFile.Name = "tbFile";
        tbFile.Size = new Size(401, 27);
        tbFile.TabIndex = 6;
        // 
        // btnBrowseFile
        // 
        btnBrowseFile.Location = new Point(505, 118);
        btnBrowseFile.Name = "btnBrowseFile";
        btnBrowseFile.Size = new Size(94, 29);
        btnBrowseFile.TabIndex = 7;
        btnBrowseFile.Text = "Browse";
        btnBrowseFile.UseVisualStyleBackColor = true;
        btnBrowseFile.Click += btnBrowseFile_Click;
        // 
        // btnGenerate
        // 
        btnGenerate.Location = new Point(93, 270);
        btnGenerate.Name = "btnGenerate";
        btnGenerate.Size = new Size(202, 29);
        btnGenerate.TabIndex = 8;
        btnGenerate.Text = "Generate";
        btnGenerate.UseVisualStyleBackColor = true;
        btnGenerate.Click += BtnGenerate_Click;
        // 
        // label3
        // 
        label3.Location = new Point(22, 169);
        label3.Name = "label3";
        label3.Size = new Size(577, 62);
        label3.TabIndex = 9;
        label3.Text = "Excluded keywords. Files containing this keyword will be excluded, with multiple keywords separated by commas.";
        // 
        // tbKeywords
        // 
        tbKeywords.Location = new Point(93, 225);
        tbKeywords.MaxLength = 255;
        tbKeywords.Name = "tbKeywords";
        tbKeywords.Size = new Size(401, 27);
        tbKeywords.TabIndex = 10;
        // 
        // tbLogs
        // 
        tbLogs.Dock = DockStyle.Bottom;
        tbLogs.Location = new Point(0, 319);
        tbLogs.Name = "tbLogs";
        tbLogs.Size = new Size(659, 248);
        tbLogs.TabIndex = 11;
        tbLogs.Text = "";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(9F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(659, 567);
        Controls.Add(tbLogs);
        Controls.Add(tbKeywords);
        Controls.Add(label3);
        Controls.Add(btnGenerate);
        Controls.Add(btnBrowseFile);
        Controls.Add(tbFile);
        Controls.Add(label2);
        Controls.Add(rbOutputType2);
        Controls.Add(rbOutputType1);
        Controls.Add(btnBrowseFolder);
        Controls.Add(tbFolder);
        Controls.Add(label1);
        FormBorderStyle = FormBorderStyle.Fixed3D;
        MaximizeBox = false;
        Name = "Form1";
        Text = "Binding Redirector";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private TextBox tbFolder;
    private Button btnBrowseFolder;
    private RadioButton rbOutputType1;
    private RadioButton rbOutputType2;
    private Label label2;
    private TextBox tbFile;
    private Button btnBrowseFile;
    private Button btnGenerate;
    private Label label3;
    private TextBox tbKeywords;
    private RichTextBox tbLogs;
}
