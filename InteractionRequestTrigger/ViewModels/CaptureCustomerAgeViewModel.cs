using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using System;

namespace InteractionRequestTrigger.ViewModels
{
    public class CaptureCustomerAgeViewModel : ObservableConfirmation, IInteractionRequestAware
    {
        public DelegateCommand CloseDialog { get; set; }
        public DelegateCommand BypassCloseDialog { get; set; }
        public Action FinishInteraction { get; set; }
        private ConfirmCustomerAge CapturedAge { get; set; }
        public INotification Notification
        {
            get
            {
                return CapturedAge;
            }
            set
            {
                var val = value as ConfirmCustomerAge;
                if (val != null)
                {
                    CapturedAge = val;
                    RaisePropertyChanged();
                }
            }
        }

        public CaptureCustomerAgeViewModel()
        {
            CloseDialog = new DelegateCommand(HandleCloseDialog, IsQualified).ObservesProperty(() => CapturedAge.BirthDate);
            BypassCloseDialog = new DelegateCommand(BypassHandleCloseDialog);

        }

        private bool IsQualified()
        {
            return CapturedAge != null
                && CapturedAge.BirthDate.HasValue
                && CapturedAge.BirthDate.Value < DateTime.Now.AddYears(MinYears * -1).Date;
        }

        private void HandleCloseDialog()
        {
            CapturedAge.Confirmed = true;
            FinishInteraction();
        }

        private void BypassHandleCloseDialog()
        {
            CapturedAge.BirthDate = DateTime.Now.AddYears(MinYears + 2);

            HandleCloseDialog();
        }

        public int MinYears
        {
            get
            {
                return 18;
            }
        }

        public DateTime MinBirthDate
        {
            get
            {
                return DateTime.Now.AddYears(MinYears * -1);
            }
        }
    }
}
