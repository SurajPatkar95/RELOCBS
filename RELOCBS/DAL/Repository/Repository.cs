using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using RELOCBS.DAL.Extensions;

namespace RELOCBS.DAL.Repository
{
    public abstract class Repository<TEntity> where TEntity : new()
    {
        #region Methods
        //bool Insert(TEntity entity);
        //bool Update(TEntity entity);
        //bool DeleteById(int id);
        //TEntity SelectById(int id);
        //List<TEntity> SelectAll();

        protected IEnumerable<TEntity> ToList(IDbCommand command)
        {
            using (var record = command.ExecuteReader())
            {
                List<TEntity> items = new List<TEntity>();
                while (record.Read())
                {

                    items.Add(Map<TEntity>(record));
                }
                return items;
            }
        }

        protected TEntity Map<TEntity>(IDataRecord record)
        {
            var objT = Activator.CreateInstance<TEntity>();
            foreach (var property in typeof(TEntity).GetProperties())
            {
                if (record.HasColumn(property.Name) && !record.IsDBNull(record.GetOrdinal(property.Name)))
                    property.SetValue(objT, record[property.Name], null);


            }
            return objT;
        }

        #endregion

    }
}
