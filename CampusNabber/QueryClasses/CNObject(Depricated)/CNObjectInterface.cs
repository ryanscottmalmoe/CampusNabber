using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCode.CNObjectFolder
{
    interface CNObjectInterface
    {
        void setKeyWithValue(string key, dynamic value);
        void createObject();
        void updateObject();
        void deleteObject();
        void setQueryObjects(Dictionary<string, dynamic> queryObjects);
        Dictionary<string, dynamic> getQueryObjects();
        string getTableName();
        void setTableName(string tableName);

    }
}
