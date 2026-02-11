using InterviewTracking.Maui.ViewModels;

namespace InterviewTracking.Maui.Views;

public partial class AddEditInterviewPage : ContentPage
{
    public AddEditInterviewPage(AddEditInterviewViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
