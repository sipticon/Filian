using System.Collections.Generic;
using System.Data.SqlClient;

namespace Filian.MVVM.Model
{
    public class TestModel
    {
        protected static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public static Queue<Word> Words { get; set; }

        protected static readonly string SqlConnectionString =
            @"Data Source=OLEKSANDRM-T470;Initial Catalog=filian_database;Integrated Security=true";

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