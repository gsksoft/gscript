//------------------------------------------------------------------------------
// <copyright file="FrmMain.cs">
//     Copyright (c) gsksoft. All rights reserved.
// </copyright>
// <description></description>
//------------------------------------------------------------------------------
namespace Gsksoft.GScript.Editor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    
    using ICSharpCode.TextEditor.Document;
    
    using Gsksoft.GScript.Core;

    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void mnuRun_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void UpdateUI()
        {
            // NOTE: xshd for GScript has been embedded in ICSharpCode.TextEditor
            textEditorControl.Document.HighlightingStrategy
                = HighlightingStrategyFactory.CreateHighlightingStrategy("GScript");
        }

        private void Run()
        {
            Lexer lexer = new Lexer();
            string source = textEditorControl.Text;
            var tokens = lexer.Scan(source);

            Parser parser = new Parser();
            var program = parser.Parse(tokens);

            ExecutionContext context = ExecutionContext.CreateContext(
                GScriptIO.Create(Output), Scope.CreateGlobalScope());

            string result = (program.Eval(context) ?? "None").ToString();
            Output(string.Format("> {0}", result));
        }

        private void Output(object o)
        {
            txtOutput.AppendText(o + Environment.NewLine);
        }
    }
}
