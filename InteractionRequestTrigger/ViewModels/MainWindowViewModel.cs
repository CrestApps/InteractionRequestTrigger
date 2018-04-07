using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace InteractionRequestTrigger.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public InteractionRequest<INotification> ErrorNotification { get; private set; }
        public InteractionRequest<ConfirmCustomerAge> AgeConfirmation { get; private set; }
        public DelegateCommand ConfirmAge { get; set; }

        public MainWindowViewModel()
        {
            ErrorNotification = new InteractionRequest<INotification>();
            AgeConfirmation = new InteractionRequest<ConfirmCustomerAge>();
            ConfirmAge = new DelegateCommand(HandleConfirmAge);
        }

        private void HandleConfirmAge()
        {
            var dialog = new ConfirmCustomerAge
            {
                Title = "Verify Customer's Age",
            };

            AgeConfirmation.Raise(dialog, DoSomethingCool);
        }

        private void DoSomethingCool(ConfirmCustomerAge obj)
        {
            if (!obj.Confirmed)
            {
                return;
            }

            ShowErrorMessage("Verification", "All Good!");
        }

        private void ShowErrorMessage(string title, string content)
        {
            ErrorNotification.Raise(
                                new Notification
                                {
                                    Content = content,
                                    Title = title
                                });
        }
    }
}
