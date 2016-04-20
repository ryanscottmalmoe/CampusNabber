using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using DatabaseCode.FactoryFiles;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using CampusNabber.Models;
using System.Data.Entity;

namespace DatabaseCode.CNObjectFolder
{

    /// <summary>
    /// CNObject item = new CNObject("User");
    /// <para />item.setKeyWithValue ("object_id", Guid.NewGuid());
    /// <para />item.setKeyWithValue ("username", "testUsername");
    ///	<para />item.setKeyWithValue ("pasword", Utiltiy.encryptPassword("testPassword"));
    /// <para />item.setKeyWithValue ("student_email", "testEmail@ewu.edu");
    /// <para />item.setKeyWithvalue ("school_name", "testName");
    ///	<para />item.createObject ();
    /// </summary>
    public class CNObject : CNObjectInterface
    {
        private string tableName;
        private Dictionary<string, dynamic> queryObjects; //dynamic variable

        public CNObject()
        {
            tableName = "";
            queryObjects = new Dictionary<string, dynamic>();
        }

        public CNObject(string tableName)
        {
            if (tableName == null || tableName.Equals(""))
                throw new ArgumentNullException("No class name provided");
            this.tableName = tableName;
            queryObjects = new Dictionary<string, dynamic>();
        }

        public void setKeyWithValue(string key, dynamic value)
        {
            if (key == null || key.Equals("") || value == null || value.Equals(""))
                throw new ArgumentNullException("No key or value specified");
            queryObjects.Add(key, value);
        }


        /// <summary> 
        /// Saves a new CNObject to SQL server.
        /// </summary>
        /// <returns> Returns void.</returns>
        public void createObject()
        { 
            if (tableName == null || tableName.Equals("") || queryObjects == null || queryObjects.Count == 0)
                throw new ArgumentNullException("No table or object to specified.");

            EntityFactory entityFactory = new EntityFactory(); //creates entity factory 
            var entityObject = entityFactory.getEntityObject(tableName); //returns entity object
            entityObject.setAttributes(this); //sets entity object values with cnobject
           
            //Creates new context and saves local variable to server
            ContextFactory cf = new ContextFactory();
            using (var context = new CampusNabberEntities())
            {
                DbSet set = cf.getDbSet(context, tableName);
                set.Add(entityObject);
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine(dbEx.Message);
                    foreach (var entries in dbEx.Entries)
                    {
                        Console.WriteLine(entries.Entity);
                        Console.WriteLine(dbEx.InnerException);
                    }
                }
            }
        }

        public void updateObject()
        {
            if (tableName == null || tableName.Equals("") || queryObjects == null || queryObjects.Count == 0)
                throw new ArgumentNullException("No table or object to specified.");

            EntityFactory entityFactory = new EntityFactory(); //creates entity factory 
            var entityObject = entityFactory.getEntityObject(tableName); //returns entity object
            entityObject.setAttributes(this); //sets entity object values with cnobject

            ContextFactory cf = new ContextFactory();
            using (var context = new CampusNabberEntities())
            { 
                var item = cf.getEntity(context, tableName, queryObjects["object_id"]);
                item.setAttributes(this);
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
                catch (DbUpdateException dbEx)
                {
                    Console.WriteLine(dbEx.Message);
                    foreach (var entries in dbEx.Entries)
                    {
                        Console.WriteLine(entries.Entity);
                        Console.WriteLine(dbEx.InnerException);
                    }
                }
            }
        }
        public void deleteObject()
        {
            if (tableName == null || tableName.Equals("") || queryObjects == null || queryObjects.Count == 0)
                throw new ArgumentNullException("No table or object to specified.");

        }

        public string getTableName() { return this.tableName; }
        public void setTableName(string tableName) { this.tableName = tableName; }
        public void setQueryObjects(Dictionary<string, dynamic> queryObjects) { this.queryObjects = queryObjects; }
        public Dictionary<string, dynamic> getQueryObjects() { return this.queryObjects; }
    }
}
