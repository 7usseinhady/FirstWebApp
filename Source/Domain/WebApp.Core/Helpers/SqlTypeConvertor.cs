﻿using System.Collections;
using System.Data;
namespace WebApp.Core.Helpers
{
    public sealed class SqlTypeConvertor
    {
        private struct DbTypeMapEntry
        {
            public Type Type;
            public DbType DbType;
            public SqlDbType SqlDbType;
            public DbTypeMapEntry(Type type, DbType dbType, SqlDbType sqlDbType)
            {
                Type = type;
                DbType = dbType;
                SqlDbType = sqlDbType;
            }

        };

        public static ArrayList DbTypeList { get; set; } = new ArrayList();

        #region Constructors

        static SqlTypeConvertor()
        {
            DbTypeMapEntry dbTypeMapEntry
            = new DbTypeMapEntry(typeof(bool), DbType.Boolean, SqlDbType.Bit);
            DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry
            = new DbTypeMapEntry(typeof(byte), DbType.Double, SqlDbType.TinyInt);
            DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry
            = new DbTypeMapEntry(typeof(byte[]), DbType.Binary, SqlDbType.Image);
            DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry
            = new DbTypeMapEntry(typeof(DateTime), DbType.DateTime, SqlDbType.DateTime);
            DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry
            = new DbTypeMapEntry(typeof(decimal), DbType.Decimal, SqlDbType.Decimal);
            DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry
            = new DbTypeMapEntry(typeof(double), DbType.Double, SqlDbType.Float);
            DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry
            = new DbTypeMapEntry(typeof(Guid), DbType.Guid, SqlDbType.UniqueIdentifier);
            DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry
            = new DbTypeMapEntry(typeof(short), DbType.Int16, SqlDbType.SmallInt);
            DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry
            = new DbTypeMapEntry(typeof(int), DbType.Int32, SqlDbType.Int);
            DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry
            = new DbTypeMapEntry(typeof(long), DbType.Int64, SqlDbType.BigInt);
            DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry
            = new DbTypeMapEntry(typeof(object), DbType.Object, SqlDbType.Variant);
            DbTypeList.Add(dbTypeMapEntry);

            dbTypeMapEntry
            = new DbTypeMapEntry(typeof(string), DbType.String, SqlDbType.VarChar);
            DbTypeList.Add(dbTypeMapEntry);

        }

        public SqlTypeConvertor()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert db type to .Net data type
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static Type ToNetType(DbType dbType)
        {
            DbTypeMapEntry entry = Find(dbType);
            return entry.Type;
        }

        /// <summary>
        /// Convert TSQL type to .Net data type
        /// </summary>
        /// <param name="sqlDbType"></param>
        /// <returns></returns>
        public static Type ToNetType(SqlDbType sqlDbType)
        {
            DbTypeMapEntry entry = Find(sqlDbType);
            return entry.Type;
        }

        /// <summary>
        /// Convert .Net type to Db type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DbType ToDbType(Type type)
        {
            DbTypeMapEntry entry = Find(type);
            return entry.DbType;
        }

        /// <summary>
        /// Convert TSQL data type to DbType
        /// </summary>
        /// <param name="sqlDbType"></param>
        /// <returns></returns>
        public static DbType ToDbType(SqlDbType sqlDbType)
        {
            DbTypeMapEntry entry = Find(sqlDbType);
            return entry.DbType;
        }

        /// <summary>
        /// Convert .Net type to TSQL data type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SqlDbType ToSqlDbType(Type type)
        {
            DbTypeMapEntry entry = Find(type);
            return entry.SqlDbType;
        }

        /// <summary>
        /// Convert DbType type to TSQL data type
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static SqlDbType ToSqlDbType(DbType dbType)
        {
            DbTypeMapEntry entry = Find(dbType);
            return entry.SqlDbType;
        }

        private static DbTypeMapEntry Find(Type type)
        {
            object retObj = null!;
            for (int i = 0; i < DbTypeList.Count; i++)
            {
                DbTypeMapEntry entry = (DbTypeMapEntry)DbTypeList[i]!;
                if (entry.Type == (Nullable.GetUnderlyingType(type) ?? type))
                {
                    retObj = entry;
                    break;
                }
            }
            if (retObj == null)
            {
                throw
                new ApplicationException("Referenced an unsupported Type " + type.ToString())!;
            }

            return (DbTypeMapEntry)retObj;
        }

        private static DbTypeMapEntry Find(DbType dbType)
        {
            object retObj = null!;
            for (int i = 0; i < DbTypeList.Count; i++)
            {
                DbTypeMapEntry entry = (DbTypeMapEntry)DbTypeList[i]!;
                if (entry.DbType == dbType)
                {
                    retObj = entry;
                    break;
                }
            }
            if (retObj == null)
            {
                throw
                new ApplicationException("Referenced an unsupported DbType " + dbType.ToString())!;
            }

            return (DbTypeMapEntry)retObj;
        }

        private static DbTypeMapEntry Find(SqlDbType sqlDbType)
        {
            object retObj = null!;
            for (int i = 0; i < DbTypeList.Count; i++)
            {
                DbTypeMapEntry entry = (DbTypeMapEntry)DbTypeList[i]!;
                if (entry.SqlDbType == sqlDbType)
                {
                    retObj = entry;
                    break;
                }
            }
            if (retObj == null)
            {
                throw
                new ApplicationException("Referenced an unsupported SqlDbType")!;
            }

            return (DbTypeMapEntry)retObj;
        }

        #endregion
    }
}
