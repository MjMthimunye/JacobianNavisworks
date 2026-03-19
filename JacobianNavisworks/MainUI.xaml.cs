using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using System;
using System.Linq;
using System.Windows;

namespace JacobianNavisworks
{
    /// <summary>
    /// Interaction logic for MainUI.xaml
    /// </summary>
    public partial class MainUI 
    {
        public MainUI()
        {
            InitializeComponent();
            Loaded += (s, e) => LoadClashData();
        }

        private void LoadClashData()
        {
            int newClashes = 0;
            int activeClashes = 0;
            int resolvedClashes = 0;

            try
            {
                Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
                if (doc == null)
                    return;

                DocumentClash documentClash = doc.GetClash();
                if (documentClash?.TestsData == null)
                    return;

                DocumentClashTests clashTests = documentClash.TestsData;

                // Flatten all clash results (tests → groups → individual clashes)
                var allResults = clashTests.Tests.Cast<ClashTest>()
                    .SelectMany(test => test.Children)
                    .SelectMany(child => child is ClashResultGroup group
                        ? group.Children.Cast<ClashResult>()
                        : new[] { child as ClashResult })
                    .Where(result => result != null);

                foreach (var result in allResults)
                {
                    switch (result.Status)
                    {
                        case ClashResultStatus.New:
                            newClashes++;
                            break;

                        case ClashResultStatus.Active:
                            activeClashes++;
                            break;

                        case ClashResultStatus.Resolved:
                            resolvedClashes++;
                            break;
                    }
                }

                int totalClashes = newClashes + activeClashes + resolvedClashes;

                // Update UI
                TotalClashesText.Text = totalClashes.ToString();
                NewClashesText.Text = newClashes.ToString();
                ActiveClashesText.Text = activeClashes.ToString();
                ResolvedClashesText.Text = resolvedClashes.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

    }
}
