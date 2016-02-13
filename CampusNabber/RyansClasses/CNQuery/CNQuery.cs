using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCode.CNObjectFolder;
using System.Linq.Dynamic;
using DatabaseCode.FactoryFiles;
using System.Data.Entity.Core;
using CampusNabber.Models;
using System.Data.Entity;

namespace DatabaseCode.CNQueryFolder
{

    /// <summary>
    /// CNQuery item = new CNQuery("PostItem");
    /// <para />item.setQueryWhereKeyEqualToCondition("school_name", "EWU");
    ///	<para />item.setQueryWhereKeyGreaterThanOrEqualToCondition("price", 60);
    ///	<para /> List(PostItem) posts = item.select().Cast(PostItem)().ToList();
    ///	<para /> Angle brackets around PostItem
    /// </summary>
    public class CNQuery : CNQueryInterface
    {
        private string queryClassName;
        private Dictionary<string, dynamic> queryWhereConditions;
        private List<dynamic> queryValues;

        public CNQuery()
        {
            queryClassName = "";
            queryWhereConditions = new Dictionary<string, dynamic>();
            queryValues = new List<dynamic>();
        }

        public CNQuery(string queryClassName)
        {
            if (queryClassName == null || queryClassName.Equals(""))
                throw new ArgumentNullException("No class name provided");
            this.queryClassName = queryClassName;
            queryWhereConditions = new Dictionary<string, dynamic>();
            queryValues = new List<dynamic>();
        }

        public void setQueryWhereKeyEqualToCondition(string key, dynamic condition)
        {
            if (key == null || key.Equals("") || condition == null || condition.Equals(""))
                throw new ArgumentNullException("No key or value specified");
            queryWhereConditions.Add(key + "=", condition);
            queryValues.Add(condition);
        }

        public void setQueryWhereKeyNotEqualToCondition(string key, dynamic condition)
        {
            if (key == null || key.Equals("") || condition == null || condition.Equals(""))
                throw new ArgumentNullException("No key or value specified");
            queryWhereConditions.Add(key + "!=", condition);
            queryValues.Add(condition);
        }

        public void setQueryWhereKeyLessThanOrEqualToCondition(string key, dynamic condition)
        {
            if (key == null || key.Equals("") || condition == null || condition.Equals(""))
                throw new ArgumentNullException("No key or value specified");
            queryWhereConditions.Add(key + "<=", condition);
            queryValues.Add(condition);
        }

        public void setQueryWhereKeyGreaterThanOrEqualToCondition(string key, dynamic condition)
        {
            if (key == null || key.Equals("") || condition == null || condition.Equals(""))
                throw new ArgumentNullException("No key or value specified");
            queryWhereConditions.Add(key + ">=", condition);
            queryValues.Add(condition);
        }


        public string buildWhereString()
        {
            List<string> updateStrings = new List<string>();
            int i = 0;
            foreach (KeyValuePair<string, dynamic> entry in queryWhereConditions)
            {
                updateStrings.Add(entry.Key + "@" + i + " ");
                i++;
            }
            return string.Join(" And ", updateStrings);
        }

        public string buildValueString()
        {
            List<dynamic> updateStrings = new List<dynamic>();
            foreach (KeyValuePair<string, dynamic> entry in queryWhereConditions)
            {
                updateStrings.Add(entry.Value);
            }
            return string.Join(", ", updateStrings);
        }

        public dynamic selectObjectById()
        {
            if (queryClassName == null || queryClassName.Equals(""))
                throw new ArgumentNullException("No class name provided");
            ContextFactory cf = new ContextFactory();
            using (var context = new CampusNabberEntities())
            {
                return cf.getEntity(context, queryClassName, queryWhereConditions["object_id="]);
            }
        }


        public List<dynamic> select()
        {
            if (queryClassName == null || queryClassName.Equals(""))
                throw new ArgumentNullException("No class name provided");
            List<dynamic> resultsList = null;
            ContextFactory cf = new ContextFactory();
            using (var context = new CampusNabberEntities())
            {
                IQueryable<dynamic> test = cf.getIQueryableSet(context, queryClassName);

                if (queryWhereConditions.Count == 0)
                {
                    resultsList = test.ToList<dynamic>();
                }
                else
                {
                    try
                    {
                        resultsList = test
                                    .Where(buildWhereString(), queryValues.ToArray()).ToList<dynamic>();
                    }
                    catch (ParseException ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }         
            }
            if (resultsList != null)
            {
                return resultsList;
            }

            return null;
        }


        public void setClassName(string queryClassName)
        {
            if (queryClassName == null || queryClassName.Equals(""))
                throw new ArgumentNullException("No class name provided");
            this.queryClassName = queryClassName;
        }

        public Dictionary<string, dynamic> getQueryCondition() { return this.queryWhereConditions; }
        public string getClassName() { return this.queryClassName; }
    }
}
