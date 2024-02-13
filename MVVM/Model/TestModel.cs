using System.Collections.Generic;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class TestModel
    {
        protected static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public static Queue<Word> Words { get; set; }

        protected static readonly string SqlConnectionString = "Data Source=filian-database.c5ce82k0wnfb.us-west-2.rds.amazonaws.com,1433;Initial Catalog=filian_db;User ID=admin;Password=filian_admin;";

        protected string ConditionForIds;

        protected SqlConnection SqlConnection;
        protected SqlCommand SqlCommand;
        protected SqlDataReader SqlDataReader;

        protected string CreateConditionForIds(List<int> underThemeIds)
        {
            foreach (int underThemeId in underThemeIds)
            {
                ConditionForIds += $" theme_id = {underThemeId} OR";
            }

            ConditionForIds = ConditionForIds.Remove(ConditionForIds.Length - 3);

            return ConditionForIds;
        }
    }
}