namespace Leave_Management_System.Utility
{
    public class UtilityMethods
    {

        public static IEnumerable<DateTime> GetDatesBetween(DateTime start, DateTime end)
        {

            var dates = new List<DateTime>();

            for (DateTime dt = start; dt <= end; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            return dates;
        }
    }
}
