namespace TaskTrackingSystem.Shared.Entities
{
    public class OperationResult
    {
        public OperationResult(bool success, string errorMessage)
        {
            this.Success = success;
            this.ErrorMessage = errorMessage;
        }

        public OperationResult(bool success)
        {
            this.Success = success;
        }

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
