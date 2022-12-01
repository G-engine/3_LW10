using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class TickerdbContext : DbContext
{
    public TickerdbContext()
    {
    }

    public TickerdbContext(DbContextOptions<TickerdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<Ticker> Tickers { get; set; }

    public virtual DbSet<TodaysCondition> TodaysConditions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\serv;Database=tickerdb;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Price>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.Price1).HasColumnName("price");
            entity.Property(e => e.TickerId).HasColumnName("tickerId");

            entity.HasOne(d => d.Ticker).WithMany(p => p.Prices)
                .HasForeignKey(d => d.TickerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prices_Tickers");
        });

        modelBuilder.Entity<Ticker>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ticker1)
                .HasMaxLength(50)
                .HasColumnName("ticker");
        });

        modelBuilder.Entity<TodaysCondition>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.TickerId).HasColumnName("tickerId");

            entity.HasOne(d => d.Ticker).WithMany(p => p.TodaysConditions)
                .HasForeignKey(d => d.TickerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TodaysConditions_Tickers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
