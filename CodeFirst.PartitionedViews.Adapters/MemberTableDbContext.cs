using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.PartitionedViews.Adapters
{
    public class MemberTableDbContext<T>:DbContext where T:class
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.RegisterEntityType(DataType);
            modelBuilder.Types()
                .Where(t=>t == DataType)
                .Configure(c => c.HasKey(PrimaryKeyPropertyNames));
            
            PartitionedViewConfiguration.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        public Type DataType
        {
            get { return typeof(T); }
        }
    
        public IEnumerable<string> PrimaryKeyPropertyNames {get; private set;}

        public string PartitionDataRange { get; private set; }
        PartitionedViewConfiguration PartitionedViewConfiguration { get; }

        public MemberTableDbContext(string suffix, PartitionedViewConfiguration config)
            : base(config.ConnectionName)
        {           
            PartitionDataRange = suffix;
            PrimaryKeyPropertyNames = config.PrimaryKeyPropertyNames;
            PartitionedViewConfiguration = config;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MemberTableDbContext<T>, Configuration<T>>(true));            
        }
        
           
        
    }
}
