﻿using System;
using System.Reflection;

namespace SqlSugar
{
    public class SqliteInsertBuilder : InsertBuilder
    {
        public override string SqlTemplate
        {
            get
            {
                if (IsReturnIdentity)
                {
                    return @"INSERT INTO {0} 
           ({1})
     VALUES
           ({2}) ;SELECT LAST_INSERT_ROWID();";
                }
                else
                {
                    return @"INSERT INTO {0} 
           ({1})
     VALUES
           ({2}) ;";

                }
            }
        }

        public override string SqlTemplateBatch
        {
            get
            {
                return "INSERT INTO {0} ({1})";
            }
        }

        public override object FormatValue(object value)
        {
            if (value == null)
            {
                return "NULL";
            }
            else
            {
                var type = value.GetType();
                if (type == PubConst.DateType)
                {
                    var date = value.ObjToDate();
                    if (date < Convert.ToDateTime("1900-1-1"))
                    {
                        date = Convert.ToDateTime("1900-1-1");
                    }
                    return "'" + date.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                }
                else if (type.IsEnum())
                {
                    return Convert.ToInt64(value);
                }
                else if (type == PubConst.BoolType)
                {
                    return value.ObjToBool() ? "1" : "0";
                }
                else if (type == PubConst.StringType || type == PubConst.ObjType)
                {
                    return "'" + value.ToString().ToSqlFilter() + "'";
                }
                else
                {
                    return   "'"+value.ToString() + "'";
                }
            }
        }
    }
}
