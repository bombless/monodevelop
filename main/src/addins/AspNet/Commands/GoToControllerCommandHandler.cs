//
// GoToControllerCommandHandler.cs
//
// Author:
//       Piotr Dowgiallo <sparekd@gmail.com>
// 
// Copyright (c) 2012 Piotr Dowgiallo
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.AspNet.Projects;

namespace MonoDevelop.AspNet.Commands
{

	class GoToControllerCommandHandler : CommandHandler
	{
		protected override void Update (CommandInfo info)
		{
			var doc = IdeApp.Workbench.ActiveDocument;
			AspNetAppProject project;
			if (doc == null || (project = doc.Project as AspNetAppProject) == null || !project.IsAspMvcProject) {
				info.Enabled = info.Visible = false;
				return;
			}
			var baseDirectory = doc.FileName.ParentDirectory.ParentDirectory;
			if (!string.Equals (baseDirectory.FileName, "Views"))
				info.Enabled = info.Visible = false;
		}

		protected override void Run ()
		{
			var doc = IdeApp.Workbench.ActiveDocument;
			var name = doc.FileName.ParentDirectory.FileName;
			var controller = doc.ProjectContent.GetAllTypeDefinitions ().FirstOrDefault (t => t.Name == name + "Controller");

			if (controller != null)
				IdeApp.Workbench.OpenDocument (controller.UnresolvedFile.FileName, doc.Project);
			else
				MessageService.ShowError ("Matching controller cannot be found.");
		}
	}
	
}
