using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Biddo.Models;

    public class BiddoContext : DbContext
    {
        public BiddoContext (DbContextOptions<BiddoContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> UserModel { get; set; } = default!;
        public DbSet<VendorModel> VendorModel { get; set; } = default!;

        public DbSet<EventModelTable> EventModelTable { get; set; } = default!;

        public DbSet<SelectedServiceModel> SelectedServiceTable { get; set; } = default!;

        public DbSet<ProvidedServiceModel> ProvidedServiceTable { get; set; } = default!;

        public DbSet<QueryModel> QueryTable { get; set; } = default!;

        public DbSet<BiddingModel> BiddingTable { get; set; } = default!;

        public DbSet<AuctionModelTable> AuctionModelTable { get; set; } = default!;

        public DbSet<ConversationModel> ConvensationTable { get; set; } = default!;

        public DbSet<TimelineCommentModel> TimelineCommentModel { get; set; } = default!;

        public DbSet<RatingModel> RatingTable { get; set; } = default!;

}
