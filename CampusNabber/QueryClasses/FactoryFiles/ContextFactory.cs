using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampusNabber.Models;

namespace DatabaseCode.FactoryFiles
{

    class ContextFactory : CampusNabberEntities
    {
        public dynamic getEntity(CampusNabberEntities context, string contextName, Guid guid)
        {
            if (contextName.Equals("PostItem"))
            {
                return (from o in context.PostItems
                        where o.object_id.Equals(guid)
                        select o).First();
            }
            else if (contextName.Equals("School"))
            {
                return (from o in context.Schools
                        where o.object_id.Equals(guid)
                        select o).First();
            }
            return null;
        }

        public dynamic getDbSet(CampusNabberEntities context, string contextName)
        {
                if (contextName.Equals("PostItem"))
                {
                    return context.PostItems;
                }
                else if (contextName.Equals("School"))
                {
                    return context.Schools;
                }
            return null;
        }

        public IQueryable<dynamic> getIQueryableSet(CampusNabberEntities context, string contextName)
        {
            if (contextName.Equals("PostItem"))
            {
                return context.PostItems;
            }
            else if (contextName.Equals("School"))
            {
                return context.Schools;
            }
            return null;
        }
    }
}
