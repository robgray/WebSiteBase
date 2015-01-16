namespace WebBase.Mvc.ViewModels
{
    public enum SiteMessageType
    {
        Success,
        Error,
        Warning,
        Info
    }

    public class SiteMessageViewModel
    {
        public SiteMessageViewModel(SiteMessageType messageType, string message)
        {
            MessageType = messageType;
            Message = message;
            Duration = 5000;
        }

        public SiteMessageViewModel(SiteMessageType messageType, string message, int duration) : this(messageType, message)
        {            
            Duration = duration;
        }

        public SiteMessageType MessageType { get; set; }
        public string Message { get; set; }
        public int Duration { get; set; }
    }
}