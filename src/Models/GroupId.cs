namespace Models
{
    public class GroupId
    {
        public GroupId(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Group id value contains empty string of whitespaces.", nameof(value));
            }

            Value = value;
        }

        public string Value { get; }
    }
}
