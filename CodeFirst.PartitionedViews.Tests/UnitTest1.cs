using System;
using System.ComponentModel.DataAnnotations;
using CodeFirst.PartitionedViews.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeFirst.PartitionedViews.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public class Player
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class PokerTable
        {            
            public string Code { get; set; }
            public string Description { get; set; }
        }
        public class PokerHand
        {
            public int Id { get; set; }
            public int Year { get; set; }
            public decimal Amount { get; set; }
            public Player Player { get; set; }
            public int PlayerId { get; set; }

            public PokerTable PokerTable { get; set; }
            public string PokerTableCode { get; set; }


        }

        [TestMethod]
        public void TestMethod1()
        {
            var config = new PartitionedViewConfiguration<PokerHand>
            {
                DataRangeKeyExpression = hand => new { hand.Year, hand.Id },
                PrimaryKeyExpression = hand => new { hand.Year, hand.Id },

            };
            config.ModelCreating += modelBuilder => 
            {
                modelBuilder.Entity<PokerTable>().HasKey(t => t.Code);
            };
            var adapter = new PartitionedViewAdapterFactory().Create(config);
            adapter.AddPartitionFor(new PokerHand
            {
                Player = new Player
                {
                    Name = "Giscard"
                },
                Amount = 200,
                Year = 2000
            });
        }

        private void Config_ModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
