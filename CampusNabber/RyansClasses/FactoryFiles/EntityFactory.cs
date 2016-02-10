using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCode.FactoryFiles
{
    public class EntityFactory
    {
        public const string OBJECT_ID = "object_id";
        public const string USERNAME = "username";
        public const string SCHOOL_NAME = "school_name";
        public const string POST_DATE = "post_date";
        public const string PRICE = "price";
        public const string TITLE = "title";
        public const string DESCRIPTION = "description";
        public const string PHOTO_PATH = "photo_path";
        public const string CATEGORY = "category";
        public const string ADDRESS = "address";
        public const string ENCRYPTED_PASSWORD = "encrypted_password";
        public const string STUDENT_EMAIL = "student_email";

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
