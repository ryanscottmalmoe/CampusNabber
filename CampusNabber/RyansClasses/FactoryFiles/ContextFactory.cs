﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampusNabber.Models;

namespace DatabaseCode.FactoryFiles
{
    /// <summary>
    /// Used to dynamically determine which DbSet to choose from
    /// when dynamically building LINQ queries.
    /// </summary>
    class ContextFactory : CampusNabberEntities
    {    
        public IQueryable<dynamic> getDbSet(CampusNabberEntities entities, string contextName)
        {
            if(contextName.Equals("User"))
            {
                return entities.Users;
            } else if(contextName.Equals("PostItem"))
            {
                return entities.PostItems;
            } else if(contextName.Equals("School"))
            {
                return entities.Schools;
            }
            return null;
        }
    }
}
