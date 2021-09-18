namespace StrictlyStatsDataLayer
{
    public static class Global
    {
        public static string DBNAME = "StrictlyStats.db";
        private static IStrictlyStatsUOW uow = null;
        public static IStrictlyStatsUOW UOW
        {
            get
            {
                if (uow == null)
                    uow = new StrictlyStatsUow(DBNAME);
                return uow;
            }
        }

        public static int NUMBEROFCONTESTANTSINFINAL = 2;
    }
}