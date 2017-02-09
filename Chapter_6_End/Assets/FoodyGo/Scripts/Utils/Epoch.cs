using System;

namespace packt.FoodyGo.Utils
{
    public static class Epoch
    {
        public static double Now
        {
            get
            {
                System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
                int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
                return cur_time;
            }
        }
    }
}
