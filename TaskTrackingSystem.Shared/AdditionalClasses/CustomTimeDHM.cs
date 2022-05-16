namespace TaskTrackingSystem.Shared.AdditionalClasses
{
    public class CustomTimeDHM
    {
        private int minutes = 0;

        public void Add(int days, int hours, int mins)
        {
            minutes += days * 24 * 60;
            minutes += hours * 60;
            minutes += mins;
        }

        public void Remove(int days, int hours, int mins)
        {
            minutes -= days * 24 * 60;
            minutes -= hours * 60;
            minutes -= mins;
        }

        public override string ToString()
        {
            var d = minutes / 1440;
            var h = minutes / 60;
            var m = minutes % 1440;


            return $"{minutes}";
        }

    }
}