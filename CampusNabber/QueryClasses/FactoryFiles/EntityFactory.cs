using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CampusNabber.Models;
using DatabaseCode.CNObjectFolder;

namespace DatabaseCode.FactoryFiles
{
    public class EntityFactory
    {

        public dynamic getEntityObject(string tableName)
        {
            if (tableName.Equals("User"))
            {
                return new User();
            } else if (tableName.Equals("PostItem"))
            {
                return new PostItem();
            } else if (tableName.Equals("School"))
            {
                return new School();
            }
            return new PostItem();
        }
    }
}
