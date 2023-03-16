using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace Kool.ExpandAll;

[Guid(Ids.PACKAGE)]
[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
[ProvideMenuResource("Menus.ctmenu", 1)]
public sealed class Package : ToolkitPackage
{
    internal const string VERSION = "0.0.0";
    internal const string NAME = "Expand All";
    internal const string URL = "https://github.com/heku/kool.expandall";

    protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        await this.RegisterCommandsAsync();
    }

    [Command(Ids.EXPAND_ALL_CMD_ID)]
    public sealed class ExpandAllCommand : BaseCommand<ExpandAllCommand>
    {
        // https://github.com/VsixCommunity/Community.VisualStudio.Toolkit/blob/master/demo/VSSDK.TestExtension/Commands/ExpandSelectedItemsCommand.cs
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs _)
        {
            var solutionExplorer = await VS.Windows.GetSolutionExplorerWindowAsync();
            if (solutionExplorer is not null)
            {
                var selectedItems = await solutionExplorer.GetSelectionAsync();
                if (selectedItems is not null)
                {
                    foreach (var item in selectedItems)
                    {
                        solutionExplorer.Expand(item, SolutionItemExpansionMode.Recursive);
                    }
                }
            }
        }
    }
}