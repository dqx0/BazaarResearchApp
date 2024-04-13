using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BazaarResearchApp.Models;

public partial class BazaarResearchContext : DbContext
{
    public BazaarResearchContext()
    {
    }

    public BazaarResearchContext(DbContextOptions<BazaarResearchContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Search> Searches { get; set; }
    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=BazaarResearch;Integrated Security=False;User ID=sa;Password=Bfuk2v6ffxa;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;");
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.NpcPrice).HasColumnName("npc_price");
            entity.Property(e => e.Url)
                .HasMaxLength(50)
                .HasColumnName("url");
        });

        modelBuilder.Entity<List>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.End)
                .HasColumnType("date")
                .HasColumnName("end");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SearchId).HasColumnName("search_id");
            entity.Property(e => e.Seller)
                .HasMaxLength(10)
                .UseCollation("Japanese_CI_AS")
                .HasColumnName("seller");
            entity.Property(e => e.SinglePrice).HasColumnName("single_price");
            entity.Property(e => e.Start)
                .HasColumnType("date")
                .HasColumnName("start");

            entity.HasOne(d => d.Search).WithMany(p => p.Lists)
                .HasForeignKey(d => d.SearchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lists_Searches");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.MaterialId).HasColumnName("material_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Item).WithMany(p => p.RecipeItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recipes_Items");

            entity.HasOne(d => d.Material).WithMany(p => p.RecipeMaterials)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Materials_Items");
        });

        modelBuilder.Entity<Search>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.NumberOfListing).HasColumnName("number_of_listing");
            entity.Property(e => e.SinglePriceAverage).HasColumnName("single_price_average");

            entity.HasOne(d => d.Item).WithMany(p => p.Searches)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Searches_Items");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
