using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCode.CNQueryFolder
{
    interface CNQueryInterface
    {
         void setQueryWhereKeyEqualToCondition(string key, dynamic condition);
         void setQueryWhereKeyNotEqualToCondition(string key, dynamic condition);
         void setQueryWhereKeyLessThanOrEqualToCondition(string key, dynamic condition);
         void setQueryWhereKeyGreaterThanOrEqualToCondition(string key, dynamic condition);
         string buildWhereString();
         string buildValueString();
         List<dynamic> select();
         void setClassName(string queryClassName);
         string getClassName();
    }
}
