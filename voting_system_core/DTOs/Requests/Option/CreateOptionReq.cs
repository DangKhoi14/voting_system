namespace voting_system_core.DTOs.Requests.Option
{
    public class CreateOptionReq
    {
        public string PollIdStr { get; set; }

        public string OptionText { get; set; }
    }
}
