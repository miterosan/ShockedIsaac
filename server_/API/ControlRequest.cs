namespace ShockedIsaac.API
{
    public class ControlRequest
    {
        public required Shocker Shocker { get; set; }
        public required ShockerCommandType Type { get; set; }
        public required int Amount { get; set; }
        public required int Duration { get; set; }
        public required string Name { get; set; }
    }
}