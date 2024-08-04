namespace ME3TweaksModManager.ui
{
    public class M3ProgressEventArgs : EventArgs
    {
        private readonly long _amountDone;
        private readonly long _total;
        private readonly bool _isIndeterminate;

        /// <summary>
        /// Initializes a new instance of the DetailedProgressEventArgs class.
        /// </summary>
        /// <param name="amountCompleted">Amount of work that has been cumulatively completed.</param>
        /// <param name="total">The total amount of work to complete.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        public M3ProgressEventArgs(long amountCompleted, long total, bool isIndeterminate)
        {
            if (amountCompleted < 0 || amountCompleted > total)
            {
                throw new ArgumentOutOfRangeException(@"amountCompleted",
                    @"The amount of completed work (" + amountCompleted + @") must be less than the total (" + total + @").");
            }

            _amountDone = amountCompleted;
            _total = total;
            _isIndeterminate = isIndeterminate;
        }

        /// <summary>
        /// Gets the amount of work that has been completed.
        /// </summary>
        public long AmountCompleted => _amountDone;
        /// <summary>
        /// Gets the total amount of work to do.
        /// </summary>
        public long TotalAmount => _total;

        /// <summary>
        /// If progress is indeterminate
        /// </summary>
        public bool IsIndeterminate => _isIndeterminate;
    }
}
