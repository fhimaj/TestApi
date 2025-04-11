using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace TestApi.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }

        [OracleBool]
        public bool Deleted { get; set; }
        public int InsertedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class OracleBoolAttribute : Attribute { }

    public class OracleBooleanInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            ModifyCommand(command);
            return result;
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            ModifyCommand(command);
            return new ValueTask<InterceptionResult<DbDataReader>>(result);
        }

        private void ModifyCommand(DbCommand command)
        {
            foreach (DbParameter parameter in command.Parameters)
            {
                if (parameter.Value is bool boolValue)
                {
                    parameter.Value = boolValue ? 1 : 0;
                }
            }           
        }
    }
}
